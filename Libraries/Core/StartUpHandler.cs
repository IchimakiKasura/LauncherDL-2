namespace launcherDL
{
    /// <summary>
    /// Handlers all the startup process
    /// </summary>
    class LauncherDL_StartUpHandler : MainWindow
    {
        /// <summary>
        /// Initiate the components
        /// </summary>
        public static void StartUp()
        {
            // check if ydl.bin exist
            if (!File.Exists(@"./ffmpeg/ydl.bin"))
            {
                MessageBox.Show("the file \"ydl.bin\" isn't found on the ffmpeg folder! please reinstall\n\nERROR_FILE_NOT_FOUND\ncode: 0x2", "                  Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(2);
            }

            // Input Texts
            _main.Input_Link.Foreground = Brushes.DimGray;
            _main.Input_Name.Foreground = Brushes.DimGray;
            _main.Input_Link.Text = "Input⠀Link⠀Here";
            _main.Input_Name.Text = "Name⠀(optional)";

            _main.RichTextBox_Console.AddFormattedText("<White>welcome, <#ff4747%25|ExtraBlack>めぐみん <>here!");

            _main.AstolfoImageTemp = _main.Button_Download.Background;
            _main.AkiraImageTemp = _main.Button_FileFormat.Background;
            _main.VentiImageTemp = _main.Button_Update.Background;

            // Load Config
            var loadConfig = new LauncherDL_config();
            _main.ComboBox_FormatType.SelectedIndex = loadConfig.DefaultFileTypeOnStartUp;
            _main.defaultOutput = loadConfig.DefaultOutput;
            _main.outputSystemConsole = loadConfig.ShowSystemOutput;
            _main.ShowProgressBar = loadConfig.ShowProgressBar;
            _main.isPlaylist = loadConfig.EnablePlayList;
            _main.outputFolder = Directory.Exists(_main.defaultOutput);

            if(_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>FileFormat Default: <#7f7aff>\"best\"");

            if (_main.outputFolder) _main.Button_OpenFolder.Visibility = Visibility.Visible;
            
            string ffmpeg = YTD.FfmpegCheck();
            if (ffmpeg == "success")
            {
                isFfmpegExist = true;
                if(_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>Ffmpeg <DarkGreen>loaded!");
            } 
            else
            {
                isFfmpegExist = false;
                if(_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>Ffmpeg load <DarkRed>failed!");
            }
        
        }
    }
}