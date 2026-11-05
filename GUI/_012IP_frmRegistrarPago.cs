using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    public partial class _012IP_frmRegistrarPago : Form, IObserver
    {
        private _012IP_PagoBLL pagoBLL;

        private readonly int idSocio;
        private readonly int? idCuota;
        private readonly int? idMembresia;
        private readonly string nombreSocio;
        private readonly string dniSocio;
        private readonly DateTime? periodo;
        private readonly decimal importe;

       
        public _012IP_PagoBE PagoRegistrado { get; private set; }

        // Cuando es true, la pantalla solo valida y devuelve el medio de pago elegido;
        // el registro lo hace quien la invocó (por ejemplo, la renovación dentro de su transacción).
        public bool SoloCapturarMedioDePago { get; set; }
        public string MedioDePagoElegido { get; private set; }
        // Resultado de la validación de tarjeta (CUN-04): solo marca y últimos 4 dígitos.
        public string MarcaTarjeta { get; private set; }
        public string UltimosDigitos { get; private set; }

        private const int AltoTarjeta = 118;

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmRegistrarPago()
        {
            InitializeComponent();
        }

        
        public _012IP_frmRegistrarPago(int idSocio, string nombreSocio, string dniSocio,
            decimal importe, int? idCuota = null, int? idMembresia = null, DateTime? periodo = null) : this()
        {
            this.idSocio = idSocio;
            this.nombreSocio = nombreSocio;
            this.dniSocio = dniSocio;
            this.importe = importe;
            this.idCuota = idCuota;
            this.idMembresia = idMembresia;
            this.periodo = periodo;
        }

        

        private void _012IP_frmRegistrarPago_Load(object sender, EventArgs e)
        {
            try
            {
                pagoBLL = new _012IP_PagoBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }

            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);

            
            lblSocioValue.Text = $"{nombreSocio}  -  DNI {dniSocio}";
            lblPeriodoValue.Text = periodo.HasValue ? periodo.Value.ToString("MM/yyyy") : "-";
            lblImporteValue.Text = _012IP_Moneda(importe);

            if (cbMedioPago.Items.Count > 0)
                cbMedioPago.SelectedIndex = 0;

            lblMsj.ForeColor = Color.White;
            lblMsj.Text = _012IP_Texto("RegPagoIngreseDatos");
        }

        private void _012IP_frmRegistrarPago_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

    

        private void _012IP_btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                string medioDePago = cbMedioPago.SelectedItem as string ?? cbMedioPago.Text;

               
                if (!pagoBLL._012IP_ValidarDatosPago(idSocio, idCuota, importe, medioDePago))
                {
                    lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
                    lblMsj.Text = _012IP_Texto("RegPagoDatosInvalidos");
                    return;
                }

                MarcaTarjeta = null;
                UltimosDigitos = null;
                if (_012IP_PagoBLL._012IP_EsTarjeta(medioDePago))
                {
                    // CUN-04: validar la tarjeta antes de aceptar el pago
                    _012IP_TarjetaBE tarjeta = pagoBLL._012IP_ValidarTarjeta(txtNumeroTarjeta.Text, txtVencimientoTarjeta.Text, txtCVV.Text);
                    txtCVV.Clear();
                    if (!tarjeta.Valida)
                    {
                        lblMarcaValue.Text = string.Empty;
                        lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
                        lblMsj.Text = _012IP_Texto(tarjeta.Motivo);
                        return;
                    }
                    MarcaTarjeta = tarjeta.Marca;
                    UltimosDigitos = tarjeta.UltimosDigitos;
                    lblMarcaValue.Text = tarjeta.Marca + "  **** " + tarjeta.UltimosDigitos;
                    txtNumeroTarjeta.Clear();
                }

                if (SoloCapturarMedioDePago)
                {
                    MedioDePagoElegido = medioDePago;
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                _012IP_PagoBE pago = pagoBLL._012IP_RegistrarPago(idSocio, idCuota, idMembresia, importe, medioDePago, MarcaTarjeta, UltimosDigitos);

                PagoRegistrado = pago;

               
                MessageBox.Show(
                    string.Format(_012IP_Texto("RegPagoExitoso"), _012IP_Moneda(pago.Monto)),
                    _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
                lblMsj.Text = ex.Message;
            }
        }

        // Muestra los datos de la tarjeta solo cuando el medio de pago es tarjeta y agranda el formulario.
        private void _012IP_cbMedioPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            string medio = cbMedioPago.SelectedItem as string ?? cbMedioPago.Text;
            bool esTarjeta = _012IP_PagoBLL._012IP_EsTarjeta(medio);
            if (grpTarjeta.Visible == esTarjeta) return;

            int delta = esTarjeta ? AltoTarjeta : -AltoTarjeta;
            grpTarjeta.Visible = esTarjeta;
            lblMsj.Top += delta;
            btnConfirmar.Top += delta;
            btnCancelar.Top += delta;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + delta);
            lblMarcaValue.Text = string.Empty;
        }

        private void _012IP_btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private string _012IP_Moneda(decimal monto)
        {
            return "$" + monto.ToString("N0");
        }

        private void _012IP_MostrarError(Exception ex)
        {
            MessageBox.Show(ex.Message, _012IP_Texto("RenMemError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("RegPagoTitulo");
            grpDatos.Text = _012IP_Texto("RegPagoGrpDatos");
            lblSocio.Text = _012IP_Texto("RegPagoSocio");
            lblPeriodo.Text = _012IP_Texto("RenMemPeriodo");
            lblImporte.Text = _012IP_Texto("RenMemColImporte");
            lblMedioPago.Text = _012IP_Texto("RegPagoMedioPago");
            grpTarjeta.Text = _012IP_Texto("TarjetaGrpDatos");
            lblNumeroTarjeta.Text = _012IP_Texto("TarjetaNumero");
            lblVencimientoTarjeta.Text = _012IP_Texto("TarjetaVencimiento");
            lblCVV.Text = _012IP_Texto("TarjetaCVV");
            btnConfirmar.Text = _012IP_Texto("RegPagoBtnConfirmar");
            btnCancelar.Text = _012IP_Texto("btnCancelar");
        }

      

        private void _012IP_btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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