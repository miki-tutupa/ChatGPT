namespace ChatGPT
{
    public class TextService
    {
        public static void LineWritter(string text)
        {
            Random random = new();
            text = text.Replace("\n", " \n ");
            int width = Console.WindowWidth;
            string[] words = text.Split(" ");
            string line = "";

            foreach (string word in words)
            {
                if (word == "\n")
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        Thread.Sleep(random.Next(5, 51));
                        Console.Write(line[i]);
                    }
                    Console.WriteLine();
                    line = "";
                }
                else if (line.Length + word.Length + 1 <= width)
                {
                    line += word + " ";
                }
                else
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        Thread.Sleep(random.Next(5, 51));
                        Console.Write(line[i]);
                    }
                    Console.WriteLine();
                    line = word + " ";
                }
            }
            Console.WriteLine(line);
        }
    }
}
