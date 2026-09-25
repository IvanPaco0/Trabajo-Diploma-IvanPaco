using _012IP_BE;
using _012IP_GUI;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ConexionConfig.ExisteConfiguracion())
            {
                FrmConexionInicial frmConfig = new FrmConexionInicial();
                DialogResult resultado = frmConfig.ShowDialog();

                if (resultado != DialogResult.OK)
                {
                    return;
                }
            }

            Application.Run(new Login());
            //Application.Run(new _012IP_frmRenovarMembresia());
        }
    }
}