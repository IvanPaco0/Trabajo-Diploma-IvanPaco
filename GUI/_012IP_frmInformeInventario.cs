using _012IP_BE;
using _012IP_BLL;
using Servicios;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _012IP_GUI
{
    // CUN-10 Emitir informe de inventario y patrimonio.
    // Se muestra en pantalla al cerrar una auditoría: resultado de la sección, detalle de las
    // diferencias con su resolución y estado del stock. Opcionalmente se guarda como .txt.
    public class _012IP_frmInformeInventario : Form
    {
        private readonly _012IP_AuditoriaBE auditoria;
        private readonly _012IP_ConciliacionBE conciliacion;
        private readonly _012IP_StockBE stock;
        private readonly string informeTexto;

        private readonly Label lblMsj = new Label();

        public _012IP_frmInformeInventario(_012IP_AuditoriaBE auditoria, _012IP_ConciliacionBE conciliacion,
                                           _012IP_StockBE stock, string informeTexto)
        {
            this.auditoria = auditoria;
            this.conciliacion = conciliacion;
            this.stock = stock;
            this.informeTexto = informeTexto;
            _012IP_ArmarPantalla();
        }

        private static string T(string clave) => LanguageManager.Instance.GetTraduction(clave);

        private void _012IP_ArmarPantalla()
        {
            Text = T("InfInvTitulo");
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(900, 640);
            BackColor = Color.FromArgb(0, 0, 64);
            ForeColor = Color.White;
            Font = new Font("Microsoft Sans Serif", 9.75F);

            var titulo = new Label
            {
                Text = T("InfInvTitulo"),
                Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 12)
            };
            var cabecera = new Label
            {
                Text = T("AudInfSeccion") + ": " + auditoria.Seccion + "     " +
                       T("AudInfFecha") + ": " + auditoria.Fecha.ToString("dd/MM/yyyy HH:mm") + "     " +
                       T("AudInfUsuario") + ": " + auditoria.Responsable,
                AutoSize = true,
                Location = new Point(18, 48)
            };

            // Resultado de la auditoría
            var grpResultado = _012IP_Grupo(T("InfInvResultado"), new Point(16, 78), new Size(420, 150));
            _012IP_Fila(grpResultado, 0, T("AudInfEsperados"), auditoria.Esperados);
            _012IP_Fila(grpResultado, 1, T("AudInfCorrectos"), conciliacion.Correctos);
            _012IP_Fila(grpResultado, 2, T("AudInfFaltantes"), auditoria.Faltantes);
            _012IP_Fila(grpResultado, 3, T("AudInfSobrantes"), auditoria.Sobrantes);

            // Estado del stock (patrimonio)
            var grpStock = _012IP_Grupo(T("InfInvStock"), new Point(464, 78), new Size(420, 150));
            _012IP_Fila(grpStock, 0, T("InfInvTotal"), stock.Total, 0);
            _012IP_Fila(grpStock, 1, T("InfInvDisponibles"), stock.Disponibles, 0);
            _012IP_Fila(grpStock, 2, T("InfInvDeteriorados"), stock.Deteriorados, 0);
            _012IP_Fila(grpStock, 0, T("InfInvRestauracion"), stock.EnRestauracion, 1);
            _012IP_Fila(grpStock, 1, T("InfInvBajas"), stock.Bajas, 1);
            _012IP_Fila(grpStock, 2, T("InfInvPendientes"), stock.PendientesUbicar, 1);

            // Detalle de diferencias
            var lblDif = new Label { Text = T("AudInfDiferencias"), AutoSize = true, Location = new Point(16, 240) };
            var grilla = new DataGridView
            {
                Location = new Point(16, 262),
                Size = new Size(868, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                EnableHeadersVisualStyles = false
            };
            grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            grilla.DefaultCellStyle.ForeColor = Color.Black;
            grilla.DefaultCellStyle.SelectionBackColor = Color.Maroon;
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;
            grilla.Columns.Add("Resultado", T("InfInvColResultado"));
            grilla.Columns.Add("Codigo", T("InfInvColCodigo"));
            grilla.Columns.Add("Titulo", T("InfInvColTitulo"));
            grilla.Columns.Add("Detalle", T("InfInvColDetalle"));
            grilla.Columns.Add("Resolucion", T("InfInvColResolucion"));
            foreach (_012IP_DiferenciaAuditoriaBE d in conciliacion.Diferencias)
                grilla.Rows.Add(_012IP_AuditoriaBLL._012IP_TextoResultado(d.Resultado), d.CodigoBarras, d.Titulo,
                                d.Detalle, _012IP_AuditoriaBLL._012IP_TextoResolucion(d));
            if (conciliacion.Diferencias.Count == 0)
                grilla.Rows.Add("-", "", T("AudInfSinDiferencias"), "", "");

            lblMsj.Location = new Point(16, 580);
            lblMsj.Size = new Size(500, 40);

            var btnGuardar = _012IP_Boton(T("InfInvGuardar"), new Point(564, 576));
            btnGuardar.Click += _012IP_btnGuardar_Click;
            var btnCerrar = _012IP_Boton(T("InfInvCerrar"), new Point(734, 576));
            btnCerrar.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { titulo, cabecera, grpResultado, grpStock, lblDif, grilla, lblMsj, btnGuardar, btnCerrar });
            AcceptButton = btnCerrar;
        }

        private static GroupBox _012IP_Grupo(string texto, Point lugar, Size tam)
        {
            return new GroupBox { Text = texto, Location = lugar, Size = tam, ForeColor = Color.White };
        }

        // Dos columnas de pares "etiqueta: valor" dentro de un grupo.
        private static void _012IP_Fila(GroupBox grupo, int fila, string etiqueta, int valor, int columna = 0)
        {
            int x = 14 + columna * 205, y = 28 + fila * 28;
            grupo.Controls.Add(new Label { Text = etiqueta + ":", AutoSize = false, Size = new Size(140, 22), Location = new Point(x, y) });
            grupo.Controls.Add(new Label
            {
                Text = valor.ToString(),
                AutoSize = false,
                Size = new Size(55, 22),
                Location = new Point(x + 140, y),
                Font = new Font("Microsoft Sans Serif", 10.5F, FontStyle.Bold),
                ForeColor = Color.LightGreen
            });
        }

        private static Button _012IP_Boton(string texto, Point lugar)
        {
            var b = new Button
            {
                Text = texto,
                Location = lugar,
                Size = new Size(150, 40),
                BackColor = Color.Maroon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void _012IP_btnGuardar_Click(object sender, EventArgs e)
        {
            string seccionLimpia = new string(auditoria.Seccion.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c).ToArray());
            using (var dialogo = new SaveFileDialog())
            {
                dialogo.Filter = T("AudInformeFiltro");
                dialogo.FileName = "Informe_" + seccionLimpia + "_" + auditoria.Fecha.ToString("yyyyMMdd_HHmm") + ".txt";
                if (dialogo.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    File.WriteAllText(dialogo.FileName, informeTexto, Encoding.UTF8);
                    lblMsj.ForeColor = Color.LightGreen;
                    lblMsj.Text = T("AudInformeGuardado");
                }
                catch (Exception ex)
                {
                    lblMsj.ForeColor = Color.FromArgb(255, 156, 156);
                    lblMsj.Text = ex.Message;
                }
            }
        }
    }
}
