using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;

namespace DreamWorld.Interface.Debug.Forms
{
    public partial class EntityForm : Form
    {
        public Level Level { private get; set; }
        public Entity Entity { get; set; }        
        public DebugCamera DebugCamera { private get; set; }

        private bool updating;
        private int lastEntityTypeSelected;
        public EntityForm()
        {
            InitializeComponent();
            
            foreach (string entityTypeName in findEntityTypeNames())
                entityTypes.Items.Add(entityTypeName);
        }
        
        public void UpdateEntity()
        {
            
        }

        public void UpdateForm()
        {
            updating = true;
            Text = "Entity: " + Entity.SpawnInformation.Name;

            for (int i = 0; i < entityTypes.Items.Count; i++)
            {
                if (entityTypes.Items[i].Equals(Entity.SpawnInformation.TypeName))
                {
                    entityTypes.SelectedIndex = i;
                    lastEntityTypeSelected = i;
                    break;
                }
            }

            entityName.Text = Entity.SpawnInformation.Name;

            positionX.Text = Entity.SpawnInformation.Position.X.ToString();
            positionY.Text = Entity.SpawnInformation.Position.Y.ToString();
            positionZ.Text = Entity.SpawnInformation.Position.Z.ToString();

            rotationX.Text = Entity.SpawnInformation.Rotation.X.ToString();
            rotationY.Text = Entity.SpawnInformation.Rotation.Y.ToString();
            rotationZ.Text = Entity.SpawnInformation.Rotation.Z.ToString();

            scaleX.Text = Entity.SpawnInformation.Scale.X.ToString();
            scaleY.Text = Entity.SpawnInformation.Scale.Y.ToString();
            scaleZ.Text = Entity.SpawnInformation.Scale.Z.ToString();
            updating = false;
        }

        private List<string> findEntityTypeNames()
        {
            List<string> entityTypeNames = new List<string>();
            Type[] types = Assembly.GetAssembly(GetType()).GetTypes();
            foreach (Type type in types)
            {
                Type subType = type;
                while (subType != typeof(object))
                {
                    if (subType == typeof(Entity) && type.GetField("ListInEditor") != null)
                    {                                                                       
                        entityTypeNames.Add(type.Namespace + "." + type.Name);
                        break;
                    }
                    subType = subType.BaseType;
                }
            }
            return entityTypeNames;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(updating)
                return;
            if(Level.EntityNameExists(entityName.Text) && !Entity.SpawnInformation.Name.Equals(entityName.Text))
            {
                MessageBox.Show("Entity " + entityName.Text + " already exists!");
                entityName.Text = Entity.SpawnInformation.Name;                
                return;
            }         
            
            bool changed = false;

            if(!Entity.SpawnInformation.Name.Equals(entityName.Text))
            {
                Level.RenameEntity(Entity.SpawnInformation.Name, entityName.Text);
                Entity.SpawnInformation.Name = entityName.Text;
                changed = true;
            }

            if(lastEntityTypeSelected != entityTypes.SelectedIndex)
            {             
                Entity.SpawnInformation.TypeName = entityTypes.Items[entityTypes.SelectedIndex].ToString();

                Level.RemoveEntity(Entity.SpawnInformation.Name);                
                Level.CreateEntity(Entity.SpawnInformation);

                lastEntityTypeSelected = entityTypes.SelectedIndex;
                changed = true;
            }
             

            float x, y, z;
            if (float.TryParse(positionX.Text, out x) &&
                float.TryParse(positionY.Text, out y) &&
                float.TryParse(positionZ.Text, out z))
            {
                Entity.SpawnInformation.Position = new Vector3(x, y, z);
                changed = true;
            }
            if (float.TryParse(rotationX.Text, out x) &&
                float.TryParse(rotationY.Text, out y) &&
                float.TryParse(rotationZ.Text, out z))
            {
                Entity.SpawnInformation.Rotation = new Vector3(x, y, z);
                changed = true;
            }
            if (float.TryParse(scaleX.Text, out x) &&
                float.TryParse(scaleY.Text, out y) &&
                float.TryParse(scaleZ.Text, out z))
            {
                Entity.SpawnInformation.Scale = new Vector3(x, y, z);
                changed = true;
            }

            if (changed)
            {
                Entity.Spawn();
                LevelInformation.Save(Level.LevelInformation,
                                     Level.LevelInformationFileName);        
            }            
        }

        

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Level.RemoveEntity(Entity.SpawnInformation.Name);
                Level.LevelInformation.Remove(Entity.SpawnInformation.Name);
                LevelInformation.Save(Level.LevelInformation,
                                      Level.LevelInformationFileName);        
                DebugCamera.ToggleMouseLook(true);
                Entity = null;
                Hide();
            }
        }

        private void EntityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Entity = null;
            DebugCamera.ToggleMouseLook(true);
        }       
    }
}
