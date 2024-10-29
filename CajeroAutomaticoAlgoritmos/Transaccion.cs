using System;

public class Transaccion
{
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; }
    public double Monto { get; set; }

    public Transaccion(DateTime fecha, string tipo, double monto)
    {
        Fecha = fecha;
        Tipo = tipo;
        Monto = monto;
    }
}
