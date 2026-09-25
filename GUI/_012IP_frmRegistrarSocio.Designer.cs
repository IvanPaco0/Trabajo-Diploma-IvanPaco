using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    partial class _012IP_frmRegistrarSocio
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
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblMsj = new System.Windows.Forms.Label();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.BarraTitulo.SuspendLayout();
            this.grpDatos.SuspendLayout();
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
            this.lblTitulo.Text = "Registrar socio";
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
            this.grpDatos.Controls.Add(this.lblDNI);
            this.grpDatos.Controls.Add(this.txtDNI);
            this.grpDatos.Controls.Add(this.lblNombre);
            this.grpDatos.Controls.Add(this.txtNombre);
            this.grpDatos.Controls.Add(this.lblApellido);
            this.grpDatos.Controls.Add(this.txtApellido);
            this.grpDatos.Controls.Add(this.lblEmail);
            this.grpDatos.Controls.Add(this.txtEmail);
            this.grpDatos.Controls.Add(this.lblTelefono);
            this.grpDatos.Controls.Add(this.txtTelefono);
            this.grpDatos.Location = new System.Drawing.Point(12, 55);
            this.grpDatos.Name = "grpDatos";
            this.grpDatos.Size = new System.Drawing.Size(456, 285);
            this.grpDatos.TabIndex = 1;
            this.grpDatos.TabStop = false;
            this.grpDatos.Text = "Datos del socio";
            //
            // lblDNI
            //
            this.lblDNI.AutoSize = true;
            this.lblDNI.Location = new System.Drawing.Point(15, 28);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(29, 16);
            this.lblDNI.TabIndex = 0;
            this.lblDNI.Text = "DNI";
            //
            // txtDNI
            //
            this.txtDNI.BackColor = System.Drawing.Color.White;
            this.txtDNI.ForeColor = System.Drawing.Color.Black;
            this.txtDNI.Location = new System.Drawing.Point(15, 48);
            this.txtDNI.MaxLength = 10;
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(420, 22);
            this.txtDNI.TabIndex = 1;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(15, 78);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(55, 16);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre";
            //
            // txtNombre
            //
            this.txtNombre.BackColor = System.Drawing.Color.White;
            this.txtNombre.ForeColor = System.Drawing.Color.Black;
            this.txtNombre.Location = new System.Drawing.Point(15, 98);
            this.txtNombre.MaxLength = 100;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(420, 22);
            this.txtNombre.TabIndex = 3;
            //
            // lblApellido
            //
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(15, 128);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(58, 16);
            this.lblApellido.TabIndex = 4;
            this.lblApellido.Text = "Apellido";
            //
            // txtApellido
            //
            this.txtApellido.BackColor = System.Drawing.Color.White;
            this.txtApellido.ForeColor = System.Drawing.Color.Black;
            this.txtApellido.Location = new System.Drawing.Point(15, 148);
            this.txtApellido.MaxLength = 100;
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(420, 22);
            this.txtApellido.TabIndex = 5;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(15, 178);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(40, 16);
            this.lblEmail.TabIndex = 6;
            this.lblEmail.Text = "Email";
            //
            // txtEmail
            //
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.ForeColor = System.Drawing.Color.Black;
            this.txtEmail.Location = new System.Drawing.Point(15, 198);
            this.txtEmail.MaxLength = 150;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(420, 22);
            this.txtEmail.TabIndex = 7;
            //
            // lblTelefono
            //
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(15, 228);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(58, 16);
            this.lblTelefono.TabIndex = 8;
            this.lblTelefono.Text = "Teléfono";
            //
            // txtTelefono
            //
            this.txtTelefono.BackColor = System.Drawing.Color.White;
            this.txtTelefono.ForeColor = System.Drawing.Color.Black;
            this.txtTelefono.Location = new System.Drawing.Point(15, 248);
            this.txtTelefono.MaxLength = 20;
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(420, 22);
            this.txtTelefono.TabIndex = 9;
            //
            // lblMsj
            //
            this.lblMsj.ForeColor = System.Drawing.Color.White;
            this.lblMsj.Location = new System.Drawing.Point(12, 348);
            this.lblMsj.Name = "lblMsj";
            this.lblMsj.Size = new System.Drawing.Size(456, 55);
            this.lblMsj.TabIndex = 2;
            //
            // btnRegistrar
            //
            this.btnRegistrar.BackColor = System.Drawing.Color.Maroon;
            this.btnRegistrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegistrar.FlatAppearance.BorderSize = 0;
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.Location = new System.Drawing.Point(12, 408);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(220, 38);
            this.btnRegistrar.TabIndex = 3;
            this.btnRegistrar.Text = "Registrar socio";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Click += new System.EventHandler(this._012IP_btnRegistrar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.Maroon;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(248, 408);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(220, 38);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this._012IP_btnCancelar_Click);
            //
            // _012IP_frmRegistrarSocio
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(480, 460);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.lblMsj);
            this.Controls.Add(this.grpDatos);
            this.Controls.Add(this.BarraTitulo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "_012IP_frmRegistrarSocio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar socio";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._012IP_frmRegistrarSocio_FormClosed);
            this.Load += new System.EventHandler(this._012IP_frmRegistrarSocio_Load);
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            this.grpDatos.ResumeLayout(false);
            this.grpDatos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpDatos;
        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblMsj;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
