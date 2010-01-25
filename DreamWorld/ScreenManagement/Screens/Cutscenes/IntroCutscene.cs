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
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot1"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot2"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot3"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot4"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot5"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot6"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot7"), TimeSpan.FromSeconds(2)));
            textures.Add(new CutsceneTexture(Content.Load<Texture2D>(@"Textures\Cutscenes\Intro\shot8"), TimeSpan.FromSeconds(2)));
            return textures;
        }

        protected override List<CutsceneLine> LoadLines()
        {
            List<CutsceneLine> lines = new List<CutsceneLine>();
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\01 once_upon"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\02 kowaalaa's"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\03 planet_Diamonds"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\04 fierce_Competition"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\05 planet_Ordr"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\06 wealth_prosperity"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\07 TDMC"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\08 looking_good"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\09 tdmc_ruthless"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\10 mines_wages"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\11 many_few"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\12 villages_ghostlike"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\13 children_fathers"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\14 mothers_children"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\15 city_education"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\16 school_travelling"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\17 villages_deserted"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\18 help_villages"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\19 WVHO"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\20 food_resources"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\21 wise_men"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\22 magic_potions"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\23 council_succeed"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\24 WVHO_villages"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\25 uncombinable_pair"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\26 man_woman_son"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\27 world_mansion"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\28 adventurous_boy"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\29 exploring_landscapes"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\30 wanted_more"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\31 devised_plan"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\31 hidden_ship"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\32 departed_sleep"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\33 awoken_violent"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\34 thunderstorms_hold"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\35 floor_fallen"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\36 fallen_crates"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\37 sky_ground"), "TODO: Subtitle"));
            lines.Add(new CutsceneLine(Content.Load<SoundEffect>(@"Audio\Voice\Intro\38 confused_village"), "TODO: Subtitle"));

            return lines;
        }

        protected override Screen LoadNextScreen()
        {
            return new GameScreen(new VillageLevel());
        }
    }
}
