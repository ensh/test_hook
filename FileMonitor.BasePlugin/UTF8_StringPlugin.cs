using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor
{
    public class UTF8_StringPlugin : IPlugin
    {
        public const int MaxFileSize = 1000000;
        public virtual string Extension => throw new NotImplementedException();

        public virtual string Description => throw new NotImplementedException();

        // предположим, что файл не очень большой
        public virtual object Process(FileStream dataStream, object context)
        {
            if (new FileInfo(dataStream.Name).Length > MaxFileSize)
            {
                throw new FileSizeProcessingException("Превышен размер файла для обработки");
            }

            var text = default(string);
            dataStream.Seek(0, SeekOrigin.Begin);
            using (var memoryStream = new MemoryStream())
            {
                dataStream.CopyTo(memoryStream);
                text = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return Process(text, context);
        }

        public virtual object Process(string data, object context)
        {
            throw new NotImplementedException();
        }
    }
}
