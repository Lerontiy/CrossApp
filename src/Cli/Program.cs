using Core.Dto; 
using Core.Import;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv"); 
if (!File.Exists(path)) 
{ 
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");  
    return 1; 
} 

string extension = Path.GetExtension(path).ToLowerInvariant();

ImportResult result = extension switch
{
    ".csv" => ProductCsvImporter.Load(path),
    ".json" => BookJsonImporter.Load(path),
    _ => new ImportResult([], [], [$"Непідтримуваний формат файлу: {extension}"])
};

Console.WriteLine($"Завантажено книг: {result.Books.Count}"); 
foreach (BookDto b in result.Books.Take(5)) 
    Console.WriteLine($" [B] {b.Id,-6} {b.Isbn,-10} {b.Title,-26} {b.Year,5} {b.Author}"); 

Console.WriteLine($"Завантажено читачів: {result.Readers.Count}"); 
foreach (ReaderDto r in result.Readers.Take(5)) 
    Console.WriteLine($" [R] {r.Id,-6} {r.Name}"); 

if (result.Errors.Count > 0) 
{ 
    Console.WriteLine($"Пропущено рядків/елементів: {result.Errors.Count}"); 
    foreach (string e in result.Errors.Take(5)) 
        Console.WriteLine($" ! {e}"); 
} 

int accepted = result.Books.Count + result.Readers.Count;
int skipped = result.Errors.Count;
int total = accepted + skipped;
double errorPercent = total == 0 ? 0 : (double)skipped / total * 100;

Console.WriteLine($"\n[Статистика] Усього: {total} | Прийнято: {accepted} | Пропущено: {skipped} | Помилок: {errorPercent:F1}%");

return 0;