using System;
using System.IO;
using System.Xml.Serialization;

namespace DreamWorld
{
    [Serializable]
    public class Config
    {
        public const string FILE = "config.xml";

        public int Width {get; set; }
        public int Height { get; set;}
        public bool Fullscreen { get; set; }
        public bool AntiAliasing { get; set; }
        public bool VerticalSync { get; set; }
        public bool Subtitles { get; set; }
        public bool InvertCamera { get; set; }

        public Config()
        {
            // Default values
            Width = 1280;
            Height = 720;
            Fullscreen = false;
            AntiAliasing = false;
            VerticalSync = false;
            Subtitles = true;
            InvertCamera = false;
        }

        public static Config Load()
        {            
            if(File.Exists(FILE))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));
                StreamReader reader = File.OpenText(FILE);
                Config c = (Config)xs.Deserialize(reader);
                reader.Close();
                return c;    
            }
            Config newConfig = new Config();
            Save(newConfig);
            return newConfig;
        }

        public static void Save(Config c)
        {
            XmlSerializer xs = new XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(FILE);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }

    }
}
