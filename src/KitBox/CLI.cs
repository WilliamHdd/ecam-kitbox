using System;

namespace KitBox
{
    public enum Role
    {
        CUSTOMER,
        WORKER
    }

    public class CLI
    {
        static private bool run = true;
        static private Role role = Role.CUSTOMER;

        public static void Run()
        {
            CLI.RoleMenu();
            while (CLI.run)
            {
                CLI.MainMenu();
            }
        }

        private static void RoleMenu()
        {
            Menu role_menu = new Menu();

            role_menu
                .set_title("Welcome, choose between customer and worker...")
                .add_option("Customer", () => { role = Role.CUSTOMER; return true; }, false)
                .add_option("Worker", () => { role = Role.WORKER; return true; }, false);

            role_menu.Run();
        }


        private static void MainMenu()
        {
            Console.Clear();
            //Console.WriteLine("\nWhat would you like to do?\n");

            Menu main_menu = new Menu();

            main_menu
                .set_title("What would you like to do?")
                .add_option("Exit", CLI.Exit, false);
            
            main_menu.Run();
        }

        private static bool Exit()
        {
            CLI.run = false;
            return true;
        }
    }
}
