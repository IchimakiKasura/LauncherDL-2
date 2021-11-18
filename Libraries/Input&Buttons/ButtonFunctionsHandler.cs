#nullable disable
namespace launcherDL
{
    class LauncherDL_ButtonEvent : MainWindow
    {
        public async static void Download()
        {
            string urlHost = string.Empty;
            string urlPath = string.Empty;
            string FileFormat = string.Empty;
            string FileFormatName = string.Empty;
            string link = _main.Input_Link.Text;
            string Name = _main.Input_Name.Text;

            _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Fetching...");

            var getTitle = LauncherDL_fetcher.GetURLTitle(link);

            // Check if the URL is valid
            try
            {
                Uri url = new(link);
                urlHost = LauncherDL_regexClass.URLname.Match(url.Host).Groups["host"].ToString();
                urlPath = url.PathAndQuery;

                if(urlPath.Contains("&list=") && _main.isPlaylist)
                {
                    var box = MessageBox.Show("Are you sure to download all the playlist? ","Yow",MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                    if(box == MessageBoxResult.No) _main.isPlaylist = false;
                    if(box == MessageBoxResult.Cancel) 
                    {
                        _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Operation cancelled!");
                        LauncherDL_InputButtons.EnableAllComponents();
                        return;
                    }
                }
            }
            catch
            {
                _main.RichTextBox_Console.AddFormattedText("<Red>[ERROR] <>Invalid URL!");
                LauncherDL_InputButtons.EnableAllComponents();
                return ;
            }

            string Title = await getTitle;

            // Cancel the operation if there's an internet problem
            if(Title == "Error")
            {
                _main.RichTextBox_Console.AddFormattedText("<Red>[ERROR] <>Something went wrong!");
                _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Please check your connection");
                LauncherDL_InputButtons.EnableAllComponents();
                return;
            }

            if (urlPath.Length > 26) urlPath = urlPath.Remove(24) + "...";
            if (Title.Length > 26) Title = Title.Remove(24) + "...";

            // Check if it has a custom name
            if (Name == "Name⠀(optional)") Name = string.Empty;
            else
            {
                Name = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Name));
                _main.TemporaryEncodedName = Name;
            }

            // Check the ComboBox if any changes made to the File Format Text
            if (_main.Input_FileFormat.SelectedIndex != -1)
            {
                FileFormat = _main.TemporaryFormatList[_main.Input_FileFormat.SelectedIndex];
                string selected = _main.Input_FileFormat.Text;
                string name = LauncherDL_regexClass.SelectedRes.Match(selected).Groups["name"].ToString();
                string size = LauncherDL_regexClass.SelectedRes.Match(selected).Groups["size"].ToString().Trim();
                FileFormatName = $"{name} [{size}]";
            } else
            {
                FileFormatName = _main.Input_FileFormat.Text;
                FileFormat = _main.Input_FileFormat.Text;
            }


            // output the info to the user interface console
            _main.RichTextBox_Console.Break("Gray");
            _main.RichTextBox_Console.AddFormattedText(
                $"<Gray>[]Type: {_main.ComboBox_FormatType.SelectionBoxItem}\r"+
                $"<Gray>[]Options: {FileFormatName}\r"+
                $"<Gray>[]MP3: {_main.CheckBox_MP3.IsChecked}\r"+
                $"<Gray>[]Site: {urlHost.ToUpper()}\r"+
                $"<Gray>[]-: {Title}\r"+
                $"<Gray>[]Dir: {urlPath}"
                );
            _main.RichTextBox_Console.Break("Gray");

            await Task.Delay(2000);

            _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Downloading");

            string processString = string.Empty;

            // switch to the combo box format type
            switch (_main.ComboBox_FormatType.SelectedIndex)
            {
                case 0:
                    processString = YTD.Ydl_Download_Custom(link, FileFormat, Name);
                    break;
                case 1:
                    processString = YTD.Ydl_Download(link, Name, true);
                    break;
                case 2:
                    if (_main.CheckBox_MP3.IsChecked.Value) processString = YTD.Ydl_Download(link, Name, false, true);
                    else processString = YTD.Ydl_Download(link, Name);
                    break;
            }
            if(!_main.isPlaylist) processString = processString + " --no-playlist";
            _main.StartProcess(processString);
        }

        public static void Update()
        {
            _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Updating...");
            _main.StartProcess("-U");
        }

        public async static void FileFormat()
        {
            _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Fetching...");

            var getTitle = LauncherDL_fetcher.GetURLTitle(_main.Input_Link.Text);
            string link = _main.Input_Link.Text;

            // checks if the link is valid.
            try
            {
                Uri url = new(_main.Input_Link.Text);
                string path = url.PathAndQuery;
                if(path.Contains("&list="))
                {
                    _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Playlist isn't supported on FileFormat.");
                    LauncherDL_InputButtons.EnableAllComponents();
                    return;
                }
            }
            catch
            {
                _main.RichTextBox_Console.AddFormattedText("<Red>[ERROR] <>Invalid URL!");
                LauncherDL_InputButtons.EnableAllComponents();
                return;
            }

            string Title = await getTitle;

            if(Title == "Error")
            {
                _main.RichTextBox_Console.AddFormattedText("<Red>[ERROR] <>Something went wrong!");
                _main.RichTextBox_Console.AddFormattedText("<Yellow>[INFO] <>Please check your connection");
                LauncherDL_InputButtons.EnableAllComponents();
                return;
            }
            
            if (Title.Length > 26) Title = Title.Remove(24) + "...";

            _main.RichTextBox_Console.AddFormattedText($"<Gray%16>[fetched] <Gray%16>{Title}");
            _main.TemporaryFormatList.Clear();
            _main.TemporaryFormatNames.Clear();
            _main.Input_FileFormat.Items.Clear();
            _main.StartProcess(YTD.Ydl_File_Format(_main.Input_Link.Text));
        }

        public static void OpenOutputHandler(string output)
        {
            Process.Start(new ProcessStartInfo
            {
                Arguments = output,
                FileName = "explorer.exe"
            });
        }
    }
}