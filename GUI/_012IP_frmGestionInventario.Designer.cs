using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    partial class _012IP_frmGestionInventario
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.BarraTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblStock = new System.Windows.Forms.Label();
            this.grpIngreso = new System.Windows.Forms.GroupBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblNumeroInv = new System.Windows.Forms.Label();
            this.txtNumeroInv = new System.Windows.Forms.TextBox();
            this.lblTituloLibro = new System.Windows.Forms.Label();
            this.txtTituloLibro = new System.Windows.Forms.TextBox();
            this.lblAutor = new System.Windows.Forms.Label();
            this.txtAutor = new System.Windows.Forms.TextBox();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.cmbOrigen = new System.Windows.Forms.ComboBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.grpUbicacion = new System.Windows.Forms.GroupBox();
            this.lblPasillo = new System.Windows.Forms.Label();
            this.txtPasillo = new System.Windows.Forms.TextBox();
            this.lblSeccion = new System.Windows.Forms.Label();
            this.txtSeccion = new System.Windows.Forms.TextBox();
            this.lblEstanteria = new System.Windows.Forms.Label();
            this.txtEstanteria = new System.Windows.Forms.TextBox();
            this.btnUbicar = new System.Windows.Forms.Button();
            this.grpEstado = new System.Windows.Forms.GroupBox();
            this.lblNuevoEstado = new System.Windows.Forms.Label();
            this.cmbNuevoEstado = new System.Windows.Forms.ComboBox();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.btnCambiarEstado = new System.Windows.Forms.Button();
            this.btnBaja = new System.Windows.Forms.Button();
            this.lblFiltro = new System.Windows.Forms.Label();
            this.cmbFiltro = new System.Windows.Forms.ComboBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblSeleccionado = new System.Windows.Forms.Label();
            this.dgvEjemplares = new System.Windows.Forms.DataGridView();
            this.grpXml = new System.Windows.Forms.GroupBox();
            this.txtRutaSer = new System.Windows.Forms.TextBox();
            this.btnExaminarSer = new System.Windows.Forms.Button();
            this.btnSerializar = new System.Windows.Forms.Button();
            this.txtRutaDes = new System.Windows.Forms.TextBox();
            this.btnExaminarDes = new System.Windows.Forms.Button();
            this.btnDesserializar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.lblMsj = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.BarraTitulo.SuspendLayout();
            this.grpIngreso.SuspendLayout();
            this.grpUbicacion.SuspendLayout();
            this.grpEstado.SuspendLayout();
            this.grpXml.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjemplares)).BeginInit();
            this.SuspendLayout();
            // 
            // BarraTitulo
            // 
            this.BarraTitulo.BackColor = System.Drawing.Color.Maroon;
            this.BarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.BarraTitulo.Name = "BarraTitulo";
            this.BarraTitulo.Size = new System.Drawing.Size(1100, 43);
            this.BarraTitulo.TabIndex = 0;
            this.BarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseDown);
            this.BarraTitulo.MouseMove += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseMove);
            this.BarraTitulo.MouseUp += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseUp);
            this.BarraTitulo.Controls.Add(this.lblTitulo);
            this.BarraTitulo.Controls.Add(this.btnMinimizar);
            this.BarraTitulo.Controls.Add(this.btnCerrar);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 8);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(160, 24);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de inventario";
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.BackColor = System.Drawing.Color.Maroon;
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(1026, 5);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(32, 32);
            this.btnMinimizar.TabIndex = 1;
            this.btnMinimizar.Text = "–";
            this.btnMinimizar.UseVisualStyleBackColor = false;
            this.btnMinimizar.Click += new System.EventHandler(this._012IP_btnMinimizar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Maroon;
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(1062, 5);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(32, 32);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this._012IP_btnCerrar_Click);
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStock.Location = new System.Drawing.Point(12, 54);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(400, 18);
            this.lblStock.TabIndex = 1;
            this.lblStock.Text = "Stock";
            // 
            // grpIngreso
            // 
            this.grpIngreso.Location = new System.Drawing.Point(12, 82);
            this.grpIngreso.Name = "grpIngreso";
            this.grpIngreso.Size = new System.Drawing.Size(420, 232);
            this.grpIngreso.TabStop = false;
            this.grpIngreso.TabIndex = 2;
            this.grpIngreso.Text = "Registrar ingreso de ejemplar";
            this.grpIngreso.Controls.Add(this.lblCodigo);
            this.grpIngreso.Controls.Add(this.txtCodigo);
            this.grpIngreso.Controls.Add(this.lblNumeroInv);
            this.grpIngreso.Controls.Add(this.txtNumeroInv);
            this.grpIngreso.Controls.Add(this.lblTituloLibro);
            this.grpIngreso.Controls.Add(this.txtTituloLibro);
            this.grpIngreso.Controls.Add(this.lblAutor);
            this.grpIngreso.Controls.Add(this.txtAutor);
            this.grpIngreso.Controls.Add(this.lblOrigen);
            this.grpIngreso.Controls.Add(this.cmbOrigen);
            this.grpIngreso.Controls.Add(this.btnRegistrar);
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(15, 25);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(60, 16);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código de barras";
            // 
            // txtCodigo
            // 
            this.txtCodigo.BackColor = System.Drawing.Color.White;
            this.txtCodigo.ForeColor = System.Drawing.Color.Black;
            this.txtCodigo.Location = new System.Drawing.Point(15, 45);
            this.txtCodigo.MaxLength = 13;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(185, 22);
            this.txtCodigo.TabIndex = 1;
            // 
            // lblNumeroInv
            // 
            this.lblNumeroInv.AutoSize = true;
            this.lblNumeroInv.Location = new System.Drawing.Point(215, 25);
            this.lblNumeroInv.Name = "lblNumeroInv";
            this.lblNumeroInv.Size = new System.Drawing.Size(60, 16);
            this.lblNumeroInv.TabIndex = 2;
            this.lblNumeroInv.Text = "Nº de inventario";
            // 
            // txtNumeroInv
            // 
            this.txtNumeroInv.BackColor = System.Drawing.Color.White;
            this.txtNumeroInv.ForeColor = System.Drawing.Color.Black;
            this.txtNumeroInv.Location = new System.Drawing.Point(215, 45);
            this.txtNumeroInv.MaxLength = 20;
            this.txtNumeroInv.Name = "txtNumeroInv";
            this.txtNumeroInv.Size = new System.Drawing.Size(185, 22);
            this.txtNumeroInv.TabIndex = 3;
            // 
            // lblTituloLibro
            // 
            this.lblTituloLibro.AutoSize = true;
            this.lblTituloLibro.Location = new System.Drawing.Point(15, 78);
            this.lblTituloLibro.Name = "lblTituloLibro";
            this.lblTituloLibro.Size = new System.Drawing.Size(60, 16);
            this.lblTituloLibro.TabIndex = 4;
            this.lblTituloLibro.Text = "Título";
            // 
            // txtTituloLibro
            // 
            this.txtTituloLibro.BackColor = System.Drawing.Color.White;
            this.txtTituloLibro.ForeColor = System.Drawing.Color.Black;
            this.txtTituloLibro.Location = new System.Drawing.Point(15, 98);
            this.txtTituloLibro.MaxLength = 150;
            this.txtTituloLibro.Name = "txtTituloLibro";
            this.txtTituloLibro.Size = new System.Drawing.Size(385, 22);
            this.txtTituloLibro.TabIndex = 5;
            // 
            // lblAutor
            // 
            this.lblAutor.AutoSize = true;
            this.lblAutor.Location = new System.Drawing.Point(15, 131);
            this.lblAutor.Name = "lblAutor";
            this.lblAutor.Size = new System.Drawing.Size(60, 16);
            this.lblAutor.TabIndex = 6;
            this.lblAutor.Text = "Autor";
            // 
            // txtAutor
            // 
            this.txtAutor.BackColor = System.Drawing.Color.White;
            this.txtAutor.ForeColor = System.Drawing.Color.Black;
            this.txtAutor.Location = new System.Drawing.Point(15, 151);
            this.txtAutor.MaxLength = 100;
            this.txtAutor.Name = "txtAutor";
            this.txtAutor.Size = new System.Drawing.Size(185, 22);
            this.txtAutor.TabIndex = 7;
            // 
            // lblOrigen
            // 
            this.lblOrigen.AutoSize = true;
            this.lblOrigen.Location = new System.Drawing.Point(215, 131);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(60, 16);
            this.lblOrigen.TabIndex = 8;
            this.lblOrigen.Text = "Origen";
            // 
            // cmbOrigen
            // 
            this.cmbOrigen.BackColor = System.Drawing.Color.White;
            this.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigen.ForeColor = System.Drawing.Color.Black;
            this.cmbOrigen.FormattingEnabled = true;
            this.cmbOrigen.Location = new System.Drawing.Point(215, 151);
            this.cmbOrigen.Name = "cmbOrigen";
            this.cmbOrigen.Size = new System.Drawing.Size(185, 24);
            this.cmbOrigen.TabIndex = 9;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.BackColor = System.Drawing.Color.Maroon;
            this.btnRegistrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegistrar.FlatAppearance.BorderSize = 0;
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.Location = new System.Drawing.Point(15, 190);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(385, 32);
            this.btnRegistrar.TabIndex = 10;
            this.btnRegistrar.Text = "Registrar ingreso";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Click += new System.EventHandler(this._012IP_btnRegistrar_Click);
            // 
            // grpUbicacion
            // 
            this.grpUbicacion.Location = new System.Drawing.Point(440, 82);
            this.grpUbicacion.Name = "grpUbicacion";
            this.grpUbicacion.Size = new System.Drawing.Size(300, 232);
            this.grpUbicacion.TabStop = false;
            this.grpUbicacion.TabIndex = 3;
            this.grpUbicacion.Text = "Ubicación topográfica";
            this.grpUbicacion.Controls.Add(this.lblPasillo);
            this.grpUbicacion.Controls.Add(this.txtPasillo);
            this.grpUbicacion.Controls.Add(this.lblSeccion);
            this.grpUbicacion.Controls.Add(this.txtSeccion);
            this.grpUbicacion.Controls.Add(this.lblEstanteria);
            this.grpUbicacion.Controls.Add(this.txtEstanteria);
            this.grpUbicacion.Controls.Add(this.btnUbicar);
            // 
            // lblPasillo
            // 
            this.lblPasillo.AutoSize = true;
            this.lblPasillo.Location = new System.Drawing.Point(15, 25);
            this.lblPasillo.Name = "lblPasillo";
            this.lblPasillo.Size = new System.Drawing.Size(60, 16);
            this.lblPasillo.TabIndex = 0;
            this.lblPasillo.Text = "Pasillo";
            // 
            // txtPasillo
            // 
            this.txtPasillo.BackColor = System.Drawing.Color.White;
            this.txtPasillo.ForeColor = System.Drawing.Color.Black;
            this.txtPasillo.Location = new System.Drawing.Point(15, 45);
            this.txtPasillo.MaxLength = 20;
            this.txtPasillo.Name = "txtPasillo";
            this.txtPasillo.Size = new System.Drawing.Size(125, 22);
            this.txtPasillo.TabIndex = 1;
            // 
            // lblSeccion
            // 
            this.lblSeccion.AutoSize = true;
            this.lblSeccion.Location = new System.Drawing.Point(155, 25);
            this.lblSeccion.Name = "lblSeccion";
            this.lblSeccion.Size = new System.Drawing.Size(60, 16);
            this.lblSeccion.TabIndex = 2;
            this.lblSeccion.Text = "Sección";
            // 
            // txtSeccion
            // 
            this.txtSeccion.BackColor = System.Drawing.Color.White;
            this.txtSeccion.ForeColor = System.Drawing.Color.Black;
            this.txtSeccion.Location = new System.Drawing.Point(155, 45);
            this.txtSeccion.MaxLength = 30;
            this.txtSeccion.Name = "txtSeccion";
            this.txtSeccion.Size = new System.Drawing.Size(130, 22);
            this.txtSeccion.TabIndex = 3;
            // 
            // lblEstanteria
            // 
            this.lblEstanteria.AutoSize = true;
            this.lblEstanteria.Location = new System.Drawing.Point(15, 78);
            this.lblEstanteria.Name = "lblEstanteria";
            this.lblEstanteria.Size = new System.Drawing.Size(60, 16);
            this.lblEstanteria.TabIndex = 4;
            this.lblEstanteria.Text = "Estantería";
            // 
            // txtEstanteria
            // 
            this.txtEstanteria.BackColor = System.Drawing.Color.White;
            this.txtEstanteria.ForeColor = System.Drawing.Color.Black;
            this.txtEstanteria.Location = new System.Drawing.Point(15, 98);
            this.txtEstanteria.MaxLength = 20;
            this.txtEstanteria.Name = "txtEstanteria";
            this.txtEstanteria.Size = new System.Drawing.Size(270, 22);
            this.txtEstanteria.TabIndex = 5;
            // 
            // btnUbicar
            // 
            this.btnUbicar.BackColor = System.Drawing.Color.Maroon;
            this.btnUbicar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUbicar.FlatAppearance.BorderSize = 0;
            this.btnUbicar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUbicar.ForeColor = System.Drawing.Color.White;
            this.btnUbicar.Location = new System.Drawing.Point(15, 190);
            this.btnUbicar.Name = "btnUbicar";
            this.btnUbicar.Size = new System.Drawing.Size(270, 32);
            this.btnUbicar.TabIndex = 6;
            this.btnUbicar.Text = "Ubicar ejemplar";
            this.btnUbicar.UseVisualStyleBackColor = false;
            this.btnUbicar.Click += new System.EventHandler(this._012IP_btnUbicar_Click);
            // 
            // grpEstado
            // 
            this.grpEstado.Location = new System.Drawing.Point(748, 82);
            this.grpEstado.Name = "grpEstado";
            this.grpEstado.Size = new System.Drawing.Size(340, 232);
            this.grpEstado.TabStop = false;
            this.grpEstado.TabIndex = 4;
            this.grpEstado.Text = "Estado físico";
            this.grpEstado.Controls.Add(this.lblNuevoEstado);
            this.grpEstado.Controls.Add(this.cmbNuevoEstado);
            this.grpEstado.Controls.Add(this.lblMotivo);
            this.grpEstado.Controls.Add(this.txtMotivo);
            this.grpEstado.Controls.Add(this.btnCambiarEstado);
            this.grpEstado.Controls.Add(this.btnBaja);
            // 
            // lblNuevoEstado
            // 
            this.lblNuevoEstado.AutoSize = true;
            this.lblNuevoEstado.Location = new System.Drawing.Point(15, 25);
            this.lblNuevoEstado.Name = "lblNuevoEstado";
            this.lblNuevoEstado.Size = new System.Drawing.Size(60, 16);
            this.lblNuevoEstado.TabIndex = 0;
            this.lblNuevoEstado.Text = "Nuevo estado";
            // 
            // cmbNuevoEstado
            // 
            this.cmbNuevoEstado.BackColor = System.Drawing.Color.White;
            this.cmbNuevoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNuevoEstado.ForeColor = System.Drawing.Color.Black;
            this.cmbNuevoEstado.FormattingEnabled = true;
            this.cmbNuevoEstado.Location = new System.Drawing.Point(15, 45);
            this.cmbNuevoEstado.Name = "cmbNuevoEstado";
            this.cmbNuevoEstado.Size = new System.Drawing.Size(310, 24);
            this.cmbNuevoEstado.TabIndex = 1;
            // 
            // lblMotivo
            // 
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Location = new System.Drawing.Point(15, 78);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(60, 16);
            this.lblMotivo.TabIndex = 2;
            this.lblMotivo.Text = "Motivo (obligatorio)";
            // 
            // txtMotivo
            // 
            this.txtMotivo.BackColor = System.Drawing.Color.White;
            this.txtMotivo.ForeColor = System.Drawing.Color.Black;
            this.txtMotivo.Location = new System.Drawing.Point(15, 98);
            this.txtMotivo.MaxLength = 100;
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(310, 22);
            this.txtMotivo.TabIndex = 3;
            // 
            // btnCambiarEstado
            // 
            this.btnCambiarEstado.BackColor = System.Drawing.Color.Maroon;
            this.btnCambiarEstado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCambiarEstado.FlatAppearance.BorderSize = 0;
            this.btnCambiarEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarEstado.ForeColor = System.Drawing.Color.White;
            this.btnCambiarEstado.Location = new System.Drawing.Point(15, 190);
            this.btnCambiarEstado.Name = "btnCambiarEstado";
            this.btnCambiarEstado.Size = new System.Drawing.Size(150, 32);
            this.btnCambiarEstado.TabIndex = 4;
            this.btnCambiarEstado.Text = "Cambiar estado";
            this.btnCambiarEstado.UseVisualStyleBackColor = false;
            this.btnCambiarEstado.Click += new System.EventHandler(this._012IP_btnCambiarEstado_Click);
            // 
            // btnBaja
            // 
            this.btnBaja.BackColor = System.Drawing.Color.Maroon;
            this.btnBaja.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBaja.FlatAppearance.BorderSize = 0;
            this.btnBaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBaja.ForeColor = System.Drawing.Color.White;
            this.btnBaja.Location = new System.Drawing.Point(175, 190);
            this.btnBaja.Name = "btnBaja";
            this.btnBaja.Size = new System.Drawing.Size(150, 32);
            this.btnBaja.TabIndex = 5;
            this.btnBaja.Text = "Dar de baja";
            this.btnBaja.UseVisualStyleBackColor = false;
            this.btnBaja.Click += new System.EventHandler(this._012IP_btnBaja_Click);
            // 
            // lblFiltro
            // 
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Location = new System.Drawing.Point(12, 331);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(60, 16);
            this.lblFiltro.TabIndex = 5;
            this.lblFiltro.Text = "Filtrar por";
            // 
            // cmbFiltro
            // 
            this.cmbFiltro.BackColor = System.Drawing.Color.White;
            this.cmbFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltro.ForeColor = System.Drawing.Color.Black;
            this.cmbFiltro.FormattingEnabled = true;
            this.cmbFiltro.Location = new System.Drawing.Point(85, 327);
            this.cmbFiltro.Name = "cmbFiltro";
            this.cmbFiltro.Size = new System.Drawing.Size(170, 24);
            this.cmbFiltro.TabIndex = 6;
            this.cmbFiltro.SelectionChangeCommitted += new System.EventHandler(this._012IP_cmbFiltro_SelectionChangeCommitted);
            // 
            // txtBuscar
            // 
            this.txtBuscar.BackColor = System.Drawing.Color.White;
            this.txtBuscar.ForeColor = System.Drawing.Color.Black;
            this.txtBuscar.Location = new System.Drawing.Point(268, 328);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(230, 22);
            this.txtBuscar.TabIndex = 7;
            this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this._012IP_txtBuscar_KeyDown);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.Maroon;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(508, 324);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 28);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this._012IP_btnBuscar_Click);
            // 
            // lblSeleccionado
            // 
            this.lblSeleccionado.AutoEllipsis = true;
            this.lblSeleccionado.Location = new System.Drawing.Point(620, 331);
            this.lblSeleccionado.Name = "lblSeleccionado";
            this.lblSeleccionado.Size = new System.Drawing.Size(468, 18);
            this.lblSeleccionado.TabIndex = 9;
            this.lblSeleccionado.Text = "";
            // 
            // dgvEjemplares
            // 
            this.dgvEjemplares.Location = new System.Drawing.Point(12, 360);
            this.dgvEjemplares.Name = "dgvEjemplares";
            this.dgvEjemplares.Size = new System.Drawing.Size(1076, 232);
            this.dgvEjemplares.TabIndex = 10;
            this.dgvEjemplares.SelectionChanged += new System.EventHandler(this._012IP_dgvEjemplares_SelectionChanged);
            // 
            // grpXml
            // 
            this.grpXml.Location = new System.Drawing.Point(12, 598);
            this.grpXml.Name = "grpXml";
            this.grpXml.Size = new System.Drawing.Size(1076, 62);
            this.grpXml.TabStop = false;
            this.grpXml.TabIndex = 11;
            this.grpXml.Text = "Serialización XML";
            this.grpXml.Controls.Add(this.txtRutaSer);
            this.grpXml.Controls.Add(this.btnExaminarSer);
            this.grpXml.Controls.Add(this.btnSerializar);
            this.grpXml.Controls.Add(this.txtRutaDes);
            this.grpXml.Controls.Add(this.btnExaminarDes);
            this.grpXml.Controls.Add(this.btnDesserializar);
            this.grpXml.Controls.Add(this.btnLimpiar);
            // 
            // txtRutaSer
            // 
            this.txtRutaSer.BackColor = System.Drawing.Color.White;
            this.txtRutaSer.ForeColor = System.Drawing.Color.Black;
            this.txtRutaSer.Location = new System.Drawing.Point(15, 28);
            this.txtRutaSer.ReadOnly = true;
            this.txtRutaSer.Name = "txtRutaSer";
            this.txtRutaSer.Size = new System.Drawing.Size(255, 22);
            this.txtRutaSer.TabIndex = 0;
            // 
            // btnExaminarSer
            // 
            this.btnExaminarSer.BackColor = System.Drawing.Color.Maroon;
            this.btnExaminarSer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExaminarSer.FlatAppearance.BorderSize = 0;
            this.btnExaminarSer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExaminarSer.ForeColor = System.Drawing.Color.White;
            this.btnExaminarSer.Location = new System.Drawing.Point(275, 26);
            this.btnExaminarSer.Name = "btnExaminarSer";
            this.btnExaminarSer.Size = new System.Drawing.Size(32, 26);
            this.btnExaminarSer.TabIndex = 1;
            this.btnExaminarSer.Text = "...";
            this.btnExaminarSer.UseVisualStyleBackColor = false;
            this.btnExaminarSer.Click += new System.EventHandler(this._012IP_btnExaminarSer_Click);
            // 
            // btnSerializar
            // 
            this.btnSerializar.BackColor = System.Drawing.Color.Maroon;
            this.btnSerializar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSerializar.FlatAppearance.BorderSize = 0;
            this.btnSerializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerializar.ForeColor = System.Drawing.Color.White;
            this.btnSerializar.Location = new System.Drawing.Point(315, 25);
            this.btnSerializar.Name = "btnSerializar";
            this.btnSerializar.Size = new System.Drawing.Size(135, 28);
            this.btnSerializar.TabIndex = 2;
            this.btnSerializar.Text = "SERIALIZAR";
            this.btnSerializar.UseVisualStyleBackColor = false;
            this.btnSerializar.Click += new System.EventHandler(this._012IP_btnSerializar_Click);
            // 
            // txtRutaDes
            // 
            this.txtRutaDes.BackColor = System.Drawing.Color.White;
            this.txtRutaDes.ForeColor = System.Drawing.Color.Black;
            this.txtRutaDes.Location = new System.Drawing.Point(475, 28);
            this.txtRutaDes.ReadOnly = true;
            this.txtRutaDes.Name = "txtRutaDes";
            this.txtRutaDes.Size = new System.Drawing.Size(255, 22);
            this.txtRutaDes.TabIndex = 3;
            // 
            // btnExaminarDes
            // 
            this.btnExaminarDes.BackColor = System.Drawing.Color.Maroon;
            this.btnExaminarDes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExaminarDes.FlatAppearance.BorderSize = 0;
            this.btnExaminarDes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExaminarDes.ForeColor = System.Drawing.Color.White;
            this.btnExaminarDes.Location = new System.Drawing.Point(735, 26);
            this.btnExaminarDes.Name = "btnExaminarDes";
            this.btnExaminarDes.Size = new System.Drawing.Size(32, 26);
            this.btnExaminarDes.TabIndex = 4;
            this.btnExaminarDes.Text = "...";
            this.btnExaminarDes.UseVisualStyleBackColor = false;
            this.btnExaminarDes.Click += new System.EventHandler(this._012IP_btnExaminarDes_Click);
            // 
            // btnDesserializar
            // 
            this.btnDesserializar.BackColor = System.Drawing.Color.Maroon;
            this.btnDesserializar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDesserializar.FlatAppearance.BorderSize = 0;
            this.btnDesserializar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesserializar.ForeColor = System.Drawing.Color.White;
            this.btnDesserializar.Location = new System.Drawing.Point(775, 25);
            this.btnDesserializar.Name = "btnDesserializar";
            this.btnDesserializar.Size = new System.Drawing.Size(150, 28);
            this.btnDesserializar.TabIndex = 5;
            this.btnDesserializar.Text = "DES-SERIALIZAR";
            this.btnDesserializar.UseVisualStyleBackColor = false;
            this.btnDesserializar.Click += new System.EventHandler(this._012IP_btnDesserializar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.Maroon;
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(935, 25);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(126, 28);
            this.btnLimpiar.TabIndex = 6;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this._012IP_btnLimpiar_Click);
            // 
            // lblMsj
            // 
            this.lblMsj.AutoEllipsis = true;
            this.lblMsj.Location = new System.Drawing.Point(12, 664);
            this.lblMsj.Name = "lblMsj";
            this.lblMsj.Size = new System.Drawing.Size(975, 40);
            this.lblMsj.TabIndex = 12;
            this.lblMsj.Text = "";
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.Maroon;
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(998, 684);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(90, 30);
            this.btnSalir.TabIndex = 13;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this._012IP_btnSalir_Click);
            // 
            // _012IP_frmGestionInventario
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1100, 720);
            this.Controls.Add(this.BarraTitulo);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.grpIngreso);
            this.Controls.Add(this.grpUbicacion);
            this.Controls.Add(this.grpEstado);
            this.Controls.Add(this.lblFiltro);
            this.Controls.Add(this.cmbFiltro);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lblSeleccionado);
            this.Controls.Add(this.dgvEjemplares);
            this.Controls.Add(this.grpXml);
            this.Controls.Add(this.lblMsj);
            this.Controls.Add(this.btnSalir);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "_012IP_frmGestionInventario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de inventario";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._012IP_frmGestionInventario_FormClosed);
            this.Load += new System.EventHandler(this._012IP_frmGestionInventario_Load);
            this.grpXml.ResumeLayout(false);
            this.grpXml.PerformLayout();
            this.grpEstado.ResumeLayout(false);
            this.grpEstado.PerformLayout();
            this.grpUbicacion.ResumeLayout(false);
            this.grpUbicacion.PerformLayout();
            this.grpIngreso.ResumeLayout(false);
            this.grpIngreso.PerformLayout();
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjemplares)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.GroupBox grpIngreso;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNumeroInv;
        private System.Windows.Forms.TextBox txtNumeroInv;
        private System.Windows.Forms.Label lblTituloLibro;
        private System.Windows.Forms.TextBox txtTituloLibro;
        private System.Windows.Forms.Label lblAutor;
        private System.Windows.Forms.TextBox txtAutor;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.GroupBox grpUbicacion;
        private System.Windows.Forms.Label lblPasillo;
        private System.Windows.Forms.TextBox txtPasillo;
        private System.Windows.Forms.Label lblSeccion;
        private System.Windows.Forms.TextBox txtSeccion;
        private System.Windows.Forms.Label lblEstanteria;
        private System.Windows.Forms.TextBox txtEstanteria;
        private System.Windows.Forms.Button btnUbicar;
        private System.Windows.Forms.GroupBox grpEstado;
        private System.Windows.Forms.Label lblNuevoEstado;
        private System.Windows.Forms.ComboBox cmbNuevoEstado;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.TextBox txtMotivo;
        private System.Windows.Forms.Button btnCambiarEstado;
        private System.Windows.Forms.Button btnBaja;
        private System.Windows.Forms.Label lblFiltro;
        private System.Windows.Forms.ComboBox cmbFiltro;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblSeleccionado;
        private System.Windows.Forms.DataGridView dgvEjemplares;
        private System.Windows.Forms.GroupBox grpXml;
        private System.Windows.Forms.TextBox txtRutaSer;
        private System.Windows.Forms.Button btnExaminarSer;
        private System.Windows.Forms.Button btnSerializar;
        private System.Windows.Forms.TextBox txtRutaDes;
        private System.Windows.Forms.Button btnExaminarDes;
        private System.Windows.Forms.Button btnDesserializar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Label lblMsj;
        private System.Windows.Forms.Button btnSalir;
    }
}