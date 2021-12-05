#nullable disable
namespace launcherDL
{
    class LauncherDL_ConsoleOutputHandler : LauncherDL_Task
    {
        private static string temp = string.Empty;
        public static void onReceivedDownload(object sender, DataReceivedEventArgs handler)
        {
            string Output = handler.Data ?? string.Empty;

            if (!_main.RichTextBox_Console.Dispatcher.CheckAccess())
            {
                _main.RichTextBox_Console.Dispatcher.BeginInvoke(new DataReceivedEventHandler(onReceivedDownload), new[] { sender, handler });
            }
            else
            {
                // Download
                if (Output.Contains("[download]"))
                {
                    _main.RichTextBox_Console.LoadText(_main.documentTemp);
                    
                    string progress = LauncherDL_regexClass.progress.Match(Output).Groups["percent"].ToString();
                    string size = LauncherDL_regexClass.progress.Match(Output).Groups["size"].ToString();
                    string speed = LauncherDL_regexClass.progress.Match(Output).Groups["speed"].ToString();
                    string eta = LauncherDL_regexClass.progress.Match(Output).Groups["time"].ToString();
                    string TotalPlaylistDownloaded = string.Empty;

                    // Playlist
                    if (Output.Contains("[download] Downloading video"))
                    {
                        TotalPlaylistDownloaded = Regex.Match(Output, @"[0-9].*of.*").Value;
                    }

                    string color = "White";

                    #region Change speed foreground based on the speed.
                    if (speed.Contains("K"))
                    {
                        double speeds = double.Parse(Regex.Replace(speed, @"[a-zA-Z\/]", "").ToString());
                        if (speeds < 199.99) color = "#381900";
                        else color = "Red";
                    }
                    if (speed.Contains("M"))
                    {
                        double speeds = double.Parse(Regex.Replace(speed, @"[a-zA-Z\/]", "").ToString());
                        if (speeds < 0.99) color = "#fff154";
                        else color = "#83fa57";
                    }
                    if (speed.Contains("G")) color = "Pink";
                    #endregion

                    if (TotalPlaylistDownloaded != string.Empty) _main.RichTextBox_Console.AddFormattedText($"<Cyan>[ Playlist  ] <>{TotalPlaylistDownloaded}");

                    _main.RichTextBox_Console.AddFormattedText($"<Cyan>[ PROGRESS  ] <>{progress.Trim()}%");
                    _main.RichTextBox_Console.AddFormattedText($"<Cyan>[ SIZE      ] <>{size}");
                    _main.RichTextBox_Console.AddFormattedText($"<Cyan>[ SPEED     ] <{color}>{speed}");
                    _main.RichTextBox_Console.AddFormattedText($"<Cyan>[ TIME LEFT ] <>{eta}");

                    // ProgressBar
                    if (progress != string.Empty)
                        _main.ProgressBar_bar.Value = int.Parse(Regex.Replace(progress.Trim(), @"\..*", "", RegexOptions.Compiled).ToString());

                    if(progress.Contains("progress"))
                    {
                        _main.RichTextBox_Console.LoadText(_main.documentTemp);
                    }
                }

                //FFmpeg converting
                if (Output.Contains("[ExtractAudio]"))
                {
                    _main.RichTextBox_Console.LoadText(_main.documentTemp);
                    _main.RichTextBox_Console.AddFormattedText("<#83fa57>[FFMPEG] <>Processing フォマっと...");
                }

                if (Output.Contains("ERROR"))
                {
                    _main.RichTextBox_Console.AddFormattedText($"<Red>[ERROR] <>Something went wrong.");
                }
            }
        }
        public static void onReceivedError(object sender, DataReceivedEventArgs handler)
        {
            string Output = handler.Data ?? string.Empty;

            if (!_main.RichTextBox_Console.Dispatcher.CheckAccess())
            {
                _main.RichTextBox_Console.Dispatcher.BeginInvoke(new DataReceivedEventHandler(onReceivedError), new[] { sender, handler });
            }
            else
            {
                if(Output.Contains("Unable to recognize playlist"))
                {
                    _main.RichTextBox_Console.AddFormattedText($"<Red>[ERROR] <>Unable to recognize playlist.");
                    _main.documentTemp = _main.RichTextBox_Console.SaveText();
                }
            }
            proc.CloseMainWindow();
        }
        
