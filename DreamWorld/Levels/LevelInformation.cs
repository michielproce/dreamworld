using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DreamWorld.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels
{
    [Serializable]
    public class LevelInformation
    {
        public const string CONTENT_LOCATION = @"..\..\..\Content\Levels\";

        public Vector3 PlayerStartPosition { get; set; }
        public Vector3 PlayerStartRotation { get; set; }
        public List<SpawnInformation> Spawns { get; set; }
        public List<GroupColorInformation> GroupColors { get; set; }

        public LevelInformation()
        {
            Spawns = new List<SpawnInformation>();
            GroupColors = new List<GroupColorInformation>();            
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

        public Color GetGroupColor(int groupID)
        {
            foreach (GroupColorInformation gci in GroupColors)
            {
                if (gci.ID == groupID)
                    return gci.GetColor();
            }
            return Color.White;            
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
            StreamWriter writer = File.CreateText(CONTENT_LOCATION + fileName); // We use CONTENT_LOCATION because we don't want to save in the 'bin/Content' directory
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
