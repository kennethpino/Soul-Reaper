public static class PrettyString
{
    public static string FormatString(string text, bool bold, string color)
    {
        string formattedText = string.Format("<color={0}>{1}</color>",
            color, text);
        formattedText = bold ? string.Format("<b>{0}</b>", formattedText) : formattedText;

        return formattedText;
    }
}
