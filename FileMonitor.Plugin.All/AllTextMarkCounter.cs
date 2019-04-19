namespace FileMonitor.Plugin
{
    public class AllTextMarkCounter : UTF8_StringPlugin
    {
        public override string Extension => null;

        public override string Description => "Подсчет знаков препинания в текстовом файле";

        public override object Process(string text, object context)
        {
            int counter = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '.' || c == ',' || c == '-' || c == '!' || c == '?' || c == ':')
                    counter++;
            }

            return "Количество знаков препинания: " + counter.ToString();
        }
    }
}
