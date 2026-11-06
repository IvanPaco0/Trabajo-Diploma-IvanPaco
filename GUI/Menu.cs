using _012IP_GUI;
using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Menu : Form, IObserver
    {
        int posX, posY;
        bool arrastrando = false;
        public Menu()
        {
            InitializeComponent();
            customMenu();
        }
        // Submenús del menú lateral (solo uno abierto a la vez).
        private Panel[] Submenus => new[]
        {
            panelAdminSubmenu, panelMaestroSubmenu, panelUsuarioSubmenu, panelMembresiaSubmenu,
            panelInventarioSubmenu, panelReportesSubmenu, panelAyudaSubmenu
        };

        private void customMenu()
        {
            esconderSubmenu();
        }

        private void esconderSubmenu()
        {
            foreach (Panel submenu in Submenus)
                submenu.Visible = false;
        }

        private void mostrarSubmenu(Panel submenu)
        {
            bool abrir = !submenu.Visible;
            esconderSubmenu();
            submenu.Visible = abrir;
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelAdminSubmenu);
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelUsuarioSubmenu);
        }

        private void btnMaestro_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelMaestroSubmenu);
        }

        private void btnMembresias_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelMembresiaSubmenu);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelReportesSubmenu);
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelAyudaSubmenu);
        }

        // Reportes y ayuda en línea: se completan en la Entrega 3 (A02 y D02).
        private void MostrarProximamente()
        {
            MessageBox.Show(LanguageManager.Instance.GetTraduction("MenuProximamente"),
                LanguageManager.Instance.GetTraduction("Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Rep. 1 (RFN1): informe de recaudación y vencimientos (CUN-05)
        private void btnReporteRFN1_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Renovar Membresia"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("InfMemSinPermiso"),
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            _012IP_frmInformeMembresias frm = new _012IP_frmInformeMembresias();
            frm.Show();
        }

        private void btnReporteRFN2_Click(object sender, EventArgs e) => MostrarProximamente();

        // Reporte de socios (listado con estado de membresía y exportación a PDF)
        private void btnReporteSocios_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Renovar Membresia"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("InfMemSinPermiso"),
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            _012IP_frmReporteSocios frm = new _012IP_frmReporteSocios();
            frm.Show();
        }

        private void btnAyudaMembresias_Click(object sender, EventArgs e) => MostrarProximamente();

        private void btnAyudaInventario_Click(object sender, EventArgs e) => MostrarProximamente();

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            MessageBox.Show(LanguageManager.Instance.GetTraduction("MenuAcercaDeTexto"),
                LanguageManager.Instance.GetTraduction("MenuAcercaDe"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            AplicarPermisos();
            customMenu();
            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);
        }

        // ===================== Menú según el perfil del usuario =====================
        // Cada opción se muestra solo si el usuario tiene el permiso que la habilita.
        // Si un grupo (ADMIN, MAESTROS, ...) se queda sin opciones, se oculta el grupo entero.
        private void AplicarPermisos()
        {
            var usuario = SessionManager.Instance.UsuarioActual();
            Func<string, bool> tiene = permiso => usuario != null && usuario.TienePermiso(permiso);

            // ADMIN
            Mostrar(btnUsuarios, tiene("Ver Usuarios"));
            Mostrar(btnPerfiles, tiene("Ver Perfiles"));
            Mostrar(btnDigitoVerificador, tiene("Gestionar DigitoVerificadores"));
            Mostrar(btnBitacora, tiene("Ver Bitacora"));
            Mostrar(btnBackUp, tiene("Ver Respaldos"));
            Mostrar(btnRestore, tiene("Ver Respaldos"));

            // MAESTROS
            Mostrar(btnRegistrarSocio, tiene("Renovar Membresia"));
            Mostrar(btnGestionInventario, tiene("Gestionar Inventario"));
            Mostrar(btnBitacoraCambios, tiene("Ver Bitacora Cambios"));

            // MEMBRESÍAS
            Mostrar(btnRenovarMembresia, tiene("Renovar Membresia"));

            // INVENTARIO
            Mostrar(btnAuditoriaInventario, tiene("Auditar Inventario"));

            // REPORTES
            Mostrar(btnReporteRFN1, tiene("Renovar Membresia"));
            Mostrar(btnReporteSocios, tiene("Renovar Membresia"));
            Mostrar(btnReporteRFN2, tiene("Gestionar Inventario") || tiene("Auditar Inventario"));

            // AYUDA (Acerca de siempre visible)
            Mostrar(btnAyudaMembresias, tiene("Renovar Membresia"));
            Mostrar(btnAyudaInventario, tiene("Gestionar Inventario") || tiene("Auditar Inventario"));

            // USUARIO (cambiar clave, idioma, cerrar sesión) queda siempre visible.

            AjustarGrupo(btnAdmin, panelAdminSubmenu);
            AjustarGrupo(btnMaestro, panelMaestroSubmenu);
            AjustarGrupo(btnUsuario, panelUsuarioSubmenu);
            AjustarGrupo(btnMembresias, panelMembresiaSubmenu);
            AjustarGrupo(btnInventario, panelInventarioSubmenu);
            AjustarGrupo(btnReportes, panelReportesSubmenu);
            AjustarGrupo(btnAyuda, panelAyudaSubmenu);
        }

        // Visible no sirve para leer el estado (da false mientras el submenú está cerrado),
        // por eso lo que corresponde mostrar se guarda en Tag.
        private void Mostrar(Control opcion, bool visible)
        {
            opcion.Visible = visible;
            opcion.Tag = visible;
        }

        // El alto del submenú se ajusta a las opciones permitidas; sin opciones se oculta el encabezado.
        private void AjustarGrupo(Button encabezado, Panel submenu)
        {
            int alto = 0;
            foreach (Control c in submenu.Controls)
                if (!(c.Tag is bool permitido) || permitido)
                    alto += c.Height;
            submenu.Height = alto;
            encabezado.Visible = alto > 0;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                arrastrando = true;
                posX = e.X;
                posY = e.Y;
            }
        }

        private void BarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastrando)
            {
                this.Location = new Point(this.Location.X + (e.X - posX), this.Location.Y + (e.Y - posY));
            }
        }

        private void btnBitacora_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Ver Bitacora"))
            {
                MessageBox.Show("No tiene permisos para acceder a la Bitácora.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            FormBitacora formBitacora = new FormBitacora();
            formBitacora.Show();
        }

        // T06.B Bitácora de cambios de la tabla Socio (historial y restauración de versiones)
        private void btnBitacoraCambios_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Ver Bitacora Cambios"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitCamSinPermiso"),
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            _012IP_frmBitacoraCambios frm = new _012IP_frmBitacoraCambios();
            frm.Show();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Ver Usuarios"))
            {
                MessageBox.Show("No tiene permisos para acceder a la gestión de usuarios.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            Usuarios usuarios = new Usuarios();
            usuarios.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            string Decision = MessageBox.Show(LanguageManager.Instance.GetTraduction("menumsj1"), LanguageManager.Instance.GetTraduction("menumsj2"), MessageBoxButtons.YesNo).ToString();
            if (Decision == "Yes")
            {
                IdiomaBLL idiomaBLL = new IdiomaBLL();

                string username = SessionManager.Instance.UsuarioActual().Username;
                idiomaBLL.GuardarIdioma(username, LanguageManager.Instance.CodigoIdiomaActual);

                SessionManager.Instance.Desloguear();

                this.Hide();
                Login login = new Login();
                login.Show();
            }

        }


        private void btnCambiarClave_Click(object sender, EventArgs e)
        {
            this.Hide();
            CambiarContraseña cambiarContraseña = new CambiarContraseña();
            cambiarContraseña.Show();
        }

        private void btnPerfiles_Click(object sender, EventArgs e)
        {

            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Ver Perfiles"))
            {
                MessageBox.Show("No tiene permisos para acceder a Gestión de Perfiles.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            frmGestionPerfiles gestionPerfiles = new frmGestionPerfiles();
            this.Hide();
            gestionPerfiles.Show();
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Ver Respaldos"))
            {
                MessageBox.Show("No tiene permisos para acceder a Backup.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            FormBackup formBackup = new FormBackup();
            formBackup.Show();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Ver Respaldos"))
            {
                MessageBox.Show("No tiene permisos para acceder a Restore.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            FormRestore formRestore = new FormRestore();
            formRestore.Show();
        }

        private void BarraTitulo_MouseUp(object sender, MouseEventArgs e)
        {
            arrastrando = false;
        }

        private void btnCambiarIdioma_Click(object sender, EventArgs e)
        {
            this.Hide();
            CambiarIdioma cambiarContraseña = new CambiarIdioma();
            cambiarContraseña.Show();
        }

        private void btnDigitoVerificador_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Gestionar DigitoVerificadores"))
            {
                MessageBox.Show("No tiene permisos para acceder a Digitos Verificadores.",
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            frmReparacionDV frmReparacionDV = new frmReparacionDV();
            frmReparacionDV.Show();
        }

        private void btnRenovarMembresia_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Renovar Membresia"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("RenMemSinPermiso"),
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            _012IP_frmRenovarMembresia frm = new _012IP_frmRenovarMembresia();
            frm.Show();
        }

        private void btnRegistrarSocio_Click(object sender, EventArgs e)
        {
            // Gestión de socios: alta, modificación y baja lógica
            this.Hide();
            var frmGestionSocios = new _012IP_frmGestionSocios();
            frmGestionSocios.Show();
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(panelInventarioSubmenu);
        }

        private void btnGestionInventario_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Gestionar Inventario"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("InvSinPermiso"),
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            _012IP_frmGestionInventario frm = new _012IP_frmGestionInventario();
            frm.Show();
        }

        private void btnAuditoriaInventario_Click(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.UsuarioActual().TienePermiso("Auditar Inventario"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("AudSinPermiso"),
                    "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            this.Hide();
            _012IP_frmAuditoriaInventario frm = new _012IP_frmAuditoriaInventario();
            frm.Show();
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            Func<string, string> t = LanguageManager.Instance.GetTraduction;

            btnAdmin.Text = t("btnAdmin");
            btnUsuarios.Text = t("MenuGestionUsuarios");
            btnPerfiles.Text = t("MenuGestionPerfiles");
            btnDigitoVerificador.Text = t("MenuDigitosVerificadores");
            btnBitacora.Text = t("MenuEventos");
            btnBitacoraCambios.Text = t("MenuBitacoraCambios");
            btnBackUp.Text = t("btnBackUp");
            btnRestore.Text = t("btnRestore");

            btnMaestro.Text = t("MenuGrupoMaestros");
            btnRegistrarSocio.Text = t("MenuSocios");
            btnGestionInventario.Text = t("MenuEjemplares");

            btnUsuario.Text = t("MenuGrupoUsuario");
            btnCambiarClave.Text = t("btnCambiarClave");
            btnLogout.Text = t("btnLogout");
            btnCambiarIdioma.Text = t("btnCambiarIdioma");

            btnMembresias.Text = t("MenuGrupoMembresias");
            btnRenovarMembresia.Text = t("RenMemTitulo");

            btnInventario.Text = t("MenuGrupoInventario");
            btnAuditoriaInventario.Text = t("InvMenuAuditoria");

            btnReportes.Text = t("MenuGrupoReportes");
            btnReporteRFN1.Text = t("MenuReporteRFN1");
            btnReporteSocios.Text = t("MenuReporteSocios");
            btnReporteRFN2.Text = t("MenuReporteRFN2");

            btnAyuda.Text = t("MenuGrupoAyuda");
            btnAyudaMembresias.Text = t("MenuMembresias");
            btnAyudaInventario.Text = t("InvMenuGrupo");
            btnAcercaDe.Text = t("MenuAcercaDe");
        }
    }
}