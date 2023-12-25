using ElectionApp.Common.DataBase;
using ElectionApp.Main;

namespace ElectionApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DBInitializer.InitializeDatabase();
            string connectionString = DBInitializer.GetConnectionString();

            Application.Run(new MainPage(connectionString));
        }
    }
}
