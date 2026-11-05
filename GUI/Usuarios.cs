using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI
{
    public partial class Usuarios : Form, IObserver
    {
        int posX, posY;
        bool arrastrando = false;
        private UserAction userAction;
        private UsuarioBLL usuarioBLL;
        private PerfilBLL perfilBLL;

        public Usuarios()
        {
            InitializeComponent();
            usuarioBLL = new UsuarioBLL();
            perfilBLL = new PerfilBLL();
            lblTextoTabla.Text = LanguageManager.Instance.GetTraduction("lblTextoTablaUserActivos");
            MostrarUsuarios(dataGridView1, usuarioBLL.ListarUsuariosActivos());
            MostrarCantidadUsuarios();
            ConfigurarGrillaSeleccionFila(dataGridView1);
            ReiniciarBotones();
        }

        private void CargarPerfiles()
        {
            cbRol.Items.Clear();
            foreach (var perfil in perfilBLL.ObtenerPerfiles())
                cbRol.Items.Add(perfil.Nombre);
        }

        private void MostrarCantidadUsuarios()
        {
            if (rbActivos.Checked)
            {
                lblCantidadUsers.Text = $"{LanguageManager.Instance.GetTraduction("lblCantidadUser")}: {usuarioBLL.ListarUsuariosActivos().Count}";
            }
            else
            {
                lblCantidadUsers.Text = $"{LanguageManager.Instance.GetTraduction("lblCantidadUser")}: {usuarioBLL.ListarTodosUsuarios().Count}";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActivos.Checked)
            {
                lblTextoTabla.Text = LanguageManager.Instance.GetTraduction("lblTextoTablaUserActivos");
                MostrarUsuarios(dataGridView1, usuarioBLL.ListarUsuariosActivos() as List<Usuario>);
                MostrarCantidadUsuarios();
            }
            else
            {
                lblTextoTabla.Text = LanguageManager.Instance.GetTraduction("lblTextoTablaUserTodos");
                MostrarCantidadUsuarios();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu menu = new Menu();
            menu.Show();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e) => arrastrando = false;

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastrando)
                this.Location = new Point(this.Location.X + (e.X - posX), this.Location.Y + (e.Y - posY));
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMinimizar_Click(object sender, EventArgs e) =>
            this.WindowState = FormWindowState.Minimized;

        private void panModificarUsuario_EnabledChanged(object sender, EventArgs e)
        {
            Color color = panModificarUsuario.Enabled ? Color.White : Color.Gray;
            txtDni.BackColor = color;
            txtNom.BackColor = color;
            txtApe.BackColor = color;
            txtEmail.BackColor = color;
            txtUsuario.BackColor = color;
            cbRol.BackColor = color;
        }

      

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            try
            {
                switch (userAction)
                {
                    case UserAction.Add:
                        if (ValidarCamposVacios(txtDni, txtApe, txtNom, txtEmail, txtUsuario) && ValidarComboBox(cbRol))
                        {
                            usuarioBLL.RegistrarUsuario(new Usuario(
                                txtDni.Text, txtNom.Text, txtApe.Text, txtUsuario.Text,
                                Encriptador.GetHash256(txtDni.Text + txtNom.Text),
                                txtEmail.Text, false, true, cbRol.SelectedItem.ToString()));
                            ReiniciarBotones();
                            MostrarUsuarios(dataGridView1, usuarioBLL.ListarUsuariosActivos());
                            MostrarCantidadUsuarios();
                        }
                        break;

                    case UserAction.Delete:
                        usuarioBLL.EliminarLogico((dataGridView1.SelectedRows[0].DataBoundItem as Usuario).DNI);
                        ReiniciarBotones();
                        MostrarUsuarios(dataGridView1, rbActivos.Checked
                            ? usuarioBLL.ListarUsuariosActivos()
                            : usuarioBLL.ListarTodosUsuarios());
                        MostrarCantidadUsuarios();

                        MessageBox.Show(LanguageManager.Instance.GetTraduction("UserElimi"), LanguageManager.Instance.GetTraduction("Alerta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                            case UserAction.Modify:
                                // Sacamos el txtDni.ReadOnly = true de acá porque ya se pone en btnModificar_Click
                                if (ValidarCamposVacios(txtNom, txtApe, txtDni, txtUsuario, txtEmail) && ValidarComboBox(cbRol) && ValidarEntradaUsuario())
                                {
                                    string dniOriginal = (dataGridView1.SelectedRows[0].DataBoundItem as Usuario).DNI;
                                    string rolNuevo = cbRol.SelectedItem.ToString();

                                    usuarioBLL.Modificar(dniOriginal, new Usuario(
                                        dniOriginal,        // forzamos el DNI original, ignoramos txtDni
                                        txtNom.Text, txtApe.Text, txtUsuario.Text,
                                        string.Empty, txtEmail.Text, default, default, rolNuevo));

                                    Usuario usuarioEnSesion = SessionManager.Instance.UsuarioActual();
                                    if (usuarioEnSesion.DNI == dniOriginal)
                                    {
                                        var perfiles = perfilBLL.ObtenerPerfiles();
                                        var perfilNuevo = perfiles.FirstOrDefault(p => p.Nombre == rolNuevo);
                                        usuarioEnSesion.Permisos.Clear();
                                        if (perfilNuevo != null)
                                            usuarioEnSesion.Permisos.Add(perfilNuevo);
                                    }

                                    ReiniciarBotones();
                                    MessageBox.Show(LanguageManager.Instance.GetTraduction("UserModif"),
                                        LanguageManager.Instance.GetTraduction("Alerta"),
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    MostrarUsuarios(dataGridView1, usuarioBLL.ListarUsuariosActivos());

                                    if (usuarioEnSesion.DNI == dniOriginal && !usuarioEnSesion.TienePermiso("Ver Usuarios"))
                                    {
                                        MessageBox.Show(LanguageManager.Instance.GetTraduction("UserMsj2"),
                                            LanguageManager.Instance.GetTraduction("UserMsj3"),
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        btnCerrar_Click(sender, e);
                                        return;
                                    }
                                    ValidarPermisos();
                                }
                                break;

                            case UserAction.UnBlock:
                        ReiniciarBotones();
                        usuarioBLL.DesbloquearUsuario(dataGridView1.SelectedRows[0].DataBoundItem as Usuario);
                        MostrarUsuarios(dataGridView1, rbActivos.Checked
                            ? usuarioBLL.ListarUsuariosActivos()
                            : usuarioBLL.ListarTodosUsuarios());
                        MessageBox.Show(LanguageManager.Instance.GetTraduction("UserDesbloqueado"), LanguageManager.Instance.GetTraduction("Alerta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case UserAction.Activate:
                        usuarioBLL.EliminarLogico((dataGridView1.SelectedRows[0].DataBoundItem as Usuario).DNI);
                        ReiniciarBotones();
                        if (rbActivos.Checked)
                        {
                            MostrarUsuarios(dataGridView1, usuarioBLL.ListarUsuariosActivos());
                        }
                        else
                        {
                            MostrarUsuarios(dataGridView1, usuarioBLL.ListarTodosUsuarios());

                        }
                        MostrarUsuarios(dataGridView1, rbActivos.Checked
                            ? usuarioBLL.ListarUsuariosActivos()
                            : usuarioBLL.ListarTodosUsuarios());
                        MessageBox.Show(LanguageManager.Instance.GetTraduction("UserActi"), LanguageManager.Instance.GetTraduction("Alerta"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        break;

                    case UserAction.Consult:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Ultima lista traida de la base (activos o todos); la busqueda filtra sobre esta lista.
        private List<Usuario> usuariosCargados = new List<Usuario>();

        public void MostrarUsuarios(DataGridView dgv, object obj)
        {
            usuariosCargados = (obj as List<Usuario>) ?? new List<Usuario>();
            AplicarBusquedaUsuarios();
        }

        // Busqueda dinamica: se ejecuta en el evento TextChanged del cuadro de busqueda.
        private void txtBuscarUsuario_TextChanged(object sender, EventArgs e) => AplicarBusquedaUsuarios();

        private void AplicarBusquedaUsuarios()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = usuarioBLL.FiltrarUsuarios(usuariosCargados, txtBuscarUsuario.Text);
        }

        private bool ValidarCamposVacios(params Control[] controles)
        {
            StringBuilder mensaje = new StringBuilder();
            bool hayVacios = false;
            Control primerInvalido = null;

            foreach (Control c in controles)
            {
                string nombreCampo = c.Tag?.ToString() ?? c.Name;
                if (c is TextBox txt && string.IsNullOrWhiteSpace(txt.Text))
                {
                    mensaje.AppendLine($"{LanguageManager.Instance.GetTraduction("ElCampo")} " + " " + nombreCampo + " " + LanguageManager.Instance.GetTraduction("EstaVacio"));
                    if (primerInvalido == null) primerInvalido = txt;
                    hayVacios = true;
                }
                else if (c is ComboBox cb && cb.SelectedIndex == -1)
                {
                    mensaje.AppendLine($"{LanguageManager.Instance.GetTraduction("ElCampo")}" + " " + nombreCampo + " " + LanguageManager.Instance.GetTraduction("NoFueSelecc"));
                    if (primerInvalido == null) primerInvalido = cb;
                    hayVacios = true;
                }
            }

            if (hayVacios)
            {
                MessageBox.Show(mensaje.ToString(), LanguageManager.Instance.GetTraduction("FaltaCompl"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                primerInvalido?.Focus(); // Enfocar el primer campo con error
            }
            return !hayVacios;
        }
        private bool ValidarComboBox(ComboBox cb)
        {
            if (cb.SelectedIndex == -1)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("UserMsj1"), LanguageManager.Instance.GetTraduction("FaltaCompl"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cb.Focus();
                return false;
            }
            return true;
        }

        private bool ValidarEntradaUsuario()
        {
            StringBuilder errores = new StringBuilder();

            if (!Regex.IsMatch(txtNom.Text.Trim(), @"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
                errores.AppendLine(LanguageManager.Instance.GetTraduction("CampoNombre"));

            if (!Regex.IsMatch(txtApe.Text.Trim(), @"^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
                errores.AppendLine(LanguageManager.Instance.GetTraduction("CampoApell"));

            
            if (!Regex.IsMatch(txtDni.Text.Trim(), @"^\d{8}$"))
                errores.AppendLine(LanguageManager.Instance.GetTraduction("CampoDni"));

            if (!(userAction == UserAction.Add))
            {
                
                if (string.IsNullOrWhiteSpace(txtUsuario.Text) || txtUsuario.Text.Length < 4)
                    errores.AppendLine(LanguageManager.Instance.GetTraduction("ElNombreUser"));
            }
           
            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errores.AppendLine(LanguageManager.Instance.GetTraduction("ElCorreoElec"));
            
            if (errores.Length > 0)
            {
                MessageBox.Show(errores.ToString(), LanguageManager.Instance.GetTraduction("ErrorEntradaDeDatos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                arrastrando = true;
                posX = e.X;
                posY = e.Y;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => ReiniciarBotones();

        private void SetModoEdicion(bool enEdicion)
        {
            // Botones de acción: deshabilitados durante edición
            btnCrear.Enabled = !enEdicion;
            btnDesbloquear.Enabled = !enEdicion;
            btnModificar.Enabled = !enEdicion;
            btnActDes.Enabled = !enEdicion;

            btnCrear.BackColor = !enEdicion ? Color.Green : Color.Maroon;
            btnDesbloquear.BackColor = !enEdicion ? Color.Green : Color.Maroon;
            btnModificar.BackColor = !enEdicion ? Color.Green : Color.Maroon;
            btnActDes.BackColor = !enEdicion ? Color.Green : Color.Maroon;

            // Aplicar/Cancelar: solo activos durante edición
            btnAplicar.Enabled = enEdicion;
            btnCancelar.Enabled = enEdicion;
            btnAplicar.BackColor = enEdicion ? Color.Green : Color.Maroon;
            btnCancelar.BackColor = enEdicion ? Color.Green : Color.Maroon;
            panel10.BackColor = enEdicion ? Color.Green : Color.Maroon;
            panel11.BackColor = enEdicion ? Color.Green : Color.Maroon;

            // Panel de filtrado y formulario
            pnFiltrado.Enabled = !enEdicion;
            panel3.Enabled = !enEdicion;
            panModificarUsuario.Enabled = enEdicion;
        }
        private void ReiniciarBotones()
        {
            txtNom.Clear();
            txtApe.Clear();
            txtDni.Clear();
            txtEmail.Clear();
            txtUsuario.Clear();
            cbRol.SelectedIndex = -1;
            txtDni.ReadOnly = false;

            SetModoEdicion(false);

            txtBoxModo.Text = LanguageManager.Instance.GetTraduction("txtBoxModoConsulta");
            userAction = UserAction.Consult;

            if (SessionManager.Instance.Logueado())
                ValidarPermisos();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) =>
            MostrarUsuarios(dataGridView1, usuarioBLL.ListarTodosUsuarios());

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (userAction == UserAction.Delete || userAction == UserAction.Modify || userAction == UserAction.UnBlock)
            {
                if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Cells[0].Value != null)
                {
                    Usuario user = usuarioBLL.BuscarUsuarioPorDNI(
                        dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    if (user == null) return;

                    txtDni.Text = user.DNI;
                    txtDni.ReadOnly = (userAction == UserAction.Modify); // mantiene el lock si estamos modificando
                    txtNom.Text = user.Nombre;
                    txtApe.Text = user.Apellido;
                    txtUsuario.Text = user.Username;
                    txtEmail.Text = user.Email;
                    cbRol.SelectedItem = user.Rol;
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem is Usuario usuario)
                e.CellStyle.ForeColor = usuario.Bloqueado ? Color.Red : Color.Black;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            userAction = UserAction.Add;
            CargarPerfiles();
            txtBoxModo.Text = LanguageManager.Instance.GetTraduction("txtBoxModoAnadir");
            SetModoEdicion(true);
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                userAction = UserAction.UnBlock;
                txtBoxModo.Text = LanguageManager.Instance.GetTraduction("txtBoxModoDesbloquear");
                SetModoEdicion(true);
                panModificarUsuario.Enabled = false; // en desbloqueo no se editan campos
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells[0].Value == null)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("UserMsj4")); // "Seleccione un usuario" o similar
                return;
            }

            userAction = UserAction.Modify;
            txtBoxModo.Text = LanguageManager.Instance.GetTraduction("txtBoxModoModificar");
            CargarPerfiles();
            SetModoEdicion(true);

            // Rellenamos los campos con los datos del usuario seleccionado
            Usuario user = usuarioBLL.BuscarUsuarioPorDNI(
                dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            if (user == null) return;

            txtDni.Text = user.DNI;
            txtNom.Text = user.Nombre;
            txtApe.Text = user.Apellido;
            txtUsuario.Text = user.Username;
            txtEmail.Text = user.Email;
            cbRol.SelectedItem = user.Rol;
            txtDni.ReadOnly = true;
        }

        private void btnActDes_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Cells[0].Value != null)
            {
                Usuario us = usuarioBLL.BuscarUsuarioPorDNI(
                    dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                if (us == null) return;

                if (!us.Activo)
                {
                    userAction = UserAction.Activate;
                    txtBoxModo.Text = LanguageManager.Instance.GetTraduction("txtBoxModoActivar");
                }
                else
                {
                    userAction = UserAction.Delete;
                    txtBoxModo.Text = LanguageManager.Instance.GetTraduction("txtBoxModoEliminar");
                }
                SetModoEdicion(true);
                panModificarUsuario.Enabled = false;
            }
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);
            ValidarPermisos();
        }

        private void ValidarPermisos()
        {
            Usuario usuarioActual = SessionManager.Instance.UsuarioActual();

            btnCrear.Enabled = usuarioActual.TienePermiso("Crear Usuario");
            btnModificar.Enabled = usuarioActual.TienePermiso("Modificar Usuario");
            btnActDes.Enabled = usuarioActual.TienePermiso("Activar Desactivar Usuario");
            btnDesbloquear.Enabled = usuarioActual.TienePermiso("Desbloquear Usuario");
        }

        private void ConfigurarGrillaSeleccionFila(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTextoTabla.Text = lenguaje.GetTraduction("lblTextoTabla");
            lblBuscarUsuario.Text = lenguaje.GetTraduction("UsuBuscarAyuda");
            txtDni.Text = lenguaje.GetTraduction("DNI");
            lblApe.Text = lenguaje.GetTraduction("lblApe");
            lblNombre.Text = lenguaje.GetTraduction("lblNombre");
            lblEmail.Text = lenguaje.GetTraduction("lblEmail");
            lblRol.Text = lenguaje.GetTraduction("lblRol");
            lblUsuarioFU.Text = lenguaje.GetTraduction("lblUsuarioFU");
            lblMsj.Text = lenguaje.GetTraduction("lblMsj");
            lblOpciones.Text = lenguaje.GetTraduction("lblOpciones");

            btnCrear.Text = lenguaje.GetTraduction("btnCrear");
            btnDesbloquear.Text = lenguaje.GetTraduction("btnDesbloquear");
            btnModificar.Text = lenguaje.GetTraduction("btnModificar");
            btnActDes.Text = lenguaje.GetTraduction("btnActDes");
            btnAplicar.Text = lenguaje.GetTraduction("btnAplicar");
            btnCancelar.Text = lenguaje.GetTraduction("btnCancelar");
            btnSalir.Text = lenguaje.GetTraduction("btnSalir");

            rbTodos.Text = lenguaje.GetTraduction("rbTodos");
            rbActivos.Text = lenguaje.GetTraduction("rbActivos");
        }
    }
}