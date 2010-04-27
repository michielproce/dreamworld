using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DreamWorld.Entities;
using DreamWorld.Levels;
using DreamWorld.ScreenManagement.Screens;

namespace DreamWorld.Interface.Help
{
    [Serializable]
    public class Help
    {
        public const string HELP_FILE = @"\Texts\Help.xml";
        public const float HELP_DISTANCE = 30f;

        public List<HelpItem> Items { get; set; }

        private Dictionary<string, string> itemsDictionary;

        private static Help instance;

        public static Help Instance
        {
            get
            {
                if (instance == null)
                    LoadInstance();
                return instance;
            }
        }

        private Help()
        {            
        }

        public static void LoadInstance()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Help));
            StreamReader reader = File.OpenText(GameScreen.Instance.Content.RootDirectory + HELP_FILE);
            instance = (Help)xs.Deserialize(reader);
            reader.Close();

            // Build the internal dictionary
            instance.itemsDictionary = new Dictionary<string, string>();
            foreach (HelpItem item in instance.Items)
                instance.itemsDictionary.Add(item.Entity, item.Text);
        }

        public string FindHelp(Entity entity)
        {
            return FindHelp(entity.Name);
        }

        public string FindHelp(string entityName)
        {
            string text = null;
            itemsDictionary.TryGetValue(entityName, out text);
            return text;
        }

        public bool HasHelp(Entity entity)
        {
            return HasHelp(entity.Name);
        }

        public bool HasHelp(string entityName)
        {
            return itemsDictionary.ContainsKey(entityName);
        }
            
    }
}