        public static void onReceivedUpdate(object sender, DataReceivedEventArgs handler)
        {
            string Output = handler.Data ?? string.Empty;
            if(!_main.RichTextBox_Console.Dispatcher.CheckAccess())
            {
                _main.RichTextBox_Console.Dispatcher.BeginInvoke(new DataReceivedEventHandler(onReceivedUpdate), new[] { sender, handler });
            }
            else
            {
                // Update
                if (Output != string.Empty && Output.Contains("yt-dlp is up to date"))
                {
                    if (_main.outputSystemConsole) _main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>ydl.bin is in latest!");
                }
                _main.ProgressBar_bar.Visibility = Visibility.Hidden;
            }
        }

        public static void onReceivedFormats(object sender, DataReceivedEventArgs handler)
        {
            string data = handler.Data ?? string.Empty;
            if (!_main.ProgressBar_bar.Dispatcher.CheckAccess())
            {
                _main.ProgressBar_bar.Dispatcher.BeginInvoke(new DataReceivedEventHandler(onReceivedFormats), new[] { sender, handler });
            }
            else
            {
                if (data.Contains("["))
                {
                    _main.ProgressBar_bar.Value += 25;

                    if (data.Contains("[info] Available formats"))
                    {
                        _main.ProgressBar_bar.Value += 100;
                    }
                    else
                    {
                        if (_main.ProgressBar_bar.Value >= 90)
                        {
                            _main.ProgressBar_bar.Value = 75;
                        }
                    }
                }
            }

            if (!_main.RichTextBox_Console.Dispatcher.CheckAccess())
            {
                _main.RichTextBox_Console.Dispatcher.BeginInvoke(new DataReceivedEventHandler(onReceivedFormats), new[] { sender, handler });
            }
            else
            {
                string id = LauncherDL_regexClass.Info.Match(data).Groups["id"].Value.Trim();;
                string resolution = LauncherDL_regexClass.Info.Match(data).Groups["fullResolution"].Value.Trim();
                string size = LauncherDL_regexClass.Info.Match(data).Groups["size"].Value.Trim();
                string bitrate = LauncherDL_regexClass.Info.Match(data).Groups["Videobitrate"].Value.Trim();
                string fps = LauncherDL_regexClass.Info.Match(data).Groups["fps"].Value.Trim();
                string format = LauncherDL_regexClass.Info.Match(data).Groups["format"].Value.Trim();

                if(resolution == string.Empty) resolution = LauncherDL_regexClass.Info.Match(data).Groups["audioOnly"].Value.Trim(); 

                if (Regex.IsMatch(resolution, @".*x.*", RegexOptions.Compiled))
                {
                    resolution = Regex.Replace(resolution, @".*x", "", RegexOptions.Compiled) + "p";
                    switch (format)
                    {
                        case "mp4":
                            id += "+140";
                            break;
                        case "webm":
                            id += "+bestaudio";
                            break;
                    }
                }

                if (data != string.Empty && !data.Contains("["))
                {
                    if(fps != string.Empty) fps += "fps";

                    dynamic obj = new
                    {
                        data = data,
                        id = id,
                        resolution = resolution,
                        size = size,
                        bitrate = bitrate,
                        fps = fps,
                        format = format
                    };

                    if(temp == string.Empty)
                    {
                        temp = data;
                        if (resolution != string.Empty)
                        {
                            FormatAdder(obj);
                        }
                    }
                    else
                    {
                        if(temp != data)
                        {
                            temp = data;
                            if (resolution != string.Empty)
                            {
                                FormatAdder(obj);
                            }
                        } else temp = string.Empty;
                    }
                }
            }
        }
    
        // Not really part of the console output but i'll leave it here.
        private static void FormatAdder(dynamic options)
        {
            string format = options.format;

            // ye I kinda hate when its not aligned.
            if(options.format == "mp4") format = "mp4    ";
            if(options.format == "m4a") format = "m4a    ";
            if(options.format == "3gp") format = "3gp     ";

            _main.TemporaryFormatNames.Add(options.data);
            _main.TemporaryFormatList.Add(options.id);
            _main.Input_FileFormat.Items.Add($"[{format}]       {options.resolution}    -   {options.size}; {options.bitrate};  {options.fps}   ");
            _main.RichTextBox_Console.AddFormattedText($"<#a85192>[SYSTEM]<Gray>Added:{options.resolution};{options.size}");
        }
    }
}