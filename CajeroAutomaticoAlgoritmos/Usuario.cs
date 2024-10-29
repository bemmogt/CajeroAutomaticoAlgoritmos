using CajeroAutomaticoAlgoritmos;
using System;
using System.Collections.Generic;

public class Usuario
{
    public string Nombre { get; set; }       
    public string NumeroCuenta { get; set; }
    public string PIN { get; set; }
    public double Saldo { get; set; }
    public List<Transaccion> Historial { get; set; }

   
    public Usuario(string nombre, string numeroCuenta, string pin, double saldoInicial)
    {
        Nombre = nombre;
        NumeroCuenta = numeroCuenta;
        PIN = pin;
        Saldo = saldoInicial;
        Historial = new List<Transaccion>();
    }

    public void AgregarTransaccion(string tipo, double monto)
    {
        Transaccion transaccion = new Transaccion(DateTime.Now, tipo, monto);
        Historial.Add(transaccion);
    }

    public void MostrarHistorial()
    {
        if (Historial.Count == 0)
        {
            Console.WriteLine("No hay transacciones registradas.");
        }
        else
        {
            foreach (var transaccion in Historial)
            {
                Console.WriteLine($"{transaccion.Fecha}: {transaccion.Tipo} de Q{transaccion.Monto}");
            }
        }
    }
}
