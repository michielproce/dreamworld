using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    [Serializable]
    public class LevelInformation
    {
        public const string CONTENT_LOCATION = @"..\..\..\Content\Levels\";

        public Vector3 PlayerStartPosition { get; set; }
        public Vector3 PlayerStartRotation { get; set; }
        public List<SpawnInformation> Spawns { get; set; }

        public LevelInformation()
        {
            Spawns = new List<SpawnInformation>();
        }

        public void Remove(string name)
        {
            SpawnInformation spawn = null;
            foreach (SpawnInformation s in Spawns)
                if(s.Name.Equals(name))
                    spawn = s;
            if (spawn != null)
                Spawns.Remove(spawn);            
        }

        public static LevelInformation Load(string fileName)
        {
            if (File.Exists(CONTENT_LOCATION + fileName))
            {
                XmlSerializer xs = new XmlSerializer(typeof(LevelInformation));
                StreamReader reader = File.OpenText(CONTENT_LOCATION + fileName);
                LevelInformation levelInformation = (LevelInformation)xs.Deserialize(reader);
                reader.Close();
                return levelInformation;
            }
            return new LevelInformation();
        }

        public static void Save(LevelInformation levelInformation, string fileName)
        {
            XmlSerializer xs = new XmlSerializer(levelInformation.GetType());
            StreamWriter writer = File.CreateText(CONTENT_LOCATION + fileName);
            xs.Serialize(writer, levelInformation);
            writer.Flush();
            writer.Close();
        }
    }
}
