using System;
using DreamWorld.Levels;
using DreamWorld.Levels.PuzzleLevel1;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class MorwirCutscene : CutsceneScreen
    {
        public MorwirCutscene()
        {
            TransitionOnTime = TimeSpan.FromSeconds(3);

            delay = TimeSpan.FromSeconds(3);
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new PuzzleLevel1());
        }
   
        protected override void LoadCutscene()
        {
            texture = Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot1"); // initial texture

            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\39 awake_morwir"), "TODO: Subtitle"));

//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\40 i_am_morwir"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\41 old_mans_poverty"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\42 morwir_knows"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\43 not_kidnapped"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\44 why_here"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\45 big_kettle"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\46 kettle_potion"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\47 dimension_dreamworld"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\48 use_mind"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\49 tubbles_dreamworld"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\50 weak_old"), "TODO: Subtitle"));
//            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Morwir\51 potion_help"), "TODO: Subtitle"));

        }
    }
}
