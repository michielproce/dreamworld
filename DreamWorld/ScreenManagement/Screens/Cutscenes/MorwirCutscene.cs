﻿using System;
using DreamWorld.Levels.TutorialLevel;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class MorwirCutscene : CutsceneScreen
    {
        public MorwirCutscene()
        {
            TransitionOnTime = TimeSpan.FromSeconds(3);

            Delay = TimeSpan.FromSeconds(3);
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new TutorialLevel());
        }
   
        protected override void LoadCutscene()
        {
            Texture = Content.Load<Texture2D>(@"Textures\Cutscenes\Morwir\shot1"); // initial texture

            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\39 awake_morwir", "Ah, I see you are awake.. Let me take you in my house, where I will tell you about last night and where you are. I am Morwir, by the way."));
//            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\40 i_am_morwir", "I am Morwir, by the way."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\41 old_mans_poverty", "The old man sits down in a big brown chair and begins to tell about the poverty in the village.", Content.Load<Texture2D>(@"Textures\Cutscenes\Morwir\shot2")));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\42 morwir_knows", "Morwir appears to know who Tubbles is when he speaks about the mines and The Diamond Mining Company."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\43 not_kidnapped", "He quickly reassures Tubbles he has not been kidnapped."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\44 why_here", "Tubbles asks Morwir why he is here."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\45 big_kettle", "Morwir walks to a big kettle in a corner of the house."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\46 kettle_potion", "This kettle is filled with a potion to transport people to another dimension.", Content.Load<Texture2D>(@"Textures\Cutscenes\Morwir\shot3")));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\47 dimension_dreamworld", "Within this dimension, called Dreamworld, people are confronted with problems in the real world in an extremely simplified version."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\48 use_mind", "They are represented through intricate puzzles, which forces the one who drank the potion to use his mind, in stead of anything else."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\49 tubbles_dreamworld", "Thats why you are here, Tubbles. You need to go in the Dreamworld for me.", Content.Load<Texture2D>(@"Textures\Cutscenes\Morwir\shot4")));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\50 weak_old", "The villagers are too weak and I am too old."));
            Lines.Add(new CutsceneLine(@"Audio\Voice\Morwir\51 potion_help", "Take this cup of potion and help my village."));
        }
    }
}
