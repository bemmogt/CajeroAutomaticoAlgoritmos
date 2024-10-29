using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Banco
{
    private Dictionary<string, (int intentosFallidos, DateTime ultimaFalla, bool bloqueada)> seguimientoIntentos;
    private TimeSpan tiempoBloqueo = TimeSpan.FromMinutes(5); // Ajustar el tiempo de bloqueo como necesites
    public List<Usuario> clientes;
    public List<Usuario> administradores;


    public Banco()
    {
        clientes = new List<Usuario>();
        administradores = new List<Usuario>();
        seguimientoIntentos = new Dictionary<string, (int intentosFallidos, DateTime ultimaFalla, bool bloqueada)>();
    }

    // Cargar datos de clientes y administradores desde archivos JSON
    public void CargarUsuarios()
    {
        try
        {
            // Cargar clientes
            if (File.Exists("clientes.json"))
            {
                Console.Write("Cargando datos de clientes\n");

                // Animación de barra de progreso para clientes
                int totalClientesSteps = 50;
                for (int i = 0; i <= totalClientesSteps; i++)
                {
                    Console.Write("[");
                    Console.Write(new string('=', i));
                    Console.Write(new string(' ', totalClientesSteps - i));
                    Console.Write($"] {i * 2}%");
                    System.Threading.Thread.Sleep(50); // Ajusta la velocidad de carga
                    Console.SetCursorPosition(0, Console.CursorTop);
                }

                string jsonStringClientes = File.ReadAllText("clientes.json");
                clientes = JsonConvert.DeserializeObject<List<Usuario>>(jsonStringClientes);
                Console.WriteLine("\nClientes cargados exitosamente.");
            }
            else
            {
                clientes = new List<Usuario>();
                Console.WriteLine("No se encontraron datos de clientes.");
            }

            // Cargar administradores
            if (File.Exists("administradores.json"))
            {
                Console.Write("\nCargando datos de administradores\n");

                // Animación de barra de progreso para administradores
                int totalAdminSteps = 50;
                for (int i = 0; i <= totalAdminSteps; i++)
                {
                    Console.Write("[");
                    Console.Write(new string('=', i));
                    Console.Write(new string(' ', totalAdminSteps - i));
                    Console.Write($"] {i * 2}%");
                    System.Threading.Thread.Sleep(50); // Ajusta la velocidad de carga
                    Console.SetCursorPosition(0, Console.CursorTop);
                }

                string jsonStringAdmins = File.ReadAllText("administradores.json");
                administradores = JsonConvert.DeserializeObject<List<Usuario>>(jsonStringAdmins);
                Console.WriteLine("\nAdministradores cargados exitosamente.");
            }
            else
            {
                administradores = new List<Usuario>();
                Console.WriteLine("No se encontraron datos de administradores.");
            }

            // Limpiar la consola después de cargar
            Console.Clear();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error al acceder a los archivos: {ex.Message}");
        }
    }



    // Guardar clientes y administradores en archivos JSON
    public void GuardarUsuarios()
    {
        try
        {
            // Guardar clientes
            Console.WriteLine("Guardando datos de clientes...");
            string jsonStringClientes = JsonConvert.SerializeObject(clientes, Formatting.Indented);
            File.WriteAllText("clientes.json", jsonStringClientes);
            Console.WriteLine("Clientes guardados exitosamente.");

            // Guardar administradores
            Console.WriteLine("Guardando datos de administradores...");
            string jsonStringAdmins = JsonConvert.SerializeObject(administradores, Formatting.Indented);
            File.WriteAllText("administradores.json", jsonStringAdmins);
            Console.WriteLine("Administradores guardados exitosamente.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error al guardar los archivos: {ex.Message}");
        }
    }


    // Método para iniciar sesión (para clientes y administradores)
    public Usuario IniciarSesion(string numeroCuenta, string pin, bool esAdmin)
    {
        List<Usuario> lista = esAdmin ? administradores : clientes;

        // Verificar si la cuenta está bloqueada
        if (seguimientoIntentos.ContainsKey(numeroCuenta))
        {
            var (intentosFallidos, ultimaFalla, bloqueada) = seguimientoIntentos[numeroCuenta];

            if (bloqueada)
            {
                // Si la cuenta está bloqueada, verificar si ya pasó el tiempo de desbloqueo
                if (DateTime.Now - ultimaFalla >= tiempoBloqueo)
                {
                    // Si ya pasó el tiempo de bloqueo, desbloquear y resetear el contador de fallos
                    seguimientoIntentos[numeroCuenta] = (0, DateTime.Now, false);
                    Console.WriteLine($"Cuenta desbloqueada: {numeroCuenta}. Intenta de nuevo.");
                }
                else
                {
                    TimeSpan tiempoRestante = tiempoBloqueo - (DateTime.Now - ultimaFalla);
                    Console.WriteLine($"Cuenta bloqueada. Intenta nuevamente en {tiempoRestante.Minutes} minutos y {tiempoRestante.Seconds} segundos.");
                    return null;
                }
            }
        }

        // Verificar si el número de cuenta y PIN coinciden
        foreach (Usuario usuario in lista)
        {
            if (usuario.NumeroCuenta == numeroCuenta && usuario.PIN == pin)
            {
                Console.WriteLine($"\nInicio de sesión exitoso. Bienvenido, {usuario.Nombre}.");
                seguimientoIntentos[numeroCuenta] = (0, DateTime.Now, false); // Resetear intentos fallidos
                return usuario;
            }
        }

        // Si el PIN es incorrecto
        if (seguimientoIntentos.ContainsKey(numeroCuenta))
        {
            var (intentosFallidos, ultimaFalla, bloqueada) = seguimientoIntentos[numeroCuenta];
            intentosFallidos++;

            // Bloquear la cuenta si ya se alcanzaron 3 intentos fallidos
            if (intentosFallidos >= 3)
            {
                seguimientoIntentos[numeroCuenta] = (intentosFallidos, DateTime.Now, true);
                Console.WriteLine("Cuenta bloqueada por varios intentos fallidos.");
            }
            else
            {
                seguimientoIntentos[numeroCuenta] = (intentosFallidos, DateTime.Now, false);
                Console.WriteLine("\nNúmero de cuenta o PIN incorrectos.");
            }
        }
        else
        {
            // Si es el primer intento fallido
            seguimientoIntentos[numeroCuenta] = (1, DateTime.Now, false);
            Console.WriteLine("\nNúmero de cuenta o PIN incorrectos.");
        }

        return null;
    }



    // Crear un nuevo usuario (cliente o administrador)
    public void CrearUsuario(string nombre, string numeroCuenta, string pin, double saldoInicial, bool esAdmin)
    {
        // Validación de campos vacíos
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(numeroCuenta) || string.IsNullOrWhiteSpace(pin))
        {
            Console.WriteLine("Error: Todos los campos son obligatorios.");
            return;
        }

        // Validación de formato de PIN
        if (pin.Length != 4 || !pin.All(char.IsDigit))
        {
            Console.WriteLine("Error: El PIN debe tener 4 dígitos numéricos.");
            return;
        }

        // Validación de duplicados de cuentas
        if (clientes.Any(c => c.NumeroCuenta == numeroCuenta) || administradores.Any(a => a.NumeroCuenta == numeroCuenta))
        {
            Console.WriteLine("Error: El número de cuenta ya está en uso.");
            return;
        }

        // Creación del nuevo usuario
        Usuario nuevoUsuario = new Usuario(nombre, numeroCuenta, pin, saldoInicial);

        // Agregar a la lista correspondiente y confirmar creación
        if (esAdmin)
        {
            administradores.Add(nuevoUsuario);
            Console.WriteLine($"Administrador creado exitosamente: {nombre}");
        }
        else
        {
            clientes.Add(nuevoUsuario);
            Console.WriteLine($"Cliente creado exitosamente: {nombre}");
        }

        // Guardar los usuarios en el archivo
        GuardarUsuarios();
        Console.ReadKey();
    }



    // Lista de clientes
    public void MostrarClientes()
    {
        Console.WriteLine("\nLista de clientes:");
        foreach (var cliente in clientes)
        {
            Console.WriteLine($"- {cliente.Nombre} | Cuenta: {cliente.NumeroCuenta}");
        }
    }

    // Cambiar PIN de cliente
    public void CambiarPINCliente(string numeroCuenta, string nuevoPIN)
    {
        Usuario cliente = clientes.Find(c => c.NumeroCuenta == numeroCuenta);
        if (cliente != null)
        {
            cliente.PIN = nuevoPIN;
            GuardarUsuarios();
            Console.WriteLine("PIN actualizado exitosamente.");
        }
        else
        {
            Console.WriteLine("Cliente no encontrado.");
        }
    }

    // Mostrar historial de transacciones (incluyendo la tabla)
    public void MostrarHistorial(Usuario usuario)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                Historial de Transacciones              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════╦════════════════════╦══════════════╗");
        Console.WriteLine("║       Fecha        ║      Tipo          ║     Monto    ║");
        Console.WriteLine("╠════════════════════╬════════════════════╬══════════════╣");

        if (usuario.Historial.Count == 0)
        {
            Console.WriteLine("║           No hay transacciones registradas             ║");
        }
        else
        {
            foreach (var transaccion in usuario.Historial)
            {
                Console.WriteLine($"║ {transaccion.Fecha:dd/MM/yyyy HH:mm}   ║ {transaccion.Tipo,-15}    ║ Q{transaccion.Monto,10:F2}  ║");
            }
        }

        Console.WriteLine("╚════════════════════╩════════════════════╩══════════════╝\n");
        Console.ResetColor();

        // Mostrar el saldo total del usuario
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Saldo total: Q{usuario.Saldo:F2}");
        Console.ResetColor();
    }

    // Consultar saldo del usuario actual
    public void ConsultarSaldo(Usuario usuario)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nSaldo actual de la cuenta de {usuario.Nombre}: Q{usuario.Saldo:F2}\n");
        Console.ResetColor();
    }

    // Retirar dinero de la cuenta del usuario
    public void RetirarDinero(Usuario usuario, double cantidad)
    {
        // Validación de cantidad de retiro
        if (cantidad <= 0)
        {
            Console.WriteLine("Error: El monto del retiro debe ser mayor a cero.");
            return;
        }

        // Validación de saldo suficiente
        if (usuario.Saldo >= cantidad)
        {
            usuario.Saldo -= cantidad;
            usuario.AgregarTransaccion("Retiro", cantidad);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Retiro exitoso de Q{cantidad:F2}. Saldo actual: Q{usuario.Saldo:F2}\n");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: Saldo insuficiente para realizar el retiro.\n");
            Console.ResetColor();
        }
    }

    // Depositar dinero en la cuenta del usuario
    public void DepositarDinero(Usuario usuario, double cantidad)
    {
        // Validación de cantidad de depósito
        if (cantidad <= 0)
        {
            Console.WriteLine("Error: El monto del depósito debe ser mayor a cero.");
            return;
        }

        usuario.Saldo += cantidad;
        usuario.AgregarTransaccion("Depósito", cantidad);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Depósito exitoso de Q{cantidad:F2}. Saldo actual: Q{usuario.Saldo:F2}\n");
        Console.ResetColor();
    }

}
