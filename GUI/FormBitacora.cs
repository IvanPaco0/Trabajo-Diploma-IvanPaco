using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iText = iTextSharp.text;

namespace GUI
{
    public partial class FormBitacora : Form, IObserver
    {
        int posX, posY;
        bool arrastrando = false;
        BitacoraBLL bll;
        public FormBitacora()
        {
            InitializeComponent();
            bll = new BitacoraBLL();

            this.Load += FormBitacora_Load;

            dtpFechaFinal.Value = DateTime.Now;
            dtpFechaInicio.Value = DateTime.Now.AddDays(-3);

            CargarFiltros();

            try
            {
                List<Bitacora> listaInicial = bll.FiltrarEventos(null, null, null, "Todos", null, dtpFechaInicio.Value, dtpFechaFinal.Value, null);

                dtgvBitacora.DataSource = listaInicial;
                ConfigurarDtgv();
                MostrarPrimerRegistro();
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("Bitmsj1"), LanguageManager.Instance.GetTraduction("Bitmsj2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dtgvBitacora.SelectionChanged += DtgvBitacora_SelectionChanged;

        }

        private void MostrarPrimerRegistro()
        {
            if (dtgvBitacora.Rows.Count > 0)
            {
                dtgvBitacora.Rows[0].Selected = true;
                if (dtgvBitacora.Rows[0].DataBoundItem is Bitacora b)
                    MostrarUsuario(b.Login);
            }
            else
            {
                txtNombre.Text = string.Empty;
                txtApellido.Text = string.Empty;
            }
        }
        private void MostrarUsuario(string login)
        {
            try
            {
                Usuario u = bll.ObtenerUsuarioPorLogin(login);
                if (u != null)
                {
                    txtNombre.Text = u.Nombre;
                    txtApellido.Text = u.Apellido;
                }
                else
                {
                    txtNombre.Text = string.Empty;
                    txtApellido.Text = string.Empty;
                }
            }
            catch (Exception)
            {
                txtNombre.Text = string.Empty;
                txtApellido.Text = string.Empty;
            }
        }

        private void CargarFiltros()
        {
            try
            {
                cmbLogin.SelectedIndexChanged -= cmbLogin_SelectedIndexChanged;

                cmbLogin.Items.Clear();
                foreach (var l in bll.ObtenerLogins())
                    cmbLogin.Items.Add(l);

                cmbModulo.SelectedIndexChanged -= cmbModulo_SelectedIndexChanged;
                cmbModulo.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbModulo.DropDownWidth = 200;
                cmbModulo.Items.Clear();
                cmbModulo.Items.Add(CatalogoEventos.Todos);
                foreach (string m in CatalogoEventos.Modulos())
                    cmbModulo.Items.Add(m);
                cmbModulo.SelectedIndex = 0;
                cmbModulo.SelectedIndexChanged += cmbModulo_SelectedIndexChanged;

                cmbEvento.DropDownWidth = 300;
                CargarEventos(null);

                cmbCriticidad.Items.Clear();
                cmbCriticidad.Items.Add("Todos");
                for (int i = 1; i <= 5; i++)
                    cmbCriticidad.Items.Add(i.ToString());
                cmbCriticidad.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj3"), LanguageManager.Instance.GetTraduction("ErrorP"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                cmbLogin.SelectedIndexChanged += cmbLogin_SelectedIndexChanged;
            }
        }

        private void CargarEventos(string modulo)
        {
            cmbEvento.Items.Clear();
            foreach (string ev in CatalogoEventos.EventosDe(modulo))
                cmbEvento.Items.Add(ev);
            cmbEvento.SelectedIndex = -1;
            cmbEvento.Text = string.Empty;
        }

        private void cmbModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarEventos(cmbModulo.SelectedItem?.ToString());
        }

        private void DtgvBitacora_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgvBitacora.CurrentRow?.DataBoundItem is Bitacora b)
                MostrarUsuario(b.Login);
        }


        private void ConfigurarDtgv()
        {
            dtgvBitacora.ReadOnly = true;
            dtgvBitacora.AllowUserToAddRows = false;
            dtgvBitacora.AllowUserToDeleteRows = false;
            dtgvBitacora.AllowUserToOrderColumns = false;
            dtgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dtgvBitacora.Columns["Id_Evento"] != null)
                dtgvBitacora.Columns["Id_Evento"].Visible = false;

            if (dtgvBitacora.Columns["Fecha"] != null)
                dtgvBitacora.Columns["Fecha"].Visible = false;

            if (dtgvBitacora.Columns["Evento"] != null)
                dtgvBitacora.Columns["Evento"].HeaderText = "Evento";

