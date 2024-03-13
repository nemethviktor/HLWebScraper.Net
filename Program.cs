namespace HLWebScraper.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        // Check if the operating system is supported
        if (!IsSupportedOS())
        {
            MessageBox.Show(text: "This application is not compatible with Windows 7 or Windows 8." +
                                  "\nPlease run this application on Windows 10 or later.",
                caption: "Unsupported Operating System", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            return;
        }

        // Start the WinForms application
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(defaultValue: false);
        Application.Run(mainForm: new FrmMainApp());
    }

    private static bool IsSupportedOS()
    {
        // Get the current operating system version
        Version osVersion = Environment.OSVersion.Version;


        return osVersion.Major >= 10;
    }
}