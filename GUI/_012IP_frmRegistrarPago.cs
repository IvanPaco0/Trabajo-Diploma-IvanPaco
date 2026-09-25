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

               
                _012IP_PagoBE pago = pagoBLL._012IP_RegistrarPago(idSocio, idCuota, idMembresia, importe, medioDePago);

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