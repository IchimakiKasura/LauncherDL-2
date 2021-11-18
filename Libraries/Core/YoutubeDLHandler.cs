#nullable disable

namespace launcherDL
{
    class YTD : MainWindow
    {
        private static string ffmpeg = $"{Directory.GetCurrentDirectory()}\\ffmpeg";

        // FFmpeg file checker
        public static string FfmpegCheck()
        {
            string[] Files_ = { "avcodec-58.dll", "avdevice-58.dll", "avfilter-7.dll", "avformat-58.dll", "avutil-56.dll", "ffmpeg.exe", "ffplay.exe", "ffprobe.exe", "postproc-55.dll", "swresample-3.dll", "swscale-5.dll" };
            string MissingFiles = "";
            for (var i = 0; i < Files_.Length; i++)
            {
                if (!File.Exists($"{ffmpeg}\\{Files_[i]}"))
                {
                    MissingFiles += $"{ Files_[i]}\n";
                }
            }

            if (MissingFiles != "")
            {
                return MissingFiles;
            }

            return "success";
        }
    
        /// <summary>
        /// Check the file format of the link
        /// </summary>
        /// <param name="link">Any link that has a video/audio innit boi</param>
        public static string Ydl_File_Format(string link)
        {
            MainWindow._main.RichTextBox_Console.AddFormattedText("<#a85192>[SYSTEM] <Gray>Loading Formats...");
            return($"-F \"{link}\"");
        }

        /// <summary>
        /// Download the link given
        /// </summary>
        /// <param name="link">Any link that has a video/audio innit boi</param>
        /// <param name="IsVideo">IS IT VIDEO FORMAT?</param>
        /// <param name="IsMp3">IS IT MP3 FORMAT?</param>
        public static string Ydl_Download(string link, string name = default, bool IsVideo = false, bool IsMp3 = false)
        {
            string processStartString = string.Empty;
            if (IsVideo)
            {
                if (name == string.Empty)
                {
                    processStartString = $"--format best {link} -o {_main.defaultOutput}/Video/%(title)s.%(ext)s --no-part";
                }
                else
                {
                    processStartString = $"--format best {link} -o {_main.defaultOutput}/Video/{name}.%(ext)s --no-part";
                }
            }
            else
            {
                if (IsMp3)
                {
                    if(name == string.Empty)
                    {
                        processStartString = $"-x --audio-format mp3 {link} -o {_main.defaultOutput}/Audio/%(title)s.%(ext)s --ffmpeg-location \"{ffmpeg}\" --no-part";
                    }
                    else
                    {
                        processStartString = $"-x --audio-format mp3 {link} -o {_main.defaultOutput}/Audio/{name}.%(ext)s --ffmpeg-location \"{ffmpeg}\" --no-part";
                    }

                    if (isFfmpegExist)
                    {
                       return(processStartString);
                    }
                    else
                    {
                        _main.RichTextBox_Console.AddFormattedText("<Red>[ERROR] <White>Ffmpeg is not loaded!");
                        return string.Empty;
                    }
                }
                else
                {
                    if(name == string.Empty)
                    {
                        processStartString = $"--format bestaudio \"{link}\" -o {_main.defaultOutput}/Audio/%(title)s.%(ext)s\"";
                    }
                    else
                    {
                        processStartString = $"--format bestaudio \"{link}\" -o {_main.defaultOutput}/Audio/{name}.%(ext)s\"";
                    }
                }
            }

            return processStartString;
        }
        
        /// <summary>
        /// Download the link given
        /// </summary>
        /// <param name="link">Any link that has a video/audio innit boi</param>
        /// <param name="format">youtube-dl video file type format etc</param>
        public static string Ydl_Download_Custom(string link, string format, string name = default)
        {
            string processStartString = string.Empty;

            if (name == string.Empty)
            {
                processStartString = $"--format {format} \"{link}\" -o {_main.defaultOutput}/formatted/%(ext)s/%(title)s.%(ext)s --ffmpeg-location \"{ffmpeg}\" --no-part";
            }
            else
            {
                processStartString = $"--format {format} \"{link}\" -o {_main.defaultOutput}/formatted/%(ext)s/{name}.%(ext)s --ffmpeg-location \"{ffmpeg}\" --no-part";
            }

            if (isFfmpegExist)
            {
                return processStartString;
            }
            else
            {
                _main.RichTextBox_Console.AddFormattedText("<Red>[ERROR] <White>Ffmpeg is not loaded!");
            }

            return processStartString;
        }
    }
}