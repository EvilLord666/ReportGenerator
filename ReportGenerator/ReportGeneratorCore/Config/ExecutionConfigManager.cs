using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ReportGenerator.Core.Config
{
    public class ExecutionConfigManager
    {
        public static ExecutionConfig Read(string file)
        {
            if (!File.Exists(file))
                return null;
            ExecutionConfig result;
            using (Stream reader = new FileStream(file, FileMode.Open))
            {
                result = _serializer.Deserialize(reader) as ExecutionConfig;
            }
            return result;
        }

        public static bool Write(string file, ExecutionConfig config)
        {
            if (config == null)
                return false;
            StringWriter stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                _serializer.Serialize(writer, config);
            }
            return true;
        }

        private static XmlSerializer _serializer = new XmlSerializer(typeof(ExecutionConfig));
    }
}
