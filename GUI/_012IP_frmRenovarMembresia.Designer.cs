using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    partial class _012IP_frmRenovarMembresia
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
            this.grpBusqueda = new System.Windows.Forms.GroupBox();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvSocios = new System.Windows.Forms.DataGridView();
            this.btnRegistrarSocio = new System.Windows.Forms.Button();
            this.btnModificarSocio = new System.Windows.Forms.Button();
            this.grpEstadoCuenta = new System.Windows.Forms.GroupBox();
            this.lblEstadoCuenta = new System.Windows.Forms.Label();
            this.pnlDeuda = new System.Windows.Forms.Panel();
            this.dgvCuotasImpagas = new System.Windows.Forms.DataGridView();
            this.btnRegularizarDeuda = new System.Windows.Forms.Button();
            this.grpRenovacion = new System.Windows.Forms.GroupBox();
            this.lblTipoMembresia = new System.Windows.Forms.Label();
            this.cbMembresias = new System.Windows.Forms.ComboBox();
            this.lblCosto = new System.Windows.Forms.Label();
            this.lblCostoValor = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblPeriodoValor = new System.Windows.Forms.Label();
            this.btnRenovar = new System.Windows.Forms.Button();
            this.lblMeses = new System.Windows.Forms.Label();
            this.nudMeses = new System.Windows.Forms.NumericUpDown();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalValor = new System.Windows.Forms.Label();
            this.lblMsj = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.BarraTitulo.SuspendLayout();
            this.grpBusqueda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocios)).BeginInit();
            this.grpEstadoCuenta.SuspendLayout();
            this.pnlDeuda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuotasImpagas)).BeginInit();
            this.grpRenovacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeses)).BeginInit();
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
            this.BarraTitulo.Size = new System.Drawing.Size(900, 43);
            this.BarraTitulo.TabIndex = 0;
            this.BarraTitulo.MouseDown += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseDown);
            this.BarraTitulo.MouseMove += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseMove);
            this.BarraTitulo.MouseUp += new System.Windows.Forms.MouseEventHandler(this._012IP_BarraTitulo_MouseUp);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 7);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(230, 29);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Renovar membresía";
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(826, 5);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(32, 32);
            this.btnMinimizar.TabIndex = 1;
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
            this.btnCerrar.Location = new System.Drawing.Point(862, 5);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(32, 32);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this._012IP_btnCerrar_Click);
            // 
            // grpBusqueda
            // 
            this.grpBusqueda.Controls.Add(this.lblBuscar);
            this.grpBusqueda.Controls.Add(this.txtBuscar);
            this.grpBusqueda.Controls.Add(this.dgvSocios);
            this.grpBusqueda.Controls.Add(this.btnRegistrarSocio);
            this.grpBusqueda.Controls.Add(this.btnModificarSocio);
            this.grpBusqueda.Location = new System.Drawing.Point(12, 55);
            this.grpBusqueda.Name = "grpBusqueda";
            this.grpBusqueda.Size = new System.Drawing.Size(876, 222);
            this.grpBusqueda.TabIndex = 1;
            this.grpBusqueda.TabStop = false;
            this.grpBusqueda.ForeColor = System.Drawing.Color.White;
            this.grpBusqueda.Text = "Buscar socio";
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(15, 25);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(300, 16);
            this.lblBuscar.TabIndex = 0;
            this.lblBuscar.Text = "Buscar por DNI, nombre o apellido";
            // 
            // txtBuscar
            // 
            this.txtBuscar.BackColor = System.Drawing.Color.White;
            this.txtBuscar.ForeColor = System.Drawing.Color.Black;
            this.txtBuscar.Location = new System.Drawing.Point(15, 46);
            this.txtBuscar.MaxLength = 100;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(520, 22);
            this.txtBuscar.TabIndex = 1;
            this.txtBuscar.TextChanged += new System.EventHandler(this._012IP_txtBuscar_TextChanged);
            this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this._012IP_txtBuscar_KeyDown);
            // 
            // dgvSocios
            // 
            this.dgvSocios.Location = new System.Drawing.Point(15, 82);
            this.dgvSocios.Name = "dgvSocios";
            this.dgvSocios.Size = new System.Drawing.Size(846, 95);
            this.dgvSocios.TabIndex = 7;
            this.dgvSocios.SelectionChanged += new System.EventHandler(this._012IP_dgvSocios_SelectionChanged);
            // 
            // btnRegistrarSocio
            // 
            this.btnRegistrarSocio.BackColor = System.Drawing.Color.Maroon;
            this.btnRegistrarSocio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegistrarSocio.FlatAppearance.BorderSize = 0;
            this.btnRegistrarSocio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrarSocio.ForeColor = System.Drawing.Color.White;
            this.btnRegistrarSocio.Location = new System.Drawing.Point(711, 42);
            this.btnRegistrarSocio.Name = "btnRegistrarSocio";
            this.btnRegistrarSocio.Size = new System.Drawing.Size(150, 28);
            this.btnRegistrarSocio.TabIndex = 8;
            this.btnRegistrarSocio.Text = "Registrar socio";
            this.btnRegistrarSocio.UseVisualStyleBackColor = false;
            this.btnRegistrarSocio.Click += new System.EventHandler(this._012IP_btnRegistrarSocio_Click);
            // 
            // btnModificarSocio
            // 
            this.btnModificarSocio.BackColor = System.Drawing.Color.Maroon;
            this.btnModificarSocio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModificarSocio.FlatAppearance.BorderSize = 0;
            this.btnModificarSocio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificarSocio.ForeColor = System.Drawing.Color.White;
            this.btnModificarSocio.Location = new System.Drawing.Point(551, 42);
            this.btnModificarSocio.Name = "btnModificarSocio";
            this.btnModificarSocio.Size = new System.Drawing.Size(150, 28);
            this.btnModificarSocio.TabIndex = 9;
            this.btnModificarSocio.Text = "Modificar datos";
            this.btnModificarSocio.UseVisualStyleBackColor = false;
            this.btnModificarSocio.Click += new System.EventHandler(this._012IP_btnModificarSocio_Click);
            // 
            // grpEstadoCuenta
            // 
            this.grpEstadoCuenta.Controls.Add(this.lblEstadoCuenta);
            this.grpEstadoCuenta.Controls.Add(this.pnlDeuda);
            this.grpEstadoCuenta.Location = new System.Drawing.Point(12, 287);
            this.grpEstadoCuenta.Name = "grpEstadoCuenta";
            this.grpEstadoCuenta.Size = new System.Drawing.Size(500, 270);
            this.grpEstadoCuenta.TabIndex = 2;
            this.grpEstadoCuenta.TabStop = false;
            this.grpEstadoCuenta.ForeColor = System.Drawing.Color.White;
            this.grpEstadoCuenta.Text = "Estado de cuenta";
            // 
            // lblEstadoCuenta
            // 
            this.lblEstadoCuenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoCuenta.Location = new System.Drawing.Point(15, 25);
            this.lblEstadoCuenta.Name = "lblEstadoCuenta";
            this.lblEstadoCuenta.Size = new System.Drawing.Size(470, 28);
            this.lblEstadoCuenta.TabIndex = 0;
            // 
            // pnlDeuda
            // 
            this.pnlDeuda.Controls.Add(this.dgvCuotasImpagas);
            this.pnlDeuda.Controls.Add(this.btnRegularizarDeuda);
            this.pnlDeuda.Location = new System.Drawing.Point(15, 60);
            this.pnlDeuda.Name = "pnlDeuda";
            this.pnlDeuda.Size = new System.Drawing.Size(470, 195);
            this.pnlDeuda.TabIndex = 1;
            this.pnlDeuda.Visible = false;
            // 
            // dgvCuotasImpagas
            // 
            this.dgvCuotasImpagas.Location = new System.Drawing.Point(0, 0);
            this.dgvCuotasImpagas.Name = "dgvCuotasImpagas";
            this.dgvCuotasImpagas.Size = new System.Drawing.Size(470, 125);
            this.dgvCuotasImpagas.TabIndex = 0;
            // 
            // btnRegularizarDeuda
            // 
            this.btnRegularizarDeuda.BackColor = System.Drawing.Color.Maroon;
            this.btnRegularizarDeuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegularizarDeuda.FlatAppearance.BorderSize = 0;
            this.btnRegularizarDeuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegularizarDeuda.ForeColor = System.Drawing.Color.White;
            this.btnRegularizarDeuda.Location = new System.Drawing.Point(0, 140);
            this.btnRegularizarDeuda.Name = "btnRegularizarDeuda";
            this.btnRegularizarDeuda.Size = new System.Drawing.Size(170, 30);
            this.btnRegularizarDeuda.TabIndex = 1;
            this.btnRegularizarDeuda.Text = "Regularizar deuda";
            this.btnRegularizarDeuda.UseVisualStyleBackColor = false;
            this.btnRegularizarDeuda.Click += new System.EventHandler(this._012IP_btnRegularizarDeuda_Click);
            // 
            // grpRenovacion
            // 
            this.grpRenovacion.Controls.Add(this.lblTipoMembresia);
            this.grpRenovacion.Controls.Add(this.cbMembresias);
            this.grpRenovacion.Controls.Add(this.lblCosto);
            this.grpRenovacion.Controls.Add(this.lblCostoValor);
            this.grpRenovacion.Controls.Add(this.lblPeriodo);
            this.grpRenovacion.Controls.Add(this.lblPeriodoValor);
            this.grpRenovacion.Controls.Add(this.lblMeses);
            this.grpRenovacion.Controls.Add(this.nudMeses);
            this.grpRenovacion.Controls.Add(this.lblTotal);
            this.grpRenovacion.Controls.Add(this.lblTotalValor);
            this.grpRenovacion.Controls.Add(this.btnRenovar);
            this.grpRenovacion.Location = new System.Drawing.Point(522, 287);
            this.grpRenovacion.Name = "grpRenovacion";
            this.grpRenovacion.Size = new System.Drawing.Size(366, 270);
            this.grpRenovacion.TabIndex = 3;
            this.grpRenovacion.TabStop = false;
            this.grpRenovacion.ForeColor = System.Drawing.Color.White;
            this.grpRenovacion.Text = "Renovación";
            // 
            // lblTipoMembresia
            // 
            this.lblTipoMembresia.AutoSize = true;
            this.lblTipoMembresia.Location = new System.Drawing.Point(15, 30);
            this.lblTipoMembresia.Name = "lblTipoMembresia";
            this.lblTipoMembresia.Size = new System.Drawing.Size(110, 16);
            this.lblTipoMembresia.TabIndex = 0;
            this.lblTipoMembresia.Text = "Tipo de membresía";
            // 
            // cbMembresias
            // 
            this.cbMembresias.BackColor = System.Drawing.Color.White;
            this.cbMembresias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMembresias.ForeColor = System.Drawing.Color.Black;
            this.cbMembresias.FormattingEnabled = true;
            this.cbMembresias.Location = new System.Drawing.Point(15, 52);
            this.cbMembresias.Name = "cbMembresias";
            this.cbMembresias.Size = new System.Drawing.Size(335, 24);
            this.cbMembresias.TabIndex = 1;
            this.cbMembresias.SelectedIndexChanged += new System.EventHandler(this._012IP_cbMembresias_SelectedIndexChanged);
            // 
            // lblCosto
            // 
            this.lblCosto.AutoSize = true;
            this.lblCosto.Location = new System.Drawing.Point(15, 118);
            this.lblCosto.Name = "lblCosto";
            this.lblCosto.Size = new System.Drawing.Size(88, 16);
            this.lblCosto.TabIndex = 2;
            this.lblCosto.Text = "Costo mensual";
            // 
            // lblCostoValor
            // 
            this.lblCostoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoValor.Location = new System.Drawing.Point(160, 115);
            this.lblCostoValor.Name = "lblCostoValor";
            this.lblCostoValor.Size = new System.Drawing.Size(190, 22);
            this.lblCostoValor.TabIndex = 3;
            this.lblCostoValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(15, 174);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(88, 16);
            this.lblPeriodo.TabIndex = 4;
            this.lblPeriodo.Text = "Nuevo período";
            // 
            // lblPeriodoValor
            // 
            this.lblPeriodoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodoValor.Location = new System.Drawing.Point(120, 171);
            this.lblPeriodoValor.Name = "lblPeriodoValor";
            this.lblPeriodoValor.Size = new System.Drawing.Size(230, 22);
            this.lblPeriodoValor.TabIndex = 5;
            this.lblPeriodoValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMeses
            // 
            this.lblMeses.AutoSize = true;
            this.lblMeses.Location = new System.Drawing.Point(15, 90);
            this.lblMeses.Name = "lblMeses";
            this.lblMeses.Size = new System.Drawing.Size(92, 16);
            this.lblMeses.TabIndex = 7;
            this.lblMeses.Text = "Meses a pagar";
            // 
            // nudMeses
            // 
            this.nudMeses.Location = new System.Drawing.Point(290, 87);
            this.nudMeses.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.nudMeses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudMeses.Name = "nudMeses";
            this.nudMeses.Size = new System.Drawing.Size(60, 22);
            this.nudMeses.TabIndex = 8;
            this.nudMeses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMeses.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudMeses.ValueChanged += new System.EventHandler(this._012IP_nudMeses_ValueChanged);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(15, 146);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(85, 16);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "Total a pagar";
            // 
            // lblTotalValor
            // 
            this.lblTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalValor.Location = new System.Drawing.Point(160, 143);
            this.lblTotalValor.Name = "lblTotalValor";
            this.lblTotalValor.Size = new System.Drawing.Size(190, 22);
            this.lblTotalValor.TabIndex = 10;
            this.lblTotalValor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRenovar
            // 
            this.btnRenovar.BackColor = System.Drawing.Color.Maroon;
            this.btnRenovar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRenovar.FlatAppearance.BorderSize = 0;
            this.btnRenovar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenovar.ForeColor = System.Drawing.Color.White;
            this.btnRenovar.Location = new System.Drawing.Point(15, 205);
            this.btnRenovar.Name = "btnRenovar";
            this.btnRenovar.Size = new System.Drawing.Size(335, 38);
            this.btnRenovar.TabIndex = 6;
            this.btnRenovar.Text = "Renovar membresía";
            this.btnRenovar.UseVisualStyleBackColor = false;
            this.btnRenovar.Click += new System.EventHandler(this._012IP_btnRenovar_Click);
            // 
            // lblMsj
            // 
            this.lblMsj.Location = new System.Drawing.Point(12, 567);
            this.lblMsj.Name = "lblMsj";
            this.lblMsj.Size = new System.Drawing.Size(770, 40);
            this.lblMsj.TabIndex = 4;
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.Maroon;
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(798, 570);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(90, 30);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this._012IP_btnSalir_Click);
            // 
            // _012IP_frmRenovarMembresia
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblMsj);
            this.Controls.Add(this.grpRenovacion);
            this.Controls.Add(this.grpEstadoCuenta);
            this.Controls.Add(this.grpBusqueda);
            this.Controls.Add(this.BarraTitulo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "_012IP_frmRenovarMembresia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renovar membresía";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._012IP_frmRenovarMembresia_FormClosed);
            this.Load += new System.EventHandler(this._012IP_frmRenovarMembresia_Load);
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            this.grpBusqueda.ResumeLayout(false);
            this.grpBusqueda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocios)).EndInit();
            this.grpEstadoCuenta.ResumeLayout(false);
            this.pnlDeuda.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuotasImpagas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMeses)).EndInit();
            this.grpRenovacion.ResumeLayout(false);
            this.grpRenovacion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpBusqueda;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvSocios;
        private System.Windows.Forms.Button btnRegistrarSocio;
        private System.Windows.Forms.Button btnModificarSocio;
        private System.Windows.Forms.GroupBox grpEstadoCuenta;
        private System.Windows.Forms.Label lblEstadoCuenta;
        private System.Windows.Forms.Panel pnlDeuda;
        private System.Windows.Forms.DataGridView dgvCuotasImpagas;
        private System.Windows.Forms.Button btnRegularizarDeuda;
        private System.Windows.Forms.GroupBox grpRenovacion;
        private System.Windows.Forms.Label lblTipoMembresia;
        private System.Windows.Forms.ComboBox cbMembresias;
        private System.Windows.Forms.Label lblCosto;
        private System.Windows.Forms.Label lblCostoValor;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblPeriodoValor;
        private System.Windows.Forms.Label lblMeses;
        private System.Windows.Forms.NumericUpDown nudMeses;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalValor;
        private System.Windows.Forms.Button btnRenovar;
        private System.Windows.Forms.Label lblMsj;
        private System.Windows.Forms.Button btnSalir;
    }
}
