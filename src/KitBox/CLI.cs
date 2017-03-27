using System;
using System.Text;

namespace KitBox
{

    public class CLI
    {
        static private bool run = true;
        static Company kitbox;
        static Person user_session;
        static private Role role = Role.CUSTOMER;

        public static void Run()
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var db = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");

            kitbox = new Company(host, db, user);

            // Verify if there is an 'admin' in the database, if not, create one
            if (kitbox.PersonManager.SelectPerson(Role.WORKER, "admin", "admin") == null)
            {
                var admin = new Worker("admin", "admin", "admin", "admin@admin.com");
                kitbox.PersonManager.InsertPerson(admin, Role.WORKER, "admin");
            }

            CLI.RoleMenu();
            while (CLI.run)
            {
                if (role == Role.CUSTOMER)
                {
                    CLI.CustomerMenu();
                }
                else if (role == Role.WORKER)
                {
                    CLI.WorkerMenu();
                }
            }
        }

        private static void RoleMenu()
        {
            Menu role_menu = new Menu();

            role_menu
                .set_title("Welcome, choose between customer and worker...")
                .add_option("Customer", () => { role = Role.CUSTOMER; return Login(); }, true)
                .add_option("Worker", () => { role = Role.WORKER; return Login(); }, true);

            role_menu.Run();
        }


        private static void CustomerMenu()
        {
            Console.Clear();

            Menu main_menu = new Menu();

            main_menu
                .set_title("What would you like to do?")
                .add_option("Create new order", CLI.Dummy, false)
                .add_option("View active orders", CLI.Dummy, false)
                .add_option("Exit", CLI.Exit, false);

            main_menu.Run();
        }

        private static void WorkerMenu()
        {
            Console.Clear();

            Menu main_menu = new Menu();

            main_menu
                .set_title("What would you like to do?")
                .add_option("Create new order", CLI.Dummy, false)
                .add_option("View order", CLI.Dummy, false)
                .add_option("List active orders", CLI.Dummy, false)
                .add_option("Manage supplier orders", CLI.Dummy, false)
                .add_option("Exit", CLI.Exit, false);

            main_menu.Run();
        }

        private static bool Login()
        {
            Console.Clear();
            Console.WriteLine("\n");
            Console.Write("email: ");
            var email = Console.ReadLine();
            Console.Write("password: ");
            var password = GetConsolePassword();

            var person_manager = kitbox.PersonManager;

            user_session = person_manager.SelectPerson(role, email, password);

            if (user_session == null)
            {
                Console.WriteLine("Username or password is incorrect");
                return false;
            }

            return true;
        }

        // Source: https://gist.github.com/huobazi/1039424
        private static string GetConsolePassword()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        sb.Length--;
                    }

                    continue;
                }

                Console.Write('*');
                sb.Append(cki.KeyChar);
            }

            return sb.ToString();
        }

        private static bool Exit()
        {
            CLI.run = false;
            return true;
        }

        // Dummy method for testing purposes
        private static bool Dummy()
        {
            return true;
        }
    }
}
