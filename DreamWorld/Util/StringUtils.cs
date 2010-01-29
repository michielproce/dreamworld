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
    }
}
