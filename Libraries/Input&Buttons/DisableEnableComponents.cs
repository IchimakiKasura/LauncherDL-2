namespace launcherDL
{
    class LauncherDL_InputButtons : MainWindow
    {
        public static void DisableAllComponents()
        {
            _main.OnLoaded = true;

            _main.Input_Link.IsEnabled = false;
            _main.Input_Name.IsEnabled = false;
            _main.Input_FileFormat.IsEnabled = false;
            _main.ComboBox_FormatType.IsEnabled = false;

            _main.Button_Download.IsEnabled = false;
            _main.Button_FileFormat.IsEnabled = false;
            _main.Button_Update.IsEnabled = false;

            _main.Label_Copyright.Visibility = Visibility.Hidden;
            _main.Button_OpenFolder.Visibility = Visibility.Hidden;
        }

        public static void EnableAllComponents()
        {
            _main.OnLoaded = false;

            _main.Input_Link.IsEnabled = true;
            _main.Input_Name.IsEnabled = true;
            if(_main.ComboBox_FormatType.SelectedIndex == 0) _main.Input_FileFormat.IsEnabled = true;
            _main.ComboBox_FormatType.IsEnabled = true;

            _main.Button_Download.IsEnabled = true;
            _main.Button_Update.IsEnabled = true;
            _main.Button_FileFormat.IsEnabled = true;

            _main.Label_Copyright.Visibility = Visibility.Visible;
            if (Directory.Exists("output")) _main.Button_OpenFolder.Visibility = Visibility.Visible;
        }
       
    }
}