using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _012IP_GUI
{
    public partial class _012IP_frmInformeMembresias : Form, IObserver
    {
        private _012IP_PagoBLL pagoBLL;
        private _012IP_SuscripcionBLL suscripcionBLL;

        int posX, posY;
        bool arrastrando = false;

        public _012IP_frmInformeMembresias()
        {
            InitializeComponent();
        }

        private void _012IP_frmInformeMembresias_Load(object sender, EventArgs e)
        {
            try
            {
                pagoBLL = new _012IP_PagoBLL();
                suscripcionBLL = new _012IP_SuscripcionBLL();
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
            }

            _012IP_ConfigurarGrilla(dgvMedios);
            _012IP_ConfigurarGrilla(dgvMembresias);
            _012IP_ConfigurarGrilla(dgvVencimientos);

            DateTime hoy = DateTime.Today;
            dtpDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpHasta.Value = hoy;

            LanguageManager.Instance.AgregarObservador(this);
            Actualizar(LanguageManager.Instance);
            _012IP_Generar();
        }

        private void _012IP_frmInformeMembresias_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.EliminarObservador(this);
        }

        private void _012IP_btnGenerar_Click(object sender, EventArgs e)
        {
            _012IP_Generar();
        }

        private void _012IP_Generar()
        {
            try
            {
                lblMsj.Text = string.Empty;
                _012IP_InformeMembresiaBE informe = pagoBLL._012IP_ObtenerRecaudacion(dtpDesde.Value, dtpHasta.Value);
                List<_012IP_VencimientoSocioBE> vencimientos = suscripcionBLL._012IP_ListarVencimientos(_012IP_SuscripcionBLL.DiasAvisoVencimiento);

                lblTotal.Text = string.Format(_012IP_Texto("InfMemTotal"), _012IP_Moneda(informe.TotalRecaudado), informe.CantidadPagos);

                dgvMedios.DataSource = informe.PorMedioDePago.Select(r => new
                {
                    Concepto = r.Concepto, Cantidad = r.Cantidad, Importe = _012IP_Moneda(r.Importe)
                }).ToList();
                dgvMembresias.DataSource = informe.PorMembresia.Select(r => new
                {
                    Concepto = r.Concepto, Cantidad = r.Cantidad, Importe = _012IP_Moneda(r.Importe)
                }).ToList();
                dgvVencimientos.DataSource = vencimientos.Select(v => new
                {
                    DNI = v.DNI,
                    Nombre = v.Nombre,
                    Apellido = v.Apellido,
                    Membresia = v.Membresia,
                    Vence = v.FechaVencimiento.ToString("dd/MM/yyyy"),
                    Estado = v.DiasRestantes < 0
                        ? string.Format(_012IP_Texto("InfMemVencida"), -v.DiasRestantes)
                        : string.Format(_012IP_Texto("InfMemPorVencer"), v.DiasRestantes)
                }).ToList();

                foreach (DataGridViewRow fila in dgvVencimientos.Rows)
                    fila.DefaultCellStyle.ForeColor = vencimientos[fila.Index].DiasRestantes < 0 ? Color.DarkRed : Color.DarkGoldenrod;

                _012IP_AplicarEncabezados();
                dgvMedios.ClearSelection(); dgvMembresias.ClearSelection(); dgvVencimientos.ClearSelection();
                lblMsj.ForeColor = Color.White;
                lblMsj.Text = _012IP_Texto("InfMemGenerado");
            }
            catch (Exception ex)
            {
                _012IP_MostrarError(ex);
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
            foreach (DataGridView g in new[] { dgvMedios, dgvMembresias })
            {
                _012IP_Encabezado(g, "Concepto", "InfMemColConcepto");
                _012IP_Encabezado(g, "Cantidad", "InfMemColCantidad");
                _012IP_Encabezado(g, "Importe", "InfMemColImporte");
            }
            _012IP_Encabezado(dgvVencimientos, "DNI", "DNI");
            _012IP_Encabezado(dgvVencimientos, "Nombre", "lblNombre");
            _012IP_Encabezado(dgvVencimientos, "Apellido", "lblApe");
            _012IP_Encabezado(dgvVencimientos, "Membresia", "RenMemMembresia");
            _012IP_Encabezado(dgvVencimientos, "Vence", "RenMemVence");
            _012IP_Encabezado(dgvVencimientos, "Estado", "InfMemColEstado");
        }

        private void _012IP_Encabezado(DataGridView grilla, string columna, string clave)
        {
            if (grilla.Columns.Contains(columna))
                grilla.Columns[columna].HeaderText = _012IP_Texto(clave);
        }

        private string _012IP_Texto(string clave)
        {
            return LanguageManager.Instance.GetTraduction(clave);
        }

        private string _012IP_Moneda(decimal monto)
        {
            return "$" + monto.ToString("N0");
        }

        private void _012IP_MostrarError(Exception ex)
        {
            lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
            lblMsj.Text = ex.InnerException != null ? ex.Message + " (" + ex.InnerException.Message + ")" : ex.Message;
        }

        public void Actualizar(LanguageManager lenguaje)
        {
            lblTitulo.Text = _012IP_Texto("InfMemTitulo");
            lblDesde.Text = _012IP_Texto("InfMemDesde");
            lblHasta.Text = _012IP_Texto("InfMemHasta");
            btnGenerar.Text = _012IP_Texto("InfMemGenerar");
            lblPorMedio.Text = _012IP_Texto("InfMemPorMedio");
            lblPorMembresia.Text = _012IP_Texto("InfMemPorMembresia");
            lblVencimientos.Text = string.Format(_012IP_Texto("InfMemVencimientos"), _012IP_SuscripcionBLL.DiasAvisoVencimiento);
            btnSalir.Text = _012IP_Texto("btnSalir");
            _012IP_AplicarEncabezados();
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
