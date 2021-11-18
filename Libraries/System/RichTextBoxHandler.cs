#nullable disable warnings

namespace launcherDL
{
    static class LauncherDL_RichTextBoxHandler
    {
        // I don't know why I even add this cuz I know I only used he AddFormatedText cuz I can add colours lol
        public static void AddText(this RichTextBox box, string text, string color = default)
        {
            if (color == default) color = "White";
            TextRange tr = new(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text + "\r";
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new BrushConverter().ConvertFromString(color));
            box.ScrollToEnd();
        }

        /// <summary>
        /// <see langword="&lt;[COLOR / HEX] % [FONT SIZE?] | [FONT WEIGHT?]&gt; [TEXT]"/> <br/>
        /// Append a Text with Format.<br/>
        /// Example:<br/>
        /// <see langword="&quot;&lt;Red&gt;Hello &lt;Green&gt;World!&quot;"/><br/>
        /// other Examples: <br/>
        /// <see langword="&quot;&lt;##ff0000&gt;Hello &lt;#00ff00&gt;World!&quot;"/><br/>
        /// <see langword="&quot;&lt;Red%15|Black&gt;Hello &lt;Green%15|Thin&gt;World!&quot;"/><br/>
        /// <see langword="&quot;&lt;Red%10&gt;Hello &lt;Green%5&gt;World!&quot;"/> <br/>
        /// For the "&lt;" and "&gt;" escaped characters Do this:<br/>
        /// for less that <see langword="$lt$"/>, for greater than <see langword="$gt$"/>
        /// </summary>
        /// <param name="Input">Input Text</param>
        /// <param name="DontAddNewline">Don't Automatically line?<br/>Default: <see langword="false"/></param>
        public static void AddFormattedText(this RichTextBox rt, string Input, bool DontAddNewline = false)
        {
            if(!DontAddNewline) Input = Input + "\r";
            TextRange range;
            foreach (Match textMatch in LauncherDL_regexClass.RTBregex.Matches(Input))
            {
                range = new(rt.Document.ContentEnd, rt.Document.ContentEnd);

                // "??" OPERATOR WONT WORK??
                string color = textMatch.Groups["color"].ToString();
                string size = textMatch.Groups["size"].ToString();
                string weight = textMatch.Groups["weight"].ToString();
                string text = textMatch.Groups["text"].ToString();

                #region THE "??" OPERATOR WONT WORK HELP
                if (size == string.Empty) size = textMatch.Groups["sizeOnly"].ToString();
                if (color == string.Empty) color = "White";
                if (size == string.Empty) size = "19";
                if (weight == string.Empty) weight = "Normal";
                #endregion

                text = text.Replace("$lt$", "<");
                text = text.Replace("$gt$", ">");

                range.Text = text;
                range.ApplyPropertyValue(TextElement.ForegroundProperty, new BrushConverter().ConvertFromString(color));
                range.ApplyPropertyValue(Control.FontSizeProperty, size);
                range.ApplyPropertyValue(Control.FontWeightProperty, weight);
            }
            rt.ScrollToEnd();
        }

        /// <summary>
        /// Save text to <see cref="MemoryStream"/>
        /// </summary>
        /// <returns><see cref="MemoryStream"/></returns>
        public static MemoryStream SaveText(this RichTextBox rt, string format = default)
        {
            if (format == null) format = DataFormats.Xaml;

            FlowDocument doc = rt.Document;
            TextRange range = new(doc.ContentStart, doc.ContentEnd);
            MemoryStream TextStream = new();

            range.Save(TextStream, format);
            TextStream.Flush();
            TextStream.Seek(0, SeekOrigin.Begin);

            return TextStream;
        }

        /// <summary>
        /// Load text
        /// </summary>
        public static void LoadText(this RichTextBox rt, MemoryStream Stream, string format = default)
        {
            if (format == null) format = DataFormats.Xaml;

            FlowDocument doc = new();
            new TextRange(doc.ContentStart, doc.ContentEnd).Load(Stream, format);

            rt.Document = doc;
            rt.ScrollToEnd();
        }

        /// <summary>
        /// Add line "===================================="
        /// </summary>
        /// <param name="color"></param>
        public static void Break(this RichTextBox rt, string color = default)
        {
            if (color == default) color = "White";
            TextRange tr = new(rt.Document.ContentEnd, rt.Document.ContentEnd);
            tr.Text = "====================================\r";
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new BrushConverter().ConvertFromString(color));
            rt.ScrollToEnd();
        }
    }
}