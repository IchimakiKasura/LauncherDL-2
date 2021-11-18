global using System.Text.RegularExpressions;
global using System.Collections.Generic;
global using System.Windows.Documents;
global using System.Windows.Controls;
global using System.Threading.Tasks;
global using System.ComponentModel;
global using System.Windows.Media;
global using System.Diagnostics;
global using System.Net.Http;
global using System.Windows;
global using System.Text;
global using System.IO;
global using System;
#nullable disable

global using launcherDL.regex;
global using launcherDL.configuration;

namespace launcherDL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ImageHover
        public dynamic AstolfoImageTemp;
        public dynamic VentiImageTemp;
        public dynamic AkiraImageTemp;
        public string defaultOutput;
        #endregion

        #region Config
        // checks if there's an Output folder
        public bool outputFolder;
        // show systemOutput
        public bool outputSystemConsole;
        // show progress bar
        public bool ShowProgressBar;
        // checks if ffmpeg exist
        public static bool isFfmpegExist;
        // Playlist?
        public bool isPlaylist;
        #endregion

        #region process start vars
        public MemoryStream documentTemp;
        public bool OnLoaded;
        public static MainWindow _main;
        #endregion

        #region Temporary variables
        public string TemporaryEncodedName = string.Empty;
        public List<string> TemporaryFormatNames = new List<string>();
        public List<string> TemporaryFormatList = new List<string>();
        #endregion

        private List<string> ext = new List<string>() { "mp4", "mkv", "webm", "mp3", "m4a" };

        // path of the exec
        public static string ExecPath = $"\"{Directory.GetCurrentDirectory()}\\ffmpeg\\ydl.bin\"";

        public MainWindow()
        {
            InitializeComponent();
            _main = this;
            LauncherDL_StartUpHandler.StartUp();
        }

        // Prevent user from exiting when downloading
        private void On_Close(object sender, CancelEventArgs handler)
        {
            if (OnLoaded) {
                var sagot = MessageBox.Show(
                    "Hey! You can't close me while Working! ಠ_ಠ",
                    "Megumin",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                handler.Cancel = (sagot == MessageBoxResult.OK);;
            }
        }

        private void VersionInfo(object sender, RoutedEventArgs handler)
        {
            MessageBox.Show("LauncherDL buildver5.0\n\nHoly HECK? New Design?!?!?!?\n\n That is Very Epic of me.\n\n\n Created by Kasura.", "huzuaah!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }


        #region Buttons
        private void download_Button(object sender, RoutedEventArgs handler) { LauncherDL_InputButtons.DisableAllComponents(); LauncherDL_ButtonEvent.Download();}
        private void fileFormat_Button(object sender, RoutedEventArgs handler) { LauncherDL_InputButtons.DisableAllComponents(); LauncherDL_ButtonEvent.FileFormat(); }
        private void update_Button(object sender, RoutedEventArgs handler) { LauncherDL_InputButtons.DisableAllComponents(); LauncherDL_ButtonEvent.Update();}
        private void OpenFolder_Dir_Button(object sender, RoutedEventArgs handler) { LauncherDL_ButtonEvent.OpenOutputHandler(defaultOutput); }

        #region Open_Dir ContextMenu
        private void OpenDir_Video(object sender, RoutedEventArgs handler) {LauncherDL_ButtonEvent.OpenOutputHandler($"{defaultOutput}\\Video"); }
        private void OpenDir_Audio(object sender, RoutedEventArgs handler) {LauncherDL_ButtonEvent.OpenOutputHandler($"{defaultOutput}\\Audio"); }
        private void OpenDir_mFourA(object sender, RoutedEventArgs handler) {LauncherDL_ButtonEvent.OpenOutputHandler($"{defaultOutput}\\formatted\\m4a"); }
        private void OpenDir_mpThree(object sender, RoutedEventArgs handler) {LauncherDL_ButtonEvent.OpenOutputHandler($"{defaultOutput}\\formatted\\mp3"); }
        private void OpenDir_mpFour(object sender, RoutedEventArgs handler) {LauncherDL_ButtonEvent.OpenOutputHandler($"{defaultOutput}\\formatted\\mp4"); }
        private void OpenDir_webm(object sender, RoutedEventArgs handler) {LauncherDL_ButtonEvent.OpenOutputHandler($"{defaultOutput}\\formatted\\webm"); }
        #endregion
        #endregion
    
        #region Events
        private void ComboBox_FormatType_Changed(object sender, SelectionChangedEventArgs handler) { LauncherDL_InputEventHandler.ComboBox_FormatType_Changed(); }

        // MP3 CheckBox EventHandler
        private void CheckBox_MP3_Event(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.CheckBox_MP3_Event(); }

        // OnHover thingy
        private void Input_GotFocus(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.Input_GotFocus(); }
        private void Input_LostFocus(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.Input_LostFocus(); }
        private void Input_OnDrag_Link(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.Input_OnDrag_Link(); }
        private void Input_OnDrag_Name(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.Input_OnDrag_Name(); }
        private void Input_MouseEnter(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.Input_MouseEnter(); }
        private void Input_MouseLeave(object sender, RoutedEventArgs handler) {LauncherDL_InputEventHandler.Input_MouseLeave();  }
        private void Input_Link_Check(object sender, RoutedEventArgs handler) { LauncherDL_InputEventHandler.Input_Link_Check(); }
        #endregion

        public async void StartProcess(string options)
        {
            // Save a temporary text from the richtextbox
            documentTemp = RichTextBox_Console.SaveText();

            if(options.Contains("-U"))
            {
                // start the update
                ProgressBar_bar.Visibility = Visibility.Hidden;
                await LauncherDL_Task.Update(options);
            }

            if(options.Contains("-f") || options.Contains("--format"))
            {
                // start the download
                if (ShowProgressBar) ProgressBar_bar.Visibility = Visibility.Visible;
                await LauncherDL_Task.Download(options);
            }
            else
            {
                // start the format
                if (ShowProgressBar) ProgressBar_bar.Visibility = Visibility.Visible;
                await LauncherDL_Task.Format(options);
            }

            // Wait for fcking 5ms
            await Task.Delay(500);

            // Process Is Done.
            if (!options.Contains("-U"))
            {
                if (options.Contains("-f"))
                {
                    // Check if the ConsoleOutput has an error to block the "Download Complete".
                    FlowDocument CheckError = new();
                    new TextRange(CheckError.ContentStart, CheckError.ContentEnd).Load(documentTemp, DataFormats.Xaml);
                    string str = new TextRange(CheckError.ContentStart, CheckError.ContentEnd).Text;

                    RichTextBox_Console.LoadText(documentTemp);

                    if(!str.Contains("ERROR"))
                    {
                        RichTextBox_Console.AddFormattedText("<Pink>[YEY] Download Completed!");
                    }
                }
                else
                {
                    if(TemporaryFormatList.Count > 1)
                    {
                        RichTextBox_Console.AddFormattedText($"<Pink>[YEY] Total formats: {TemporaryFormatList.Count}");
                    } 
                    else 
                    {
                        RichTextBox_Console.AddFormattedText($"<Yellow>[INFO] Fetching format failed:\r"+
                        "<Gray>1. Link is not supported?"+
                        "<Gray%13>If link is not supported try downloading it using \"Video\" type\r or setting the Format option to \"best\"\r"+
                        "<Gray>2. Slow internet might caused the problem.");
                    }
                }
            }
            else RichTextBox_Console.AddFormattedText("<Pink>[YEY] Updated!");


            ProgressBar_bar.Visibility = Visibility.Hidden;
            ProgressBar_bar.Value = 0;
            LauncherDL_InputButtons.EnableAllComponents();
            isPlaylist = true;

            // Renaming process
            if(TemporaryEncodedName != string.Empty)
            {
                string DefaultName = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(TemporaryEncodedName));
                switch(ComboBox_FormatType.SelectedIndex)
                {
                    case 0:
                        foreach(string exts in ext)
                        {
                            foreach(string folder in ext)
                            {
                                try{File.Move($"{defaultOutput}\\formatted\\{folder}\\{TemporaryEncodedName}.{exts}", $"{defaultOutput}\\formatted\\{folder}\\{DefaultName}.{exts}");} catch {}
                            }
                        }
                        break;
                    case 1: 
                        foreach(string s in ext)
                        {
                            try{File.Move($"{defaultOutput}\\Video\\{TemporaryEncodedName}.{s}", $"{defaultOutput}\\Video\\{DefaultName}.{s}");} catch {}
                        }
                        break;
                    case 2:
                        foreach(string s in ext)
                        {
                            try{File.Move($"output\\Audio\\{TemporaryEncodedName}.{s}", $"output\\Audio\\{DefaultName}.{s}");} catch {}
                        }
                        break;
                }
                TemporaryEncodedName = string.Empty;
            }

        }
    
    }

}