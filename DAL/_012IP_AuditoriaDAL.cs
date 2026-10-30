using _012IP_BE;
using DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _012IP_DAL
{
    public class _012IP_AuditoriaDAL : _012IP_AbstractDAL<_012IP_AuditoriaBE>
    {
        public _012IP_AuditoriaDAL() : base() { }

        public bool _012IP_RegistrarAuditoria(_012IP_AuditoriaBE auditoria, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = @"
                    INSERT INTO AuditoriaInventario (Fecha, Seccion, Responsable, Esperados, Encontrados, Faltantes, Sobrantes, DVH)
                    OUTPUT INSERTED.IdAuditoria
                    VALUES (@fecha, @seccion, @responsable, @esperados, @encontrados, @faltantes, @sobrantes, @dvh);";

                _sqlcommand.Parameters.AddWithValue("@fecha", auditoria.Fecha);
                _sqlcommand.Parameters.AddWithValue("@seccion", auditoria.Seccion);
                _sqlcommand.Parameters.AddWithValue("@responsable", auditoria.Responsable);
                _sqlcommand.Parameters.AddWithValue("@esperados", auditoria.Esperados);
                _sqlcommand.Parameters.AddWithValue("@encontrados", auditoria.Encontrados);
                _sqlcommand.Parameters.AddWithValue("@faltantes", auditoria.Faltantes);
                _sqlcommand.Parameters.AddWithValue("@sobrantes", auditoria.Sobrantes);
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);

                _sqlserver.Open();
                object idGenerado = _sqlcommand.ExecuteScalar();

                if (idGenerado == null || idGenerado == DBNull.Value)
                    return false;

                auditoria.IdAuditoria = Convert.ToInt32(idGenerado);
                return true;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        /// <summary>Lectura completa ordenada por PK, usada por el recalculo / auditoria de digitos verificadores.</summary>
        public List<_012IP_AuditoriaBE> _012IP_ListarTodasDVH()
        {
            var auditorias = new List<_012IP_AuditoriaBE>();
            try
            {
                _sqlcommand.CommandText = @"
                    SELECT IdAuditoria, Fecha, Seccion, Responsable, Esperados, Encontrados, Faltantes, Sobrantes, DVH
                    FROM AuditoriaInventario
                    ORDER BY IdAuditoria;";

                _sqlserver.Open();
                using (var reader = _sqlcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        auditorias.Add(new _012IP_AuditoriaBE
                        {
                            IdAuditoria = (int)reader["IdAuditoria"],
                            Fecha = (DateTime)reader["Fecha"],
                            Seccion = (string)reader["Seccion"],
                            Responsable = (string)reader["Responsable"],
                            Esperados = (int)reader["Esperados"],
                            Encontrados = (int)reader["Encontrados"],
                            Faltantes = (int)reader["Faltantes"],
                            Sobrantes = (int)reader["Sobrantes"],
                            DVH = (string)reader["DVH"]
                        });
                    }
                }
                return auditorias;
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }

        public void _012IP_ActualizarDVH(int idAuditoria, string dvh)
        {
            try
            {
                _sqlcommand.CommandText = "UPDATE AuditoriaInventario SET DVH = @dvh WHERE IdAuditoria = @id;";
                _sqlcommand.Parameters.AddWithValue("@dvh", dvh);
                _sqlcommand.Parameters.AddWithValue("@id", idAuditoria);

                _sqlserver.Open();
                _sqlcommand.ExecuteNonQuery();
            }
            catch { throw; }
            finally
            {
                _sqlcommand.Parameters.Clear();
                _sqlserver.Close();
            }
        }
    }
}