using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    partial class _012IP_frmRegistrarPago
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
            this.grpDatos = new System.Windows.Forms.GroupBox();
            this.lblSocio = new System.Windows.Forms.Label();
            this.lblSocioValue = new System.Windows.Forms.Label();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.lblPeriodoValue = new System.Windows.Forms.Label();
            this.lblImporte = new System.Windows.Forms.Label();
            this.lblImporteValue = new System.Windows.Forms.Label();
            this.lblMedioPago = new System.Windows.Forms.Label();
            this.cbMedioPago = new System.Windows.Forms.ComboBox();
            this.lblMsj = new System.Windows.Forms.Label();
            this.grpTarjeta = new System.Windows.Forms.GroupBox();
            this.lblNumeroTarjeta = new System.Windows.Forms.Label();
            this.txtNumeroTarjeta = new System.Windows.Forms.TextBox();
            this.lblVencimientoTarjeta = new System.Windows.Forms.Label();
            this.txtVencimientoTarjeta = new System.Windows.Forms.TextBox();
            this.lblCVV = new System.Windows.Forms.Label();
            this.txtCVV = new System.Windows.Forms.TextBox();
            this.lblMarcaValue = new System.Windows.Forms.Label();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.BarraTitulo.SuspendLayout();
            this.grpDatos.SuspendLayout();
            this.grpTarjeta.SuspendLayout();
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
            this.BarraTitulo.Size = new System.Drawing.Size(480, 43);
            this.BarraTitulo.TabIndex = 0;
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
            this.lblTitulo.Size = new System.Drawing.Size(160, 24);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registrar pago";
            //
            // btnMinimizar
            //
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(406, 5);
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
            this.btnCerrar.Location = new System.Drawing.Point(442, 5);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(32, 32);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this._012IP_btnCerrar_Click);
            //
            // grpDatos
            //
            this.grpDatos.Controls.Add(this.lblSocio);
            this.grpDatos.Controls.Add(this.lblSocioValue);
            this.grpDatos.Controls.Add(this.lblPeriodo);
            this.grpDatos.Controls.Add(this.lblPeriodoValue);
            this.grpDatos.Controls.Add(this.lblImporte);
            this.grpDatos.Controls.Add(this.lblImporteValue);
            this.grpDatos.Controls.Add(this.lblMedioPago);
            this.grpDatos.Controls.Add(this.cbMedioPago);
            this.grpDatos.Location = new System.Drawing.Point(12, 55);
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Size = new System.Drawing.Size(456, 235);
            this.grpDatos.TabIndex = 1;
            this.grpDatos.TabStop = false;
            this.grpDatos.ForeColor = System.Drawing.Color.White;
            this.grpDatos.Text = "Datos del pago";
            //
            // lblSocio
            //
            this.lblSocio.AutoSize = true;
            this.lblSocio.Location = new System.Drawing.Point(15, 28);
            this.lblSocio.Name = "lblSocio";
            this.lblSocio.Size = new System.Drawing.Size(43, 16);
            this.lblSocio.TabIndex = 0;
            this.lblSocio.Text = "Socio";
            //
            // lblSocioValue
            //
            this.lblSocioValue.AutoSize = true;
            this.lblSocioValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSocioValue.Location = new System.Drawing.Point(15, 48);
            this.lblSocioValue.Name = "lblSocioValue";
            this.lblSocioValue.Size = new System.Drawing.Size(16, 16);
            this.lblSocioValue.TabIndex = 1;
            this.lblSocioValue.Text = "-";
            //
            // lblPeriodo
            //
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(15, 78);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(53, 16);
            this.lblPeriodo.TabIndex = 2;
            this.lblPeriodo.Text = "Período";
            //
            // lblPeriodoValue
            //
            this.lblPeriodoValue.AutoSize = true;
            this.lblPeriodoValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodoValue.Location = new System.Drawing.Point(15, 98);
            this.lblPeriodoValue.Name = "lblPeriodoValue";
            this.lblPeriodoValue.Size = new System.Drawing.Size(16, 16);
            this.lblPeriodoValue.TabIndex = 3;
            this.lblPeriodoValue.Text = "-";
            //
            // lblImporte
            //
            this.lblImporte.AutoSize = true;
            this.lblImporte.Location = new System.Drawing.Point(15, 128);
            this.lblImporte.Name = "lblImporte";
            this.lblImporte.Size = new System.Drawing.Size(50, 16);
            this.lblImporte.TabIndex = 4;
            this.lblImporte.Text = "Importe";
            //
            // lblImporteValue
            //
            this.lblImporteValue.AutoSize = true;
            this.lblImporteValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImporteValue.ForeColor = System.Drawing.Color.LightGreen;
            this.lblImporteValue.Location = new System.Drawing.Point(15, 148);
            this.lblImporteValue.Name = "lblImporteValue";
            this.lblImporteValue.Size = new System.Drawing.Size(19, 22);
            this.lblImporteValue.TabIndex = 5;
            this.lblImporteValue.Text = "-";
            //
            // lblMedioPago
            //
            this.lblMedioPago.AutoSize = true;
            this.lblMedioPago.Location = new System.Drawing.Point(15, 185);
            this.lblMedioPago.Name = "lblMedioPago";
            this.lblMedioPago.Size = new System.Drawing.Size(94, 16);
            this.lblMedioPago.TabIndex = 6;
            this.lblMedioPago.Text = "Medio de pago";
            //
            // cbMedioPago
            //
            this.cbMedioPago.BackColor = System.Drawing.Color.White;
            this.cbMedioPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMedioPago.ForeColor = System.Drawing.Color.Black;
            this.cbMedioPago.FormattingEnabled = true;
            this.cbMedioPago.Items.AddRange(new object[] {
            "Efectivo",
            "Tarjeta de débito",
            "Tarjeta de crédito",
            "Transferencia"});
            this.cbMedioPago.Location = new System.Drawing.Point(15, 205);
            this.cbMedioPago.Name = "cbMedioPago";
            this.cbMedioPago.Size = new System.Drawing.Size(420, 24);
            this.cbMedioPago.TabIndex = 7;
            this.cbMedioPago.SelectedIndexChanged += new System.EventHandler(this._012IP_cbMedioPago_SelectedIndexChanged);
            //
            // grpTarjeta
            //
            this.grpTarjeta.Controls.Add(this.lblNumeroTarjeta);
            this.grpTarjeta.Controls.Add(this.txtNumeroTarjeta);
            this.grpTarjeta.Controls.Add(this.lblVencimientoTarjeta);
            this.grpTarjeta.Controls.Add(this.txtVencimientoTarjeta);
            this.grpTarjeta.Controls.Add(this.lblCVV);
            this.grpTarjeta.Controls.Add(this.txtCVV);
            this.grpTarjeta.Controls.Add(this.lblMarcaValue);
            this.grpTarjeta.Location = new System.Drawing.Point(12, 296);
            this.grpTarjeta.Name = "grpTarjeta";
            this.grpTarjeta.Size = new System.Drawing.Size(456, 112);
            this.grpTarjeta.TabIndex = 5;
            this.grpTarjeta.TabStop = false;
            this.grpTarjeta.ForeColor = System.Drawing.Color.White;
            this.grpTarjeta.Text = "Datos de la tarjeta";
            this.grpTarjeta.Visible = false;
            //
            // lblNumeroTarjeta
            //
            this.lblNumeroTarjeta.AutoSize = true;
            this.lblNumeroTarjeta.Location = new System.Drawing.Point(15, 24);
            this.lblNumeroTarjeta.Name = "lblNumeroTarjeta";
            this.lblNumeroTarjeta.Size = new System.Drawing.Size(55, 16);
            this.lblNumeroTarjeta.TabIndex = 0;
            this.lblNumeroTarjeta.Text = "Número";
            //
            // txtNumeroTarjeta
            //
            this.txtNumeroTarjeta.Location = new System.Drawing.Point(15, 44);
            this.txtNumeroTarjeta.MaxLength = 23;
            this.txtNumeroTarjeta.Name = "txtNumeroTarjeta";
            this.txtNumeroTarjeta.Size = new System.Drawing.Size(230, 22);
            this.txtNumeroTarjeta.TabIndex = 1;
            //
            // lblVencimientoTarjeta
            //
            this.lblVencimientoTarjeta.AutoSize = true;
            this.lblVencimientoTarjeta.Location = new System.Drawing.Point(260, 24);
            this.lblVencimientoTarjeta.Name = "lblVencimientoTarjeta";
            this.lblVencimientoTarjeta.Size = new System.Drawing.Size(97, 16);
            this.lblVencimientoTarjeta.TabIndex = 2;
            this.lblVencimientoTarjeta.Text = "Vence (MM/AA)";
            //
            // txtVencimientoTarjeta
            //
            this.txtVencimientoTarjeta.Location = new System.Drawing.Point(260, 44);
            this.txtVencimientoTarjeta.MaxLength = 5;
            this.txtVencimientoTarjeta.Name = "txtVencimientoTarjeta";
            this.txtVencimientoTarjeta.Size = new System.Drawing.Size(90, 22);
            this.txtVencimientoTarjeta.TabIndex = 3;
            //
            // lblCVV
            //
            this.lblCVV.AutoSize = true;
            this.lblCVV.Location = new System.Drawing.Point(365, 24);
            this.lblCVV.Name = "lblCVV";
            this.lblCVV.Size = new System.Drawing.Size(34, 16);
            this.lblCVV.TabIndex = 4;
            this.lblCVV.Text = "CVV";
            //
            // txtCVV
            //
            this.txtCVV.Location = new System.Drawing.Point(365, 44);
            this.txtCVV.MaxLength = 4;
            this.txtCVV.Name = "txtCVV";
            this.txtCVV.PasswordChar = '*';
            this.txtCVV.Size = new System.Drawing.Size(70, 22);
            this.txtCVV.TabIndex = 5;
            //
            // lblMarcaValue
            //
            this.lblMarcaValue.AutoSize = true;
            this.lblMarcaValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarcaValue.Location = new System.Drawing.Point(15, 80);
            this.lblMarcaValue.Name = "lblMarcaValue";
            this.lblMarcaValue.Size = new System.Drawing.Size(0, 16);
            this.lblMarcaValue.TabIndex = 6;
            //
            // lblMsj
            //
            this.lblMsj.ForeColor = System.Drawing.Color.White;
            this.lblMsj.Location = new System.Drawing.Point(12, 298);
            this.lblMsj.Name = "lblMsj";
            this.lblMsj.Size = new System.Drawing.Size(456, 55);
            this.lblMsj.TabIndex = 2;
            //
            // btnConfirmar
            //
            this.btnConfirmar.BackColor = System.Drawing.Color.Maroon;
            this.btnConfirmar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmar.FlatAppearance.BorderSize = 0;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(12, 358);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(220, 38);
            this.btnConfirmar.TabIndex = 3;
            this.btnConfirmar.Text = "Confirmar pago";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this._012IP_btnConfirmar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.Maroon;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(248, 358);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(220, 38);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this._012IP_btnCancelar_Click);
            //
            // _012IP_frmRegistrarPago
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(480, 410);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.lblMsj);
            this.Controls.Add(this.grpTarjeta);
            this.Controls.Add(this.grpDatos);
            this.Controls.Add(this.BarraTitulo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "_012IP_frmRegistrarPago";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar pago";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._012IP_frmRegistrarPago_FormClosed);
            this.Load += new System.EventHandler(this._012IP_frmRegistrarPago_Load);
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            this.grpDatos.ResumeLayout(false);
            this.grpDatos.PerformLayout();
            this.grpTarjeta.ResumeLayout(false);
            this.grpTarjeta.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.Label lblSocio;
        private System.Windows.Forms.Label lblSocioValue;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.Label lblPeriodoValue;
        private System.Windows.Forms.Label lblImporte;
        private System.Windows.Forms.Label lblImporteValue;
        private System.Windows.Forms.Label lblMedioPago;
        private System.Windows.Forms.ComboBox cbMedioPago;
        private System.Windows.Forms.Label lblMsj;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox grpTarjeta;
        private System.Windows.Forms.Label lblNumeroTarjeta;
        private System.Windows.Forms.TextBox txtNumeroTarjeta;
        private System.Windows.Forms.Label lblVencimientoTarjeta;
        private System.Windows.Forms.TextBox txtVencimientoTarjeta;
        private System.Windows.Forms.Label lblCVV;
        private System.Windows.Forms.TextBox txtCVV;
        private System.Windows.Forms.Label lblMarcaValue;
    }
}
