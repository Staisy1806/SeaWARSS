using System;
using System.Windows.Forms;

namespace SeaWars
{
    static class Program
    {
        public static int MapSize { get; internal set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Запускаємо програму з головного меню (форма Home)
            Application.Run(new Home());
        }
    }
}
