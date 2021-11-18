#nullable disable
namespace launcherDL
{
    class LauncherDL_InputEventHandler : MainWindow
    {
        public static void Input_GotFocus()
        {
            if (_main.Input_Link.IsFocused && _main.Input_Link.Text == "Input⠀Link⠀Here")
            {
                _main.Input_Link.Foreground = Brushes.White;
                _main.Input_Link.Text = "";
            }
            if (_main.Input_Name.IsFocused && _main.Input_Name.Text == "Name⠀(optional)")
            {
                _main.Input_Name.Foreground = Brushes.White;
                _main.Input_Name.Text = "";
            }
        }

        public static void Input_LostFocus()
        {
            if (_main.Input_Link.Text == "")
            {
                _main.Input_Link.Foreground = Brushes.DimGray;
                _main.Input_Link.Text = "Input⠀Link⠀Here";
            }
            if (_main.Input_Name.Text == "")
            {
                _main.Input_Name.Foreground = Brushes.DimGray;
                _main.Input_Name.Text = "Name⠀(optional)";
            }
        }
    
        public static void Input_OnDrag_Link()
        {
            _main.Input_Link.Foreground = Brushes.White;
            _main.Input_Link.Text = "";
        }

        public static void Input_OnDrag_Name()
        {
            _main.Input_Name.Foreground = Brushes.White;
            _main.Input_Name.Text = "";
        }

        public static void Input_MouseLeave()
        {
            _main.Button_Download.Background = _main.AstolfoImageTemp;
            _main.Button_FileFormat.Background = _main.AkiraImageTemp;
            _main.Button_Update.Background = _main.VentiImageTemp;
        }
        public static void Input_MouseEnter()
        {
            
            if (_main.Button_Download.IsMouseOver) _main.Button_Download.SetValue(BackgroundProperty, new BrushConverter().ConvertFromString("Pink"));
            if (_main.Button_FileFormat.IsMouseOver) _main.Button_FileFormat.SetValue(BackgroundProperty, new BrushConverter().ConvertFromString("DarkBlue"));
            if (_main.Button_Update.IsMouseOver) _main.Button_Update.SetValue(BackgroundProperty, new BrushConverter().ConvertFromString("Cyan"));
        }
        
        public static void Input_Link_Check()
        {
            if(_main.Input_Link.Text.Contains("&list="))
            {
                _main.Button_FileFormat.IsEnabled = false;
            } else _main.Button_FileFormat.IsEnabled = true;
        }

        public static void CheckBox_MP3_Event()
        {
            if(_main.CheckBox_MP3.IsChecked == true) if(_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>checked \"MP3 format\"");
            if(_main.CheckBox_MP3.IsChecked == false) if(_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>unchecked \"MP3 format\"");
            
        }
    
        public static void ComboBox_FormatType_Changed()
        {
            switch (_main.ComboBox_FormatType.SelectedIndex)
            {
                case 0:
                    if (_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>Changed to \"Custom\"");
                    _main.CheckBox_MP3.Visibility = Visibility.Hidden;
                    _main.Input_FileFormat.IsEnabled = true;
                    _main.Input_FileFormat.Text = "best";
                    break;
                case 1:
                    if (_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>Changed to \"Video\"");
                    _main.CheckBox_MP3.Visibility = Visibility.Hidden;
                    _main.Input_FileFormat.IsEnabled = false;
                    _main.Input_FileFormat.Text = "Unavailable";
                    break;
                case 2:
                    if (_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>Changed to \"Audio\"");
                    _main.CheckBox_MP3.Visibility = Visibility.Visible;
                    _main.Input_FileFormat.IsEnabled = false;
                    _main.Input_FileFormat.Text = "Unavailable";
                    break;
            }
        }
    }
}