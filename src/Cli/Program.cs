using Core.Dto; 
using Core.Import;
string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv"); 
if (!File.Exists(path)) 
{ 
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");  return 1; 
} 
ImportResult<BookDto> result = ProductCsvImporter.Load(path); 
Console.WriteLine($"Завантажено записів: {result.Items.Count}"); 
foreach (BookDto p in result.Items.Take(5)) 
    Console.WriteLine($" {p.Id,-6} {p.Isbn,-10} {p.Title,-26} {p.Year,5} {p.Author}"); 
if (result.Errors.Count > 0) 
{ 
    Console.WriteLine($"Пропущено рядків: {result.Errors.Count}"); 
    foreach (string e in result.Errors) 
        Console.WriteLine($" ! {e}"); 
} 
return 0; 
