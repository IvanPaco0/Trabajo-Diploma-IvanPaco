using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL usuarioDAL;
        private readonly BitacoraBLL bitacoraBLL;
        private readonly DigitoVerificadorBLL digitoVerificadorBLL;

        public UsuarioBLL()
        {
            usuarioDAL = new UsuarioDAL();
            bitacoraBLL = new BitacoraBLL();
            digitoVerificadorBLL = new DigitoVerificadorBLL();
        }

        public LoginResultado Login(string usuario, string contraseña)
        {
            try
            {
                if (SessionManager.Instance.Logueado())
                {
                    throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText1"));
                }

                if (usuarioDAL == null)
                {
                    throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText2"));
                }

                Usuario usuarioBE = usuarioDAL.ObtenerPorUserName(usuario);
                if (usuarioBE != null)
                {
                    if (usuarioBE.Bloqueado) return LoginResultado.Bloqueado;

                    if (usuarioBE.Password != Encriptador.GetHash256(contraseña))
                    {
                        usuarioDAL.SumarIntentoFallido(usuarioBE);

                        if (usuarioDAL.ObtenerIntentosFallidos(usuarioBE.DNI) >= 3)
                        {
                            usuarioDAL.BloquearUsuario(usuarioBE.Username);
                            try
                            {
                                bitacoraBLL.RegistrarEvento(new Bitacora
                                {
                                    Login = usuarioBE.Username,
                                    Modulo = "Usuarios",
                                    Evento = "Bloquear usuario - 3 intentos fallidos",
                                    Criticidad = 1
                                });
                            }
                            catch { }
                            return LoginResultado.Bloqueado;
                        }
                        return LoginResultado.ContraseñaIncorrecta;
                    }
                    else
                    {
                    
                        BLL.PerfilBLL perfilBLL = new BLL.PerfilBLL();
                        var todosLosPerfiles = perfilBLL.ObtenerPerfiles();
                        var perfilUsuario = todosLosPerfiles.FirstOrDefault(p => p.Nombre == usuarioBE.Rol);

                        if (perfilUsuario != null)
                        {
                            usuarioBE.Permisos.Add(perfilUsuario);
                        }

                        SessionManager.Instance.Loguear(usuarioBE);

                        return LoginResultado.Valido;
                    }
                }
                return LoginResultado.UsuarioNoEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Desloguear()
        {
            try
            {
                Bitacora bitacora = new Bitacora();
                bitacora.Login = SessionManager.Instance.UsuarioActual().Username;

                SessionManager.Instance.Desloguear();

                bitacora.Modulo = "Usuarios";
                bitacora.Evento = "Logout";
                bitacora.Criticidad = 1;
                bitacoraBLL.RegistrarEvento(bitacora);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RegistrarUsuario(Usuario usuarioBE)
        {
            try
            {
                ValidarCaracteresUsuario(usuarioBE);
                if (usuarioDAL.BuscarUsuarioPorDNI(usuarioBE.DNI) == null)
                {
                    string cadenaDV = 
                        usuarioBE.DNI + 
                        usuarioBE.Nombre + 
                        usuarioBE.Apellido + 
                        usuarioBE.Username + 
                        usuarioBE.Password + 
                        usuarioBE.Email + 
                        usuarioBE.Bloqueado + 
                        usuarioBE.Activo +
                        usuarioBE.Rol;
                    
                    usuarioDAL.Registrar(usuarioBE, DigitoVerificador.CalcularDVH(cadenaDV));
                    digitoVerificadorBLL.RecalcularDVV_Usuario();
                    Bitacora bitacora = new Bitacora();
                    bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                    bitacora.Modulo = "Usuarios";
                    bitacora.Evento = "Crear usuario";
                    bitacora.Criticidad = 1;
                    bitacoraBLL.RegistrarEvento(bitacora);
                }
                else
                {
                    throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText3"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ValidarCaracteresUsuario(Usuario usuario)
        {
            if (!Regex.IsMatch(usuario.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText4"));
            if (!Regex.IsMatch(usuario.DNI, @"^\d{8}$")) throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText5"));
            if (!Regex.IsMatch(usuario.Nombre, @"^.{3,}$") || !Regex.IsMatch(usuario.Apellido, @"^.{3,}$"))
                throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText6"));
        }

        public List<Usuario> ListarUsuariosActivos()
        {
            var todos = usuarioDAL.ListarTodosLosUsuarios();
            var activos = todos.Where(u => u.Activo).ToList();
            return activos;
        }

        public void EliminarLogico(string dNI)
        {
            try
            {
                Usuario user = usuarioDAL.BuscarUsuarioPorDNI(dNI);

                if (user.Activo)
                {
                    usuarioDAL.EliminarLogico(dNI);
                    digitoVerificadorBLL.RecalcularDVH_Usuario();    
                    digitoVerificadorBLL.RecalcularDVV_Usuario();

                    Bitacora bitacora = new Bitacora();
                    bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                    bitacora.Modulo = "Usuarios";
                    bitacora.Evento = "Baja lógica de usuario";
                    bitacora.Criticidad = 1;
                    bitacoraBLL.RegistrarEvento(bitacora);
                }
                else
                {
                    usuarioDAL.ActivarUsuario(dNI);
                    digitoVerificadorBLL.RecalcularDVH_Usuario();
                    digitoVerificadorBLL.RecalcularDVV_Usuario();
                    Bitacora bitacora = new Bitacora();
                    bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                    bitacora.Modulo = "Usuarios";
                    bitacora.Evento = "Reactivar usuario";
                    bitacora.Criticidad = 1;
                    bitacoraBLL.RegistrarEvento(bitacora);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LoginResultado CambiarClave(string usuario, string contraActual, string nuevaContra)
        {
            try
            {
                Usuario user = usuarioDAL.ObtenerPorUserName(usuario);

                if (user == null) return LoginResultado.UsuarioNoEncontrado;
                if (user.Password == Encriptador.GetHash256(nuevaContra)) return LoginResultado.ContraseñaIguales;
                if (user.Password != Encriptador.GetHash256(contraActual)) return LoginResultado.ContraseñaIncorrecta;
                if (user.Bloqueado) return LoginResultado.Bloqueado;

                usuarioDAL.CambiarClave(usuario, Encriptador.GetHash256(nuevaContra));
                digitoVerificadorBLL.RecalcularDVH_Usuario();
                digitoVerificadorBLL.RecalcularDVV_Usuario();
                Bitacora bitacora = new Bitacora();
                bitacora.Login = usuario;
                bitacora.Modulo = "Usuarios";
                bitacora.Evento = "Cambiar clave";
                bitacora.Criticidad = 1;
                bitacoraBLL.RegistrarEvento(bitacora);

                return LoginResultado.Valido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(string dNI, Usuario usuarioBE)
        {
            Usuario existente = usuarioDAL.BuscarUsuarioPorDNI(usuarioBE.DNI);
            if (existente != null && existente.DNI != dNI)
                throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText7"));
            else
            {
                Usuario repetido = usuarioDAL.ObtenerPorUserName(usuarioBE.Username);
                if (repetido != null && repetido.DNI != dNI)
                    throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText8"));
                else
                {
                    usuarioDAL.Modificar(dNI, usuarioBE);
                    digitoVerificadorBLL.RecalcularDVH_Usuario();
                    digitoVerificadorBLL.RecalcularDVV_Usuario();
                    Bitacora bitacora = new Bitacora();
                    bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                    bitacora.Modulo = "Usuarios";
                    bitacora.Evento = "Modificar usuario";
                    bitacora.Criticidad = 1;
                    bitacoraBLL.RegistrarEvento(bitacora);
                }
            }
        }

        public void DesbloquearUsuario(Usuario usuarioBE)
        {
            try
            {
                Usuario user = usuarioDAL.BuscarUsuarioPorDNI(usuarioBE.DNI);

                if (user.Bloqueado)
                {
                    string nuevaClave = Encriptador.GetHash256(user.DNI + user.Nombre);

                    usuarioDAL.DesbloquearUsuario(user.DNI, nuevaClave);
                    digitoVerificadorBLL.RecalcularDVH_Usuario();
                    digitoVerificadorBLL.RecalcularDVV_Usuario();
                    Bitacora bitacora = new Bitacora();
                    bitacora.Login = SessionManager.Instance.UsuarioActual().Username;
                    bitacora.Modulo = "Usuarios";
                    bitacora.Evento = "Desbloquear usuario";
                    bitacora.Criticidad = 1;
                    bitacoraBLL.RegistrarEvento(bitacora);
                }
                else
                {
                    throw new Exception(LanguageManager.Instance.GetTraduction("UserBLLText9"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> FiltrarUsuarios(List<Usuario> usuarios, string texto)
        {
            if (usuarios == null) return new List<Usuario>();
            string[] palabras = (texto ?? "").Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (palabras.Length == 0) return usuarios;

            return usuarios.Where(u => palabras.All(p =>
                    (u.DNI ?? "").StartsWith(p, StringComparison.OrdinalIgnoreCase)
                 || (u.Nombre ?? "").IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0
                 || (u.Apellido ?? "").IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0
                 || (u.Username ?? "").IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();
        }

        public List<Usuario> ListarTodosUsuarios() => usuarioDAL.ListarTodosLosUsuarios();
        public Usuario BuscarUsuarioPorDNI(string v) => usuarioDAL.BuscarUsuarioPorDNI(v);
        public Usuario BuscarUsuarioPorUserName(string user) => usuarioDAL.ObtenerPorUserName(user);

        public bool ValidarNuevoUsuario(string user, string contra)
        {
            Usuario usuario = usuarioDAL.ObtenerPorUserName(user);
            if (usuario != null)
            {
                if (usuario.Password == Encriptador.GetHash256(usuario.DNI + usuario.Nombre))
                {
                    if (Encriptador.GetHash256(contra) == usuario.Password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string RetornarRol(string username)
        {
            Usuario usuario = BuscarUsuarioPorUserName(username);
            return usuario.Rol;
        }
    }
}