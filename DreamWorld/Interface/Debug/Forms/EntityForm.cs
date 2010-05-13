using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DreamWorld.Cameras;
using DreamWorld.Entities;
using DreamWorld.Entities.Global;
using DreamWorld.Helpers.Debug;
using DreamWorld.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Interface.Debug.Forms
{
    public partial class EntityForm : Form
    {
        public Level Level { private get; set; }
        public Entity Entity { get; set; }        
        public DebugCamera DebugCamera { private get; set; }

        private bool _updating;
        private int _lastEntityTypeSelected;
        
        private readonly bool isPuzzleLevel;

        public EntityForm(bool isPuzzleLevel)
        {
            InitializeComponent();

            this.isPuzzleLevel = isPuzzleLevel;

            if (isPuzzleLevel)
            {
                groupLabel.Visible = true;
                group.Visible = true;
            }

            foreach (string entityTypeName in FindEntityTypeNames())
                entityTypes.Items.Add(entityTypeName);
        }

        public void UpdateForm()
        {            
            _updating = true;
            Text = "Entity: " + Entity.SpawnInformation.Name;

            for (int i = 0; i < entityTypes.Items.Count; i++)
            {
                if (entityTypes.Items[i].Equals(Entity.SpawnInformation.TypeName))
                {
                    entityTypes.SelectedIndex = i;
                    _lastEntityTypeSelected = i;
                    break;
                }
            }

            entityName.Text = Entity.SpawnInformation.Name;
            group.Text = Entity.SpawnInformation.Group.ToString();

            positionX.Text = Entity.SpawnInformation.Position.X.ToString();
            positionY.Text = Entity.SpawnInformation.Position.Y.ToString();
            positionZ.Text = Entity.SpawnInformation.Position.Z.ToString();

            rotationX.Text = Entity.SpawnInformation.Rotation.X.ToString();
            rotationY.Text = Entity.SpawnInformation.Rotation.Y.ToString();
            rotationZ.Text = Entity.SpawnInformation.Rotation.Z.ToString();

            scaleX.Text = Entity.SpawnInformation.Scale.X.ToString();
            scaleY.Text = Entity.SpawnInformation.Scale.Y.ToString();
            scaleZ.Text = Entity.SpawnInformation.Scale.Z.ToString();
            _updating = false;
        }

        private List<string> FindEntityTypeNames()
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
            if(isPuzzleLevel)
            {
                entityTypeNames.Add(typeof(PlaceHolder).Namespace + "." + typeof(PlaceHolder).Name);
                // TODO: Add GroupCenter class to types.
            }
            return entityTypeNames;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(_updating)
                return;
            if(Level.EntityNameExists(entityName.Text) && !Entity.SpawnInformation.Name.Equals(entityName.Text))
            {
                MessageBox.Show("Entity " + entityName.Text + " already exists!");
                entityName.Text = Entity.SpawnInformation.Name;                
                return;
            }         
            
            bool changed = false;

            if (_lastEntityTypeSelected != entityTypes.SelectedIndex)
            {
                Entity.SpawnInformation.TypeName = entityTypes.Items[entityTypes.SelectedIndex].ToString();
                _lastEntityTypeSelected = entityTypes.SelectedIndex;
                changed = true;
            }

            if(!Entity.SpawnInformation.Name.Equals(entityName.Text))
            {
                Entity.SpawnInformation.Name = entityName.Text;
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
            
            int groupId;
            if(int.TryParse(group.Text, out groupId))
            {
                Entity.SpawnInformation.Group = groupId;
                changed = true;
            }
            
            if (changed)
            {
                // TODO: update it in stead of removing + adding
                Entity.Group = null; // Remove it from the group's list
                Entity = Entity.CreateFromSpawnInfo(Entity.SpawnInformation);
                Entity.Initialize();
                Entity.Spawn();

                LevelInformation.Save(Level.LevelInformation,
                                     Level.LevelInformationFileName);        
            }            
        }

        

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Entity.Group = null; // Remove it from the group's list
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

        private void snapToTerrain_Click(object sender, EventArgs e)
        {
            if(Level.Terrain == null)
                return;

            float min = float.MaxValue;
            foreach (ModelMesh mesh in Entity.Model.Meshes)
            {
                List<Vector3[]> triangles = TriangleFinder.find(mesh, Entity.World);
                foreach (Vector3[] triangle in triangles)
                    foreach (Vector3 corner in triangle)
                        if(corner.Y < min)                        
                            min = corner.Y;
            }
            float diff = Entity.Body.Position.Y - min;            
            float height = Level.Terrain.HeightMapInfo.GetHeight(Entity.Body.Position);                        
            height -= diff;
            positionY.Text = "" + height;
            OKButton_Click(sender, e);
        }       
    }
}
