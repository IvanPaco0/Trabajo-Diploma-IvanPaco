using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    /// <summary>
    /// GUI-RegistrarSocio (CU-02). Se abre como diálogo modal desde donde haga falta
    /// dar de alta un socio (p. ej. CU-01 Renovar membresía, cuando "Socio no encontrado").
    /// Toda la validación (ValidarFormato, ValidarDNIUnico, RegistrarNuevoSocio + Bitácora)
    /// ya vive en _012IP_SocioBLL; esta GUI solo llama al método y muestra el resultado,
    /// tal como está modelado en el diagrama de secuencia N02.1.2.2.
    /// </summary>
    public partial class _012IP_frmRegistrarSocio : Form, IObserver
    {
        private _012IP_SocioBLL socioBLL;

        /// <summary>Socio efectivamente registrado (ok() del diagrama). Null si se canceló.</summary>
        public _012IP_SocioBE SocioRegistrado { get; private set; }

        private readonly string dniInicial;
        private readonly string nombreInicial;
        private readonly string apellidoInicial;

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmRegistrarSocio()
        {
            InitializeComponent();
        }

        /// <summary>Permite precargar lo que el recepcionista ya había tipeado al buscar el socio.</summary>
        public _012IP_frmRegistrarSocio(string dni, string nombre, string apellido) : this()
        {
            dniInicial = dni;
            nombreInicial = nombre;
            apellidoInicial = apellido;
        }

        // ------------------------------------------------------------------ carga / cierre

        private void _012IP_frmRegistrarSocio_Load(object sender, EventArgs e)
        {
            try
            {
                socioBLL = new _012IP_SocioBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }

            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);

            if (!string.IsNullOrWhiteSpace(dniInicial) && dniInicial.Trim().Length > 0 && System.Linq.Enumerable.All(dniInicial.Trim(), char.IsDigit))
                txtDNI.Text = dniInicial.Trim();
            txtNombre.Text = nombreInicial?.Trim();
            txtApellido.Text = apellidoInicial?.Trim();

            lblMsj.ForeColor = Color.White;
            lblMsj.Text = _012IP_Texto("RegSocioIngreseDatos");
        }

        private void _012IP_frmRegistrarSocio_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

        // ------------------------------------------------------------------ CU-02: registrar

        private void _012IP_btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // ValidarFormato + ValidarDNIUnico + RegistrarNuevoSocio (calcula DVH y registra
                // el evento en Bitácora) ocurren dentro del BLL. Si algo falla, tira una excepción
                // con el mensaje correspondiente al bloque "alt" del diagrama:
                //  - FormatoInvalido()          -> SocioBLLText6
                //  - "Ya existe un socio..."    -> SocioBLLText7
                //  - InformeError()             -> SocioBLLText8
                _012IP_SocioBE socio = socioBLL._012IP_RegistrarNuevoSocio(
                    txtDNI.Text, txtNombre.Text, txtApellido.Text, txtEmail.Text, txtTelefono.Text);

                SocioRegistrado = socio;

                // Registro exitoso(idSocio, DNI, Nombre, Apellido, Email, Telefono, EstadoCuenta)
                MessageBox.Show(
                    string.Format(_012IP_Texto("RegSocioExitoso"), socio.Nombre, socio.Apellido, socio.DNI),
                    _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                // Se queda en el formulario ("loop datos válidos"): el recepcionista corrige
                // los datos y puede volver a presionar "Registrar socio".
                lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
                lblMsj.Text = ex.Message;
            }
        }

        private void _012IP_btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // ------------------------------------------------------------------ auxiliares

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private void _012IP_MostrarError(Exception ex)
        {
            MessageBox.Show(ex.Message, _012IP_Texto("RenMemError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ------------------------------------------------------------------ idioma (patrón Observer)

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("RegSocioTitulo");
            grpDatos.Text = _012IP_Texto("RegSocioGrpDatos");
            lblDNI.Text = _012IP_Texto("DNI");
            lblNombre.Text = _012IP_Texto("lblNombre");
            lblApellido.Text = _012IP_Texto("lblApe");
            lblEmail.Text = _012IP_Texto("lblEmail");
            lblTelefono.Text = _012IP_Texto("RenMemTelefono");
            btnRegistrar.Text = _012IP_Texto("RegSocioBtnRegistrar");
            btnCancelar.Text = _012IP_Texto("btnCancelar");
        }

        // ------------------------------------------------------------------ barra de título

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
