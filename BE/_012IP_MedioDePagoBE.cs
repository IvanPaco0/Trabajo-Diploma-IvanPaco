namespace _012IP_BE
{
    public class _012IP_MedioDePagoBE
    {
        public int IdMedioDePago { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EsTarjeta { get; set; }

        public override string ToString() => Nombre;
    }
}
