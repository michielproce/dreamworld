using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DreamWorld.ScreenManagement.Screens.Cutscenes
{
    public class GlobalIntroCutscene : CutsceneScreen
    {
        public GlobalIntroCutscene()
        {
            TransitionOnTime = TimeSpan.FromSeconds(3);

            delay = TimeSpan.FromSeconds(5);
        }

        protected override Screen LoadNextScreen()
        {
            return new MainMenuScreen();
        }

        protected override void LoadCutscene()
        {
            song = Content.Load<Song>(@"Audio\Music\Intro");
            volume = .4f;

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
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\22 magic_potions", "These old medicine men had the power to create magic potions."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\23 council_succeed", "A council of medicine men is trying to find a potion to help the villages, but have yet to succeed."));
            lines.Add(new CutsceneLine(@"Audio\Voice\Intro\24 WVHO_villages", "So, the WVHO helps the villages.", TimeSpan.FromSeconds(2)));
        }   
    }
}
