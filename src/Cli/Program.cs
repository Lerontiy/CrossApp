using System.Runtime.InteropServices;
using System.Text.Json;

var appInfo = new {
    App = "CrossApp",
    Student = "Назаренко Олександр",
    Group = "ФЕІ-33",
    OS = RuntimeInformation.OSDescription,
    DotNetVersion = Environment.Version.ToString(),
    Domain = "Бібліотека"
};

if (args.Contains("--json"))
{
    string jsonString = JsonSerializer.Serialize(appInfo);
    Console.WriteLine(jsonString);
}
else
{
    Console.WriteLine("CrossApp – практикум з крос-платформного програмування");
    Console.WriteLine("Студент: Назаренко Олександр, група ФЕІ-33");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"ОС (OSDescription) : {RuntimeInformation.OSDescription}");
    Console.WriteLine($"ОС (Environment) : {Environment.OSVersion}");
    Console.WriteLine($"Архітектура процесу : {RuntimeInformation.ProcessArchitecture}");
    Console.WriteLine($"Версія .NET (CLR) : {Environment.Version}");
    Console.WriteLine($"Runtime : {RuntimeInformation.FrameworkDescription}");
    Console.WriteLine($"Каталог застосунку : {AppContext.BaseDirectory}");
    Console.WriteLine($"Поточний каталог : {Environment.CurrentDirectory}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine("Предметна область: Бібліотека");
}