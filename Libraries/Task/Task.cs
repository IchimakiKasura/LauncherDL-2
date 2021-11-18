#nullable disable
namespace launcherDL
{
    class LauncherDL_Task : MainWindow
    {
        public static Process proc;
        private async static Task ProcessStartInfoInitialize(DataReceivedEventHandler output, string options)
        {
            proc = new Process();
            proc.StartInfo = new ProcessStartInfo(ExecPath, options)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };
            proc.EnableRaisingEvents = true;
            proc.OutputDataReceived += output;
            proc.ErrorDataReceived += LauncherDL_ConsoleOutputHandler.onReceivedError;
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await proc.WaitForExitAsync();
        }

        public async static Task Download(string options)
        {
            await ProcessStartInfoInitialize(LauncherDL_ConsoleOutputHandler.onReceivedDownload, options);
        }

        public async static Task Update(string options)
        {
            await ProcessStartInfoInitialize(LauncherDL_ConsoleOutputHandler.onReceivedUpdate, options);
        }
        public async static Task Format(string options)
        {
            await ProcessStartInfoInitialize(LauncherDL_ConsoleOutputHandler.onReceivedFormats, options);
        }

    }
}