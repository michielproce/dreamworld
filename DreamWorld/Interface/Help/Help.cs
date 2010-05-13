using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DreamWorld.Entities;
using DreamWorld.ScreenManagement.Screens;
using DreamWorld.Util;

namespace DreamWorld.Interface.Help
{
    [Serializable]
    public class Help
    {
        private const string HelpFile = @"\Texts\Help.xml";
        public const float HelpDistance = 15f;

        public List<HelpItem> Items { get; set; }

        private Dictionary<string, string> _itemsDictionary;

        private static Help _instance;

        public static Help Instance
        {
            get
            {
                if (_instance == null)
                    LoadInstance();
                return _instance;
            }
        }

        private Help()
        {            
        }

        public static void LoadInstance()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Help));
            StreamReader reader = File.OpenText(GameScreen.Instance.Content.RootDirectory + HelpFile);
            _instance = (Help)xs.Deserialize(reader);
            reader.Close();

            // Build the internal dictionary
            _instance._itemsDictionary = new Dictionary<string, string>();
            foreach (HelpItem item in _instance.Items)
                _instance._itemsDictionary.Add(item.Entity, item.Text);
        }

        public string FindHelp(Entity entity)
        {
            return FindHelp(entity.Name);
        }

        private string FindHelp(string entityName)
        {
            string text;
            _itemsDictionary.TryGetValue(entityName, out text);
            return StringUtil.ParsePlatform(text);
        }

        public bool HasHelp(Entity entity)
        {
            return HasHelp(entity.Name);
        }

        private bool HasHelp(string entityName)
        {
            return _itemsDictionary.ContainsKey(entityName);
        }
            
    }
}
