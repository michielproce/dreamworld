using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;

namespace DreamWorld.Levels
{
    [Serializable]
    public class LevelInformation
    {
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
                if (s.Name.Equals(name))
                {
                    spawn = s;
                    break;
                }
            if (spawn != null)
                Spawns.Remove(spawn);            
        }

        public static LevelInformation Load(string fileName)
        {
            if (File.Exists(AttachPath(fileName)))
            {
                XmlSerializer xs = new XmlSerializer(typeof(LevelInformation));
                StreamReader reader = File.OpenText(AttachPath(fileName));
                LevelInformation levelInformation = (LevelInformation)xs.Deserialize(reader);
                reader.Close();
                return levelInformation;
            }
            return new LevelInformation();
        }

        public static void Save(LevelInformation levelInformation, string fileName)
        {
            XmlSerializer xs = new XmlSerializer(levelInformation.GetType());
            StreamWriter writer = File.CreateText(AttachPath(fileName));
            xs.Serialize(writer, levelInformation);
            writer.Flush();
            writer.Close();
        }

        private static String AttachPath(string fileName)
        {
            return GameScreen.Instance.Content.RootDirectory + @"\Levels\" + fileName;
        }
    }
}
