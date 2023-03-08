namespace ChatGPT
{
    public class ConfigService
    {
        public static string GetApiKey()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "OpenAIApiKey.txt");
            string content = File.ReadAllText(filePath);
            return content;
        }
    }
}
