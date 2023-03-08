using ChatGPT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

#region Configuration
string apiKey;

#if DEBUG
    var builder = Host.CreateDefaultBuilder();
    builder.ConfigureHostConfiguration(icb => { icb.AddUserSecrets<Program>(); });
    var host = builder.Build();
    var configuration = host.Services.GetRequiredService<IConfiguration>();
    apiKey = configuration.GetSection("OpenAIServiceOptions")["ApiKey"]!;
#else
    var algo = ConfigService.GetApiKey();
    string[] arguments = Environment.GetCommandLineArgs();
    if (arguments.Length > 1)
        apiKey = arguments[1];
    else
        apiKey = ConfigService.GetApiKey();
#endif

if (apiKey == null || apiKey == "")
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine("ERROR: ApiKey no encontrada.");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("No se ha encontrado la clave API de OpenAI, por favor, añádela al archivo 'OpenAIApiKey.txt'");
    Console.WriteLine("o ejecuta el programa indicando la ApiKey como parámetro, por ejemplo: ./ChatGPT mi-api-key");
    Console.WriteLine();
    Console.Write("Puedes obtener una clave API en ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("https://platform.openai.com/account/api-keys");
    Console.WriteLine();
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Pulsa cualquier tecla para salir...");
    Console.ReadKey();
    return;
}
var openAiService = new OpenAIService(new OpenAiOptions() { ApiKey = apiKey! });
#endregion

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("Bienvenido a la consola de ChatGPT, para terminar escribe 'Salir'.");
Console.WriteLine();

Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("¿Cómo te llamas? ");
Console.ForegroundColor = ConsoleColor.White;
string name = Console.ReadLine()!;
Console.WriteLine();

string prompt = $"Hola, me llamo {name}";

var messages = new List<ChatMessage>
{
    //// INFO: FromSystem (developer), FromAssistance (AI), FromUser (user)
    //ChatMessage.FromSystem($"El usuario se llama {name}."),
};

do
{
    if (prompt != null)
    {
        messages.Add(ChatMessage.FromUser(prompt));
        var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = messages,
            Model = Models.ChatGpt3_5Turbo,
            MaxTokens = 250,
            Temperature = (float?)0.8
        });

        if (completionResult.Successful)
        {
            var response = completionResult.Choices.First().Message.Content;
            if (response[..4] == @"\n\n") response = response[4..];
            messages.Add(ChatMessage.FromAssistance(response));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-ChatGPT dice: ");

            Console.ForegroundColor = ConsoleColor.White;
            TextService.LineWritter(response);
            Console.WriteLine();
        }
        else
        {
            if (completionResult.Error == null)
            {
                throw new Exception("Unknown Error");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR! {completionResult.Error.Code}: {completionResult.Error.Message}");
            Console.WriteLine();
        }
    }
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"-{name ??= "Anónimo"} dice: ");
    Console.ForegroundColor = ConsoleColor.White;
    prompt = Console.ReadLine()!;
    Console.WriteLine();

}
while (prompt.ToLower() != "salir");