            if (dtgvBitacora.Columns["FechaSolo"] != null)
                dtgvBitacora.Columns["FechaSolo"].HeaderText = "Fecha";

            if (dtgvBitacora.Columns["HoraSolo"] != null)
                dtgvBitacora.Columns["HoraSolo"].HeaderText = "Hora";


            dtgvBitacora.Columns["Login"].DisplayIndex = 0;
            dtgvBitacora.Columns["FechaSolo"].DisplayIndex = 1;
            dtgvBitacora.Columns["HoraSolo"].DisplayIndex = 2;
            dtgvBitacora.Columns["Modulo"].DisplayIndex = 3;
            dtgvBitacora.Columns["Evento"].DisplayIndex = 4;
            dtgvBitacora.Columns["Criticidad"].DisplayIndex = 5;

        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            arrastrando = false;
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Menu"] != null)
            {
                Application.OpenForms["Menu"].Show();
            }
            else
            {
                Menu menu = new Menu();
                menu.Show();
            }
            this.Close();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (!ValidarFiltros()) return;

            try
            {
                string nombre = string.IsNullOrWhiteSpace(txtNombre.Text) ? null : txtNombre.Text.Trim();
                string apellido = string.IsNullOrWhiteSpace(txtApellido.Text) ? null : txtApellido.Text.Trim();
                string login = string.IsNullOrWhiteSpace(cmbLogin.Text) ? null : cmbLogin.Text.Trim();
                string modulo = cmbModulo.SelectedItem?.ToString();
                string evento = string.IsNullOrWhiteSpace(cmbEvento.Text) ? null : cmbEvento.Text.Trim();
                DateTime desde = dtpFechaInicio.Value;
                DateTime hasta = dtpFechaFinal.Value;

                int? criticidad = null;
                if (!string.IsNullOrEmpty(cmbCriticidad.Text) && cmbCriticidad.Text != "Todos")
                    criticidad = Convert.ToInt32(cmbCriticidad.Text);

                List<Bitacora> listaFiltrada = bll.FiltrarEventos(nombre, apellido, login, modulo, evento, desde, hasta, criticidad);

                dtgvBitacora.SelectionChanged -= DtgvBitacora_SelectionChanged;
                dtgvBitacora.DataSource = null;
                dtgvBitacora.DataSource = listaFiltrada;
                ConfigurarDtgv();
                dtgvBitacora.SelectionChanged += DtgvBitacora_SelectionChanged;

                if (listaFiltrada.Count == 0)
                {
                    txtNombre.Text = string.Empty;
                    txtApellido.Text = string.Empty;
                    MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj4"), LanguageManager.Instance.GetTraduction("BitMsj5"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MostrarPrimerRegistro();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj6"), LanguageManager.Instance.GetTraduction("ErrorP"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool ValidarFiltros()
        {
            if (dtpFechaInicio.Value.Date > dtpFechaFinal.Value.Date)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj7"), LanguageManager.Instance.GetTraduction("bitMsj8"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFechaInicio.Focus();
                return false;
            }


            return true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (arrastrando)
            {
                this.Location = new Point(this.Location.X + (e.X - posX), this.Location.Y + (e.Y - posY));
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbLogin.SelectedIndexChanged -= cmbLogin_SelectedIndexChanged;
            dtgvBitacora.SelectionChanged -= DtgvBitacora_SelectionChanged;

            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            cmbLogin.SelectedIndex = -1;
            cmbModulo.SelectedIndex = 0;
            cmbEvento.SelectedIndex = -1;
            cmbCriticidad.SelectedIndex = 0;

            dtpFechaInicio.Value = DateTime.Now.AddDays(-3);
            dtpFechaFinal.Value = DateTime.Now;

            try
            {
                dtgvBitacora.DataSource = null;
                dtgvBitacora.DataSource = bll.FiltrarEventos(null, null, null, "Todos", null, dtpFechaInicio.Value, dtpFechaFinal.Value, null);
                ConfigurarDtgv();
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMjs9"), LanguageManager.Instance.GetTraduction("ErrorP"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmbLogin.SelectedIndexChanged += cmbLogin_SelectedIndexChanged;
                dtgvBitacora.SelectionChanged += DtgvBitacora_SelectionChanged;
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dtgvBitacora.Rows.Count == 0)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMjs10"), LanguageManager.Instance.GetTraduction("BitMsj11"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF|*.pdf";
            saveDialog.FileName = "Bitacora_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                iText.Document doc = new iText.Document(iText.PageSize.A4, 20f, 20f, 20f, 20f);
                PdfWriter.GetInstance(doc, new FileStream(saveDialog.FileName, FileMode.Create));
                doc.Open();

                iText.Font fontTitulo = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA_BOLD, 14);
                iText.Font fontFecha = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA, 9);
                iText.Font fontHeader = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA_BOLD, 9);
                iText.Font fontCell = iText.FontFactory.GetFont(iText.FontFactory.HELVETICA, 8);
                iText.BaseColor colorHeader = new iText.BaseColor(70, 130, 180);

