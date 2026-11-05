using System.Drawing;
using System.Windows.Forms;

namespace _012IP_GUI
{
    partial class _012IP_frmAuditoriaInventario
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
            this.grpSeccion = new System.Windows.Forms.GroupBox();
            this.lblSeccionAud = new System.Windows.Forms.Label();
            this.cmbSeccion = new System.Windows.Forms.ComboBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.lblResumen = new System.Windows.Forms.Label();
            this.grpEsperado = new System.Windows.Forms.GroupBox();
            this.dgvEsperados = new System.Windows.Forms.DataGridView();
            this.grpEncontrado = new System.Windows.Forms.GroupBox();
            this.txtEncontrados = new System.Windows.Forms.TextBox();
            this.btnConciliar = new System.Windows.Forms.Button();
            this.grpDiferencias = new System.Windows.Forms.GroupBox();
            this.dgvDiferencias = new System.Windows.Forms.DataGridView();
            this.btnBajaPerdida = new System.Windows.Forms.Button();
            this.btnReubicar = new System.Windows.Forms.Button();
            this.btnCerrarAuditoria = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.lblMsj = new System.Windows.Forms.Label();
            this.BarraTitulo.SuspendLayout();
            this.grpSeccion.SuspendLayout();
            this.grpEsperado.SuspendLayout();
            this.grpEncontrado.SuspendLayout();
            this.grpDiferencias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEsperados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiferencias)).BeginInit();
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
            this.lblTitulo.Text = "Auditoría de inventario";
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
            // grpSeccion
            // 
            this.grpSeccion.Location = new System.Drawing.Point(12, 55);
            this.grpSeccion.Name = "grpSeccion";
            this.grpSeccion.Size = new System.Drawing.Size(1076, 78);
            this.grpSeccion.TabStop = false;
            this.grpSeccion.TabIndex = 1;
            this.grpSeccion.ForeColor = System.Drawing.Color.White;
            this.grpSeccion.Text = "Sección a auditar";
            this.grpSeccion.Controls.Add(this.lblSeccionAud);
            this.grpSeccion.Controls.Add(this.cmbSeccion);
            this.grpSeccion.Controls.Add(this.btnGenerar);
            this.grpSeccion.Controls.Add(this.lblResumen);
            // 
            // lblSeccionAud
            // 
            this.lblSeccionAud.AutoSize = true;
            this.lblSeccionAud.Location = new System.Drawing.Point(15, 25);
            this.lblSeccionAud.Name = "lblSeccionAud";
            this.lblSeccionAud.Size = new System.Drawing.Size(60, 16);
            this.lblSeccionAud.TabIndex = 0;
            this.lblSeccionAud.Text = "Sección";
            // 
            // cmbSeccion
            // 
            this.cmbSeccion.BackColor = System.Drawing.Color.White;
            this.cmbSeccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSeccion.ForeColor = System.Drawing.Color.Black;
            this.cmbSeccion.FormattingEnabled = true;
            this.cmbSeccion.Location = new System.Drawing.Point(15, 45);
            this.cmbSeccion.Name = "cmbSeccion";
            this.cmbSeccion.Size = new System.Drawing.Size(260, 24);
            this.cmbSeccion.TabIndex = 1;
            this.cmbSeccion.SelectionChangeCommitted += new System.EventHandler(this._012IP_cmbSeccion_SelectionChangeCommitted);
            // 
            // btnGenerar
            // 
            this.btnGenerar.BackColor = System.Drawing.Color.Maroon;
            this.btnGenerar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerar.FlatAppearance.BorderSize = 0;
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.ForeColor = System.Drawing.Color.White;
            this.btnGenerar.Location = new System.Drawing.Point(290, 42);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(240, 30);
            this.btnGenerar.TabIndex = 2;
            this.btnGenerar.Text = "Generar listado esperado";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this._012IP_btnGenerar_Click);
            // 
            // lblResumen
            // 
            this.lblResumen.AutoEllipsis = true;
            this.lblResumen.Location = new System.Drawing.Point(550, 48);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(510, 20);
            this.lblResumen.TabIndex = 3;
            this.lblResumen.Text = "";
            // 
            // grpEsperado
            // 
            this.grpEsperado.Location = new System.Drawing.Point(12, 140);
            this.grpEsperado.Name = "grpEsperado";
            this.grpEsperado.Size = new System.Drawing.Size(520, 300);
            this.grpEsperado.TabStop = false;
            this.grpEsperado.TabIndex = 2;
            this.grpEsperado.ForeColor = System.Drawing.Color.White;
            this.grpEsperado.Text = "Listado esperado";
            this.grpEsperado.Controls.Add(this.dgvEsperados);
            // 
            // dgvEsperados
            // 
            this.dgvEsperados.Location = new System.Drawing.Point(15, 25);
            this.dgvEsperados.Name = "dgvEsperados";
            this.dgvEsperados.Size = new System.Drawing.Size(490, 265);
            this.dgvEsperados.TabIndex = 0;
            // 
            // grpEncontrado
            // 
            this.grpEncontrado.Location = new System.Drawing.Point(540, 140);
            this.grpEncontrado.Name = "grpEncontrado";
            this.grpEncontrado.Size = new System.Drawing.Size(548, 300);
            this.grpEncontrado.TabStop = false;
            this.grpEncontrado.TabIndex = 3;
            this.grpEncontrado.ForeColor = System.Drawing.Color.White;
            this.grpEncontrado.Text = "Ejemplares encontrados (un código de barras por línea)";
            this.grpEncontrado.Controls.Add(this.txtEncontrados);
            this.grpEncontrado.Controls.Add(this.btnConciliar);
            // 
            // txtEncontrados
            // 
            this.txtEncontrados.BackColor = System.Drawing.Color.White;
            this.txtEncontrados.ForeColor = System.Drawing.Color.Black;
            this.txtEncontrados.Location = new System.Drawing.Point(15, 25);
            this.txtEncontrados.AcceptsReturn = true;
            this.txtEncontrados.Multiline = true;
            this.txtEncontrados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEncontrados.Name = "txtEncontrados";
            this.txtEncontrados.Size = new System.Drawing.Size(518, 225);
            this.txtEncontrados.TabIndex = 0;
            // 
            // btnConciliar
            // 
            this.btnConciliar.BackColor = System.Drawing.Color.Maroon;
            this.btnConciliar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConciliar.FlatAppearance.BorderSize = 0;
            this.btnConciliar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConciliar.ForeColor = System.Drawing.Color.White;
            this.btnConciliar.Location = new System.Drawing.Point(15, 258);
            this.btnConciliar.Name = "btnConciliar";
            this.btnConciliar.Size = new System.Drawing.Size(518, 30);
            this.btnConciliar.TabIndex = 1;
            this.btnConciliar.Text = "Conciliar";
            this.btnConciliar.UseVisualStyleBackColor = false;
            this.btnConciliar.Click += new System.EventHandler(this._012IP_btnConciliar_Click);
            // 
            // grpDiferencias
            // 
            this.grpDiferencias.Location = new System.Drawing.Point(12, 448);
            this.grpDiferencias.Name = "grpDiferencias";
            this.grpDiferencias.Size = new System.Drawing.Size(1076, 150);
            this.grpDiferencias.TabStop = false;
            this.grpDiferencias.TabIndex = 4;
            this.grpDiferencias.ForeColor = System.Drawing.Color.White;
            this.grpDiferencias.Text = "Diferencias detectadas";
            this.grpDiferencias.Controls.Add(this.dgvDiferencias);
            // 
            // dgvDiferencias
            // 
            this.dgvDiferencias.Location = new System.Drawing.Point(15, 25);
            this.dgvDiferencias.Name = "dgvDiferencias";
            this.dgvDiferencias.Size = new System.Drawing.Size(1046, 115);
            this.dgvDiferencias.TabIndex = 0;
            this.dgvDiferencias.SelectionChanged += new System.EventHandler(this._012IP_dgvDiferencias_SelectionChanged);
            // 
            // btnBajaPerdida
            // 
            this.btnBajaPerdida.BackColor = System.Drawing.Color.Maroon;
            this.btnBajaPerdida.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBajaPerdida.FlatAppearance.BorderSize = 0;
            this.btnBajaPerdida.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBajaPerdida.ForeColor = System.Drawing.Color.White;
            this.btnBajaPerdida.Location = new System.Drawing.Point(12, 606);
            this.btnBajaPerdida.Name = "btnBajaPerdida";
            this.btnBajaPerdida.Size = new System.Drawing.Size(170, 30);
            this.btnBajaPerdida.TabIndex = 5;
            this.btnBajaPerdida.Text = "Baja por pérdida";
            this.btnBajaPerdida.UseVisualStyleBackColor = false;
            this.btnBajaPerdida.Click += new System.EventHandler(this._012IP_btnBajaPerdida_Click);
            // 
            // btnReubicar
            // 
            this.btnReubicar.BackColor = System.Drawing.Color.Maroon;
            this.btnReubicar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReubicar.FlatAppearance.BorderSize = 0;
            this.btnReubicar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReubicar.ForeColor = System.Drawing.Color.White;
            this.btnReubicar.Location = new System.Drawing.Point(190, 606);
            this.btnReubicar.Name = "btnReubicar";
            this.btnReubicar.Size = new System.Drawing.Size(170, 30);
            this.btnReubicar.TabIndex = 6;
            this.btnReubicar.Text = "Reubicar";
            this.btnReubicar.UseVisualStyleBackColor = false;
            this.btnReubicar.Click += new System.EventHandler(this._012IP_btnReubicar_Click);
            // 
            // btnCerrarAuditoria
            // 
            this.btnCerrarAuditoria.BackColor = System.Drawing.Color.Maroon;
            this.btnCerrarAuditoria.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarAuditoria.FlatAppearance.BorderSize = 0;
            this.btnCerrarAuditoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarAuditoria.ForeColor = System.Drawing.Color.White;
            this.btnCerrarAuditoria.Location = new System.Drawing.Point(368, 606);
            this.btnCerrarAuditoria.Name = "btnCerrarAuditoria";
            this.btnCerrarAuditoria.Size = new System.Drawing.Size(200, 30);
            this.btnCerrarAuditoria.TabIndex = 7;
            this.btnCerrarAuditoria.Text = "Cerrar auditoría";
            this.btnCerrarAuditoria.UseVisualStyleBackColor = false;
            this.btnCerrarAuditoria.Click += new System.EventHandler(this._012IP_btnCerrarAuditoria_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.Maroon;
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(998, 606);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(90, 30);
            this.btnSalir.TabIndex = 8;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this._012IP_btnSalir_Click);
            // 
            // lblMsj
            // 
            this.lblMsj.AutoEllipsis = true;
            this.lblMsj.Location = new System.Drawing.Point(12, 644);
            this.lblMsj.Name = "lblMsj";
            this.lblMsj.Size = new System.Drawing.Size(1076, 26);
            this.lblMsj.TabIndex = 9;
            this.lblMsj.Text = "";
            // 
            // _012IP_frmAuditoriaInventario
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1100, 676);
            this.Controls.Add(this.BarraTitulo);
            this.Controls.Add(this.grpSeccion);
            this.Controls.Add(this.grpEsperado);
            this.Controls.Add(this.grpEncontrado);
            this.Controls.Add(this.grpDiferencias);
            this.Controls.Add(this.btnBajaPerdida);
            this.Controls.Add(this.btnReubicar);
            this.Controls.Add(this.btnCerrarAuditoria);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblMsj);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "_012IP_frmAuditoriaInventario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auditoría de inventario";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._012IP_frmAuditoriaInventario_FormClosed);
            this.Load += new System.EventHandler(this._012IP_frmAuditoriaInventario_Load);
            this.grpDiferencias.ResumeLayout(false);
            this.grpDiferencias.PerformLayout();
            this.grpEncontrado.ResumeLayout(false);
            this.grpEncontrado.PerformLayout();
            this.grpEsperado.ResumeLayout(false);
            this.grpEsperado.PerformLayout();
            this.grpSeccion.ResumeLayout(false);
            this.grpSeccion.PerformLayout();
            this.BarraTitulo.ResumeLayout(false);
            this.BarraTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEsperados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiferencias)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BarraTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpSeccion;
        private System.Windows.Forms.Label lblSeccionAud;
        private System.Windows.Forms.ComboBox cmbSeccion;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.GroupBox grpEsperado;
        private System.Windows.Forms.DataGridView dgvEsperados;
        private System.Windows.Forms.GroupBox grpEncontrado;
        private System.Windows.Forms.TextBox txtEncontrados;
        private System.Windows.Forms.Button btnConciliar;
        private System.Windows.Forms.GroupBox grpDiferencias;
        private System.Windows.Forms.DataGridView dgvDiferencias;
        private System.Windows.Forms.Button btnBajaPerdida;
        private System.Windows.Forms.Button btnReubicar;
        private System.Windows.Forms.Button btnCerrarAuditoria;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblMsj;
    }
}