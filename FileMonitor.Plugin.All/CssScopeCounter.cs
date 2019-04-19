namespace FileMonitor.Plugin
{
    public class CssScopeCounter : UTF8_StringPlugin
    {
        public override string Extension => ".css";

        public override string Description => "Проверка количества скобок { и } в файле css";

        public override object Process(string text, object context)
        {
            int open_counter = 0, close_counter = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '{')
                    open_counter++;
                else
                    if (text[i] == '}')
                        close_counter++;
            }

            return "Число скобок { и } " + ((open_counter == close_counter) ? "совпадает" : "не совпадает");
        }
    }
}
