namespace FileMonitor.Plugin
{
    public class HtmlDivCounter : UTF8_StringPlugin
    {
        public override string Extension => ".html";

        public override string Description => "Подсчет тегов div в файле html";

        public override object Process(string text, object context)
        {
            int counter = 0;
            for (int i = 4; i < text.Length; i++)
            {
                if (text[i - 4] == '<' && text[i - 3] == 'd' && text[i - 2] == 'i' && text[i - 1] == 'v' && (text[i] == '>' || text[i] == ' '))
                {
                    counter++;
                }
            }

            return "Число тегов div равно: " + counter.ToString();
        }
    }
}
