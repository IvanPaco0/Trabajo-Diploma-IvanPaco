using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace _012IP_GUI
{
    public partial class _012IP_frmGestionInventario : Form, IObserver
    {
        private _012IP_EjemplarBLL ejemplarBLL;
        private List<_012IP_EjemplarBE> ejemplaresMostrados = new List<_012IP_EjemplarBE>();
        private bool actualizandoGrilla;
        private bool modoXml;                       // la grilla muestra datos del XML (solo lectura) o esta limpia
        private readonly ToolTip toolTip = new ToolTip();

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmGestionInventario()
        {
            InitializeComponent();
        }

        private void _012IP_frmGestionInventario_Load(object sender, EventArgs e)
        {
            try
            {
                ejemplarBLL = new _012IP_EjemplarBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }

            LanguageManager.Instance.AgregarObservador(this);
            _012IP_ConfigurarGrilla(dgvEjemplares);
            Actualizar(LanguageManager.Instance);
            _012IP_Info(_012IP_Texto("InvIngreseDatos"));
        }

        private void _012IP_frmGestionInventario_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

        // ------------------------------------------------------------------ idioma

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("InvTitulo");
            grpIngreso.Text = _012IP_Texto("InvGrpIngreso");
            lblCodigo.Text = _012IP_Texto("InvCodigoBarras");
            lblNumeroInv.Text = _012IP_Texto("InvNumeroInventario");
            lblTituloLibro.Text = _012IP_Texto("InvTituloLibro");
            lblAutor.Text = _012IP_Texto("InvAutor");
            lblOrigen.Text = _012IP_Texto("InvOrigen");
            btnRegistrar.Text = _012IP_Texto("InvBtnRegistrar");
            grpUbicacion.Text = _012IP_Texto("InvGrpUbicacion");
            lblPasillo.Text = _012IP_Texto("InvPasillo");
            lblSeccion.Text = _012IP_Texto("InvSeccion");
            lblEstanteria.Text = _012IP_Texto("InvEstanteria");
            btnUbicar.Text = _012IP_Texto("InvBtnUbicar");
            grpEstado.Text = _012IP_Texto("InvGrpEstado");
            lblNuevoEstado.Text = _012IP_Texto("InvNuevoEstado");
            lblMotivo.Text = _012IP_Texto("InvMotivo");
            btnCambiarEstado.Text = _012IP_Texto("InvBtnCambiarEstado");
            btnBaja.Text = _012IP_Texto("InvBtnBaja");
            lblFiltro.Text = _012IP_Texto("InvFiltroEstado");
            btnBuscar.Text = _012IP_Texto("InvBtnBuscar");
            btnSalir.Text = _012IP_Texto("btnSalir");
            grpXml.Text = _012IP_Texto("InvGrpXml");
            btnSerializar.Text = _012IP_Texto("InvBtnSerializar");
            btnDesserializar.Text = _012IP_Texto("InvBtnDesserializar");
            btnLimpiar.Text = _012IP_Texto("InvBtnLimpiar");
            toolTip.SetToolTip(btnLimpiar, _012IP_Texto("InvTipLimpiar"));
            toolTip.SetToolTip(btnBuscar, _012IP_Texto("InvTipBuscar"));
            toolTip.SetToolTip(btnExaminarSer, _012IP_Texto("InvTipExaminarSer"));
            toolTip.SetToolTip(btnExaminarDes, _012IP_Texto("InvTipExaminarDes"));

            _012IP_BindCombo(cmbOrigen, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(_012IP_EjemplarBLL.ORIGEN_COMPRA, _012IP_EjemplarBLL._012IP_NombreOrigen(_012IP_EjemplarBLL.ORIGEN_COMPRA)),
                new KeyValuePair<string, string>(_012IP_EjemplarBLL.ORIGEN_DONACION, _012IP_EjemplarBLL._012IP_NombreOrigen(_012IP_EjemplarBLL.ORIGEN_DONACION))
            });

            _012IP_BindCombo(cmbFiltro, new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Todos", _012IP_Texto("InvFiltroTodos")),
                new KeyValuePair<string, string>("Pendientes", _012IP_Texto("InvFiltroPendientes")),
                new KeyValuePair<string, string>(_012IP_EjemplarBLL.ESTADO_BUENO, _012IP_EjemplarBLL._012IP_NombreEstado(_012IP_EjemplarBLL.ESTADO_BUENO)),
                new KeyValuePair<string, string>(_012IP_EjemplarBLL.ESTADO_DETERIORADO, _012IP_EjemplarBLL._012IP_NombreEstado(_012IP_EjemplarBLL.ESTADO_DETERIORADO)),
                new KeyValuePair<string, string>(_012IP_EjemplarBLL.ESTADO_RESTAURACION, _012IP_EjemplarBLL._012IP_NombreEstado(_012IP_EjemplarBLL.ESTADO_RESTAURACION)),
                new KeyValuePair<string, string>(_012IP_EjemplarBLL.ESTADO_BAJA, _012IP_EjemplarBLL._012IP_NombreEstado(_012IP_EjemplarBLL.ESTADO_BAJA))
            });

            if (ejemplarBLL != null)
                _012IP_RefrescarTodo(null);
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private void _012IP_BindCombo(ComboBox combo, List<KeyValuePair<string, string>> items)
        {
            string anterior = combo.SelectedValue as string;
            combo.DataSource = null;
            combo.DisplayMember = "Value";
            combo.ValueMember = "Key";
            combo.DataSource = items;

            if (anterior != null && items.Any(i => i.Key == anterior))
                combo.SelectedValue = anterior;
            else
                combo.SelectedIndex = items.Count > 0 ? 0 : -1;
        }

        // ------------------------------------------------------------------ mensajes

        private void _012IP_Info(string mensaje)
        {
            lblMsj.ForeColor = Color.White;
            lblMsj.Text = mensaje;
        }

        private void _012IP_Ok(string mensaje)
        {
            lblMsj.ForeColor = Color.FromArgb(156, 255, 156);
            lblMsj.Text = mensaje;
        }

        private void _012IP_Error(string mensaje)
        {
            lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
            lblMsj.Text = mensaje;
        }

        private void _012IP_MostrarError(Exception ex)
        {
            MessageBox.Show(ex.Message, _012IP_Texto("RenMemError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ------------------------------------------------------------------ grilla / stock

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

        private void _012IP_Encabezado(string columna, string clave)
        {
            if (dgvEjemplares.Columns.Contains(columna))
                dgvEjemplares.Columns[columna].HeaderText = _012IP_Texto(clave);
        }

        private void _012IP_RefrescarTodo(string codigoASeleccionar)
        {
            try
            {
                _012IP_StockBE s = ejemplarBLL._012IP_ObtenerStock();
                lblStock.Text = string.Format(_012IP_Texto("InvStockFormato"),
                    s.Total, s.Disponibles, s.Deteriorados, s.EnRestauracion, s.Bajas, s.PendientesUbicar);

                string previo = codigoASeleccionar ?? _012IP_CodigoSeleccionado();
                string filtro = Convert.ToString(cmbFiltro.SelectedValue) ?? "Todos";

                modoXml = false;
                _012IP_MostrarEnGrilla(ejemplarBLL._012IP_ListarEjemplares(filtro, txtBuscar.Text), previo);
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        /// <summary>Muestra una lista de ejemplares en la grilla (vengan de la base o de un XML).</summary>
        private void _012IP_MostrarEnGrilla(List<_012IP_EjemplarBE> lista, string previo)
        {
            ejemplaresMostrados = lista;

            actualizandoGrilla = true;
            try
            {
                dgvEjemplares.DataSource = ejemplaresMostrados.Select(x => new
                {
                    CodigoBarras = x.CodigoBarras,
                    NumeroInventario = x.NumeroInventario,
                    Titulo = x.Titulo,
                    Autor = x.Autor,
                    Origen = _012IP_EjemplarBLL._012IP_NombreOrigen(x.Origen),
                    Estado = _012IP_EjemplarBLL._012IP_NombreEstado(x.Estado),
                    Ubicacion = _012IP_EjemplarBLL._012IP_TextoUbicacion(x),
                    Disponible = x.EstaDisponible ? _012IP_Texto("InvSi") : _012IP_Texto("InvNo")
                }).ToList();

                _012IP_Encabezado("CodigoBarras", "InvCodigoBarras");
                _012IP_Encabezado("NumeroInventario", "InvNumeroInventario");
                _012IP_Encabezado("Titulo", "InvTituloLibro");
                _012IP_Encabezado("Autor", "InvAutor");
                _012IP_Encabezado("Origen", "InvOrigen");
                _012IP_Encabezado("Estado", "InvColEstado");
                _012IP_Encabezado("Ubicacion", "InvColUbicacion");
                _012IP_Encabezado("Disponible", "InvColDisponible");

                dgvEjemplares.ClearSelection();
                dgvEjemplares.CurrentCell = null;

                if (!string.IsNullOrEmpty(previo))
                {
                    foreach (DataGridViewRow fila in dgvEjemplares.Rows)
                    {
                        if (Convert.ToString(fila.Cells["CodigoBarras"].Value) == previo)
                        {
                            dgvEjemplares.CurrentCell = fila.Cells[0];
                            fila.Selected = true;
                            break;
                        }
                    }
                }
            }
            finally
            {
                actualizandoGrilla = false;
            }

            _012IP_AlCambiarSeleccion();
        }

        private string _012IP_CodigoSeleccionado()
        {
            if (dgvEjemplares.CurrentRow == null || !dgvEjemplares.CurrentRow.Selected)
                return null;

            return Convert.ToString(dgvEjemplares.CurrentRow.Cells["CodigoBarras"].Value);
        }

        private _012IP_EjemplarBE _012IP_EjemplarSeleccionado()
        {
            string codigo = _012IP_CodigoSeleccionado();
            if (codigo == null) return null;
            return ejemplaresMostrados.FirstOrDefault(x => x.CodigoBarras == codigo);
        }

        private void _012IP_AlCambiarSeleccion()
        {
            _012IP_EjemplarBE ej = _012IP_EjemplarSeleccionado();

            if (ej == null)
            {
                lblSeleccionado.Text = "";
                txtPasillo.Text = txtSeccion.Text = txtEstanteria.Text = "";
                _012IP_BindCombo(cmbNuevoEstado, new List<KeyValuePair<string, string>>());
                btnUbicar.Enabled = btnCambiarEstado.Enabled = btnBaja.Enabled = false;
                return;
            }

            lblSeleccionado.Text = string.Format(_012IP_Texto("InvSeleccionado"), ej.CodigoBarras, ej.Titulo);
            txtPasillo.Text = ej.Pasillo;
            txtSeccion.Text = ej.Seccion;
            txtEstanteria.Text = ej.Estanteria;

            var siguientes = _012IP_EjemplarBLL._012IP_EstadosPermitidos(ej.Estado)
                .Where(s => s != _012IP_EjemplarBLL.ESTADO_BAJA)
                .Select(s => new KeyValuePair<string, string>(s, _012IP_EjemplarBLL._012IP_NombreEstado(s)))
                .ToList();
            _012IP_BindCombo(cmbNuevoEstado, siguientes);

            bool esBaja = ej.Estado == _012IP_EjemplarBLL.ESTADO_BAJA;
            btnUbicar.Enabled = !esBaja;
            btnCambiarEstado.Enabled = siguientes.Count > 0;
            btnBaja.Enabled = !esBaja;

            if (modoXml)
                btnUbicar.Enabled = btnCambiarEstado.Enabled = btnBaja.Enabled = false;
        }

        private void _012IP_dgvEjemplares_SelectionChanged(object sender, EventArgs e)
        {
            if (actualizandoGrilla) return;
            _012IP_AlCambiarSeleccion();
        }

        // ------------------------------------------------------------------ acciones

        private void _012IP_btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string origen = Convert.ToString(cmbOrigen.SelectedValue);
                _012IP_EjemplarBE ej = ejemplarBLL._012IP_RegistrarIngreso(
                    txtCodigo.Text, txtNumeroInv.Text, txtTituloLibro.Text, txtAutor.Text, origen);

                txtCodigo.Clear();
                txtNumeroInv.Clear();
                txtTituloLibro.Clear();
                txtAutor.Clear();

                _012IP_RefrescarTodo(ej.CodigoBarras);
                _012IP_Ok(string.Format(_012IP_Texto("InvIngresoOk"), ej.Titulo, ej.CodigoBarras));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnUbicar_Click(object sender, EventArgs e)
        {
            _012IP_EjemplarBE ej = _012IP_EjemplarSeleccionado();
            if (ej == null)
            {
                _012IP_Error(_012IP_Texto("InvSeleccioneEjemplar"));
                return;
            }

            try
            {
                ejemplarBLL._012IP_UbicarEjemplar(ej.CodigoBarras, txtPasillo.Text, txtSeccion.Text, txtEstanteria.Text);
                _012IP_RefrescarTodo(ej.CodigoBarras);
                _012IP_Ok(_012IP_Texto("InvUbicacionOk"));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnCambiarEstado_Click(object sender, EventArgs e)
        {
            _012IP_EjemplarBE ej = _012IP_EjemplarSeleccionado();
            if (ej == null)
            {
                _012IP_Error(_012IP_Texto("InvSeleccioneEjemplar"));
                return;
            }

            string nuevo = cmbNuevoEstado.SelectedValue as string;
            if (nuevo == null)
            {
                _012IP_Error(_012IP_Texto("InvBLLEstadoInvalido"));
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                _012IP_Error(_012IP_Texto("InvBLLMotivo"));
                return;
            }

            var decision = MessageBox.Show(
                string.Format(_012IP_Texto("InvConfirmaEstado"), ej.CodigoBarras, _012IP_EjemplarBLL._012IP_NombreEstado(nuevo)),
                _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (decision != DialogResult.Yes) return;

            try
            {
                ejemplarBLL._012IP_CambiarEstado(ej.CodigoBarras, nuevo, txtMotivo.Text);
                txtMotivo.Clear();
                _012IP_RefrescarTodo(ej.CodigoBarras);
                _012IP_Ok(string.Format(_012IP_Texto("InvEstadoOk"), ej.CodigoBarras, _012IP_EjemplarBLL._012IP_NombreEstado(nuevo)));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnBaja_Click(object sender, EventArgs e)
        {
            _012IP_EjemplarBE ej = _012IP_EjemplarSeleccionado();
            if (ej == null)
            {
                _012IP_Error(_012IP_Texto("InvSeleccioneEjemplar"));
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                _012IP_Error(_012IP_Texto("InvBLLMotivo"));
                return;
            }

            var decision = MessageBox.Show(
                string.Format(_012IP_Texto("InvConfirmaBaja"), ej.CodigoBarras, ej.Titulo),
                _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (decision != DialogResult.Yes) return;

            try
            {
                ejemplarBLL._012IP_DarDeBaja(ej.CodigoBarras, txtMotivo.Text);
                txtMotivo.Clear();
                _012IP_RefrescarTodo(ej.CodigoBarras);
                _012IP_Ok(string.Format(_012IP_Texto("InvBajaOk"), ej.CodigoBarras));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnBuscar_Click(object sender, EventArgs e)
        {
            _012IP_RefrescarTodo(null);
        }

        private void _012IP_cmbFiltro_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _012IP_RefrescarTodo(null);
        }

        private void _012IP_txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _012IP_RefrescarTodo(null);
            }
        }

        // ------------------------------------------------------------------ serializacion XML

        private void _012IP_btnExaminarSer_Click(object sender, EventArgs e)
        {
            using (var dialogo = new SaveFileDialog())
            {
                dialogo.Filter = _012IP_Texto("InvXmlFiltro");
                dialogo.FileName = "Ejemplares.xml";
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    txtRutaSer.Text = dialogo.FileName;
            }
        }

        private void _012IP_btnExaminarDes_Click(object sender, EventArgs e)
        {
            using (var dialogo = new OpenFileDialog())
            {
                dialogo.Filter = _012IP_Texto("InvXmlFiltro");
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    txtRutaDes.Text = dialogo.FileName;
            }
        }

        private void _012IP_btnSerializar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaSer.Text))
            {
                _012IP_Error(_012IP_Texto("InvXmlSeleccioneRuta"));
                return;
            }

            try
            {
                // Los datos a serializar se toman directamente de la grilla.
                ejemplarBLL._012IP_SerializarEjemplares(ejemplaresMostrados, txtRutaSer.Text);
                _012IP_Ok(string.Format(_012IP_Texto("InvXmlSerializadoOk"), ejemplaresMostrados.Count, Path.GetFileName(txtRutaSer.Text)));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnDesserializar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutaDes.Text))
            {
                _012IP_Error(_012IP_Texto("InvXmlSeleccioneRuta"));
                return;
            }

            try
            {
                List<_012IP_EjemplarBE> cargados = ejemplarBLL._012IP_DesserializarEjemplares(txtRutaDes.Text);
                modoXml = true;
                _012IP_MostrarEnGrilla(cargados, null);
                _012IP_Ok(string.Format(_012IP_Texto("InvXmlDesserializadoOk"), cargados.Count));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnLimpiar_Click(object sender, EventArgs e)
        {
            // Solo limpia la grilla: no toca la base de datos ni los archivos XML.
            modoXml = true;
            _012IP_MostrarEnGrilla(new List<_012IP_EjemplarBE>(), null);
            txtRutaSer.Clear();
            txtRutaDes.Clear();
            _012IP_Info(_012IP_Texto("InvXmlLimpio"));
        }

        // ------------------------------------------------------------------ navegacion / barra de titulo

        private void _012IP_btnSalir_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Menu"] != null)
            {
                Application.OpenForms["Menu"].Show();
            }
            else
            {
                GUI.Menu menu = new GUI.Menu();
                menu.Show();
            }
            this.Close();
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