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

        // Las BLL se crean en el Load (no en el constructor) para que el diseñador de Visual Studio
        // no intente abrir la conexión a la base al mostrar el formulario.
        private _012IP_SocioBLL socioBLL;
        private _012IP_MembresiaBLL membresiaBLL;
        private _012IP_SuscripcionBLL suscripcionBLL;

        private List<_012IP_SocioBE> sociosEncontrados = new List<_012IP_SocioBE>();
        private List<_012IP_MembresiaBE> membresias;
        private List<_012IP_CuotaBE> cuotasVencidas = new List<_012IP_CuotaBE>();
        private _012IP_SocioBE socioSeleccionado;

        private bool deudaRegularizada;
        private bool actualizandoGrilla;
        private bool cargandoCombo;
        private DateTime periodoInicio;
        private DateTime periodoVencimiento;

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmRenovarMembresia()
        {
            InitializeComponent();
        }

        // ------------------------------------------------------------------ carga / cierre

        private void _012IP_frmRenovarMembresia_Load(object sender, EventArgs e)
        {
            try
            {
                socioBLL = new _012IP_SocioBLL();
                membresiaBLL = new _012IP_MembresiaBLL();
                suscripcionBLL = new _012IP_SuscripcionBLL();
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
            lblMsj.Text = _012IP_Texto("RenMemSeleccione");
        }

        private void _012IP_frmRenovarMembresia_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

        // ------------------------------------------------------------------ paso 1-3: buscar socio

        private void _012IP_btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                _012IP_LimpiarEstado();

                List<_012IP_SocioBE> socios = socioBLL._012IP_BuscarSocio(txtDNI.Text, txtNombre.Text, txtApellido.Text);

                if (socios.Count == 0)
                {
                    // Flujo alternativo: socio no encontrado -> se ofrece CU-02 Registrar socio
                    _012IP_MostrarSocios(socios, null);
                    btnRegistrarSocio.Visible = true;
                    lblMsj.Text = _012IP_Texto("RenMemNoEncontrado");
                    return;
                }

                _012IP_MostrarSocios(socios, socios.Count == 1 ? socios[0].DNI : null);

                if (socios.Count == 1)
                    _012IP_SeleccionarSocio(socios[0].DNI);
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
            // TODO CU-02: abrir el formulario "Registrar socio" (ShowDialog) y, al volver,
            // repetir la búsqueda con _012IP_btnBuscar_Click(null, null).
            MessageBox.Show("[PENDIENTE] Acá se invoca el CU-02 Registrar socio.", "CU-02",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ------------------------------------------------------------------ paso 4: estado de cuenta

        private void _012IP_SeleccionarSocio(string dni)
        {
            try
            {
                socioSeleccionado = sociosEncontrados.FirstOrDefault(s => s.DNI == dni);
                if (socioSeleccionado == null) return;

                deudaRegularizada = false;
                cuotasVencidas = socioBLL._012IP_ValidarEstadoCuenta(dni);
                _012IP_MostrarEstadoCuenta();

                // Paso 6: tipos de membresía disponibles con su costo mensual
                if (membresias == null)
                    _012IP_CargarMembresias();

                _012IP_PreseleccionarMembresia();
                _012IP_CalcularPeriodo();
                grpRenovacion.Enabled = true;
                lblMsj.Text = string.Empty;
            }
            catch (Exception ex)
            {
                _012IP_LimpiarEstado();
                _012IP_MostrarError(ex);
            }
        }

        private void _012IP_MostrarEstadoCuenta()
        {
            if (cuotasVencidas.Count == 0)
            {
                lblEstadoCuenta.ForeColor = Color.LightGreen;
                lblEstadoCuenta.Text = _012IP_Texto("RenMemAlDia");
                pnlDeuda.Visible = false;
                return;
            }

            decimal total = cuotasVencidas.Sum(c => c.Importe);
            lblEstadoCuenta.ForeColor = Color.FromArgb(255, 156, 156);
            lblEstadoCuenta.Text = string.Format(_012IP_Texto("RenMemDebe"), cuotasVencidas.Count, _012IP_Moneda(total));

            dgvCuotasImpagas.DataSource = cuotasVencidas.Select(c => new
            {
                Periodo = c.Periodo.ToString("MM/yyyy"),
                Importe = _012IP_Moneda(c.Importe),
                Vencimiento = c.FechaVencimiento.ToString("dd/MM/yyyy")
            }).ToList();
            _012IP_AplicarEncabezados();
            dgvCuotasImpagas.ClearSelection();
            pnlDeuda.Visible = true;
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

                // Diagrama: RegistrarPago() -> CU-03 -> PagoConfirmado()
                if (!_012IP_InvocarRegistrarPago(socioSeleccionado, total))
                {
                    lblMsj.Text = _012IP_Texto("RenMemPagoNoConfirmado");
                    return;
                }

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

        // ------------------------------------------------------------------ pasos 5-13: renovar

        private void _012IP_cbMembresias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoCombo) return;
            _012IP_ActualizarCosto();
        }

        private void _012IP_btnRenovar_Click(object sender, EventArgs e)
        {
            if (socioSeleccionado == null) return;

            try
            {
                if (cuotasVencidas.Count > 0 && !deudaRegularizada)
                {
                    lblMsj.Text = _012IP_Texto("RenMemDebeRegularizar");
                    return;
                }

                _012IP_MembresiaBE membresia = _012IP_MembresiaElegida();
                if (membresia == null)
                {
                    lblMsj.Text = _012IP_Texto("RenMemSeleccioneTipo");
                    return;
                }

                _012IP_CalcularPeriodo();

                // Paso 8: ConfirmaRenovacion()
                DialogResult confirma = MessageBox.Show(
                    string.Format(_012IP_Texto("RenMemConfirmaRenovar"),
                        membresia.Nombre, _012IP_Moneda(membresia.CostoMensual),
                        periodoInicio.ToString("dd/MM/yyyy"), periodoVencimiento.ToString("dd/MM/yyyy")),
                    _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirma != DialogResult.Yes) return;

                // Paso 9: CU-03 Registrar pago (tipo de membresía y costo mensual)
                if (!_012IP_InvocarRegistrarPago(socioSeleccionado, membresia.CostoMensual))
                {
                    lblMsj.Text = _012IP_Texto("RenMemPagoNoConfirmado");
                    return;
                }

                // Paso 10: actualizar la suscripción del socio
                suscripcionBLL._012IP_RenovarSuscripcion(socioSeleccionado.DNI, membresia.IdMembresia, periodoInicio, periodoVencimiento);

                // Paso 11: consultar la suscripción ya actualizada
                _012IP_SuscripcionBE actualizada = suscripcionBLL._012IP_ConsultarSuscripcion(socioSeleccionado.DNI);
                socioSeleccionado.Suscripcion = actualizada;

                // Paso 12-13: mostrar el estado actualizado e informar el nuevo período
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

        // ------------------------------------------------------------------ CU-03 (pendiente)

        /// <summary>
        /// Punto de integración con el CU-03 Registrar pago.
        /// Devuelve true si el pago quedó confirmado.
        /// TODO CU-03: reemplazar el cuerpo por la invocación real del caso de uso.
        /// </summary>
        private bool _012IP_InvocarRegistrarPago(_012IP_SocioBE socio, decimal monto)
        {
            MessageBox.Show(
                "[SIMULADO] Pago de " + _012IP_Moneda(monto) + " registrado para " + socio.Nombre + " " + socio.Apellido +
                ".\nReemplazar por el CU-03 Registrar pago.",
                "CU-03", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        // ------------------------------------------------------------------ auxiliares

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
        }

        private void _012IP_CalcularPeriodo()
        {
            if (socioSeleccionado == null) return;

            suscripcionBLL._012IP_CalcularPeriodoRenovacion(socioSeleccionado.Suscripcion, out periodoInicio, out periodoVencimiento);
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
            btnRegistrarSocio.Visible = false;
            lblCostoValor.Text = string.Empty;
            lblPeriodoValor.Text = string.Empty;
            grpRenovacion.Enabled = false;
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
            MessageBox.Show(ex.Message, _012IP_Texto("RenMemError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ------------------------------------------------------------------ idioma (patrón Observer)

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("RenMemTitulo");
            grpBusqueda.Text = _012IP_Texto("RenMemGrpBusqueda");
            lblDNI.Text = _012IP_Texto("DNI");
            lblNombre.Text = _012IP_Texto("lblNombre");
            lblApellido.Text = _012IP_Texto("lblApe");
            btnBuscar.Text = _012IP_Texto("RenMemBtnBuscar");
            btnRegistrarSocio.Text = _012IP_Texto("RenMemBtnRegistrarSocio");
            grpEstadoCuenta.Text = _012IP_Texto("RenMemGrpEstado");
            btnRegularizarDeuda.Text = _012IP_Texto("RenMemBtnRegularizar");
            grpRenovacion.Text = _012IP_Texto("RenMemGrpRenovacion");
            lblTipoMembresia.Text = _012IP_Texto("RenMemTipo");
            lblCosto.Text = _012IP_Texto("RenMemCosto");
            lblPeriodo.Text = _012IP_Texto("RenMemPeriodo");
            btnRenovar.Text = _012IP_Texto("RenMemBtnRenovar");
            btnSalir.Text = _012IP_Texto("btnSalir");
            _012IP_AplicarEncabezados();
        }

        // ------------------------------------------------------------------ barra de título / salir

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
