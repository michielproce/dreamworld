namespace DreamWorld.Util
{
    public static class StringUtil
    {
        public static string CutLine(string line, int max)
        {
            for (int i = 0; i < line.Length; i += max)
            {
                for (int j = i; j > 0; j--)
                {
                    if (line[j] == ' ')
                    {
                        line = line.Remove(j, 1).Insert(j, "\n");
                        break;
                    }
                }
            }
            return line;
        }
    }
}
