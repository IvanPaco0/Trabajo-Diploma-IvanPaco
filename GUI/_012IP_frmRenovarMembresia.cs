using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace _012IP_GUI
{
    public partial class _012IP_frmRenovarMembresia : Form, IObserver
    {
        private static readonly CultureInfo culturaAR = new CultureInfo("es-AR");

        
        private _012IP_SocioBLL socioBLL;
        private _012IP_MembresiaBLL membresiaBLL;
        private _012IP_SuscripcionBLL suscripcionBLL;
        private _012IP_PagoBLL pagoBLL;

        private List<_012IP_SocioBE> sociosEncontrados = new List<_012IP_SocioBE>();
        private List<_012IP_MembresiaBE> membresias;
        private List<_012IP_CuotaBE> cuotasVencidas = new List<_012IP_CuotaBE>();
        private _012IP_SocioBE socioSeleccionado;

        private bool deudaRegularizada;
        private bool actualizandoGrilla;
        private bool cargandoCombo;
        private DateTime periodoInicio;
        private DateTime periodoVencimiento;

        private readonly System.Windows.Forms.Timer timerBusqueda = new System.Windows.Forms.Timer { Interval = 300 };

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmRenovarMembresia()
        {
            InitializeComponent();
            timerBusqueda.Tick += _012IP_timerBusqueda_Tick;
        }

      

        private void _012IP_frmRenovarMembresia_Load(object sender, EventArgs e)
        {
            try
            {
                socioBLL = new _012IP_SocioBLL();
                membresiaBLL = new _012IP_MembresiaBLL();
                suscripcionBLL = new _012IP_SuscripcionBLL();
                pagoBLL = new _012IP_PagoBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }

            LanguageManager.Instance.AgregarObservador(this);
            _012IP_ConfigurarGrilla(dgvSocios);
            _012IP_ConfigurarGrilla(dgvCuotasImpagas);
            _012IP_LimpiarEstado();
            Actualizar(LanguageManager.Instance);
            // Al abrir se listan los socios activos; despues se va filtrando a medida que se escribe.
            if (socioBLL != null) _012IP_BuscarSocios();
            ActiveControl = txtBuscar;
        }

        private void _012IP_frmRenovarMembresia_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
            timerBusqueda.Stop();
            timerBusqueda.Dispose();
        }

       

        // Busqueda dinamica: se dispara con el evento TextChanged del unico cuadro de busqueda.
        // El timer espera un instante a que el usuario deje de tipear para no consultar la base por cada tecla.
        private void _012IP_txtBuscar_TextChanged(object sender, EventArgs e)
        {
            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void _012IP_txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            timerBusqueda.Stop();
            _012IP_BuscarSocios();
        }

        private void _012IP_timerBusqueda_Tick(object sender, EventArgs e)
        {
            timerBusqueda.Stop();
            _012IP_BuscarSocios();
        }

        private void _012IP_BuscarSocios()
        {
            try
            {
                string dniActual = socioSeleccionado?.DNI;
                _012IP_LimpiarEstado();

                List<_012IP_SocioBE> socios = socioBLL._012IP_BuscarSocioPorTexto(txtBuscar.Text);

                if (socios.Count == 0)
                {
                    _012IP_MostrarSocios(socios, null);
                    lblMsj.Text = txtBuscar.Text.Trim() == "" ? string.Empty : _012IP_Texto("RenMemNoEncontrado");
                    return;
                }

                // Si quedo un solo socio, o el que estaba elegido sigue en la lista, se selecciona solo.
                string dniASeleccionar = socios.Count == 1 ? socios[0].DNI
                    : socios.Any(x => x.DNI == dniActual) ? dniActual : null;

                _012IP_MostrarSocios(socios, dniASeleccionar);

                if (dniASeleccionar != null)
                    _012IP_SeleccionarSocio(dniASeleccionar);
                else
                    lblMsj.Text = _012IP_Texto("RenMemSeleccione");
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

        private void _012IP_dgvSocios_SelectionChanged(object sender, EventArgs e)
        {
            if (actualizandoGrilla || dgvSocios.CurrentRow == null || !dgvSocios.CurrentRow.Selected)
                return;

            string dni = Convert.ToString(dgvSocios.CurrentRow.Cells["DNI"].Value);
            if (socioSeleccionado != null && socioSeleccionado.DNI == dni)
                return;

            _012IP_SeleccionarSocio(dni);
        }

        private void _012IP_btnRegistrarSocio_Click(object sender, EventArgs e)
        {
            // Lo que se escribio en la busqueda se pasa al alta: numeros -> DNI, texto -> nombre / apellido.
            string texto = txtBuscar.Text.Trim();
            string dni = "", nombre = "", apellido = "";
            if (texto.Length > 0 && texto.All(char.IsDigit))
                dni = texto;
            else if (texto.Length > 0)
            {
                string[] partes = texto.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                nombre = partes[0];
                apellido = partes.Length > 1 ? partes[1] : "";
            }

            using (var frmRegistrarSocio = new _012IP_frmRegistrarSocio(dni, nombre, apellido))
            {
                if (frmRegistrarSocio.ShowDialog(this) == DialogResult.OK && frmRegistrarSocio.SocioRegistrado != null)
                {
                    txtBuscar.Text = frmRegistrarSocio.SocioRegistrado.DNI;
                    timerBusqueda.Stop();
                    _012IP_BuscarSocios();
                }
            }
        }

        // Corrige los datos del socio seleccionado (email, teléfono, etc.). El cambio queda
        // registrado en la bitácora de cambios de Socio (trigger de la base).
        private void _012IP_btnModificarSocio_Click(object sender, EventArgs e)
        {
            if (socioSeleccionado == null)
            {
                lblMsj.ForeColor = Color.White;
                lblMsj.Text = _012IP_Texto("RenMemSeleccione");
                return;
            }
            using (var frm = new _012IP_frmRegistrarSocio(socioSeleccionado))
            {
                if (frm.ShowDialog(this) == DialogResult.OK && frm.SocioRegistrado != null)
                {
                    txtBuscar.Text = frm.SocioRegistrado.DNI;
                    timerBusqueda.Stop();
                    socioSeleccionado = null;
                    _012IP_BuscarSocios();
                }
            }
        }

        private void _012IP_SeleccionarSocio(string dni)
        {
            try
            {
                socioSeleccionado = sociosEncontrados.FirstOrDefault(s => s.DNI == dni);
                if (socioSeleccionado == null) return;

                // Membresia prepaga: no hay deuda, solo vigencia.
                deudaRegularizada = false;
                cuotasVencidas = new List<_012IP_CuotaBE>();
                _012IP_MostrarEstadoCuenta();

                
                if (membresias == null)
                    _012IP_CargarMembresias();

                _012IP_PreseleccionarMembresia();
                _012IP_CalcularPeriodo();
                _012IP_HabilitarRenovacion(true);
                lblMsj.Text = string.Empty;
            }
            catch (Exception ex)
            {
                _012IP_LimpiarEstado();
                _012IP_MostrarError(ex);
            }
        }

        // Muestra hasta cuándo tiene la membresía el socio y si está vigente, por vencer o vencida.
        private void _012IP_MostrarEstadoCuenta()
        {
            pnlDeuda.Visible = false;
            _012IP_SuscripcionBE suscripcion = socioSeleccionado?.Suscripcion;

            if (suscripcion == null)
            {
                lblEstadoCuenta.ForeColor = Color.FromArgb(255, 156, 156);
                lblEstadoCuenta.Text = _012IP_Texto("RenMemSinMembresia");
                return;
            }

            string vence = suscripcion.FechaVencimiento.ToString("dd/MM/yyyy");
            if (!suscripcion.EstaVigente)
            {
                lblEstadoCuenta.ForeColor = Color.FromArgb(255, 156, 156);
                lblEstadoCuenta.Text = string.Format(_012IP_Texto("RenMemVencida"), vence);
            }
            else if (suscripcion.DiasRestantes <= _012IP_SuscripcionBLL.DiasAvisoVencimiento)
            {
                lblEstadoCuenta.ForeColor = Color.Khaki;
                lblEstadoCuenta.Text = string.Format(_012IP_Texto("RenMemPorVencer"), vence, suscripcion.DiasRestantes);
            }
            else
            {
                lblEstadoCuenta.ForeColor = Color.LightGreen;
                lblEstadoCuenta.Text = string.Format(_012IP_Texto("RenMemVigente"), vence, suscripcion.DiasRestantes);
            }
        }

        private void _012IP_btnRegularizarDeuda_Click(object sender, EventArgs e)
        {
            if (socioSeleccionado == null || cuotasVencidas.Count == 0) return;

            try
            {
                decimal total = cuotasVencidas.Sum(c => c.Importe);
                DialogResult confirma = MessageBox.Show(
                    string.Format(_012IP_Texto("RenMemConfirmaDeuda"), _012IP_Moneda(total)),
                    _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirma != DialogResult.Yes) return;

                _012IP_PagoBE pago = _012IP_InvocarRegistrarPago(socioSeleccionado, total, null, null, null);
                if (pago == null)
                {
                    lblMsj.Text = _012IP_Texto("RenMemPagoNoConfirmado");
                    return;
                }

                pagoBLL._012IP_MarcarCuotasComoPagadas(cuotasVencidas.Select(c => c.IdCuota).ToList(), pago.FechaPago);

                deudaRegularizada = true;
                pnlDeuda.Visible = false;
                lblEstadoCuenta.ForeColor = Color.LightGreen;
                lblEstadoCuenta.Text = _012IP_Texto("RenMemDeudaRegularizada");
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

     

        private void _012IP_cbMembresias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoCombo) return;
            _012IP_ActualizarCosto();
            _012IP_CalcularPeriodo();
        }

        private void _012IP_nudMeses_ValueChanged(object sender, EventArgs e)
        {
            _012IP_ActualizarCosto();
            _012IP_CalcularPeriodo();
        }

        private int _012IP_MesesElegidos() => (int)nudMeses.Value;

        private void _012IP_btnRenovar_Click(object sender, EventArgs e)
        {
            if (socioSeleccionado == null) return;

            try
            {
                _012IP_MembresiaBE membresia = _012IP_MembresiaElegida();
                if (membresia == null)
                {
                    lblMsj.Text = _012IP_Texto("RenMemSeleccioneTipo");
                    return;
                }

                _012IP_CalcularPeriodo();
                int meses = _012IP_MesesElegidos();
                decimal total = suscripcionBLL._012IP_CalcularImporte(membresia, meses);

                DialogResult confirma = MessageBox.Show(
                    string.Format(_012IP_Texto("RenMemConfirmaRenovar"),
                        membresia.Nombre, meses, _012IP_Moneda(total),
                        periodoInicio.ToString("dd/MM/yyyy"), periodoVencimiento.ToString("dd/MM/yyyy")),
                    _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirma != DialogResult.Yes) return;


                string marcaTarjeta, ultimosDigitos;
                string medioDePago = _012IP_SolicitarMedioDePago(socioSeleccionado, membresia, total, periodoInicio, out marcaTarjeta, out ultimosDigitos);
                if (medioDePago == null)
                {
                    lblMsj.Text = _012IP_Texto("RenMemPagoNoConfirmado");
                    return;
                }

                // Registra el pago y extiende la vigencia en una sola transaccion.
                suscripcionBLL._012IP_RenovarSuscripcion(socioSeleccionado, membresia, meses, periodoInicio, periodoVencimiento, medioDePago, marcaTarjeta, ultimosDigitos);

                _012IP_SuscripcionBE actualizada = suscripcionBLL._012IP_ConsultarSuscripcion(socioSeleccionado.DNI);
                socioSeleccionado.Suscripcion = actualizada;
                _012IP_MostrarEstadoCuenta();

                
                string dni = socioSeleccionado.DNI;
                _012IP_MostrarSocios(sociosEncontrados, dni);
                _012IP_CalcularPeriodo();

                lblMsj.Text = string.Format(_012IP_Texto("RenMemRenovada"),
                    actualizada.FechaInicio.ToString("dd/MM/yyyy"),
                    actualizada.FechaVencimiento.ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }


        private _012IP_PagoBE _012IP_InvocarRegistrarPago(_012IP_SocioBE socio, decimal monto, int? idCuota, int? idMembresia, DateTime? periodo)
        {
            using (var frmPago = new _012IP_frmRegistrarPago(
                socio.IdSocio, socio.Nombre + " " + socio.Apellido, socio.DNI,
                monto, idCuota, idMembresia, periodo))
            {
                DialogResult resultado = frmPago.ShowDialog(this);
                return resultado == DialogResult.OK ? frmPago.PagoRegistrado : null;
            }
        }



        private string _012IP_SolicitarMedioDePago(_012IP_SocioBE socio, _012IP_MembresiaBE membresia, decimal total, DateTime periodo,
            out string marcaTarjeta, out string ultimosDigitos)
        {
            marcaTarjeta = null;
            ultimosDigitos = null;
            using (var frmPago = new _012IP_frmRegistrarPago(
                socio.IdSocio, socio.Nombre + " " + socio.Apellido, socio.DNI,
                total, null, membresia.IdMembresia, periodo))
            {
                frmPago.SoloCapturarMedioDePago = true;
                DialogResult resultado = frmPago.ShowDialog(this);
                if (resultado != DialogResult.OK) return null;
                marcaTarjeta = frmPago.MarcaTarjeta;
                ultimosDigitos = frmPago.UltimosDigitos;
                return frmPago.MedioDePagoElegido;
            }
        }

        private void _012IP_CargarMembresias()
        {
            cargandoCombo = true;
            try
            {
                membresias = membresiaBLL._012IP_ListarMembresias();
                cbMembresias.DataSource = membresias.Select(m => new
                {
                    Id = m.IdMembresia,
                    Texto = m.Nombre + " - " + _012IP_Moneda(m.CostoMensual)
                }).ToList();
                cbMembresias.DisplayMember = "Texto";
                cbMembresias.ValueMember = "Id";
            }
            finally
            {
                cargandoCombo = false;
            }
        }

        private void _012IP_PreseleccionarMembresia()
        {
            if (socioSeleccionado?.Suscripcion != null && membresias != null &&
                membresias.Any(m => m.IdMembresia == socioSeleccionado.Suscripcion.IdMembresia))
            {
                cbMembresias.SelectedValue = socioSeleccionado.Suscripcion.IdMembresia;
            }
            _012IP_ActualizarCosto();
        }

        private _012IP_MembresiaBE _012IP_MembresiaElegida()
        {
            if (membresias == null || !(cbMembresias.SelectedValue is int)) return null;
            int id = (int)cbMembresias.SelectedValue;
            return membresias.FirstOrDefault(m => m.IdMembresia == id);
        }

        private void _012IP_ActualizarCosto()
        {
            _012IP_MembresiaBE membresia = _012IP_MembresiaElegida();
            lblCostoValor.Text = membresia == null ? string.Empty : _012IP_Moneda(membresia.CostoMensual);
            lblTotalValor.Text = membresia == null ? string.Empty
                : _012IP_Moneda(suscripcionBLL._012IP_CalcularImporte(membresia, _012IP_MesesElegidos()));
        }

        private void _012IP_CalcularPeriodo()
        {
            if (socioSeleccionado == null) return;

            suscripcionBLL._012IP_CalcularPeriodoRenovacion(socioSeleccionado.Suscripcion, _012IP_MesesElegidos(), out periodoInicio, out periodoVencimiento);
            lblPeriodoValor.Text = periodoInicio.ToString("dd/MM/yyyy") + " - " + periodoVencimiento.ToString("dd/MM/yyyy");
        }

        private void _012IP_MostrarSocios(List<_012IP_SocioBE> socios, string dniASeleccionar)
        {
            actualizandoGrilla = true;
            try
            {
                sociosEncontrados = socios;
                dgvSocios.DataSource = socios.Select(s => new
                {
                    DNI = s.DNI,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    Email = s.Email,
                    Telefono = s.Telefono,
                    Membresia = s.Suscripcion?.Membresia?.Nombre ?? "-",
                    Vence = s.Suscripcion != null ? s.Suscripcion.FechaVencimiento.ToString("dd/MM/yyyy") : "-"
                }).ToList();

                _012IP_AplicarEncabezados();
                dgvSocios.ClearSelection();

                if (dniASeleccionar != null)
                {
                    foreach (DataGridViewRow fila in dgvSocios.Rows)
                    {
                        if (Convert.ToString(fila.Cells["DNI"].Value) == dniASeleccionar)
                        {
                            dgvSocios.CurrentCell = fila.Cells[0];
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
        }

        private void _012IP_LimpiarEstado()
        {
            socioSeleccionado = null;
            cuotasVencidas = new List<_012IP_CuotaBE>();
            deudaRegularizada = false;
            lblEstadoCuenta.Text = string.Empty;
            pnlDeuda.Visible = false;
            lblCostoValor.Text = string.Empty;
            lblTotalValor.Text = string.Empty;
            lblPeriodoValor.Text = string.Empty;
            nudMeses.Value = 1;
            _012IP_HabilitarRenovacion(false);
        }

        // No se deshabilita el grupo entero porque los textos quedan grises e ilegibles:
        // solo se bloquean los controles que el usuario puede tocar.
        private void _012IP_HabilitarRenovacion(bool habilitar)
        {
            foreach (Control c in grpRenovacion.Controls)
                if (!(c is Label))
                    c.Enabled = habilitar;
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
            _012IP_Encabezado(dgvSocios, "DNI", "DNI");
            _012IP_Encabezado(dgvSocios, "Nombre", "lblNombre");
            _012IP_Encabezado(dgvSocios, "Apellido", "lblApe");
            _012IP_Encabezado(dgvSocios, "Email", "lblEmail");
            _012IP_Encabezado(dgvSocios, "Telefono", "RenMemTelefono");
            _012IP_Encabezado(dgvSocios, "Membresia", "RenMemMembresia");
            _012IP_Encabezado(dgvSocios, "Vence", "RenMemVence");

            _012IP_Encabezado(dgvCuotasImpagas, "Periodo", "RenMemColPeriodo");
            _012IP_Encabezado(dgvCuotasImpagas, "Importe", "RenMemColImporte");
            _012IP_Encabezado(dgvCuotasImpagas, "Vencimiento", "RenMemVence");
        }

        private void _012IP_Encabezado(DataGridView grilla, string columna, string clave)
        {
            if (grilla.Columns.Contains(columna))
                grilla.Columns[columna].HeaderText = _012IP_Texto(clave);
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private string _012IP_Moneda(decimal monto)
        {
            return "$" + monto.ToString("N0", culturaAR);
        }

        private void _012IP_MostrarError(Exception ex)
        {
            // Se muestra tambien la causa original (la excepcion mas interna) para saber que fallo.
            string mensaje = ex.Message;
            Exception causa = ex;
            while (causa.InnerException != null) causa = causa.InnerException;
            if (causa != ex && causa.Message != ex.Message)
                mensaje += Environment.NewLine + Environment.NewLine + _012IP_Texto("RenMemDetalle") + " " + causa.Message;
            MessageBox.Show(mensaje, _012IP_Texto("RenMemError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("RenMemTitulo");
            grpBusqueda.Text = _012IP_Texto("RenMemGrpBusqueda");
            lblBuscar.Text = _012IP_Texto("RenMemBuscarAyuda");
            btnRegistrarSocio.Text = _012IP_Texto("RenMemBtnRegistrarSocio");
            btnModificarSocio.Text = _012IP_Texto("RenMemBtnModificarSocio");
            grpEstadoCuenta.Text = _012IP_Texto("RenMemGrpEstado");
            btnRegularizarDeuda.Text = _012IP_Texto("RenMemBtnRegularizar");
            grpRenovacion.Text = _012IP_Texto("RenMemGrpRenovacion");
            lblTipoMembresia.Text = _012IP_Texto("RenMemTipo");
            lblCosto.Text = _012IP_Texto("RenMemCosto");
            lblPeriodo.Text = _012IP_Texto("RenMemPeriodo");
            lblMeses.Text = _012IP_Texto("RenMemMeses");
            lblTotal.Text = _012IP_Texto("RenMemTotal");
            btnRenovar.Text = _012IP_Texto("RenMemBtnRenovar");
            btnSalir.Text = _012IP_Texto("btnSalir");
            _012IP_AplicarEncabezados();
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
            {
                this.Location = new Point(this.Location.X + (e.X - posX), this.Location.Y + (e.Y - posY));
            }
        }

        private void _012IP_BarraTitulo_MouseUp(object sender, MouseEventArgs e)
        {
            arrastrando = false;
        }
    }
}
