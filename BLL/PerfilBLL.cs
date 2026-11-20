
using DAL;
using Servicios;
using Servicios.Patron_Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BLL
{
    public class PerfilBLL
    {
        private readonly PerfilDAL _dal;
        private readonly BitacoraBLL _bitacoraBLL;
        private readonly DigitoVerificadorBLL _digitoVerificadorBLL = new DigitoVerificadorBLL();

        

        public PerfilBLL()
        {
            _dal = new PerfilDAL();
            _bitacoraBLL = new BitacoraBLL();
        }

        public List<PermisoSimple> ObtenerPermisos() => _dal.ObtenerPermisos();

        public List<Familia> ObtenerFamilias()
        {
            return _dal.ObtenerFamiliasYPerfiles().Where(f => !f.EsRol).ToList();
        }

        public List<Familia> ObtenerPerfiles()
        {
            return _dal.ObtenerFamiliasYPerfiles().Where(f => f.EsRol).ToList();
        }

        public List<PermisoSimple> ObtenerPermisosDisponibles(Familia componentePadre)
        {
          
            var todosLosPermisos = _dal.ObtenerPermisos();
            if (componentePadre == null) return todosLosPermisos;

    
            var permisosYaIncluidos = new HashSet<string>();
            var familiasYaIncluidas = new HashSet<string>();

         
            ObtenerEstructuraPlana(componentePadre, permisosYaIncluidos, familiasYaIncluidas);

        
            return todosLosPermisos.Where(p => !permisosYaIncluidos.Contains(p.Nombre)).ToList();
        }

        public List<Familia> ObtenerFamiliasDisponibles(string nombrePadre)
        {
            var todasLasFamilias = ObtenerFamilias();
            var todosLosComponentes = _dal.ObtenerFamiliasYPerfiles();
            var padre = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombrePadre);

            if (padre == null) return todasLasFamilias;

            var familiasAsignadas = new HashSet<string>();
            ObtenerEstructuraPlana(padre, new HashSet<string>(), familiasAsignadas);

            return todasLasFamilias.Where(f => f.Nombre != nombrePadre && !familiasAsignadas.Contains(f.Nombre)).ToList();
        }


        public void RecargarPermisosUsuarioEnSesion()
        {
            try
            {
                if (!SessionManager.Instance.Logueado()) return;

                Usuario usuarioActual = SessionManager.Instance.UsuarioActual();
                var perfiles = ObtenerPerfiles();
                var perfilUsuario = perfiles.FirstOrDefault(p => p.Nombre == usuarioActual.Rol);

                if (perfilUsuario != null)
                {
                    usuarioActual.Permisos.Clear();
                    usuarioActual.Permisos.Add(perfilUsuario);
                }
            }
            catch { }
        }

        public void CrearPermiso(string nombre)
        {
            _dal.GuardarPermiso(new PermisoSimple { Nombre = nombre, DVH = DigitoVerificador.CalcularDVH(nombre) });
            RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Crear permiso simple: {nombre}");
        }

        public void EliminarPermiso(string nombre)
        {
            _dal.EliminarPermiso(nombre);
            RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Eliminar permiso simple: {nombre}");
            RecargarPermisosUsuarioEnSesion();
        }

        public void CrearFamilia(string nombre)
        {
            _dal.GuardarFamilia(new Familia { Nombre = nombre, EsRol = false, DVH = DigitoVerificador.CalcularDVH(nombre) });
            _digitoVerificadorBLL.RecalcularDVV_Familia();
            RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Crear familia: {nombre}");
        }

        public void CrearPerfil(string nombre)
        {
            _dal.GuardarFamilia(new Familia { Nombre = nombre, EsRol = true, DVH = DigitoVerificador.CalcularDVH(nombre) });
            _digitoVerificadorBLL.RecalcularDVV_Perfil();
            RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Crear perfil: {nombre}");
        }

        public void EliminarFamiliaOPerfil(string nombre)
        {
            var familia = _dal.ObtenerFamiliasYPerfiles().FirstOrDefault(f => f.Nombre == nombre);
            if (familia == null) return;

            if (familia.EsRol)
            {
                var usuariosConEstePerfil = new UsuarioBLL().ListarTodosUsuarios()
                    .Where(u => u.Rol == nombre)
                    .ToList();

                if (usuariosConEstePerfil.Any())
                {
                    string lista = string.Join(", ", usuariosConEstePerfil.Select(u => u.Username));
                    throw new Exception(
                        $"{LanguageManager.Instance.GetTraduction("PerfilBLLmsj1")} '{nombre}' {LanguageManager.Instance.GetTraduction("PerfilBLLmsj2")} {lista}.\n\n" +
                        LanguageManager.Instance.GetTraduction("PerfilBllMsj3"));
                }
            }

            _dal.EliminarFamilia(familia);
            _digitoVerificadorBLL.RecalcularDVV_Familia();
            _digitoVerificadorBLL.RecalcularDVV_Perfil();
            RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Eliminar contenedor jerárquico: {nombre}");
            RecargarPermisosUsuarioEnSesion();
        }

        public void QuitarHijos(string nombrePadre, List<string> hijos, bool esPermisoSimple)
        {
            var padre = _dal.ObtenerFamiliasYPerfiles().FirstOrDefault(f => f.Nombre == nombrePadre);
            if (padre == null) return;

            foreach (var nombreHijo in hijos)
            {
                var hijoAQuitar = padre.ListaHijos.FirstOrDefault(h => h.Nombre == nombreHijo);
                if (hijoAQuitar != null)
                {
                    padre.QuitarHijo(hijoAQuitar);
                }
            }

            _dal.GuardarRelaciones(padre);

            foreach (var hijo in hijos)
            {
                string tipoNodo = esPermisoSimple ? "Permiso Simple" : "Subfamilia";
                RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Desvincular {tipoNodo} '{hijo}' del contenedor '{nombrePadre}'");
            }

            RecargarPermisosUsuarioEnSesion();
        }

        public void EliminarPermisoRedundanteDeNodoContenedor(string nombreContenedorRaiz, List<string> permisosComponentes)
        {
            try
            {
                var todosLosComponentes = _dal.ObtenerFamiliasYPerfiles();
                var raiz = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombreContenedorRaiz);
                if (raiz == null) throw new Exception($"{LanguageManager.Instance.GetTraduction("PerfilBllMsj4")} '{nombreContenedorRaiz}'.");

                foreach (var permiso in permisosComponentes)
                {
                    string contenedorDirecto = BuscarContenedorDirectoDelPermiso(raiz, permiso);
                    if (!string.IsNullOrEmpty(contenedorDirecto))
                    {
                        var contenedor = todosLosComponentes.FirstOrDefault(f => f.Nombre == contenedorDirecto);
                        if (contenedor != null)
                        {
                            var permisoAQuitar = contenedor.ListaHijos.FirstOrDefault(h => h.Nombre == permiso && h is PermisoSimple);
                            if (permisoAQuitar != null)
                            {
                                contenedor.QuitarHijo(permisoAQuitar);
                                _dal.GuardarRelaciones(contenedor);
                            }
                        }
                        RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Mitigación de redundancia: Remoción del permiso '{permiso}' en subnodo '{contenedorDirecto}'");
                    }
                }

                RecargarPermisosUsuarioEnSesion();
            }
            catch (Exception ex)
            {
                RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"ERROR en EliminarPermisoRedundante: {ex.Message}");
                throw new Exception(LanguageManager.Instance.GetTraduction("PerBLLText1") + ex.Message);
            }
        }

        public ResultadoAsignacion AsignarComponentesHijos(string nombrePadre, List<string> nombresHijos, bool esPermisoSimple)
        {
            var resultado = new ResultadoAsignacion { Estado = EstadoAsignacion.Ok };
            if (!nombresHijos.Any()) return resultado;

            var todosLosComponentes = _dal.ObtenerFamiliasYPerfiles();
            var componentePadre = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombrePadre);

            if (componentePadre == null) return resultado;

            foreach (var nombreHijo in nombresHijos)
            {
                if (nombrePadre == nombreHijo)
                {
                    resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                    resultado.PermisosConflictivos.Add(nombreHijo);
                    resultado.OrigenConflicto[nombreHijo] = "Restricción: No es posible asignar un componente jerárquico a sí mismo.";
                    return resultado;
                }

                if (!esPermisoSimple)
                {
                    var componenteHijo = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombreHijo);
                    if (componenteHijo != null && componenteHijo.EsRol && !componentePadre.EsRol)
                    {
                        resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                        resultado.PermisosConflictivos.Add(nombreHijo);
                        resultado.OrigenConflicto[nombreHijo] = $"Incompatibilidad de estructura: El perfil '{nombreHijo}' no puede ser subcomponente de la familia '{nombrePadre}'.";
                        return resultado;
                    }

                    if (ValidarCicloJerarquico(nombreHijo, nombrePadre, todosLosComponentes))
                    {
                        resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                        resultado.PermisosConflictivos.Add(nombreHijo);
                        resultado.OrigenConflicto[nombreHijo] = $"Referencia circular detectada: El componente '{nombreHijo}' ya contiene al componente '{nombrePadre}'.";
                        return resultado;
                    }
                }

                var permisosActualesPadre = new HashSet<string>();
                var familiasActualesPadre = new HashSet<string>();
                ObtenerEstructuraPlana(componentePadre, permisosActualesPadre, familiasActualesPadre);

                foreach (var elemento in componentePadre.ListaHijos.OfType<PermisoSimple>())
                {
                    permisosActualesPadre.Add(elemento.Nombre);
                }

                if (esPermisoSimple)
                {
                    // Control de redundancia directa
                    if (permisosActualesPadre.Contains(nombreHijo))
                    {
                        resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                        resultado.PermisosConflictivos.Add(nombreHijo);
                        resultado.OrigenConflicto[nombreHijo] = $"Redundancia detectada: El permiso '{nombreHijo}' ya existe en '{nombrePadre}'.";
                        return resultado;
                    }

                    // Control de redundancia horizontal
                    string mensajeConflictoHorizontal = VerificarConflictoHorizontal(todosLosComponentes, componentePadre, nombreHijo);
                    if (!string.IsNullOrEmpty(mensajeConflictoHorizontal))
                    {
                        resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                        resultado.PermisosConflictivos.Add(nombreHijo);
                        resultado.OrigenConflicto[nombreHijo] = mensajeConflictoHorizontal;
                        return resultado;
                    }
                }
                else
                {
                    var subFamiliaHijo = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombreHijo);
                    if (subFamiliaHijo != null)
                    {
                        var permisosDelHijo = new HashSet<string>();
                        var familiasDelHijo = new HashSet<string>();
                        ObtenerEstructuraPlana(subFamiliaHijo, permisosDelHijo, familiasDelHijo);

                        foreach (var elemento in subFamiliaHijo.ListaHijos.OfType<PermisoSimple>())
                        {
                            permisosDelHijo.Add(elemento.Nombre);
                        }

                        if (familiasActualesPadre.Contains(nombreHijo))
                        {
                            resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                            resultado.PermisosConflictivos.Add(nombreHijo);
                            resultado.OrigenConflicto[nombreHijo] = $"Redundancia estructural: La subfamilia '{nombreHijo}' ya forma parte de '{nombrePadre}'.";
                            return resultado;
                        }

                        var colisionesPermisos = permisosActualesPadre.Intersect(permisosDelHijo).ToList();
                        if (colisionesPermisos.Any())
                        {
                            string permisoEjemplo = colisionesPermisos.First();
                            string nodoResponsable = BuscarContenedorDirectoDelPermiso(componentePadre, permisoEjemplo) ?? componentePadre.Nombre;

                            resultado.Estado = EstadoAsignacion.ConflictoPermisos;
                            resultado.PermisosConflictivos.Add(nombreHijo);
                            resultado.OrigenConflicto[nombreHijo] = $"CONFLICTO_REDUNDANCIA_HEREDADA|{string.Join(",", colisionesPermisos)}|{nombreHijo}|{nodoResponsable}";
                            return resultado;
                        }
                    }
                }
            }

            if (resultado.Estado == EstadoAsignacion.ConflictoPermisos) return resultado;

            foreach (var nombreHijo in nombresHijos)
            {
                if (esPermisoSimple)
                {
                    var permisoHijo = new PermisoSimple { Nombre = nombreHijo };
                    componentePadre.AgregarHijo(permisoHijo);
                }
                else
                {
                    var subFamilia = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombreHijo);
                    if (subFamilia != null)
                    {
                        componentePadre.AgregarHijo(subFamilia);
                    }
                }

                string tipoNodo = esPermisoSimple ? "Permiso Simple" : "Subfamilia";
                RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Asignación exitosa de {tipoNodo} '{nombreHijo}' a la raíz '{nombrePadre}'");
            }

            _dal.GuardarRelaciones(componentePadre);
            RecargarPermisosUsuarioEnSesion();

            return resultado;
        }

        public void EliminarPermisoDeContenedorEspecifico(string nombreContenedor, string nombrePermiso)
        {
            try
            {
                var todosLosComponentes = _dal.ObtenerFamiliasYPerfiles();
                var contenedor = todosLosComponentes.FirstOrDefault(f => f.Nombre == nombreContenedor);

                if (contenedor != null)
                {
                    var permisoAQuitar = contenedor.ListaHijos.FirstOrDefault(h => h.Nombre == nombrePermiso && h is PermisoSimple);
                    if (permisoAQuitar != null)
                    {
                        contenedor.QuitarHijo(permisoAQuitar);
                        _dal.GuardarRelaciones(contenedor);
                        RegistrarEnBitacora(SessionManager.Instance.UsuarioActual().Username, $"Desvinculación forzada por redundancia: Permiso '{nombrePermiso}' removido de '{nombreContenedor}'");
                    }
                }
                RecargarPermisosUsuarioEnSesion();
            }
            catch (Exception ex)
            {
                throw new Exception($"{LanguageManager.Instance.GetTraduction("PerBLLText2")} {ex.Message}", ex);
            }
        }
        private string VerificarConflictoHorizontal(List<Familia> todosLosComponentes, Familia nodoDestino, string permisoBuscar)
        {
            var ancestrosRaiz = todosLosComponentes.Where(posiblePadre => ContieneHijoRecursivo(posiblePadre, nodoDestino.Nombre)).ToList();

            foreach (var ancestro in ancestrosRaiz)
            {
                foreach (var hijoFam in ancestro.ListaHijos.OfType<Familia>())
                {
                    if (hijoFam.Nombre == nodoDestino.Nombre) continue;

                    if (TienePermisoHeredado(hijoFam, permisoBuscar))
                    {
                        string contenedorDirecto = BuscarContenedorDirectoDelPermiso(hijoFam, permisoBuscar) ?? hijoFam.Nombre;
                 
                        return $"CONFLICTO_HORIZONTAL_DETALLADO|{permisoBuscar}|{nodoDestino.Nombre}|{ancestro.Nombre}|{hijoFam.Nombre}|{contenedorDirecto}";
                    }
                }

                foreach (var hijoPermiso in ancestro.ListaHijos.OfType<PermisoSimple>())
                {
                    if (hijoPermiso.Nombre == permisoBuscar)
                    {
                        return $"CONFLICTO_HORIZONTAL_DETALLADO|{permisoBuscar}|{nodoDestino.Nombre}|{ancestro.Nombre}|{ancestro.Nombre}|{ancestro.Nombre}";
                    }
                }
            }
            return null;
        }


        private bool ContieneHijoRecursivo(Familia padre, string nombreHijoBuscar)
        {
            if (padre.ListaHijos.OfType<Familia>().Any(f => f.Nombre == nombreHijoBuscar || ContieneHijoRecursivo(f, nombreHijoBuscar)))
                return true;
            if (padre.ListaHijos.OfType<PermisoSimple>().Any(p => p.Nombre == nombreHijoBuscar))
                return true;
            return false;
        }

        private bool TienePermisoHeredado(Familia familia, string permisoBuscar)
        {
            if (familia.Nombre == permisoBuscar) return true;
            if (familia.ListaHijos.OfType<PermisoSimple>().Any(p => p.Nombre == permisoBuscar))
                return true;

            foreach (var subFam in familia.ListaHijos.OfType<Familia>())
            {
                if (TienePermisoHeredado(subFam, permisoBuscar)) return true;
            }
            return false;
        }

        private string BuscarContenedorDirectoDelPermiso(Familia nodoActual, string nombrePermiso)
        {
            if (nodoActual.ListaHijos.OfType<PermisoSimple>().Any(p => p.Nombre == nombrePermiso))
                return nodoActual.Nombre;

            foreach (var subFamilia in nodoActual.ListaHijos.OfType<Familia>())
            {
                string encontrado = BuscarContenedorDirectoDelPermiso(subFamilia, nombrePermiso);
                if (!string.IsNullOrEmpty(encontrado)) return encontrado;
            }

            return null;
        }

        private bool ValidarCicloJerarquico(string actual, string objetivo, List<Familia> componentes)
        {
            var comp = componentes.FirstOrDefault(f => f.Nombre == actual);
            if (comp == null) return false;
            if (comp.ListaHijos.Any(h => h.Nombre == objetivo)) return true;

            foreach (var sub in comp.ListaHijos.OfType<Familia>())
            {
                if (ValidarCicloJerarquico(sub.Nombre, objetivo, componentes)) return true;
            }
            return false;
        }

        private void ObtenerEstructuraPlana(Familia componente, HashSet<string> permisos, HashSet<string> familias)
        {
            foreach (var hijo in componente.ListaHijos)
            {
                if (hijo is PermisoSimple)
                {
                    permisos.Add(hijo.Nombre);
                }
                else if (hijo is Familia fam)
                {
                    familias.Add(fam.Nombre);
                    ObtenerEstructuraPlana(fam, permisos, familias);
                }
            }
        }

        private void RegistrarEnBitacora(string usuario, string evento)
        {
            try
            {
                var registro = new Bitacora
                {
                    Login = !string.IsNullOrEmpty(usuario) ? usuario : "Sistema",
                    Fecha = DateTime.Now,
                    Evento = evento,
                };
                _bitacoraBLL.RegistrarEvento(registro);
            }
            catch { }
        }

        public void ArmarArbolEstructural(Familia nodoActual, TreeNodeCollection nodosUI)
        {
            var nuevoNodo = new TreeNode(nodoActual.Nombre)
            {
                Tag = nodoActual,
                ImageIndex = nodoActual.EsRol ? 0 : 1
            };

            nodosUI.Add(nuevoNodo);

            foreach (var hijo in nodoActual.ListaHijos)
            {
                if (hijo is Familia subFamilia)
                {
                    ArmarArbolEstructural(subFamilia, nuevoNodo.Nodes);
                }
                else if (hijo is PermisoSimple permiso)
                {
                    var nodoHoja = new TreeNode(permiso.Nombre)
                    {
                        Tag = permiso,
                        ImageIndex = 2
                    };
                    nuevoNodo.Nodes.Add(nodoHoja);
                }
            }
        }
        public void DeshacerTransaccion(MementoTransaccion memento)
        {
            List<string> nomHijo = new();
            if (memento == null) return;

            if (memento.OperacionRealizada == TipoOperacion.Asignacion)
            {
                if (memento.TipoDelComponente == TipoComponente.Familia)
                {
                    nomHijo.Add(memento.NombreHijo);
                    QuitarHijos(memento.NombrePadre, nomHijo, false);
                    Console.WriteLine("DAL: Eliminando relación de familia por deshacer");
                } 
                else if (memento.TipoDelComponente == TipoComponente.Perfil)
                {
                    nomHijo.Add(memento.NombreHijo);
                    QuitarHijos(memento.NombrePadre, nomHijo, false);
                    Console.WriteLine("DAL: Eliminando realacion de perfil por deshacer");
                }
                else
                {
                    nomHijo.Add(memento.NombreHijo);
                    QuitarHijos(memento.NombrePadre, nomHijo, true);
                    Console.WriteLine("DAL: Eliminando permiso por deshacer");
                }

            }
            else if (memento.OperacionRealizada == TipoOperacion.Eliminacion)
            {
                if (memento.TipoDelComponente == TipoComponente.Familia)
                {
                    nomHijo.Add(memento.NombreHijo);
                    AsignarComponentesHijos(memento.NombrePadre, nomHijo, false);
                    Console.WriteLine("DAL: Re-insertando familia por deshacer");
                }
                else if(memento.TipoDelComponente == TipoComponente.Perfil)
                {
                    nomHijo.Add(memento.NombreHijo);
                    AsignarComponentesHijos(memento.NombrePadre, nomHijo, false);
                    Console.WriteLine("DAL: Re-insertando familia por deshacer");
                }
                else
                {
                    nomHijo.Add(memento.NombreHijo);
                    AsignarComponentesHijos(memento.NombrePadre, nomHijo, true);
                    Console.WriteLine("DAL: Permiso ");
                }

            }
            _digitoVerificadorBLL.RecalcularDVV_General();
        }

        public string exportarArbol(Familia fam, IExportadorPermisos exportador)
        {
            if (fam == null) throw new ArgumentNullException("No hay familia seleccionada");
            return exportador.ExportarReporte(fam);
        }
    }
}