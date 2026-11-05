using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _012IP_GUI
{
    // Gestión de socios (MAESTROS > Socios): alta, modificación y baja lógica / reactivación.
    // La baja es lógica (Activo = 0): el socio no se borra, deja de aparecer en Renovar membresía
    // y el cambio queda en la bitácora de cambios (trigger de la tabla Socio).
    public partial class _012IP_frmGestionSocios : Form, IObserver
    {
        private readonly System.Windows.Forms.Timer timerBusqueda = new System.Windows.Forms.Timer { Interval = 300 };

        private _012IP_SocioBLL socioBLL;
        private List<_012IP_SocioBE> socios = new List<_012IP_SocioBE>();

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmGestionSocios()
        {
            InitializeComponent();
            timerBusqueda.Tick += _012IP_timerBusqueda_Tick;
        }

        private void _012IP_frmGestionSocios_Load(object sender, EventArgs e)
        {
            try
            {
                socioBLL = new _012IP_SocioBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
                return;
            }
            _012IP_ConfigurarGrilla(dgvSocios);
            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);
            ActiveControl = txtBuscar;
        }

        private void _012IP_frmGestionSocios_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerBusqueda.Stop();
            timerBusqueda.Dispose();
            LanguageManager.Instance.EliminarObservador(this);
        }

        // ------------------------------------------------------------ búsqueda

        private void _012IP_txtBuscar_TextChanged(object sender, EventArgs e)
        {
            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void _012IP_timerBusqueda_Tick(object sender, EventArgs e)
        {
            timerBusqueda.Stop();
            _012IP_Buscar();
        }

        private void _012IP_chkInactivos_CheckedChanged(object sender, EventArgs e)
        {
            _012IP_Buscar();
        }

        private void _012IP_Buscar(int? idSeleccionar = null)
        {
            if (socioBLL == null) return;
            try
            {
                int? idActual = idSeleccionar ?? _012IP_SocioSeleccionado()?.IdSocio;
                socios = socioBLL._012IP_BuscarSocioPorTexto(txtBuscar.Text, true)
                    .Where(s => chkInactivos.Checked || s.Activo)
                    .ToList();

                dgvSocios.DataSource = socios.Select(s => new
                {
                    DNI = s.DNI,
                    Apellido = s.Apellido,
                    Nombre = s.Nombre,
                    Email = s.Email,
                    Telefono = s.Telefono,
                    Membresia = s.Suscripcion?.Membresia?.Nombre ?? "-",
                    Vence = s.Suscripcion != null ? s.Suscripcion.FechaVencimiento.ToString("dd/MM/yyyy") : "-",
                    Estado = s.Activo ? _012IP_Texto("GesSocActivo") : _012IP_Texto("GesSocDeBaja")
                }).ToList();

                foreach (DataGridViewRow fila in dgvSocios.Rows)
                    if (!socios[fila.Index].Activo)
                        fila.DefaultCellStyle.ForeColor = Color.DimGray;

                _012IP_AplicarEncabezados();
                dgvSocios.ClearSelection();
                if (idActual.HasValue)
                {
                    int indice = socios.FindIndex(s => s.IdSocio == idActual.Value);
                    if (indice >= 0)
                    {
                        dgvSocios.Rows[indice].Selected = true;
                        dgvSocios.CurrentCell = dgvSocios.Rows[indice].Cells[0];
                    }
                }
                lblResumen.Text = string.Format(_012IP_Texto("GesSocResumen"), socios.Count(s => s.Activo), socios.Count(s => !s.Activo));
                _012IP_ActualizarBotones();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

        private _012IP_SocioBE _012IP_SocioSeleccionado()
        {
            if (dgvSocios.CurrentRow == null || !dgvSocios.CurrentRow.Selected) return null;
            int i = dgvSocios.CurrentRow.Index;
            return i >= 0 && i < socios.Count ? socios[i] : null;
        }

        private void _012IP_dgvSocios_SelectionChanged(object sender, EventArgs e)
        {
            _012IP_ActualizarBotones();
        }

        private void _012IP_ActualizarBotones()
        {
            _012IP_SocioBE s = _012IP_SocioSeleccionado();
            btnModificar.Enabled = s != null;
            btnBaja.Enabled = s != null;
            btnBaja.Text = s != null && !s.Activo ? _012IP_Texto("GesSocReactivar") : _012IP_Texto("GesSocBaja");
        }

        // ------------------------------------------------------------ alta / modificación / baja

        private void _012IP_btnNuevo_Click(object sender, EventArgs e)
        {
            using (var frm = new _012IP_frmRegistrarSocio())
            {
                if (frm.ShowDialog(this) == DialogResult.OK && frm.SocioRegistrado != null)
                    _012IP_Buscar(frm.SocioRegistrado.IdSocio);
            }
        }

        private void _012IP_btnModificar_Click(object sender, EventArgs e)
        {
            _012IP_SocioBE s = _012IP_SocioSeleccionado();
            if (s == null)
            {
                _012IP_Mensaje(_012IP_Texto("GesSocSeleccione"), Color.White);
                return;
            }
            using (var frm = new _012IP_frmRegistrarSocio(s))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    _012IP_Buscar(s.IdSocio);
                    _012IP_Mensaje(_012IP_Texto("GesSocModificado"), Color.LightGreen);
                }
            }
        }

        private void _012IP_dgvSocios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) _012IP_btnModificar_Click(sender, e);
        }

        private void _012IP_btnBaja_Click(object sender, EventArgs e)
        {
            _012IP_SocioBE s = _012IP_SocioSeleccionado();
            if (s == null)
            {
                _012IP_Mensaje(_012IP_Texto("GesSocSeleccione"), Color.White);
                return;
            }

            bool reactivar = !s.Activo;
            string pregunta = string.Format(_012IP_Texto(reactivar ? "GesSocConfirmReactivar" : "GesSocConfirmBaja"), s.Nombre, s.Apellido, s.DNI);
            if (!reactivar && s.Suscripcion != null && s.Suscripcion.FechaVencimiento.Date >= DateTime.Today)
                pregunta += "\n\n" + string.Format(_012IP_Texto("GesSocAvisoVigente"), s.Suscripcion.FechaVencimiento.ToString("dd/MM/yyyy"));

            if (MessageBox.Show(pregunta, _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo,
                    reactivar ? MessageBoxIcon.Question : MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                socioBLL._012IP_CambiarEstadoSocio(s, reactivar);
                if (!reactivar && !chkInactivos.Checked)
                    _012IP_Buscar();
                else
                    _012IP_Buscar(s.IdSocio);
                _012IP_Mensaje(string.Format(_012IP_Texto(reactivar ? "GesSocReactivadoOk" : "GesSocBajaOk"), s.DNI), Color.LightGreen);
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

        // ------------------------------------------------------------ utilidades

        private void _012IP_ConfigurarGrilla(DataGridView grilla)
        {
            grilla.ReadOnly = true;
            grilla.AllowUserToAddRows = false;
            grilla.AllowUserToDeleteRows = false;
            grilla.AllowUserToResizeRows = false;
            grilla.MultiSelect = false;
            grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grilla.RowHeadersVisible = false;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grilla.BackgroundColor = Color.White;
            grilla.BorderStyle = BorderStyle.None;
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grilla.DefaultCellStyle.BackColor = Color.White;
            grilla.DefaultCellStyle.ForeColor = Color.Black;
            grilla.DefaultCellStyle.SelectionBackColor = Color.Maroon;
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void _012IP_AplicarEncabezados()
        {
            _012IP_Encabezado("DNI", "DNI");
            _012IP_Encabezado("Apellido", "lblApe");
            _012IP_Encabezado("Nombre", "lblNombre");
            _012IP_Encabezado("Email", "lblEmail");
            _012IP_Encabezado("Telefono", "RenMemTelefono");
            _012IP_Encabezado("Membresia", "RenMemMembresia");
            _012IP_Encabezado("Vence", "RenMemVence");
            _012IP_Encabezado("Estado", "GesSocColEstado");
        }

        private void _012IP_Encabezado(string columna, string clave)
        {
            if (dgvSocios.Columns.Contains(columna))
                dgvSocios.Columns[columna].HeaderText = _012IP_Texto(clave);
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private void _012IP_Mensaje(string texto, Color color)
        {
            lblMsj.ForeColor = color;
            lblMsj.Text = texto;
        }

        private void _012IP_MostrarError(Exception ex)
        {
            _012IP_Mensaje(ex.InnerException != null ? ex.Message + " (" + ex.InnerException.Message + ")" : ex.Message,
                Color.FromArgb(255, 156, 156));
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("GesSocTitulo");
            lblBuscar.Text = _012IP_Texto("RenMemBuscarAyuda");
            chkInactivos.Text = _012IP_Texto("GesSocInactivos");
            btnNuevo.Text = _012IP_Texto("GesSocNuevo");
            btnModificar.Text = _012IP_Texto("GesSocModificar");
            btnSalir.Text = _012IP_Texto("btnSalir");
            _012IP_Buscar();
        }

        private void _012IP_btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
            GUI.Menu menu = new GUI.Menu();
            menu.Show();
        }

        private void _012IP_btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void _012IP_btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void _012IP_BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                arrastrando = true;
                posX = e.X;
                posY = e.Y;
            }
        }

        private void _012IP_BarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastrando)
                this.Location = new Point(this.Location.X + (e.X - posX), this.Location.Y + (e.Y - posY));
        }

        private void _012IP_BarraTitulo_MouseUp(object sender, MouseEventArgs e)
        {
            arrastrando = false;
        }
    }
}
