namespace ChatGPT
{
    public class ConfigService
    {
        public static string GetApiKey()
        {
            try
            {
                string filePath = Path.Combine(AppContext.BaseDirectory, "OpenAIApiKey.txt");
                string content = File.ReadAllText(filePath);
                return content;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
