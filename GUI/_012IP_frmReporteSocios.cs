using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iText = iTextSharp.text;

namespace _012IP_GUI
{
    public partial class _012IP_frmReporteSocios : Form, IObserver
    {
        private readonly System.Windows.Forms.Timer timerBusqueda = new System.Windows.Forms.Timer { Interval = 300 };

        private _012IP_SocioBLL socioBLL;
        private _012IP_MembresiaBLL membresiaBLL;
        private List<_012IP_ReporteSocioBE> filas = new List<_012IP_ReporteSocioBE>();
        private bool cargando;

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmReporteSocios()
        {
            InitializeComponent();
            timerBusqueda.Tick += _012IP_timerBusqueda_Tick;
        }

        private void _012IP_frmReporteSocios_Load(object sender, EventArgs e)
        {
            try
            {
                socioBLL = new _012IP_SocioBLL();
                membresiaBLL = new _012IP_MembresiaBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
                return;
            }

            _012IP_ConfigurarGrilla(dgvSocios);
            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);
            ActiveControl = txtBuscar;
        }

        private void _012IP_frmReporteSocios_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerBusqueda.Stop();
            timerBusqueda.Dispose();
            LanguageManager.Instance.EliminarObservador(this);
        }

        private void _012IP_CargarFiltros()
        {
            cargando = true;
            try
            {
                string estadoPrevio = Convert.ToString(cmbEstado.SelectedValue);
                object membresiaPrevia = cmbMembresia.SelectedValue;

                cmbEstado.DisplayMember = "Value";
                cmbEstado.ValueMember = "Key";
                cmbEstado.DataSource = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(_012IP_SocioBLL.ESTADO_TODOS, _012IP_Texto("RepSocEstTodos")),
                    new KeyValuePair<string, string>(_012IP_SocioBLL.ESTADO_VIGENTE, _012IP_Texto("RepSocEstVigente")),
                    new KeyValuePair<string, string>(_012IP_SocioBLL.ESTADO_POR_VENCER, _012IP_Texto("RepSocEstPorVencer")),
                    new KeyValuePair<string, string>(_012IP_SocioBLL.ESTADO_VENCIDA, _012IP_Texto("RepSocEstVencida")),
                    new KeyValuePair<string, string>(_012IP_SocioBLL.ESTADO_SIN_MEMBRESIA, _012IP_Texto("RepSocEstSinMembresia"))
                };
                if (!string.IsNullOrEmpty(estadoPrevio)) cmbEstado.SelectedValue = estadoPrevio;

                var membresias = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(0, _012IP_Texto("RepSocTodas")) };
                membresias.AddRange(membresiaBLL._012IP_ListarMembresias()
                    .Select(m => new KeyValuePair<int, string>(m.IdMembresia, m.Nombre)));
                cmbMembresia.DisplayMember = "Value";
                cmbMembresia.ValueMember = "Key";
                cmbMembresia.DataSource = membresias;
                if (membresiaPrevia != null) cmbMembresia.SelectedValue = membresiaPrevia;
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
        }

        private void _012IP_txtBuscar_TextChanged(object sender, EventArgs e)
        {
            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private void _012IP_timerBusqueda_Tick(object sender, EventArgs e)
        {
            timerBusqueda.Stop();
            _012IP_Generar();
        }

        private void _012IP_Filtro_Changed(object sender, EventArgs e)
        {
            if (!cargando) _012IP_Generar();
        }

        private void _012IP_Generar()
        {
            if (socioBLL == null) return;
            try
            {
                lblMsj.Text = string.Empty;
                string estado = Convert.ToString(cmbEstado.SelectedValue) ?? _012IP_SocioBLL.ESTADO_TODOS;
                int idMembresia = cmbMembresia.SelectedValue is int id ? id : 0;

                filas = socioBLL._012IP_GenerarReporteSocios(txtBuscar.Text, estado, idMembresia > 0 ? (int?)idMembresia : null);
                _012IP_ResumenReporteSociosBE r = socioBLL._012IP_ResumirReporte(filas);

                lblResumen.Text = string.Format(_012IP_Texto("RepSocResumen"),
                    r.Total, r.Vigentes, r.PorVencer, r.Vencidas, r.SinMembresia);

                dgvSocios.DataSource = filas.Select(f => new
                {
                    DNI = f.DNI,
                    Apellido = f.Apellido,
                    Nombre = f.Nombre,
                    Email = f.Email,
                    Telefono = f.Telefono,
                    Membresia = f.Membresia,
                    Vence = f.FechaVencimiento.HasValue ? f.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "-",
                    Estado = _012IP_TextoEstado(f)
                }).ToList();

                foreach (DataGridViewRow fila in dgvSocios.Rows)
                    fila.DefaultCellStyle.ForeColor = _012IP_ColorEstado(filas[fila.Index].Estado);

                _012IP_AplicarEncabezados();
                dgvSocios.ClearSelection();
                btnExportarPdf.Enabled = filas.Count > 0;
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }
        }

        private string _012IP_TextoEstado(_012IP_ReporteSocioBE f)
        {
            switch (f.Estado)
            {
                case _012IP_SocioBLL.ESTADO_VENCIDA:
                    return string.Format(_012IP_Texto("InfMemVencida"), -(f.DiasRestantes ?? 0));
                case _012IP_SocioBLL.ESTADO_POR_VENCER:
                    return string.Format(_012IP_Texto("InfMemPorVencer"), f.DiasRestantes ?? 0);
                case _012IP_SocioBLL.ESTADO_SIN_MEMBRESIA:
                    return _012IP_Texto("RepSocEstSinMembresia");
                default:
                    return _012IP_Texto("RepSocEstVigente");
            }
        }

        private Color _012IP_ColorEstado(string estado)
        {
            switch (estado)
            {
                case _012IP_SocioBLL.ESTADO_VENCIDA: return Color.DarkRed;
                case _012IP_SocioBLL.ESTADO_POR_VENCER: return Color.DarkGoldenrod;
                case _012IP_SocioBLL.ESTADO_SIN_MEMBRESIA: return Color.DimGray;
                default: return Color.DarkGreen;
            }
        }

        private void _012IP_btnExportarPdf_Click(object sender, EventArgs e)
        {
            if (filas.Count == 0)
            {
                lblMsj.ForeColor = Color.White;
                lblMsj.Text = _012IP_Texto("RepSocSinDatos");
                return;
            }

            using (var dialogo = new SaveFileDialog
            {
                Filter = "PDF|*.pdf",
                FileName = "Reporte_Socios_" + DateTime.Now.ToString("yyyyMMdd_HHmm")
            })
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    _012IP_GenerarPdf(dialogo.FileName);
                    socioBLL._012IP_RegistrarExportacionReporte(filas.Count, dialogo.FileName);
                    lblMsj.ForeColor = Color.LightGreen;
                    lblMsj.Text = string.Format(_012IP_Texto("RepSocPdfOk"), Path.GetFileName(dialogo.FileName));
                }
                catch (Exception ex)
                {
                    _012IP_MostrarError(ex);
                }
            }
        }

        private void _012IP_GenerarPdf(string archivo)
        {
            var doc = new iText.Document(iText.PageSize.A4.Rotate(), 25f, 25f, 25f, 25f);
            using (var fs = new FileStream(archivo, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                iText.Font fTitulo = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA_BOLD, 16);
                iText.Font fTexto = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA, 9);
                iText.Font fNegrita = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA_BOLD, 9);
                iText.Font fHeader = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA_BOLD, 9, iText.BaseColor.WHITE);
                iText.Font fCelda = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA, 8);
                var colorHeader = new iText.BaseColor(128, 0, 0);

                doc.Add(new iText.Paragraph("Alejandría - " + _012IP_Texto("RepSocTitulo"), fTitulo));
                string usuario = SessionManager.Instance.UsuarioActual()?.Username ?? "-";
                doc.Add(new iText.Paragraph(string.Format(_012IP_Texto("RepSocPdfGenerado"),
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"), usuario), fTexto));
                doc.Add(new iText.Paragraph(string.Format(_012IP_Texto("RepSocPdfFiltros"),
                    string.IsNullOrWhiteSpace(txtBuscar.Text) ? "-" : txtBuscar.Text.Trim(),
                    cmbEstado.Text, cmbMembresia.Text), fTexto));
                doc.Add(new iText.Paragraph(lblResumen.Text, fNegrita));
                doc.Add(new iText.Paragraph(" "));

                string[] columnas =
                {
                    _012IP_Texto("DNI"), _012IP_Texto("lblApe"), _012IP_Texto("lblNombre"), _012IP_Texto("lblEmail"),
                    _012IP_Texto("RenMemTelefono"), _012IP_Texto("RenMemMembresia"), _012IP_Texto("RenMemVence"),
                    _012IP_Texto("InfMemColEstado")
                };
                var tabla = new PdfPTable(columnas.Length) { WidthPercentage = 100 };
                tabla.SetWidths(new float[] { 9, 12, 12, 22, 11, 10, 9, 15 });
                tabla.HeaderRows = 1;

                foreach (string c in columnas)
                    tabla.AddCell(new PdfPCell(new iText.Phrase(c, fHeader)) { BackgroundColor = colorHeader, Padding = 4 });

                foreach (_012IP_ReporteSocioBE f in filas)
                {
                    tabla.AddCell(new PdfPCell(new iText.Phrase(f.DNI, fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(f.Apellido, fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(f.Nombre, fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(f.Email, fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(f.Telefono, fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(f.Membresia, fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(
                        f.FechaVencimiento.HasValue ? f.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "-", fCelda)) { Padding = 3 });
                    tabla.AddCell(new PdfPCell(new iText.Phrase(_012IP_TextoEstado(f), fCelda)) { Padding = 3 });
                }

                doc.Add(tabla);
                doc.Close();
            }
        }

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

        private void _012IP_AplicarEncabezados()
        {
            _012IP_Encabezado("DNI", "DNI");
            _012IP_Encabezado("Apellido", "lblApe");
            _012IP_Encabezado("Nombre", "lblNombre");
            _012IP_Encabezado("Email", "lblEmail");
            _012IP_Encabezado("Telefono", "RenMemTelefono");
            _012IP_Encabezado("Membresia", "RenMemMembresia");
            _012IP_Encabezado("Vence", "RenMemVence");
            _012IP_Encabezado("Estado", "InfMemColEstado");
        }

        private void _012IP_Encabezado(string columna, string clave)
        {
            if (dgvSocios.Columns.Contains(columna))
                dgvSocios.Columns[columna].HeaderText = _012IP_Texto(clave);
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private void _012IP_MostrarError(Exception ex)
        {
            lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
            lblMsj.Text = ex.InnerException != null ? ex.Message + " (" + ex.InnerException.Message + ")" : ex.Message;
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("RepSocTitulo");
            lblBuscar.Text = _012IP_Texto("RenMemBuscarAyuda");
            lblEstado.Text = _012IP_Texto("RepSocEstado");
            lblMembresia.Text = _012IP_Texto("RenMemTipo");
            btnExportarPdf.Text = _012IP_Texto("RepSocExportar");
            btnSalir.Text = _012IP_Texto("btnSalir");
            _012IP_CargarFiltros();
            _012IP_Generar();
        }

        private void _012IP_btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
            GUI.Menu menu = new GUI.Menu();
            menu.Show();
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
