using System;
using System.Collections.Generic;
using System.Text;

namespace Servicios
{
    public enum LoginResultado
    {
        ContraseñaIncorrecta,
        UsuarioNoEncontrado,
        Valido,
        Bloqueado,
        Error,
        ContraseñaIguales,
        Inactivo,
    }
}
