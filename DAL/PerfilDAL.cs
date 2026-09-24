using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class PerfilDAL : _012IP_AbstractDAL<ComponentePermiso>
    {
        public List<PermisoSimple> ObtenerPermisos()
        {
            var lista = new List<PermisoSimple>();
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();
                _sqlcommand.CommandText = "SELECT Nombre, DVH FROM PermisoSimple";
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(new PermisoSimple
                        {
                            Nombre = reader["Nombre"].ToString(),
                            DVH = reader["DVH"].ToString()
                        });
                }
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
            }
            return lista;
        }

        public List<Familia> ObtenerFamilias()
        {
            var lista = new List<Familia>();
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();
                using (var cmd = new SqlCommand("SELECT Nombre, DVH FROM Familia", _sqlserver))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(new Familia
                        {
                            Nombre = reader["Nombre"].ToString(),
                            EsRol = false,
                            DVH = reader["DVH"].ToString()
                        });
                }
                foreach (var fam in lista)
                    CargarHijosComponente(fam);
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
            }
            return lista;
        }

        public List<Familia> ObtenerPerfiles()
        {
            var lista = new List<Familia>();
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();
                using (var cmd = new SqlCommand("SELECT Nombre, DVH FROM Perfil", _sqlserver))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(new Familia
                        {
                            Nombre = reader["Nombre"].ToString(),
                            EsRol = true,
                            DVH = reader["DVH"].ToString()
                        });
                }
                foreach (var perfil in lista)
                    CargarHijosComponente(perfil);
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
            }
            return lista;
        }

        public List<Familia> ObtenerFamiliasYPerfiles()
        {
            var listaUnificada = new List<Familia>();
            var nombresFamilias = new List<string>();
            var nombresPerfiles = new List<string>();

            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();

                // Usamos siempre new SqlCommand para consistencia
                using (var cmd = new SqlCommand("SELECT Nombre FROM Familia", _sqlserver))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        nombresFamilias.Add(reader["Nombre"].ToString());

                using (var cmd = new SqlCommand("SELECT Nombre FROM Perfil", _sqlserver))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        nombresPerfiles.Add(reader["Nombre"].ToString());

                foreach (var nombre in nombresFamilias)
                {
                    var fam = new Familia { Nombre = nombre, EsRol = false };
                    CargarHijosComponente(fam);
                    listaUnificada.Add(fam);
                }
                foreach (var nombre in nombresPerfiles)
                {
                    var perf = new Familia { Nombre = nombre, EsRol = true };
                    CargarHijosComponente(perf);
                    listaUnificada.Add(perf);
                }
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
            }
            return listaUnificada;
        }

        private void CargarHijosComponente(Familia padre)
        {
            string tablaPermisos = padre.EsRol ? "Perfil_PermisoSimple" : "Familia_PermisoSimple";
            string tablaFamilias = padre.EsRol ? "Perfil_Familia" : "Familia_Familia";
            string colPadre = padre.EsRol ? "NombrePerfil" : "NombreFamilia";
            string colPadreHijo = padre.EsRol ? "NombrePerfil" : "NombrePadre";
            string colHijo = padre.EsRol ? "NombreFamilia" : "NombreHijo";

            using (var cmd = new SqlCommand(
                $"SELECT NombrePermiso FROM {tablaPermisos} WHERE {colPadre} = @Padre", _sqlserver))
            {
                cmd.Parameters.AddWithValue("@Padre", padre.Nombre);
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        padre.AgregarHijo(new PermisoSimple { Nombre = reader["NombrePermiso"].ToString() });
            }

            var familiasHijas = new List<string>();
            using (var cmd = new SqlCommand(
                $"SELECT {colHijo} FROM {tablaFamilias} WHERE {colPadreHijo} = @Padre", _sqlserver))
            {
                cmd.Parameters.AddWithValue("@Padre", padre.Nombre);
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        familiasHijas.Add(reader[colHijo].ToString());
            }

            foreach (var nombreHijo in familiasHijas)
            {
                var sub = new Familia { Nombre = nombreHijo, EsRol = false };
                CargarHijosComponente(sub);
                padre.AgregarHijo(sub);
            }
        }

        private bool ExisteNombre(string nombre, string tabla)
        {
            using (var cmd = new SqlCommand(
                $"SELECT COUNT(*) FROM {tabla} WHERE Nombre = @Nombre", _sqlserver))
            {
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                try
                {
                    if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
                finally
                {
                    if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
                }
            }
        }

        public void GuardarPermiso(PermisoSimple permiso)
        {
            if (ExisteNombre(permiso.Nombre, "PermisoSimple"))
                throw new ArgumentException(LanguageManager.Instance.GetTraduction("PerfilDalText2"));

            // Calculamos el DVH antes de insertar
            string dvh = DigitoVerificador.CalcularDVH(permiso.Nombre);

            _sqlcommand.CommandText = "INSERT INTO PermisoSimple (Nombre, DVH) VALUES (@Nombre, @dvh)";
            _sqlcommand.Parameters.Clear();
            _sqlcommand.Parameters.AddWithValue("@Nombre", permiso.Nombre);
            _sqlcommand.Parameters.AddWithValue("@dvh", dvh);
            EjecutarNonQuery();
        }

        public void EliminarPermiso(string nombre)
        {
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();

                using (var cmd1 = new SqlCommand(
                    "DELETE FROM Familia_PermisoSimple WHERE NombrePermiso = @Nombre", _sqlserver))
                {
                    cmd1.Parameters.AddWithValue("@Nombre", nombre);
                    cmd1.ExecuteNonQuery();
                }
                using (var cmd2 = new SqlCommand(
                    "DELETE FROM Perfil_PermisoSimple WHERE NombrePermiso = @Nombre", _sqlserver))
                {
                    cmd2.Parameters.AddWithValue("@Nombre", nombre);
                    cmd2.ExecuteNonQuery();
                }

                _sqlcommand.CommandText = "DELETE FROM PermisoSimple WHERE Nombre = @Nombre";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@Nombre", nombre);
                _sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
                _sqlcommand.Parameters.Clear();
            }
        }

        public void GuardarFamilia(Familia familia)
        {
            string tabla = familia.EsRol ? "Perfil" : "Familia";

            if (ExisteNombre(familia.Nombre, tabla))
            {
                string tipo = familia.EsRol ? "perfil" : "familia";
                throw new ArgumentException(
                    $"{LanguageManager.Instance.GetTraduction("PerfilDalText2")} {tipo} {LanguageManager.Instance.GetTraduction("PerfilDalText3")}");
            }

            // Calculamos el DVH antes de insertar
            string dvh = DigitoVerificador.CalcularDVH(familia.Nombre);

            _sqlcommand.CommandText = $"INSERT INTO {tabla} (Nombre, DVH) VALUES (@Nombre, @dvh)";
            _sqlcommand.Parameters.Clear();
            _sqlcommand.Parameters.AddWithValue("@Nombre", familia.Nombre);
            _sqlcommand.Parameters.AddWithValue("@dvh", dvh);
            EjecutarNonQuery();
        }

        public void EliminarFamilia(Familia familia)
        {
            string tabla = familia.EsRol ? "Perfil" : "Familia";
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();

                LimpiarRelacionesComponente(familia.Nombre, familia.EsRol);

                if (!familia.EsRol)
                {
                    using (var cmd1 = new SqlCommand(
                        "DELETE FROM Familia_Familia WHERE NombreHijo = @Nombre", _sqlserver))
                    {
                        cmd1.Parameters.AddWithValue("@Nombre", familia.Nombre);
                        cmd1.ExecuteNonQuery();
                    }
                    using (var cmd2 = new SqlCommand(
                        "DELETE FROM Perfil_Familia WHERE NombreFamilia = @Nombre", _sqlserver))
                    {
                        cmd2.Parameters.AddWithValue("@Nombre", familia.Nombre);
                        cmd2.ExecuteNonQuery();
                    }
                }

                _sqlcommand.CommandText = $"DELETE FROM {tabla} WHERE Nombre = @Nombre";
                _sqlcommand.Parameters.Clear();
                _sqlcommand.Parameters.AddWithValue("@Nombre", familia.Nombre);
                _sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
                _sqlcommand.Parameters.Clear();
            }
        }

        public void GuardarRelaciones(Familia padre)
        {
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();

                LimpiarRelacionesComponente(padre.Nombre, padre.EsRol);

                foreach (var hijo in padre.ObtenerHijos())
                {
                    if (hijo is PermisoSimple permiso)
                    {
                        string tabla = padre.EsRol ? "Perfil_PermisoSimple" : "Familia_PermisoSimple";
                        string col1 = padre.EsRol ? "NombrePerfil" : "NombreFamilia";
                        using (var cmd = new SqlCommand(
                            $"INSERT INTO {tabla} ({col1}, NombrePermiso) VALUES (@Padre, @Hijo)", _sqlserver))
                        {
                            cmd.Parameters.AddWithValue("@Padre", padre.Nombre);
                            cmd.Parameters.AddWithValue("@Hijo", permiso.Nombre);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (hijo is Familia subFamilia)
                    {
                        string tabla = padre.EsRol ? "Perfil_Familia" : "Familia_Familia";
                        string col1 = padre.EsRol ? "NombrePerfil" : "NombrePadre";
                        string col2 = padre.EsRol ? "NombreFamilia" : "NombreHijo";
                        using (var cmd = new SqlCommand(
                            $"INSERT INTO {tabla} ({col1}, {col2}) VALUES (@Padre, @Hijo)", _sqlserver))
                        {
                            cmd.Parameters.AddWithValue("@Padre", padre.Nombre);
                            cmd.Parameters.AddWithValue("@Hijo", subFamilia.Nombre);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
            }
        }

        private void LimpiarRelacionesComponente(string nombrePadre, bool esRol)
        {
            string q1 = esRol
                ? "DELETE FROM Perfil_PermisoSimple WHERE NombrePerfil = @Padre"
                : "DELETE FROM Familia_PermisoSimple WHERE NombreFamilia = @Padre";
            string q2 = esRol
                ? "DELETE FROM Perfil_Familia WHERE NombrePerfil = @Padre"
                : "DELETE FROM Familia_Familia WHERE NombrePadre = @Padre";

            using (var cmd1 = new SqlCommand(q1, _sqlserver))
            {
                cmd1.Parameters.AddWithValue("@Padre", nombrePadre);
                cmd1.ExecuteNonQuery();
            }
            using (var cmd2 = new SqlCommand(q2, _sqlserver))
            {
                cmd2.Parameters.AddWithValue("@Padre", nombrePadre);
                cmd2.ExecuteNonQuery();
            }
        }

        private void EjecutarNonQuery()
        {
            try
            {
                if (_sqlserver.State != ConnectionState.Open) _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            finally
            {
                if (_sqlserver.State == ConnectionState.Open) _sqlserver.Close();
                _sqlcommand.Parameters.Clear();
            }
        }

        public void ActualizarPefilDVH(string nombre, string dvhCalculado)
        {
            _sqlcommand.CommandText = "UPDATE Perfil SET DVH = @dvh WHERE Nombre = @nom";
            _sqlcommand.Parameters.Clear();
            _sqlcommand.Parameters.AddWithValue("@dvh", dvhCalculado);
            _sqlcommand.Parameters.AddWithValue("@nom", nombre);
            EjecutarNonQuery();
        }

        public void ActualizarPermisoSimpleDVH(string nombre, string dvhCalculado)
        {
            _sqlcommand.CommandText = "UPDATE PermisoSimple SET DVH = @dvh WHERE Nombre = @nom";
            _sqlcommand.Parameters.Clear();
            _sqlcommand.Parameters.AddWithValue("@dvh", dvhCalculado);
            _sqlcommand.Parameters.AddWithValue("@nom", nombre);
            EjecutarNonQuery();
        }

        public void ActualizarDVH(string nombre, string dvhCalculado)
        {
            _sqlcommand.CommandText = "UPDATE Familia SET DVH = @dvh WHERE Nombre = @nom";
            _sqlcommand.Parameters.Clear();
            _sqlcommand.Parameters.AddWithValue("@dvh", dvhCalculado);
            _sqlcommand.Parameters.AddWithValue("@nom", nombre);
            EjecutarNonQuery();
        }
    }
}