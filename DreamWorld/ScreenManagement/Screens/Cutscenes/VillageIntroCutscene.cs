using System;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class VillageIntroCutscene : CutsceneScreen
    {
        public VillageIntroCutscene()
        {
            TransitionOnTime = TimeSpan.FromSeconds(3);

            delay = TimeSpan.FromSeconds(3);
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new VillageLevel());
        }

        protected override void LoadCutscene()
        {            
            texture = Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot5"); // initial texture

            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\25 uncombinable_pair", "Though the WVHO and TDMC may seem uncombinable, there is one remarkable pair coming from both organisations."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\26 man_woman_son", "These Kowaalaas, the man being a CEO of The Diamond Mining Company and the woman working on the airships of the WVHO, have a son, called Tubbles."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\27 world_mansion", "Although he has a background from both worlds, he has never seen anything beyond the gates of the huge mansion he lives in."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\28 adventurous_boy", "This has made him a very adventurous young boy,"));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\29 exploring_landscapes", "often exploring and wandering around in the house or the landscapes of the mansion."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\30 wanted_more", "But he wanted to see more of the world.", Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot6")));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\31 devised_plan", "He devised a plan to sneak on an airship when his mother would be picked up for a flight."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\31 hidden_ship", "The ship arrived soon, and carefully, Tubbles hid himself in a small chamber in the ship."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\32 departed_sleep", "The ship departed, and Tubbles tried to get some sleep.", TimeSpan.FromSeconds(2)));

            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\33 awoken_violent", "Hours later, Tubbles was awoken by violent shaking of the airship.", Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot7")));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\34 thunderstorms_hold", "He could hear thunderstorms outside and tried to grab hold of something."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\35 floor_fallen", "Before he could reach a handle, the floors suddenly opened and Tubbles fell out, along with some crates. "));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\37 sky_ground", "He fell through the sky and closed his eyes, waiting for the ground.", TimeSpan.FromSeconds(2)));

            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\38 confused_village", "Confused and sore, Tubbles woke up in a strange village.", TimeSpan.FromSeconds(2), Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot8")));
        }   
    }
}
