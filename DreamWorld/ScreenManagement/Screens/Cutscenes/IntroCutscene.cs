using System;
using DreamWorld.Levels.VillageLevel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class IntroCutscene : CutsceneScreen
    {
        public IntroCutscene()
        {
            TransitionOnTime = TimeSpan.FromSeconds(3);

            delay = TimeSpan.FromSeconds(5);
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new VillageLevel());
        }

        protected override void LoadCutscene()
        {
            song = Content.Load<Song>(@"Audio\Music\Intro");
            volume = .2f;

            texture = Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot1"); // initial texture

            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\01 once_upon", "Once upon a time, in a galaxy far, far away, a small planet called Wubbles was inhabited by a small, furry kind of people."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\02 kowaalaa's", "They were called Koowaalaas."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\03 planet_Diamonds", "The planet was rich of diamonds, and people from all over their planet and even other planets wanted to have some of them."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\04 fierce_Competition", "Fierce competition broke out between many mining companies,"));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\05 planet_Ordr", "and eventually the Planet Order decided to ban companies from other planets to mine the diamonds"));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\06 wealth_prosperity", "giving the people of Wubbles a chance at wealth and prosperity"));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\07 TDMC", "Soon after, a few companies merged into a single organisation, known simply as The Diamond Mining Company, or TDMC.", Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot2")));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\08 looking_good", "Things were looking good for the planet and the Koowaalaas", TimeSpan.FromSeconds(2)));
            
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\09 tdmc_ruthless", "However, TDMC became ruthless with its employees."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\10 mines_wages", "Many of the mines were very dangerous and the wages were low."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\11 many_few", "Nearly all the wealth gained by the mining of diamonds was controlled by a few, while the many lived in poverty."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\12 villages_ghostlike", "Villages around the mines became ghostlike.", Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot3")));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\13 children_fathers", "Children were getting ill from the fumes of the mines, fathers died young from the hard work"));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\14 mothers_children", "and mothers had hard times raising their children."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\15 city_education", "While young Koowaalaas in the cities had access to education, youngster in the villages did not."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\16 school_travelling", "Often, the nearest school was a few days travelling."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\17 villages_deserted", "So, many villages became hopeless and deserted places.", TimeSpan.FromSeconds(2)));

            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\18 help_villages", "Some Koowaalaas however, felt they had to do something to help the villages.", Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot4")));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\19 WVHO", "They founded an organisation called WVHO, Wobble Village Help Organisation."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\20 food_resources", " The Organisation uses airships to supply the villages with food and resources"));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\21 wise_men", "and held good contact with the wise men that led the villages."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\22 magic_potions", "These old medicine mans had the power to create magic potions."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\23 council_succeed", "A council of medicine men is trying to find a potion to help the villages, but have yet to succeed."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\24 WVHO_villages", "So, the WVHO helps the villages.", TimeSpan.FromSeconds(2)));

            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\25 uncombinable_pair", "Though the WVHO and TDMC may seem uncombinable, there is one remarkable pair coming from both organisations.", Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot5")));
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
