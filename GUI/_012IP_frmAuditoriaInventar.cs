using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _012IP_GUI
{
    public partial class _012IP_frmAuditoriaInventario : Form, IObserver
    {
        private _012IP_AuditoriaBLL auditoriaBLL;
        private List<_012IP_EjemplarBE> esperados = new List<_012IP_EjemplarBE>();
        private _012IP_ConciliacionBE conciliacion;
        private string seccionAuditada;
        private bool actualizandoGrilla;

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmAuditoriaInventario()
        {
            InitializeComponent();
        }

        private void _012IP_frmAuditoriaInventario_Load(object sender, EventArgs e)
        {
            try
            {
                auditoriaBLL = new _012IP_AuditoriaBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }

            LanguageManager.Instance.AgregarObservador(this);
            _012IP_ConfigurarGrilla(dgvEsperados);
            _012IP_ConfigurarGrilla(dgvDiferencias);
            _012IP_CargarSecciones();
            _012IP_Reiniciar();
            Actualizar(LanguageManager.Instance);
            _012IP_Info(_012IP_Texto("AudSeleccioneSeccion"));
        }

        private void _012IP_frmAuditoriaInventario_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

        // ------------------------------------------------------------------ idioma

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("AudTitulo");
            grpSeccion.Text = _012IP_Texto("AudGrpSeleccion");
            lblSeccionAud.Text = _012IP_Texto("InvSeccion");
            btnGenerar.Text = _012IP_Texto("AudBtnGenerar");
            grpEsperado.Text = _012IP_Texto("AudGrpEsperado");
            grpEncontrado.Text = _012IP_Texto("AudGrpEncontrado");
            btnConciliar.Text = _012IP_Texto("AudBtnConciliar");
            grpDiferencias.Text = _012IP_Texto("AudGrpDiferencias");
            btnBajaPerdida.Text = _012IP_Texto("AudBtnBaja");
            btnReubicar.Text = _012IP_Texto("AudBtnReubicar");
            btnCerrarAuditoria.Text = _012IP_Texto("AudBtnCerrar");
            btnSalir.Text = _012IP_Texto("btnSalir");

            _012IP_BindEsperados();
            _012IP_BindDiferencias();
            _012IP_ActualizarResumen();
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        // ------------------------------------------------------------------ mensajes

        private void _012IP_Info(string m) { lblMsj.ForeColor = Color.White; lblMsj.Text = m; }
        private void _012IP_Ok(string m) { lblMsj.ForeColor = Color.FromArgb(156, 255, 156); lblMsj.Text = m; }
        private void _012IP_Error(string m) { lblMsj.ForeColor = Color.FromArgb(255, 156, 156); lblMsj.Text = m; }

        private void _012IP_MostrarError(Exception ex)
        {
            MessageBox.Show(ex.Message, _012IP_Texto("RenMemError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ------------------------------------------------------------------ estado del proceso

        private void _012IP_ConfigurarGrilla(DataGridView grilla)
        {
            grilla.ReadOnly = true;
            grilla.AllowUserToAddRows = false;
            grilla.AllowUserToDeleteRows = false;
            grilla.AllowUserToResizeRows = false;
            grilla.MultiSelect = false;
            grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grilla.RowHeadersVisible = false;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grilla.BackgroundColor = Color.White;
            grilla.BorderStyle = BorderStyle.None;
            grilla.EnableHeadersVisualStyles = false;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grilla.DefaultCellStyle.BackColor = Color.White;
            grilla.DefaultCellStyle.ForeColor = Color.Black;
            grilla.DefaultCellStyle.SelectionBackColor = Color.Maroon;
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void _012IP_CargarSecciones()
        {
            try
            {
                cmbSeccion.DataSource = null;
                cmbSeccion.DataSource = auditoriaBLL._012IP_ObtenerSecciones();
                cmbSeccion.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        /// <summary>Vuelve al paso 1 del proceso: elegir seccion y generar el listado esperado.</summary>
        private void _012IP_Reiniciar()
        {
            esperados = new List<_012IP_EjemplarBE>();
            conciliacion = null;
            seccionAuditada = null;
            txtEncontrados.Clear();
            txtEncontrados.ReadOnly = true;
            _012IP_BindEsperados();
            _012IP_BindDiferencias();
            _012IP_ActualizarResumen();
            _012IP_ActualizarBotones();
        }

        private void _012IP_ActualizarBotones()
        {
            bool listado = seccionAuditada != null;
            bool conciliado = conciliacion != null;
            btnConciliar.Enabled = listado && !conciliado;
            btnCerrarAuditoria.Enabled = conciliado;

            _012IP_DiferenciaAuditoriaBE d = _012IP_DiferenciaSeleccionada();
            bool resoluble = d != null && d.Resultado == _012IP_AuditoriaBLL.RES_FALTANTE && d.Resolucion == "";
            btnBajaPerdida.Enabled = resoluble;
            btnReubicar.Enabled = resoluble;
        }

        private void _012IP_ActualizarResumen()
        {
            if (conciliacion == null)
            {
                lblResumen.Text = seccionAuditada == null
                    ? ""
                    : string.Format(_012IP_Texto("AudGenerado"), esperados.Count, seccionAuditada);
                return;
            }

            lblResumen.Text = string.Format(_012IP_Texto("AudResumen"),
                conciliacion.Esperados, conciliacion.Correctos, conciliacion.Faltantes, conciliacion.Sobrantes);
        }

        private void _012IP_BindEsperados()
        {
            dgvEsperados.DataSource = esperados.Select(x => new
            {
                CodigoBarras = x.CodigoBarras,
                NumeroInventario = x.NumeroInventario,
                Titulo = x.Titulo,
                Ubicacion = _012IP_EjemplarBLL._012IP_TextoUbicacion(x)
            }).ToList();

            if (dgvEsperados.Columns.Contains("CodigoBarras")) dgvEsperados.Columns["CodigoBarras"].HeaderText = _012IP_Texto("InvCodigoBarras");
            if (dgvEsperados.Columns.Contains("NumeroInventario")) dgvEsperados.Columns["NumeroInventario"].HeaderText = _012IP_Texto("InvNumeroInventario");
            if (dgvEsperados.Columns.Contains("Titulo")) dgvEsperados.Columns["Titulo"].HeaderText = _012IP_Texto("InvTituloLibro");
            if (dgvEsperados.Columns.Contains("Ubicacion")) dgvEsperados.Columns["Ubicacion"].HeaderText = _012IP_Texto("InvColUbicacion");
        }

        private void _012IP_BindDiferencias()
        {
            string previo = _012IP_DiferenciaSeleccionada()?.CodigoBarras;
            List<_012IP_DiferenciaAuditoriaBE> lista = conciliacion?.Diferencias ?? new List<_012IP_DiferenciaAuditoriaBE>();

            actualizandoGrilla = true;
            try
            {
                dgvDiferencias.DataSource = lista.Select(d => new
                {
                    CodigoBarras = d.CodigoBarras,
                    NumeroInventario = d.NumeroInventario,
                    Titulo = d.Titulo,
                    Resultado = _012IP_AuditoriaBLL._012IP_TextoResultado(d.Resultado),
                    Detalle = d.Detalle,
                    Resolucion = _012IP_AuditoriaBLL._012IP_TextoResolucion(d)
                }).ToList();

                if (dgvDiferencias.Columns.Contains("CodigoBarras")) dgvDiferencias.Columns["CodigoBarras"].HeaderText = _012IP_Texto("InvCodigoBarras");
                if (dgvDiferencias.Columns.Contains("NumeroInventario")) dgvDiferencias.Columns["NumeroInventario"].HeaderText = _012IP_Texto("InvNumeroInventario");
                if (dgvDiferencias.Columns.Contains("Titulo")) dgvDiferencias.Columns["Titulo"].HeaderText = _012IP_Texto("InvTituloLibro");
                if (dgvDiferencias.Columns.Contains("Resultado")) dgvDiferencias.Columns["Resultado"].HeaderText = _012IP_Texto("AudColResultado");
                if (dgvDiferencias.Columns.Contains("Detalle")) dgvDiferencias.Columns["Detalle"].HeaderText = _012IP_Texto("AudColDetalle");
                if (dgvDiferencias.Columns.Contains("Resolucion")) dgvDiferencias.Columns["Resolucion"].HeaderText = _012IP_Texto("AudColResolucion");

                dgvDiferencias.ClearSelection();
                dgvDiferencias.CurrentCell = null;

                if (!string.IsNullOrEmpty(previo))
                {
                    foreach (DataGridViewRow fila in dgvDiferencias.Rows)
                    {
                        if (Convert.ToString(fila.Cells["CodigoBarras"].Value) == previo)
                        {
                            dgvDiferencias.CurrentCell = fila.Cells[0];
                            fila.Selected = true;
                            break;
                        }
                    }
                }
            }
            finally
            {
                actualizandoGrilla = false;
            }
            _012IP_ActualizarBotones();
        }

        private _012IP_DiferenciaAuditoriaBE _012IP_DiferenciaSeleccionada()
        {
            if (conciliacion == null || dgvDiferencias.CurrentRow == null || !dgvDiferencias.CurrentRow.Selected)
                return null;

            string codigo = Convert.ToString(dgvDiferencias.CurrentRow.Cells["CodigoBarras"].Value);
            return conciliacion.Diferencias.FirstOrDefault(d => d.CodigoBarras == codigo);
        }

        private void _012IP_dgvDiferencias_SelectionChanged(object sender, EventArgs e)
        {
            if (actualizandoGrilla) return;
            _012IP_ActualizarBotones();
        }

        // ------------------------------------------------------------------ pasos del proceso

        private void _012IP_cmbSeccion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _012IP_Reiniciar();
            _012IP_Info(_012IP_Texto("AudSeleccioneSeccion"));
        }

        private void _012IP_btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                string seccion = cmbSeccion.SelectedItem as string;
                if (string.IsNullOrWhiteSpace(seccion))
                {
                    _012IP_Error(_012IP_Texto("AudBLLSeccion"));
                    return;
                }

                _012IP_Reiniciar();
                esperados = auditoriaBLL._012IP_GenerarListadoEsperado(seccion);

                if (esperados.Count == 0)
                {
                    _012IP_BindEsperados();
                    _012IP_Error(_012IP_Texto("AudSinEsperados"));
                    return;
                }

                seccionAuditada = seccion;
                txtEncontrados.ReadOnly = false;
                _012IP_BindEsperados();
                _012IP_ActualizarResumen();
                _012IP_ActualizarBotones();
                _012IP_Info(_012IP_Texto("AudCargueEncontrados"));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnConciliar_Click(object sender, EventArgs e)
        {
            try
            {
                conciliacion = auditoriaBLL._012IP_Conciliar(seccionAuditada, txtEncontrados.Lines);
                txtEncontrados.ReadOnly = true;
                _012IP_BindDiferencias();
                _012IP_ActualizarResumen();
                _012IP_Ok(_012IP_Texto("AudConciliado"));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnBajaPerdida_Click(object sender, EventArgs e)
        {
            _012IP_DiferenciaAuditoriaBE d = _012IP_DiferenciaSeleccionada();
            if (d == null || d.Resultado != _012IP_AuditoriaBLL.RES_FALTANTE || d.Resolucion != "")
            {
                _012IP_Error(_012IP_Texto("AudSeleccioneDiferencia"));
                return;
            }

            var decision = MessageBox.Show(
                string.Format(_012IP_Texto("AudConfirmaBaja"), d.CodigoBarras, d.Titulo),
                _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (decision != DialogResult.Yes) return;

            try
            {
                auditoriaBLL._012IP_ResolverFaltanteBaja(d);
                _012IP_BindDiferencias();
                _012IP_Ok(string.Format(_012IP_Texto("AudBajaOk"), d.CodigoBarras));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnReubicar_Click(object sender, EventArgs e)
        {
            _012IP_DiferenciaAuditoriaBE d = _012IP_DiferenciaSeleccionada();
            if (d == null || d.Resultado != _012IP_AuditoriaBLL.RES_FALTANTE || d.Resolucion != "")
            {
                _012IP_Error(_012IP_Texto("AudSeleccioneDiferencia"));
                return;
            }

            string pasillo, seccion, estanteria;
            if (!_012IP_PedirUbicacion(out pasillo, out seccion, out estanteria))
                return;

            try
            {
                auditoriaBLL._012IP_ResolverFaltanteReubicar(d, pasillo, seccion, estanteria);
                _012IP_BindDiferencias();
                _012IP_Ok(string.Format(_012IP_Texto("AudReubicadoOk"), d.CodigoBarras));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private void _012IP_btnCerrarAuditoria_Click(object sender, EventArgs e)
        {
            if (conciliacion == null) return;

            var decision = MessageBox.Show(
                string.Format(_012IP_Texto("AudConfirmaCierre"), conciliacion.Seccion, conciliacion.FaltantesSinResolver),
                _012IP_Texto("MsjConfirmacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (decision != DialogResult.Yes) return;

            try
            {
                _012IP_AuditoriaBE auditoria = auditoriaBLL._012IP_CerrarAuditoria(conciliacion);

                MessageBox.Show(_012IP_Texto("AudCerradaOk"), _012IP_Texto("MsjConfirmacion"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    _012IP_StockBE stock = auditoriaBLL._012IP_EmitirInforme(auditoria);
                    string informe = auditoriaBLL._012IP_GenerarInforme(auditoria, conciliacion, stock);
                    using (var frm = new _012IP_frmInformeInventario(auditoria, conciliacion, stock, informe))
                        frm.ShowDialog(this);
                }
                catch (Exception exInforme)
                {
                    _012IP_Error(exInforme.Message);
                }

                _012IP_CargarSecciones();
                _012IP_Reiniciar();
                _012IP_Ok(_012IP_Texto("AudCerradaOk"));
            }
            catch (Exception ex)
            {
                _012IP_Error(ex.Message);
            }
        }

        private bool _012IP_PedirUbicacion(out string pasillo, out string seccion, out string estanteria)
        {
            pasillo = seccion = estanteria = "";

            using (var dlg = new Form())
            {
                dlg.Text = _012IP_Texto("AudDlgUbicacionTitulo");
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ShowInTaskbar = false;
                dlg.ClientSize = new Size(320, 235);
                dlg.BackColor = Color.FromArgb(0, 0, 64);
                dlg.ForeColor = Color.White;

                var lblP = new Label { Text = _012IP_Texto("InvPasillo"), Location = new Point(15, 15), AutoSize = true };
                var txtP = new TextBox { Location = new Point(15, 35), Size = new Size(290, 22), MaxLength = 20, BackColor = Color.White, ForeColor = Color.Black };
                var lblS = new Label { Text = _012IP_Texto("InvSeccion"), Location = new Point(15, 70), AutoSize = true };
                var txtS = new TextBox { Location = new Point(15, 90), Size = new Size(290, 22), MaxLength = 30, BackColor = Color.White, ForeColor = Color.Black };
                var lblE = new Label { Text = _012IP_Texto("InvEstanteria"), Location = new Point(15, 125), AutoSize = true };
                var txtE = new TextBox { Location = new Point(15, 145), Size = new Size(290, 22), MaxLength = 20, BackColor = Color.White, ForeColor = Color.Black };

                var btnOk = new Button { Text = _012IP_Texto("AudDlgAceptar"), Location = new Point(15, 190), Size = new Size(140, 30), DialogResult = DialogResult.OK, BackColor = Color.Maroon, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                var btnNo = new Button { Text = _012IP_Texto("btnCancelar"), Location = new Point(165, 190), Size = new Size(140, 30), DialogResult = DialogResult.Cancel, BackColor = Color.Maroon, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                btnOk.FlatAppearance.BorderSize = 0;
                btnNo.FlatAppearance.BorderSize = 0;

                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnNo;
                dlg.Controls.AddRange(new Control[] { lblP, txtP, lblS, txtS, lblE, txtE, btnOk, btnNo });

                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return false;

                pasillo = txtP.Text;
                seccion = txtS.Text;
                estanteria = txtE.Text;
                return true;
            }
        }

        // ------------------------------------------------------------------ navegacion / barra de titulo

        private void _012IP_btnSalir_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Menu"] != null)
            {
                Application.OpenForms["Menu"].Show();
            }
            else
            {
                GUI.Menu menu = new GUI.Menu();
                menu.Show();
            }
            this.Close();
        }

        private void _012IP_btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                this.Location = new Point(this.Location.X + (e.X - posX), this.Location.Y + (e.Y - posY));
        }

        private void _012IP_BarraTitulo_MouseUp(object sender, MouseEventArgs e)
        {
            arrastrando = false;
        }
    }
}