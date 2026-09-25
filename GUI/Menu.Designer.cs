namespace GUI
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.BarraTitulo = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.PictureBox();
            this.btnMaximizar = new System.Windows.Forms.PictureBox();
            this.btnMinimizar = new System.Windows.Forms.PictureBox();
            this.MenuVertical = new System.Windows.Forms.Panel();
            this.panelMembresiaSubmenu = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel17 = new System.Windows.Forms.Panel();
            this.btnRegistrarSocio = new System.Windows.Forms.Button();
            this.btnRenovarMembresia = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panelUsuarioSubmenu = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnCambiarIdioma = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnCambiarClave = new System.Windows.Forms.Button();
            this.btnReLogin = new System.Windows.Forms.Button();
            this.btnUsuario = new System.Windows.Forms.Button();
            this.btnMaestro = new System.Windows.Forms.Button();
            this.panelAdminSubmenu = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDigitoVerificador = new System.Windows.Forms.Button();
            this.btnBitacora = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnBackUp = new System.Windows.Forms.Button();
            this.btnPerfiles = new System.Windows.Forms.Button();
            this.btnUsuarios = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelContenedor = new System.Windows.Forms.Panel();
            this.BarraTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimizar)).BeginInit();
            this.MenuVertical.SuspendLayout();
            this.panelMembresiaSubmenu.SuspendLayout();
            this.panelUsuarioSubmenu.SuspendLayout();
            this.panelAdminSubmenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // BarraTitulo
            // 
            this.BarraTitulo.BackColor = System.Drawing.Color.Maroon;
            this.BarraTitulo.Controls.Add(this.btnCerrar);
            this.BarraTitulo.Controls.Add(this.btnMaximizar);
            this.BarraTitulo.Controls.Add(this.btnMinimizar);
            this.BarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.BarraTitulo.Name = "BarraTitulo";
            this.BarraTitulo.Size = new System.Drawing.Size(1249, 43);
            this.BarraTitulo.TabIndex = 0;
            this.BarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BarraTitulo_MouseDown);
            this.BarraTitulo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BarraTitulo_MouseMove);
            this.BarraTitulo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BarraTitulo_MouseUp);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCerrar.Image")));
            this.btnCerrar.Location = new System.Drawing.Point(1221, 4);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(25, 35);
            this.btnCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximizar.Image = ((System.Drawing.Image)(resources.GetObject("btnMaximizar.Image")));
            this.btnMaximizar.Location = new System.Drawing.Point(1190, 4);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Size = new System.Drawing.Size(25, 35);
            this.btnMaximizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMaximizar.TabIndex = 1;
            this.btnMaximizar.TabStop = false;
            this.btnMaximizar.Click += new System.EventHandler(this.btnMaximizar_Click);
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimizar.Image")));
            this.btnMinimizar.Location = new System.Drawing.Point(1159, 4);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(25, 35);
            this.btnMinimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMinimizar.TabIndex = 0;
            this.btnMinimizar.TabStop = false;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            // 
            // MenuVertical
            // 
            this.MenuVertical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.MenuVertical.Controls.Add(this.panelMembresiaSubmenu);
            this.MenuVertical.Controls.Add(this.button1);
            this.MenuVertical.Controls.Add(this.panelUsuarioSubmenu);
            this.MenuVertical.Controls.Add(this.btnUsuario);
            this.MenuVertical.Controls.Add(this.btnMaestro);
            this.MenuVertical.Controls.Add(this.panelAdminSubmenu);
            this.MenuVertical.Controls.Add(this.btnAdmin);
            this.MenuVertical.Controls.Add(this.pictureBox1);
            this.MenuVertical.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuVertical.Location = new System.Drawing.Point(0, 43);
            this.MenuVertical.Name = "MenuVertical";
            this.MenuVertical.Size = new System.Drawing.Size(200, 698);
            this.MenuVertical.TabIndex = 1;
            // 
            // panelMembresiaSubmenu
            // 
            this.panelMembresiaSubmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panelMembresiaSubmenu.Controls.Add(this.panel12);
            this.panelMembresiaSubmenu.Controls.Add(this.panel13);
            this.panelMembresiaSubmenu.Controls.Add(this.panel14);
            this.panelMembresiaSubmenu.Controls.Add(this.panel15);
            this.panelMembresiaSubmenu.Controls.Add(this.panel16);
            this.panelMembresiaSubmenu.Controls.Add(this.panel17);
            this.panelMembresiaSubmenu.Controls.Add(this.btnRegistrarSocio);
            this.panelMembresiaSubmenu.Controls.Add(this.btnRenovarMembresia);
            this.panelMembresiaSubmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMembresiaSubmenu.Location = new System.Drawing.Point(0, 589);
            this.panelMembresiaSubmenu.Name = "panelMembresiaSubmenu";
            this.panelMembresiaSubmenu.Size = new System.Drawing.Size(200, 218);
            this.panelMembresiaSubmenu.TabIndex = 7;
            this.panelMembresiaSubmenu.Visible = false;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Maroon;
            this.panel12.Location = new System.Drawing.Point(12, 70);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(5, 35);
            this.panel12.TabIndex = 7;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Maroon;
            this.panel13.Location = new System.Drawing.Point(12, 105);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(5, 35);
            this.panel13.TabIndex = 7;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.Maroon;
            this.panel14.Location = new System.Drawing.Point(12, 140);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(5, 35);
            this.panel14.TabIndex = 7;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Maroon;
            this.panel15.Location = new System.Drawing.Point(12, 175);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(5, 35);
            this.panel15.TabIndex = 7;
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.Color.Maroon;
            this.panel16.Location = new System.Drawing.Point(12, 35);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(5, 35);
            this.panel16.TabIndex = 7;
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.Color.Maroon;
            this.panel17.Location = new System.Drawing.Point(12, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(5, 35);
            this.panel17.TabIndex = 7;
            // 
            // btnRegistrarSocio
            // 
            this.btnRegistrarSocio.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRegistrarSocio.FlatAppearance.BorderSize = 0;
            this.btnRegistrarSocio.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRegistrarSocio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrarSocio.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrarSocio.ForeColor = System.Drawing.Color.White;
            this.btnRegistrarSocio.Location = new System.Drawing.Point(0, 35);
            this.btnRegistrarSocio.Name = "btnRegistrarSocio";
            this.btnRegistrarSocio.Size = new System.Drawing.Size(200, 35);
            this.btnRegistrarSocio.TabIndex = 5;
            this.btnRegistrarSocio.Text = "Registrar socio";
            this.btnRegistrarSocio.UseVisualStyleBackColor = true;
            this.btnRegistrarSocio.Click += new System.EventHandler(this.btnRegistrarSocio_Click);
            // 
            // btnRenovarMembresia
            // 
            this.btnRenovarMembresia.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRenovarMembresia.FlatAppearance.BorderSize = 0;
            this.btnRenovarMembresia.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRenovarMembresia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenovarMembresia.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRenovarMembresia.ForeColor = System.Drawing.Color.White;
            this.btnRenovarMembresia.Location = new System.Drawing.Point(0, 0);
            this.btnRenovarMembresia.Name = "btnRenovarMembresia";
            this.btnRenovarMembresia.Size = new System.Drawing.Size(200, 35);
            this.btnRenovarMembresia.TabIndex = 4;
            this.btnRenovarMembresia.Text = "Renovar membresias";
            this.btnRenovarMembresia.UseVisualStyleBackColor = true;
            this.btnRenovarMembresia.Click += new System.EventHandler(this.btnRenovarMembresia_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(0, 554);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 35);
            this.button1.TabIndex = 6;
            this.button1.Text = "Membresias";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panelUsuarioSubmenu
            // 
            this.panelUsuarioSubmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panelUsuarioSubmenu.Controls.Add(this.panel10);
            this.panelUsuarioSubmenu.Controls.Add(this.btnCambiarIdioma);
            this.panelUsuarioSubmenu.Controls.Add(this.panel3);
            this.panelUsuarioSubmenu.Controls.Add(this.panel2);
            this.panelUsuarioSubmenu.Controls.Add(this.panel1);
            this.panelUsuarioSubmenu.Controls.Add(this.btnLogout);
            this.panelUsuarioSubmenu.Controls.Add(this.btnCambiarClave);
            this.panelUsuarioSubmenu.Controls.Add(this.btnReLogin);
            this.panelUsuarioSubmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUsuarioSubmenu.Location = new System.Drawing.Point(0, 402);
            this.panelUsuarioSubmenu.Name = "panelUsuarioSubmenu";
            this.panelUsuarioSubmenu.Size = new System.Drawing.Size(200, 152);
            this.panelUsuarioSubmenu.TabIndex = 5;
            this.panelUsuarioSubmenu.Visible = false;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Maroon;
            this.panel10.Location = new System.Drawing.Point(12, 105);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(5, 35);
            this.panel10.TabIndex = 9;
            // 
            // btnCambiarIdioma
            // 
            this.btnCambiarIdioma.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCambiarIdioma.FlatAppearance.BorderSize = 0;
            this.btnCambiarIdioma.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCambiarIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarIdioma.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarIdioma.ForeColor = System.Drawing.Color.White;
            this.btnCambiarIdioma.Location = new System.Drawing.Point(0, 105);
            this.btnCambiarIdioma.Name = "btnCambiarIdioma";
            this.btnCambiarIdioma.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCambiarIdioma.Size = new System.Drawing.Size(200, 35);
            this.btnCambiarIdioma.TabIndex = 8;
            this.btnCambiarIdioma.Text = "Cambiar idioma";
            this.btnCambiarIdioma.UseVisualStyleBackColor = true;
            this.btnCambiarIdioma.Click += new System.EventHandler(this.btnCambiarIdioma_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Maroon;
            this.panel3.Location = new System.Drawing.Point(12, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 35);
            this.panel3.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Maroon;
            this.panel2.Location = new System.Drawing.Point(12, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 35);
            this.panel2.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Maroon;
            this.panel1.Location = new System.Drawing.Point(12, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 35);
            this.panel1.TabIndex = 6;
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(0, 70);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 35);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnCambiarClave
            // 
            this.btnCambiarClave.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCambiarClave.FlatAppearance.BorderSize = 0;
            this.btnCambiarClave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCambiarClave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarClave.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarClave.ForeColor = System.Drawing.Color.White;
            this.btnCambiarClave.Location = new System.Drawing.Point(0, 35);
            this.btnCambiarClave.Name = "btnCambiarClave";
            this.btnCambiarClave.Size = new System.Drawing.Size(200, 35);
            this.btnCambiarClave.TabIndex = 1;
            this.btnCambiarClave.Text = "Cambiar clave";
            this.btnCambiarClave.UseVisualStyleBackColor = true;
            this.btnCambiarClave.Click += new System.EventHandler(this.btnCambiarClave_Click);
            // 
            // btnReLogin
            // 
            this.btnReLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReLogin.FlatAppearance.BorderSize = 0;
            this.btnReLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnReLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReLogin.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReLogin.ForeColor = System.Drawing.Color.White;
            this.btnReLogin.Location = new System.Drawing.Point(0, 0);
            this.btnReLogin.Name = "btnReLogin";
            this.btnReLogin.Size = new System.Drawing.Size(200, 35);
            this.btnReLogin.TabIndex = 0;
            this.btnReLogin.Text = "Re_Login";
            this.btnReLogin.UseVisualStyleBackColor = true;
            this.btnReLogin.Click += new System.EventHandler(this.btnReLogin_Click);
            // 
            // btnUsuario
            // 
            this.btnUsuario.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUsuario.FlatAppearance.BorderSize = 0;
            this.btnUsuario.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.btnUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuario.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsuario.ForeColor = System.Drawing.Color.White;
            this.btnUsuario.Location = new System.Drawing.Point(0, 367);
            this.btnUsuario.Name = "btnUsuario";
            this.btnUsuario.Size = new System.Drawing.Size(200, 35);
            this.btnUsuario.TabIndex = 4;
            this.btnUsuario.Text = "Usuario";
            this.btnUsuario.UseVisualStyleBackColor = true;
            this.btnUsuario.Click += new System.EventHandler(this.btnUsuario_Click_1);
            // 
            // btnMaestro
            // 
            this.btnMaestro.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMaestro.FlatAppearance.BorderSize = 0;
            this.btnMaestro.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.btnMaestro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaestro.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaestro.ForeColor = System.Drawing.Color.White;
            this.btnMaestro.Location = new System.Drawing.Point(0, 332);
            this.btnMaestro.Name = "btnMaestro";
            this.btnMaestro.Size = new System.Drawing.Size(200, 35);
            this.btnMaestro.TabIndex = 3;
            this.btnMaestro.Text = "Maestros";
            this.btnMaestro.UseVisualStyleBackColor = true;
            // 
            // panelAdminSubmenu
            // 
            this.panelAdminSubmenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panelAdminSubmenu.Controls.Add(this.panel9);
            this.panelAdminSubmenu.Controls.Add(this.panel8);
            this.panelAdminSubmenu.Controls.Add(this.panel7);
            this.panelAdminSubmenu.Controls.Add(this.panel6);
            this.panelAdminSubmenu.Controls.Add(this.panel5);
            this.panelAdminSubmenu.Controls.Add(this.panel4);
            this.panelAdminSubmenu.Controls.Add(this.btnDigitoVerificador);
            this.panelAdminSubmenu.Controls.Add(this.btnBitacora);
            this.panelAdminSubmenu.Controls.Add(this.btnRestore);
            this.panelAdminSubmenu.Controls.Add(this.btnBackUp);
            this.panelAdminSubmenu.Controls.Add(this.btnPerfiles);
            this.panelAdminSubmenu.Controls.Add(this.btnUsuarios);
            this.panelAdminSubmenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAdminSubmenu.Location = new System.Drawing.Point(0, 114);
            this.panelAdminSubmenu.Name = "panelAdminSubmenu";
            this.panelAdminSubmenu.Size = new System.Drawing.Size(200, 218);
            this.panelAdminSubmenu.TabIndex = 2;
            this.panelAdminSubmenu.Visible = false;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Maroon;
            this.panel9.Location = new System.Drawing.Point(12, 70);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(5, 35);
            this.panel9.TabIndex = 7;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Maroon;
            this.panel8.Location = new System.Drawing.Point(12, 105);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(5, 35);
            this.panel8.TabIndex = 7;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Maroon;
            this.panel7.Location = new System.Drawing.Point(12, 140);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(5, 35);
            this.panel7.TabIndex = 7;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Maroon;
            this.panel6.Location = new System.Drawing.Point(12, 175);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(5, 35);
            this.panel6.TabIndex = 7;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Maroon;
            this.panel5.Location = new System.Drawing.Point(12, 35);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(5, 35);
            this.panel5.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Maroon;
            this.panel4.Location = new System.Drawing.Point(12, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(5, 35);
            this.panel4.TabIndex = 7;
            // 
            // btnDigitoVerificador
            // 
            this.btnDigitoVerificador.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDigitoVerificador.FlatAppearance.BorderSize = 0;
            this.btnDigitoVerificador.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDigitoVerificador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigitoVerificador.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDigitoVerificador.ForeColor = System.Drawing.Color.White;
            this.btnDigitoVerificador.Location = new System.Drawing.Point(0, 175);
            this.btnDigitoVerificador.Name = "btnDigitoVerificador";
            this.btnDigitoVerificador.Size = new System.Drawing.Size(200, 35);
            this.btnDigitoVerificador.TabIndex = 5;
            this.btnDigitoVerificador.Text = "Dig. Ver.";
            this.btnDigitoVerificador.UseVisualStyleBackColor = true;
            this.btnDigitoVerificador.Click += new System.EventHandler(this.btnDigitoVerificador_Click);
            // 
            // btnBitacora
            // 
            this.btnBitacora.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBitacora.FlatAppearance.BorderSize = 0;
            this.btnBitacora.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBitacora.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBitacora.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBitacora.ForeColor = System.Drawing.Color.White;
            this.btnBitacora.Location = new System.Drawing.Point(0, 140);
            this.btnBitacora.Name = "btnBitacora";
            this.btnBitacora.Size = new System.Drawing.Size(200, 35);
            this.btnBitacora.TabIndex = 4;
            this.btnBitacora.Text = "Bitacora";
            this.btnBitacora.UseVisualStyleBackColor = true;
            this.btnBitacora.Click += new System.EventHandler(this.btnBitacora_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.ForeColor = System.Drawing.Color.White;
            this.btnRestore.Location = new System.Drawing.Point(0, 105);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(200, 35);
            this.btnRestore.TabIndex = 3;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnBackUp
            // 
            this.btnBackUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBackUp.FlatAppearance.BorderSize = 0;
            this.btnBackUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnBackUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackUp.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackUp.ForeColor = System.Drawing.Color.White;
            this.btnBackUp.Location = new System.Drawing.Point(0, 70);
            this.btnBackUp.Name = "btnBackUp";
            this.btnBackUp.Size = new System.Drawing.Size(200, 35);
            this.btnBackUp.TabIndex = 2;
            this.btnBackUp.Text = "Backup";
            this.btnBackUp.UseVisualStyleBackColor = true;
            this.btnBackUp.Click += new System.EventHandler(this.btnBackUp_Click);
            // 
            // btnPerfiles
            // 
            this.btnPerfiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPerfiles.FlatAppearance.BorderSize = 0;
            this.btnPerfiles.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnPerfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPerfiles.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerfiles.ForeColor = System.Drawing.Color.White;
            this.btnPerfiles.Location = new System.Drawing.Point(0, 35);
            this.btnPerfiles.Name = "btnPerfiles";
            this.btnPerfiles.Size = new System.Drawing.Size(200, 35);
            this.btnPerfiles.TabIndex = 1;
            this.btnPerfiles.Text = "Perfiles";
            this.btnPerfiles.UseVisualStyleBackColor = true;
            this.btnPerfiles.Click += new System.EventHandler(this.btnPerfiles_Click);
            // 
            // btnUsuarios
            // 
            this.btnUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUsuarios.FlatAppearance.BorderSize = 0;
            this.btnUsuarios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuarios.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnUsuarios.Location = new System.Drawing.Point(0, 0);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Size = new System.Drawing.Size(200, 35);
            this.btnUsuarios.TabIndex = 0;
            this.btnUsuarios.Text = "Usuarios";
            this.btnUsuarios.UseVisualStyleBackColor = true;
            this.btnUsuarios.Click += new System.EventHandler(this.btnUsuarios_Click);
            // 
            // btnAdmin
            // 
            this.btnAdmin.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdmin.FlatAppearance.BorderSize = 0;
            this.btnAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.ForeColor = System.Drawing.Color.White;
            this.btnAdmin.Location = new System.Drawing.Point(0, 79);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(200, 35);
            this.btnAdmin.TabIndex = 1;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelContenedor
            // 
            this.panelContenedor.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panelContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenedor.Location = new System.Drawing.Point(200, 43);
            this.panelContenedor.Name = "panelContenedor";
            this.panelContenedor.Size = new System.Drawing.Size(1049, 698);
            this.panelContenedor.TabIndex = 2;
            this.panelContenedor.Paint += new System.Windows.Forms.PaintEventHandler(this.panelContenedor_Paint);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(1249, 741);
            this.Controls.Add(this.panelContenedor);
            this.Controls.Add(this.MenuVertical);
            this.Controls.Add(this.BarraTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.BarraTitulo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimizar)).EndInit();
            this.MenuVertical.ResumeLayout(false);
            this.panelMembresiaSubmenu.ResumeLayout(false);
            this.panelUsuarioSubmenu.ResumeLayout(false);
            this.panelAdminSubmenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Panel MenuVertical;
        private System.Windows.Forms.Panel panelContenedor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelAdminSubmenu;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnBackUp;
        private System.Windows.Forms.Button btnPerfiles;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Button btnDigitoVerificador;
        private System.Windows.Forms.Button btnBitacora;
        private System.Windows.Forms.Panel panelUsuarioSubmenu;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnCambiarClave;
        private System.Windows.Forms.Button btnReLogin;
        private System.Windows.Forms.Button btnUsuario;
        private System.Windows.Forms.Button btnMaestro;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox btnMinimizar;
        private System.Windows.Forms.PictureBox btnCerrar;
        private System.Windows.Forms.PictureBox btnMaximizar;
        private System.Windows.Forms.Button btnCambiarIdioma;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panelMembresiaSubmenu;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Button btnRegistrarSocio;
        private System.Windows.Forms.Button btnRenovarMembresia;
        private System.Windows.Forms.Button button1;
    }
}

