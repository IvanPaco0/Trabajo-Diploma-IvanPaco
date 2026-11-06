namespace Servicios
{
    /// <summary>Tipo de evento que el sistema registra en la bitácora de eventos.</summary>
    public class TipoEvento
    {
        public string Modulo { get; }
        public string Evento { get; }
        public int Criticidad { get; }

        public TipoEvento(string modulo, string evento, int criticidad)
        {
            Modulo = modulo;
            Evento = evento;
            Criticidad = criticidad;
        }
    }

    /// <summary>
    /// Listado tipificado de todos los eventos que se registran en la bitácora (módulo, evento y criticidad).
    /// Criticidad 1 = mayor importancia, 5 = menor importancia.
    /// El texto guardado en la bitácora empieza con el nombre del evento y puede agregar un detalle
    /// (por ejemplo "Registrar socio - DNI 40111001 - ..."), por eso el filtro por evento busca por contenido.
    /// </summary>
    public static class CatalogoEventos
    {
        public const string Todos = "Todos";

        public static readonly IReadOnlyList<TipoEvento> Eventos = new List<TipoEvento>
        {
            // Usuarios
            new TipoEvento("Usuarios", "Login", 1),
            new TipoEvento("Usuarios", "Logout", 1),
            new TipoEvento("Usuarios", "Bloquear usuario", 1),
            new TipoEvento("Usuarios", "Desbloquear usuario", 1),
            new TipoEvento("Usuarios", "Crear usuario", 1),
            new TipoEvento("Usuarios", "Modificar usuario", 1),
            new TipoEvento("Usuarios", "Baja lógica de usuario", 1),
            new TipoEvento("Usuarios", "Reactivar usuario", 1),
            new TipoEvento("Usuarios", "Cambiar clave", 1),

            // Perfiles
            new TipoEvento("Perfiles", "Crear permiso simple", 1),
            new TipoEvento("Perfiles", "Eliminar permiso simple", 1),
            new TipoEvento("Perfiles", "Crear familia", 1),
            new TipoEvento("Perfiles", "Crear perfil", 1),
            new TipoEvento("Perfiles", "Asignación exitosa", 1),
            new TipoEvento("Perfiles", "Desvincular", 1),
            new TipoEvento("Perfiles", "Eliminar contenedor jerárquico", 1),
            new TipoEvento("Perfiles", "Mitigación de redundancia", 2),
            new TipoEvento("Perfiles", "Desvinculación forzada por redundancia", 2),
            new TipoEvento("Perfiles", "ERROR en EliminarPermisoRedundante", 1),

            // Respaldos
            new TipoEvento("Respaldos", "Backup generado", 1),
            new TipoEvento("Respaldos", "Error al generar Backup", 1),
            new TipoEvento("Respaldos", "Restore ejecutado", 1),
            new TipoEvento("Respaldos", "Error al ejecutar Restore", 1),

            // Dígitos verificadores
            new TipoEvento("DigitoVerificador", "Recalculo de DVV y DVH", 1),
            new TipoEvento("DigitoVerificador", "Error al recalcular DVV y DVH", 1),

            // Bitácora de cambios
            new TipoEvento("Bitácora de cambios", "Activar versión", 1),

            // Socios
            new TipoEvento("Socios", "Registrar socio", 2),
            new TipoEvento("Socios", "Modificar socio", 2),
            new TipoEvento("Socios", "Baja lógica de socio", 2),
            new TipoEvento("Socios", "Reactivar socio", 2),

            // Membresías y pagos
            new TipoEvento("Membresias", "Renovar membresía", 2),
            new TipoEvento("Pagos", "Registrar pago", 2),

            // Inventario
            new TipoEvento("Inventario", "Ingreso de ejemplar", 2),
            new TipoEvento("Inventario", "Ubicar ejemplar", 3),
            new TipoEvento("Inventario", "Cambio de estado", 2),
            new TipoEvento("Inventario", "Baja de ejemplar", 1),
            new TipoEvento("Inventario", "Auditoría de inventario cerrada", 2),
            new TipoEvento("Inventario", "Serializar ejemplares a XML", 3),
            new TipoEvento("Inventario", "Des-serializar ejemplares desde XML", 3),
            new TipoEvento("Inventario", "Emitir informe de inventario y patrimonio", 4),

            // Reportes
            new TipoEvento("Reportes", "Emitir informe de recaudación", 4),
            new TipoEvento("Reportes", "Exportar reporte de socios a PDF", 4),

            // Idioma
            new TipoEvento("Idioma", "Cambiar idioma", 5),
        };

        /// <summary>Módulos tipificados, en el orden del listado.</summary>
        public static List<string> Modulos()
        {
            return Eventos.Select(e => e.Modulo).Distinct().ToList();
        }

        /// <summary>Eventos tipificados de un módulo (todos si el módulo es null o "Todos").</summary>
        public static List<string> EventosDe(string modulo)
        {
            bool todos = string.IsNullOrEmpty(modulo) || modulo == Todos;
            return Eventos.Where(e => todos || e.Modulo == modulo).Select(e => e.Evento).Distinct().ToList();
        }
    }
}
