using Core.Dto; 
using Core.Domain;
using Core.Import;
using System;
using System.IO;
using System.Linq;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv"); 
if (!File.Exists(path)) 
{ 
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");  
    return 1; 
} 

var importService = new ImportService();
ImportResult result = importService.ImportFile(path);

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

var stats = ImportStatistics.Create(result);
Console.WriteLine($"\n[Статистика] Усього: {stats.Total} | Прийнято: {stats.Accepted} | Пропущено: {stats.Skipped} | Помилок: {stats.ErrorPercent:F1}%");

Console.WriteLine("\n=== Доменна модель: успішний сценарій ===");
BookCopy copy = BookCopy.Create("BC-001", "978-617-000-001-1");
Console.WriteLine(copy);
copy.Issue();
Console.WriteLine(copy);
copy.ReturnCopy();
Console.WriteLine(copy);

BookCopyDto snapshot = copy.ToDto();
Console.WriteLine($"Відновлено з DTO: {BookCopy.FromDto(snapshot)}");

Console.WriteLine("\n=== Доменна модель: порушення інваріантів ===");
copy.Issue();
TryDo("повторна видача", copy.Issue);
TryDo("порожній ISBN", () => BookCopy.Create("BC-002", " "));
TryDo("повернення невиданого примірника", () =>
    BookCopy.Create("BC-003", "978-617-000-003-5").ReturnCopy());
Console.WriteLine($"Стан після відмов: {copy}");

return 0;

static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($" {title}: виняток НЕ спрацював — інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" {title}: {ex.GetType().Name} — {ex.Message}");
    }
}