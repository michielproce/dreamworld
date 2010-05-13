using System;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DreamWorld.Util
{
    public static class StringUtil
    {
        public static string CutLine(string line, int max)
        {
            string[] lines = line.Split('\n');

            for (int i = 0; i < lines.Length; i++) 
            {             
                for (int j = 0; j < lines[i].Length; j += max)
                {
                    for (int k = j; k > 0; k--)
                    {
                        if (lines[i][k] == ' ')
                        {
                            lines[i] = lines[i].Remove(k, 1).Insert(k, "\n");
                            break;
                        }
                    }
                }
            }
            return string.Join("\n", lines);
        }
        
        public static string CutLine(Viewport vp, SpriteFont font, string line, float amount)
        {
            float charWidth = font.MeasureString(line.Replace("\n", "")).X / line.Length; ;
            float maxWidth = vp.Width * amount;
            int maxChars = (int)Math.Floor(maxWidth / charWidth);

            return CutLine(line, maxChars);
        }
        
        public static string ParsePlatform(string text)
        {
            // Example: Press {Enter|B} to begin.
            // {PC|XBOX} is parsed.

            int platform = GamePad.GetState(PlayerIndex.One).IsConnected ? 2 : 1;
            Regex regex = new Regex(@"{(.*?)\|(.*?)}");
            Match match = regex.Match(text);
            while(match.Success)
            {                
                text = regex.Replace(text, match.Groups[platform].Value, 1, match.Index );
                match = regex.Match(text); // Re-match, since the text has changed.
            }

            return text;
        }
    }
}
