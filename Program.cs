using System;
using System.Windows.Forms;

namespace ZenStatesDebugTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Array.Exists(args, DriverCleanup.IsCleanupArgument))
            {
                DriverCleanup.RunCleanupProcess(args);
                return;
            }

            DriverCleanup.WaitForDriverCleanup();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += ApplicationThreadException;
            try
            {
                SettingsForm MainForm = new SettingsForm();
                string appString = $"{Application.ProductName} {Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.'))}";
#if DEBUG
                appString += " (debug)";
#endif
                MainForm.Text = appString;

                // Logon task starts the tool with --applyprofile. The profile was already applied
                // in the constructor, so exit without ever showing the window.
                if (MainForm.IsApplyProfile)
                {
                    MainForm.ShutdownWithoutWindow();
                    return;
                }

                Application.Run(MainForm);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Application.Exit();
            }
        }

        static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // Handle your exception here...
            MessageBox.Show(e.Exception.Message, Properties.Resources.Error);
        }
    }
}
