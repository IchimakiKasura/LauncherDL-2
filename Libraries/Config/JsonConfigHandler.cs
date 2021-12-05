using System.Text;
using System.Xml;
using System.Text.Json;

namespace launcherDL.configuration
{
    public struct LauncherDL_configJson
    {
        public string DefaultOutput { get; set; }
        public int DefaultFileTypeOnStartUp { get; set; }
        public bool ShowSystemOutput { get; set; }
        public bool ShowProgressBar { get; set; }
        public bool EnablePlayList { get; set; }
    }

    public class LauncherDL_config
    {
        public string DefaultOutput;
        public int DefaultFileTypeOnStartUp;
        public bool ShowSystemOutput;
        public bool ShowProgressBar;
        public bool EnablePlayList;
        public LauncherDL_config()
        {
            if (File.Exists("config.json"))
            {
                try
                {
                    string json = File.ReadAllText("config.json");
                    LauncherDL_configJson data = JsonSerializer.Deserialize<LauncherDL_configJson>(json);
                    
                    if(data.DefaultFileTypeOnStartUp > 2) {
                        MessageBox.Show("Config.json load failed!\n\nERROR_BAD_FORMAT\ncode:0xB", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(11);
                    }
                    
                    DefaultOutput = data.DefaultOutput;
                    DefaultFileTypeOnStartUp = data.DefaultFileTypeOnStartUp;
                    ShowSystemOutput = data.ShowSystemOutput;
                    ShowProgressBar = data.ShowProgressBar;
                    EnablePlayList = data.EnablePlayList;
                } catch (Exception) {
                    MessageBox.Show("Config.json load failed!\n\nERROR_BAD_FORMAT\ncode:0xB", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(11);
                }
            }
            else 
            {
                LauncherDL_configJson json = new()
                {
                    DefaultOutput = "output",
                    DefaultFileTypeOnStartUp = 0,
                    ShowSystemOutput = true,
                    ShowProgressBar = true,
                    EnablePlayList = true
                };

                DefaultOutput = json.DefaultOutput;
                DefaultFileTypeOnStartUp = json.DefaultFileTypeOnStartUp;
                ShowSystemOutput = json.ShowSystemOutput;
                ShowProgressBar = json.ShowProgressBar;
                EnablePlayList = json.EnablePlayList;

                using var data = JsonDocument.Parse(JsonSerializer.Serialize(json), new(){ AllowTrailingCommas = true });
                
                MemoryStream stream = new();
                using (
                    var utf8JsonWriter = new Utf8JsonWriter(
                        stream,
                        new JsonWriterOptions
                        {
                            Indented = true
                        }
                    )
                )

                data.WriteTo(utf8JsonWriter);

                byte[] info = new UTF8Encoding(true).GetBytes(new UTF8Encoding().GetString(stream.ToArray()));
                FileStream fs = File.Create("config.json");
                fs.Write(info, 0, info.Length);
            }
        }


    }
}