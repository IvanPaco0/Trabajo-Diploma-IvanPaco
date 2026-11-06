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
    public partial class _012IP_frmBitacoraCambios : Form, IObserver
    {
        private _012IP_BitacoraCambiosBLL bitacoraCambiosBLL;
        private List<_012IP_SocioHistorialBE> versiones = new List<_012IP_SocioHistorialBE>();

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmBitacoraCambios()
        {
            InitializeComponent();
        }

        private void _012IP_frmBitacoraCambios_Load(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;
            try
            {
                bitacoraCambiosBLL = new _012IP_BitacoraCambiosBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
                return;
            }

            _012IP_ConfigurarGrilla(dgvHistorial);
            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);
            _012IP_CargarListas();
            ActiveControl = cmbDni;
        }

        private void _012IP_frmBitacoraCambios_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

        private void _012IP_chkFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpDesde.Enabled = chkDesde.Checked;
            dtpHasta.Enabled = chkHasta.Checked;
        }

        private void _012IP_btnAplicar_Click(object sender, EventArgs e)
        {
            _012IP_Consultar();
        }

        private void _012IP_btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbDni.Text = string.Empty;
            cmbNombre.Text = string.Empty;
            chkDesde.Checked = false;
            chkHasta.Checked = false;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;
            _012IP_Consultar();
        }

        private void _012IP_CargarListas()
        {
            if (bitacoraCambiosBLL == null) return;
            try
            {
                List<_012IP_SocioHistorialBE> todas = bitacoraCambiosBLL._012IP_ConsultarHistorial(null, null, null, null);
                var socios = todas.GroupBy(v => v.IdSocio)
                    .Select(g => g.OrderByDescending(v => v.VersionActiva).ThenByDescending(v => v.FechaCambio).First())
                    .ToList();

                cmbDni.Items.Clear();
                foreach (string dni in socios.Select(v => v.DNI).Distinct().OrderBy(d => d))
                    cmbDni.Items.Add(dni);

                cmbNombre.Items.Clear();
                foreach (string nombre in socios.Select(v => v.Apellido + ", " + v.Nombre).Distinct().OrderBy(n => n))
                    cmbNombre.Items.Add(nombre);
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

        private void _012IP_Consultar()
        {
            if (bitacoraCambiosBLL == null) return;
            try
            {
                lblMsj.Text = string.Empty;
                versiones = bitacoraCambiosBLL._012IP_ConsultarHistorial(
                    cmbDni.Text,
                    cmbNombre.Text,
                    chkDesde.Checked ? (DateTime?)dtpDesde.Value : null,
                    chkHasta.Checked ? (DateTime?)dtpHasta.Value : null);

                dgvHistorial.DataSource = versiones.Select(v => new
                {
                    Id = v.IdHistorial,
                    Fecha = v.FechaCambio.ToString("dd/MM/yyyy HH:mm:ss"),
                    DNI = v.DNI,
                    Apellido = v.Apellido,
                    Nombre = v.Nombre,
                    Email = v.Email,
                    Telefono = v.Telefono,
                    Operacion = v.Operacion,
                    Usuario = v.Usuario,
                    Vigente = v.VersionActiva ? _012IP_Texto("BitCamSi") : string.Empty
                }).ToList();

                if (dgvHistorial.Columns.Contains("Id"))
                    dgvHistorial.Columns["Id"].Visible = false;

                foreach (DataGridViewRow fila in dgvHistorial.Rows)
                {
                    if (versiones[fila.Index].VersionActiva)
                    {
                        fila.DefaultCellStyle.ForeColor = Color.DarkGreen;
                        fila.DefaultCellStyle.Font = new Font(dgvHistorial.Font, FontStyle.Bold);
                    }
                }

                _012IP_AplicarEncabezados();
                dgvHistorial.ClearSelection();
                btnActivar.Enabled = false;

                lblResumen.Text = string.Format(_012IP_Texto("BitCamResumen"),
                    versiones.Count, versiones.Select(v => v.IdSocio).Distinct().Count());
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

        private _012IP_SocioHistorialBE _012IP_VersionSeleccionada()
        {
            if (dgvHistorial.CurrentRow == null || !dgvHistorial.CurrentRow.Selected) return null;
            int indice = dgvHistorial.CurrentRow.Index;
            return indice >= 0 && indice < versiones.Count ? versiones[indice] : null;
        }

        private void _012IP_dgvHistorial_SelectionChanged(object sender, EventArgs e)
        {
            _012IP_SocioHistorialBE v = _012IP_VersionSeleccionada();
            btnActivar.Enabled = v != null && !v.VersionActiva;
            if (v != null && v.VersionActiva)
            {
                lblMsj.ForeColor = Color.White;
                lblMsj.Text = _012IP_Texto("BitCamYaVigente");
            }
            else
            {
                lblMsj.Text = string.Empty;
            }
        }

        private void _012IP_btnActivar_Click(object sender, EventArgs e)
        {
            _012IP_SocioHistorialBE v = _012IP_VersionSeleccionada();
            if (v == null)
            {
                lblMsj.ForeColor = Color.White;
                lblMsj.Text = _012IP_Texto("BitCamSeleccione");
                return;
            }

            string detalle = string.Format(_012IP_Texto("BitCamConfirmar"),
                v.FechaCambio.ToString("dd/MM/yyyy HH:mm"), v.DNI, v.Nombre, v.Apellido, v.Email, v.Telefono);
            if (MessageBox.Show(detalle, _012IP_Texto("MsjConfirmacion"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bitacoraCambiosBLL._012IP_ActivarVersion(v.IdHistorial);
                _012IP_Consultar();
                lblMsj.ForeColor = Color.LightGreen;
                lblMsj.Text = string.Format(_012IP_Texto("BitCamRestaurada"), v.DNI);
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

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
            _012IP_Encabezado("Fecha", "BitCamColFecha");
            _012IP_Encabezado("DNI", "DNI");
            _012IP_Encabezado("Apellido", "lblApe");
            _012IP_Encabezado("Nombre", "lblNombre");
            _012IP_Encabezado("Email", "lblEmail");
            _012IP_Encabezado("Telefono", "RenMemTelefono");
            _012IP_Encabezado("Operacion", "BitCamColOperacion");
            _012IP_Encabezado("Usuario", "BitCamColUsuario");
            _012IP_Encabezado("Vigente", "BitCamColVigente");
        }

        private void _012IP_Encabezado(string columna, string clave)
        {
            if (dgvHistorial.Columns.Contains(columna))
                dgvHistorial.Columns[columna].HeaderText = _012IP_Texto(clave);
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private void _012IP_MostrarError(Exception ex)
        {
            lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
            lblMsj.Text = ex.InnerException != null ? ex.Message + " (" + ex.InnerException.Message + ")" : ex.Message;
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("BitCamTitulo");
            lblDni.Text = _012IP_Texto("BitCamFiltroDni");
            lblNombre.Text = _012IP_Texto("BitCamFiltroNombre");
            btnAplicar.Text = _012IP_Texto("BitCamAplicar");
            btnLimpiar.Text = _012IP_Texto("BitCamLimpiar");
            chkDesde.Text = _012IP_Texto("BitCamDesde");
            chkHasta.Text = _012IP_Texto("BitCamHasta");
            btnActivar.Text = _012IP_Texto("BitCamRestaurar");
            btnSalir.Text = _012IP_Texto("btnSalir");
            dtpDesde.Enabled = chkDesde.Checked;
            dtpHasta.Enabled = chkHasta.Checked;
            _012IP_Consultar();
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
