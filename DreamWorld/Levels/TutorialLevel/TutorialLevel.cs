﻿using System;
using DreamWorld.Entities.Global;
using DreamWorld.Rendering.Postprocessing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamWorld.Levels.TutorialLevel
{
    class TutorialLevel : PuzzleLevel
    {

        public TutorialLevel()
        {
            LoadingColor = Color.White;   
        }

        public override string LevelInformationFileName
        {
            get { return "TutorialLevel.xml"; }
        }

        public override void Initialize()
        {
            base.Initialize();

            GameScreen.TransitionOnTime = TimeSpan.FromSeconds(3);

            Info[] infos = new Info[1];

            infos[0] = new Info { Name = "info0", pcText = "bla bla pc", xboxText = "bla bla xbox"};
            infos[0].Initialize();
            infos[0].Group = GetGroup(0);

            infos[0].Body.MoveTo(new Vector3(0,0,-51), Matrix.Identity);
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.Body.Position.Y < -50)
                Player.Respawn();


            // Update for bloom
            float intensity = 1 - GameScreen.TransitionAlpha / 255f;
            bloom.BaseIntensity = 1f + intensity * 6f;
            bloom.BloomIntensity = 1f + intensity * 6f;
            const float sat = 4f;
            bloom.BaseSaturation = sat - intensity * sat;
            bloom.BloomSaturation = sat - intensity * sat;
            base.Update(gameTime);
        }

        public override void InitBloom(ref Bloom bloom)
        {
            bloom.BloomThreshold = .3f;
            bloom.BlurAmount = 8f;
            bloom.BloomIntensity = 1f;
            bloom.BaseIntensity = 1f;
            bloom.BloomSaturation = 2f;
            bloom.BaseSaturation = 2f;
        }
    }
}