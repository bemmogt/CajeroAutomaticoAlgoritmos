using System;

namespace CajeroAutomaticoAlgoritmos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Aquí cargamos los usuarios desde el banco
            Banco banco = new Banco();
            banco.CargarUsuarios();
           
            // Cambiamos el color de la letra para darle estilo al inicio
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("******************************************************************");
            Console.WriteLine("**                                                              **");
            Console.WriteLine("**               Bienvenido al Cajero Automático                **");
            Console.WriteLine("**                          Integrantes                         **");
            Console.WriteLine("**     Nathalie María Amalia Carbajal García  1790-24-15648     **");
            Console.WriteLine("**     Karen Ariana Ortega de la Cruz         1790-24-22291     **");
            Console.WriteLine("**     Fabian Alexander Lopez Rivera          1790-24-14383     **");
            Console.WriteLine("**     Bayrón Esau Morales Mazariegos         1790-24-26929     **");
            Console.WriteLine("**                                                              **");
            Console.WriteLine("******************************************************************");
            // Volvemos al color original para que no se vea raro el texto
            Console.ResetColor();

            // Un ciclo infinito para que siempre esté mostrando el menú principal hasta que el usuario decida salir
            while (true)
            {
                MostrarMenuPrincipal(); // Mostramos el menú principal
                string opcion = Console.ReadLine(); // Leemos la opcion que ingresa el usuario

                // Si elige iniciar sesión como administrador
                if (opcion == "1")
                {
                    Console.Clear();
                    // Cambiamos color del texto para que se mire chulo
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("** Iniciar Sesión (Administrador) **");
                    Console.ResetColor();

                    Console.WriteLine("Ingrese su número de cuenta:");
                    string numeroCuenta = Console.ReadLine();

                    Console.WriteLine("Ingrese su PIN:");
                    string pin = Console.ReadLine();

                    // Llamamos a la función para iniciar sesión como administrador (true quiere decir que es admin)
                    Usuario usuario = banco.IniciarSesion(numeroCuenta, pin, true); // esAdmin = true

                    // Si el usuario es correcto, mostramos el menú del administrador
                    if (usuario != null)
                    {
                        MostrarMenuAdministrador(banco);
                    }
                    else
                    {
                        // Si no, mostramos un mensaje en rojo diciendo que la cuenta o el PIN están mal
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Número de cuenta o PIN incorrectos.");
                        Console.ResetColor();
                    }
                }
                // Si elige iniciar sesión como cliente
                else if (opcion == "2")
                {
                    Console.Clear();
                    // Igual que antes, cambiamos el color pa que se mire bonito
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("** Iniciar Sesión (Cliente) **");
                    Console.ResetColor();

                    Console.WriteLine("Ingrese su número de cuenta:");
                    string numeroCuenta = Console.ReadLine();

                    Console.WriteLine("Ingrese su PIN:");
                    string pin = Console.ReadLine();

                    // Llamamos a la función para iniciar sesión como cliente (false indica que no es admin)
                    Usuario usuario = banco.IniciarSesion(numeroCuenta, pin, false); // esAdmin = false

                    // Si el inicio de sesión es correcto, mostramos el menú del cliente
                    if (usuario != null)
                    {
                        MostrarMenuCliente(banco, usuario);
                    }
                    else
                    {
                        // Si no, el cliente metió mal su cuenta o PIN, y le decimos en rojo
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Número de cuenta o PIN incorrectos.");
                        Console.ResetColor();
                    }
                }

                // Si elige salir
                else if (opcion == "3")
                {
                    // Cambiamos el color para mostrar un mensaje de despedida
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Gracias por utilizar nuestro cajero automático.");
                    Console.ResetColor();
                    Console.WriteLine("\nPresione cualquier tecla para salir..."); 
                    Console.ReadKey(); // Espera a que presione cualquier tecla para salir
                    break; // Rompemos el ciclo para salir del programa
                }
                else
                {
                    // Si elige una opción que no existe, le decimos que está mala la opción.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opción no válida.");
                    Console.ResetColor();
                }
            }
            // Guardamos los usuarios cuando el programa termina
            banco.GuardarUsuarios();
        }

        // Menú principal
        static void MostrarMenuPrincipal()
        {
            // Le metemos estilo al menú principal
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔═══════════════════════════════════════════╗");
            Console.WriteLine("║            Seleccione una opción          ║");
            Console.WriteLine("╠═══════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Iniciar Sesión (Administrador)         ║");
            Console.WriteLine("║ 2. Iniciar Sesión (Cliente)               ║");
            Console.WriteLine("║ 3. Salir                                  ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.ResetColor();
            Console.Write("Opción: ");
        }

        /// Aquí mostramos el menú para el administrador
        static void MostrarMenuAdministrador(Banco banco)
        {
            while (true)
            {
                Console.Clear(); // Limpiamos la pantalla para que no se mire todo amontonado
                Console.ForegroundColor = ConsoleColor.Cyan;
                // mostramos el menú para el admin
                Console.WriteLine("\n---------------------------------------------------");
                Console.WriteLine("Menú del Administrador: Seleccione una opción:");
                Console.WriteLine("1. Crear nueva cuenta (Administrador)");
                Console.WriteLine("2. Crear nueva cuenta (Cliente)");
                Console.WriteLine("3. Editar cuenta de Cliente");
                Console.WriteLine("4. Lista de clientes");
                Console.WriteLine("5. Cambiar PIN de cliente");
                Console.WriteLine("6. Cerrar sesión");
                Console.WriteLine("----------------------------------------------------");
                Console.ResetColor();
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();

                // Según lo que el admin elija, hacemos la acción correspondiente
                switch (opcion)
                {
                    case "1":
                        CrearCuentaAdministrador(banco);
                        break;
                    case "2":
                        CrearCuentaCliente(banco);
                        break;
                    case "3":
                        EditarCuentaCliente(banco);
                        break;
                    case "4":
                        MostrarListaClientes(banco);
                        break;
                    case "5":
                        CambiarPINCliente(banco);
                        break;
                    case "6":
                        // Si elige cerrar sesión, lo sacamos del menú del admin
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Cerrando sesión...");
                        Console.ResetColor();
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey(); // Fuga de pantalla
                        Console.Clear();
                        return; // Cerrar sesión y volver al menú principal
                    default:
                        // Si elige algo que no está en el menú
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción no válida.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        static void CrearCuentaAdministrador(Banco banco)
        {
            Console.Write("Ingrese el nombre del administrador: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese el número de cuenta: ");
            string numeroCuenta = Console.ReadLine();

            Console.Write("Ingrese el PIN: ");
            string pin = Console.ReadLine();

            Console.Write("Confirme su PIN: ");
            double saldoInicial = Convert.ToDouble(Console.ReadLine());

            banco.CrearUsuario(nombre, numeroCuenta, pin, saldoInicial, true); // true indica que es un administrador
        }

        // Crear cuenta para cliente
        static void CrearCuentaCliente(Banco banco)
        {
            Console.Write("Ingrese el nombre del cliente: ");
            string nombre = Console.ReadLine();

            Console.Write("Ingrese el número de cuenta: ");
            string numeroCuenta = Console.ReadLine();

            Console.Write("Ingrese el PIN: ");
            string pin = Console.ReadLine();

            Console.Write("Ingrese el saldo inicial: ");
            double saldoInicial = Convert.ToDouble(Console.ReadLine());

            banco.CrearUsuario(nombre, numeroCuenta, pin, saldoInicial, false); // false indica que es un cliente
        }

        // Editar cuenta de cliente
        static void EditarCuentaCliente(Banco banco)
        {
            Console.Write("Ingrese el número de cuenta del cliente a editar: ");
            string numeroCuenta = Console.ReadLine();

            Usuario cliente = banco.clientes.Find(c => c.NumeroCuenta == numeroCuenta);
            if (cliente != null)
            {
                Console.Write("Ingrese el nuevo nombre (deje vacío para no cambiar): ");
                string nuevoNombre = Console.ReadLine();
                if (!string.IsNullOrEmpty(nuevoNombre)) cliente.Nombre = nuevoNombre;

                Console.Write("Ingrese el nuevo PIN (deje vacío para no cambiar): ");
                string nuevoPIN = Console.ReadLine();
                if (!string.IsNullOrEmpty(nuevoPIN)) cliente.PIN = nuevoPIN;

                banco.GuardarUsuarios(); // Guardar cambios
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Cuenta de cliente editada exitosamente.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cliente no encontrado.");
                Console.ResetColor();
            }
        }

        // Mostrar lista de clientes
        static void MostrarListaClientes(Banco banco)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nLista de clientes:");
            foreach (var cliente in banco.clientes)
            {
                Console.WriteLine($"- {cliente.Nombre} | Cuenta: {cliente.NumeroCuenta}");
            }
            Console.ResetColor();
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // Cambiar PIN de cliente
        static void CambiarPINCliente(Banco banco)
        {
            Console.Write("Ingrese el número de cuenta del cliente: ");
            string numeroCuenta = Console.ReadLine();

            Usuario cliente = banco.clientes.Find(c => c.NumeroCuenta == numeroCuenta);
            if (cliente != null)
            {
                Console.Write("Ingrese el nuevo PIN: ");
                string nuevoPIN = Console.ReadLine();
                cliente.PIN = nuevoPIN;

                banco.GuardarUsuarios(); // Guardar cambios
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("PIN cambiado exitosamente.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cliente no encontrado.");
                Console.ResetColor();
            }
        }
        // Menú del cliente
        static void MostrarMenuCliente(Banco banco, Usuario usuario)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n--------------------------------------------------");
                    Console.WriteLine($"Bienvenido {usuario.Nombre}, seleccione una opción:");
                    Console.WriteLine("1. Consultar saldo");
                    Console.WriteLine("2. Depositar");
                    Console.WriteLine("3. Retirar");
                    Console.WriteLine("4. Estado de cuenta (Historial)");
                    Console.WriteLine("5. Salir");
                    Console.WriteLine("----------------------------------------------------");
                    Console.ResetColor();
                    Console.Write("Opción: ");
                    string opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            Console.Clear();
                            banco.ConsultarSaldo(usuario);
                            Console.WriteLine("\nPresione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                        case "2":
                            Console.Clear();
                            Console.WriteLine("Ingrese la cantidad a depositar:");
                            double deposito = Convert.ToDouble(Console.ReadLine());
                            banco.DepositarDinero(usuario, deposito);
                            Console.WriteLine("\nPresione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                        case "3":
                            Console.Clear();
                            Console.WriteLine("Ingrese la cantidad a retirar:");
                            double retiro = Convert.ToDouble(Console.ReadLine());
                            banco.RetirarDinero(usuario, retiro);
                            Console.WriteLine("\nPresione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                        case "4":
                            Console.Clear();
                            banco.MostrarHistorial(usuario);
                            Console.WriteLine("\nPresione cualquier tecla para continuar...");
                            Console.ReadKey();
                            break;
                        case "5":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Cerrando sesión...");
                            Console.ResetColor();
                            Console.WriteLine("\nPresione cualquier tecla para continuar...");
                            Console.ReadKey(); // Fuga de pantalla
                            return; // Cerrar sesión y volver al menú principal
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opción no válida.");
                            Console.ResetColor();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
