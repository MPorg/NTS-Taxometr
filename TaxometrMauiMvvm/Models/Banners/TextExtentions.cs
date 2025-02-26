namespace TaxometrMauiMvvm.Models.Banners;

internal static class TextExtentions
{
    public const char Comma = ',';
    public static string TextChanged(this string text, bool hasComma = true)
    {
        if (!hasComma)
        {
            while (text.Contains(Comma))
            {
                text = text.Remove(text.IndexOf(Comma), text.Length - text.IndexOf(Comma));
            }
            return text;
        }

        try
        {
            if (string.IsNullOrEmpty(text))
                return text;
            List<char> chars = new List<char>();
            foreach (char c in text)
            {
                chars.Add(c);
            }
            int i = 0;
            string result = "";
            for (int j = 0; j < chars.Count; j++)
            {
                if (j > 2 && chars[j - 3] == Comma)
                {
                    continue;
                }
                if (chars[j] == Comma)
                {
                    if (j == 0)
                    {
                        result += "0";
                        i++;
                    }

                    if (i > 1)
                    {
                        continue;
                    }
                }
                result += chars[j].ToString();
            }
            return result;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return text;
        }
    }

    public static string TextCompleate(this string text, bool hasComma = true)
    {
        string result = "";
        if (text == null || string.IsNullOrEmpty(text)) result += "0";
        char[] chars = text.ToCharArray();
        if (!text.Contains(Comma))
        {
            result += text;
            result += Comma;
            result += "00";
        }
        else
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (i == text.Length - 1)
                {
                    if (chars[i] == Comma)
                    {
                        result += "00";
                    }
                    else if (chars[i - 1] == Comma)
                    {
                        result += chars[i];
                        result += "0";
                        continue;
                    }
                }
                result += chars[i];
            }
        }

        return result;
    }
}
