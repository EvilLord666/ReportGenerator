﻿using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ReportGenerator.Core.Config
{
    public static class ExecutionConfigManager
    {
        public static ExecutionConfig Read(string file)
        {
            if (!File.Exists(file))
                return null;
            ExecutionConfig result;
            using (Stream reader = new FileStream(file, FileMode.Open))
            {
                result = Serializer.Deserialize(reader) as ExecutionConfig;
            }
            return result;
        }

        public static bool Write(string file, ExecutionConfig config)
        {
            if (config == null)
                return false;
            if (File.Exists(file))
                File.Delete(file);
            using (Stream writer = new FileStream(file, FileMode.CreateNew))
            {
                Serializer.Serialize(writer, config);
                writer.Flush();
            }
            return true;
        }

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ExecutionConfig));
    }
}
