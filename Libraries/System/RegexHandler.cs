namespace launcherDL.regex
{
    struct LauncherDL_regexClass
    {
        // RichTextBox
        public static Regex RTBregex = new(@"<(?<color>.*?)(?:%(?:(?<size>.*?)\|(?<weight>.*?))|%(?<sizeOnly>.*?)|)>(?<text>.*?)(?=<|$)", RegexOptions.Compiled);

        // Url
        public static Regex URLTitle = new(@"<title>(?<title>.*?)</title>", RegexOptions.Compiled);
        public static Regex URLname = new(@"\.(?<host>.*?)\.", RegexOptions.Compiled);

        // Output Console
        public static Regex progress = new(@"\[.*\].*?(?<percent>.*?)% of (?<size>.*) at (?<speed>.*) ETA (?<time>.*)", RegexOptions.Compiled);

        // Fileformat
        public static Regex Info = new(@"(?<id>.*?) .*(?<format>mp4|webm|3gp|m4a.*?) *(?:(?<fullResolution>[0-9]*x[0-9]*)|(?<audioOnly>audio only)) *(?<fps>[0-9]*).\|.*?(?<size>.*)(?<Videobitrate>\D[0-9]*k).*?\|.*?", RegexOptions.Compiled);
        public static Regex SelectedRes = new(@"\[.*\].*(?<name>(?:\D+.*p|audio only)).*-(?<size>.*?);", RegexOptions.Compiled);
    }
}