                doc.Add(new iText.Paragraph("Bitácora de Eventos", fontTitulo));
                doc.Add(new iText.Paragraph("Generado el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontFecha));
                doc.Add(new iText.Paragraph(" "));

                PdfPTable tabla = new PdfPTable(6);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 15f, 12f, 8f, 12f, 25f, 10f });

                foreach (string col in new[] { "Login", "Fecha", "Hora", "Módulo", "Evento", "Criticidad" })
                {
                    PdfPCell cell = new PdfPCell(new iText.Phrase(col, fontHeader));
                    cell.BackgroundColor = colorHeader;
                    cell.HorizontalAlignment = iText.Element.ALIGN_CENTER;
                    cell.Padding = 5;
                    tabla.AddCell(cell);
                }

                foreach (DataGridViewRow row in dtgvBitacora.Rows)
                {
                    if (row.DataBoundItem is Bitacora b)
                    {
                        tabla.AddCell(new PdfPCell(new iText.Phrase(b.Login, fontCell)) { Padding = 4 });
                        tabla.AddCell(new PdfPCell(new iText.Phrase(b.Fecha.ToString("dd/MM/yyyy"), fontCell)) { Padding = 4 });
                        tabla.AddCell(new PdfPCell(new iText.Phrase(b.Fecha.ToString("HH:mm"), fontCell)) { Padding = 4 });
                        tabla.AddCell(new PdfPCell(new iText.Phrase(b.Modulo, fontCell)) { Padding = 4 });
                        tabla.AddCell(new PdfPCell(new iText.Phrase(b.Evento, fontCell)) { Padding = 4 });
                        tabla.AddCell(new PdfPCell(new iText.Phrase(b.Criticidad.ToString(), fontCell)) { Padding = 4, HorizontalAlignment = iText.Element.ALIGN_CENTER });
                    }
                }

                doc.Add(tabla);
                doc.Close();

                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj12"), LanguageManager.Instance.GetTraduction("BitMsj13"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
            }
            catch (IOException)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj14"), LanguageManager.Instance.GetTraduction("BitMsj15"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj16"), LanguageManager.Instance.GetTraduction("ErrorP"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbLogin.Text))
                MostrarUsuario(cmbLogin.Text);
            else
            {
                txtNombre.Text = string.Empty;
                txtApellido.Text = string.Empty;
            }
        }

        private void FormBitacora_Load(object sender, EventArgs e)
        {
            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);

            ValidarPermisos();
        }

        private void ValidarPermisos()
        {

            Usuario usuarioActual = SessionManager.Instance.UsuarioActual();

            if (!usuarioActual.TienePermiso("Ver Bitacora"))
            {
                MessageBox.Show(LanguageManager.Instance.GetTraduction("BitMsj17"), LanguageManager.Instance.GetTraduction("BitMsj18"), MessageBoxButtons.OK, MessageBoxIcon.Stop);

                if (Application.OpenForms["Menu"] != null)
                {
                    Application.OpenForms["Menu"].Show();
                }
                else
                {
                    Menu menu = new Menu();
                    menu.Show();
                }

                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            btnAplicarB.Enabled = usuarioActual.TienePermiso("Filtrar Bitacora");
            btnLimpiarB.Enabled = usuarioActual.TienePermiso("Limpiar Bitacora");
            btnImprimirB.Enabled = usuarioActual.TienePermiso("Imprimir Bitacora");
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            lblBitacora.Text = lenguaje.GetTraduction("lblBitacora");
            lblNombreB.Text = lenguaje.GetTraduction("lblNombreB");
            lblApellidoB.Text = lenguaje.GetTraduction("lblApellidoB");
            lblLoginB.Text = lenguaje.GetTraduction("lblLoginB");
            lblFechaInicioB.Text = lenguaje.GetTraduction("lblFechaInicioB");
            lblFechaFinalB.Text = lenguaje.GetTraduction("lblFechaFinalB");
            lblModuloB.Text = lenguaje.GetTraduction("lblModuloB");
            lblEventoB.Text = lenguaje.GetTraduction("lblEventoB");
            lblCriticidadB.Text = lenguaje.GetTraduction("lblCriticidadB");

            btnLimpiarB.Text = lenguaje.GetTraduction("btnLimpiarB");
            btnAplicarB.Text = lenguaje.GetTraduction("btnAplicarB");
            btnImprimirB.Text = lenguaje.GetTraduction("btnImprimirB");
        }
    }
}