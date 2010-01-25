using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework.Audio;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class MorwirCutscene : CutsceneScreen
    {
        protected override List<CutsceneTexture> LoadTextures()
        {
            List<CutsceneTexture> textures = new List<CutsceneTexture>();
            return textures;
        }

        protected override List<CutsceneLine> LoadLines()
        {
            List<CutsceneLine> lines = new List<CutsceneLine>();

            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\39 awake_morwir"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\40 i_am_morwir"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\41 old_mans_poverty"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\42 morwir_knows"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\43 not_kidnapped"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\44 why_here"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\45 big_kettle"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\46 kettle_potion"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\47 dimension_dreamworld"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\48 use_mind"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\49 tubbles_dreamworld"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\50 weak_old"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\51 potion_help"), "TODO: Subtitle"));

            return lines;
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new VillageLevel());
        }
    }
}
