using System;
using System.Collections.Generic;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class IntroCutscene : CutsceneScreen
    {
        protected override List<CutsceneTexture> LoadTextures()
        {
            List<CutsceneTexture> textures = new List<CutsceneTexture>();
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\test1"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\test2"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\test3"), TimeSpan.FromSeconds(2)));
            return textures;
        }

        protected override List<CutsceneLine> LoadLines()
        {
            List<CutsceneLine> lines = new List<CutsceneLine>();
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\test1"), "Dreamworld is een heel tof spel.", TimeSpan.FromSeconds(1)));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\test2"), "Dit is een tweede zin."));
            return lines;
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new VillageLevel());
        }
    }
}
