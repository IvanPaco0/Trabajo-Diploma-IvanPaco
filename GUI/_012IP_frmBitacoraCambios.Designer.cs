namespace _012IP_GUI
{
    partial class _012IP_frmBitacoraCambios
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
            this.lblDni = new System.Windows.Forms.Label();
            this.cmbDni = new System.Windows.Forms.ComboBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.cmbNombre = new System.Windows.Forms.ComboBox();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.chkDesde = new System.Windows.Forms.CheckBox();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.chkHasta = new System.Windows.Forms.CheckBox();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.lblResumen = new System.Windows.Forms.Label();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.lblMsj = new System.Windows.Forms.Label();
            this.btnActivar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.BarraTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            //
            // BarraTitulo
            //
            this.BarraTitulo.BackColor = System.Drawing.Color.Maroon;
            this.BarraTitulo.Controls.Add(this.lblTitulo);
            this.BarraTitulo.Controls.Add(this.btnMinimizar);
            this.BarraTitulo.Controls.Add(this.btnCerrar);
            this.BarraTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarraTitulo.Location = new System.Drawing.Point(0, 0);
            this.BarraTitulo.Name = "BarraTitulo";
            this.BarraTitulo.Size = new System.Drawing.Size(1000, 43);
            this.BarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseDown);
            this.BarraTitulo.MouseMove += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseMove);
            this.BarraTitulo.MouseUp += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseUp);
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 8);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Bitácora de cambios de socios";
            //
            // btnMinimizar
            //
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(926, 5);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(32, 32);
            this.btnMinimizar.TabStop = false;
            this.btnMinimizar.Text = "–";
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this._012IP_btnMinimizar_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(962, 5);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(32, 32);
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this._012IP_btnCerrar_Click);
            //
            // lblDni
            //
            this.lblDni.AutoSize = true;
            this.lblDni.Location = new System.Drawing.Point(12, 56);
            this.lblDni.Name = "lblDni";
            this.lblDni.Text = "DNI";
            //
            // cmbDni
            //
            this.cmbDni.BackColor = System.Drawing.Color.White;
            this.cmbDni.ForeColor = System.Drawing.Color.Black;
            this.cmbDni.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDni.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDni.Location = new System.Drawing.Point(12, 77);
            this.cmbDni.MaxLength = 10;
            this.cmbDni.Name = "cmbDni";
            this.cmbDni.Size = new System.Drawing.Size(150, 24);
            this.cmbDni.TabIndex = 0;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(175, 56);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Text = "Nombre o apellido";
            //
            // cmbNombre
            //
            this.cmbNombre.BackColor = System.Drawing.Color.White;
            this.cmbNombre.ForeColor = System.Drawing.Color.Black;
            this.cmbNombre.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbNombre.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbNombre.Location = new System.Drawing.Point(175, 77);
            this.cmbNombre.MaxLength = 100;
            this.cmbNombre.Name = "cmbNombre";
            this.cmbNombre.Size = new System.Drawing.Size(230, 24);
            this.cmbNombre.TabIndex = 1;
            //
            // chkDesde
            //
            this.chkDesde.AutoSize = true;
            this.chkDesde.Location = new System.Drawing.Point(420, 55);
            this.chkDesde.Name = "chkDesde";
            this.chkDesde.TabIndex = 2;
            this.chkDesde.Text = "Desde";
            this.chkDesde.UseVisualStyleBackColor = true;
            this.chkDesde.CheckedChanged += new System.EventHandler(this._012IP_chkFecha_CheckedChanged);
            //
            // dtpDesde
            //
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(420, 77);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(130, 22);
            this.dtpDesde.TabIndex = 3;
            //
            // chkHasta
            //
            this.chkHasta.AutoSize = true;
            this.chkHasta.Location = new System.Drawing.Point(565, 55);
            this.chkHasta.Name = "chkHasta";
            this.chkHasta.TabIndex = 4;
            this.chkHasta.Text = "Hasta";
            this.chkHasta.UseVisualStyleBackColor = true;
            this.chkHasta.CheckedChanged += new System.EventHandler(this._012IP_chkFecha_CheckedChanged);
            //
            // dtpHasta
            //
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(565, 77);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(130, 22);
            this.dtpHasta.TabIndex = 5;
            //
            // btnAplicar
            //
            this.btnAplicar.BackColor = System.Drawing.Color.Maroon;
            this.btnAplicar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAplicar.FlatAppearance.BorderSize = 0;
            this.btnAplicar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar.ForeColor = System.Drawing.Color.White;
            this.btnAplicar.Location = new System.Drawing.Point(712, 70);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(134, 34);
            this.btnAplicar.TabIndex = 6;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = false;
            this.btnAplicar.Click += new System.EventHandler(this._012IP_btnAplicar_Click);
            //
            // btnLimpiar
            //
            this.btnLimpiar.BackColor = System.Drawing.Color.Maroon;
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(854, 70);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(134, 34);
            this.btnLimpiar.TabIndex = 7;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this._012IP_btnLimpiar_Click);
            //
            // lblResumen
            //
            this.lblResumen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumen.ForeColor = System.Drawing.Color.LightGreen;
            this.lblResumen.Location = new System.Drawing.Point(12, 112);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(976, 22);
            this.lblResumen.Text = "-";
            //
            // dgvHistorial
            //
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Location = new System.Drawing.Point(12, 140);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.Size = new System.Drawing.Size(976, 400);
            this.dgvHistorial.TabIndex = 8;
            this.dgvHistorial.SelectionChanged += new System.EventHandler(this._012IP_dgvHistorial_SelectionChanged);
            //
            // lblMsj
            //
            this.lblMsj.Location = new System.Drawing.Point(12, 552);
            this.lblMsj.Name = "lblMsj";
            this.lblMsj.Size = new System.Drawing.Size(600, 40);
            this.lblMsj.Text = "";
            //
            // btnActivar
            //
            this.btnActivar.BackColor = System.Drawing.Color.Maroon;
            this.btnActivar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActivar.Enabled = false;
            this.btnActivar.FlatAppearance.BorderSize = 0;
            this.btnActivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivar.ForeColor = System.Drawing.Color.White;
            this.btnActivar.Location = new System.Drawing.Point(628, 552);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(180, 40);
            this.btnActivar.TabIndex = 9;
            this.btnActivar.Text = "Activar versión";
            this.btnActivar.UseVisualStyleBackColor = false;
            this.btnActivar.Click += new System.EventHandler(this._012IP_btnActivar_Click);
            //
            // btnSalir
            //
            this.btnSalir.BackColor = System.Drawing.Color.Maroon;
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(828, 552);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(160, 40);
            this.btnSalir.TabIndex = 10;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this._012IP_btnSalir_Click);
            //
            // _012IP_frmBitacoraCambios
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1000, 606);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnActivar);
            this.Controls.Add(this.lblMsj);
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.lblResumen);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.chkHasta);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.chkDesde);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.cmbNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.cmbDni);
            this.Controls.Add(this.lblDni);
            this.Controls.Add(this.BarraTitulo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "_012IP_frmBitacoraCambios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitácora de cambios";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._012IP_frmBitacoraCambios_FormClosed);
            this.Load += new System.EventHandler(this._012IP_frmBitacoraCambios_Load);
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblDni;
        private System.Windows.Forms.ComboBox cmbDni;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.ComboBox cmbNombre;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.CheckBox chkDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.CheckBox chkHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.Label lblMsj;
        private System.Windows.Forms.Button btnActivar;
        private System.Windows.Forms.Button btnSalir;
    }
}
