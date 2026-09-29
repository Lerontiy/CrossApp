using Core.Dto; 
namespace Core.Import; 

public static class ProductCsvImporter 
{ 
    private const char Separator = ','; 
    
    public static ImportResult Load(string path) 
    { 
        var books = new List<BookDto>(); 
        var readers = new List<ReaderDto>(); 
        var errors = new List<string>(); 
        
        string[] lines = File.ReadAllLines(path); 
        for (int i = 0; i < lines.Length; i++) 
        { 
            int number = i + 1; 
            string line = lines[i]; 
            
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#')) 
                continue; 
                
            switch (ParseLine(line)) 
            { 
                case ParseOkBook okB: 
                    books.Add(okB.Value); 
                    break; 
                case ParseOkReader okR: 
                    readers.Add(okR.Value); 
                    break; 
                case ParseFailed failed: 
                    errors.Add($"рядок {number}: {failed.Reason}"); 
                    break; 
            } 
        } 
        return new ImportResult(books, readers, errors); 
    } 

    private static ParseOutcome ParseLine(string line) 
    { 
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries); 
        
        return parts switch 
        { 
            ["B", var id, var isbn, var title, var year, var author] when int.TryParse(year, out int y)
                => new ParseOkBook(new BookDto(id, isbn, title, y, author)),

            ["R", var id, var name] 
                => new ParseOkReader(new ReaderDto(id, name)), 

            ["B", _, _, _, var year, _] 
                => new ParseFailed($"Некоректний рік для книги: {year}"),

            [var prefix, ..] when prefix != "B" && prefix != "R" 
                => new ParseFailed($"Невідомий тип запису: {prefix}"),

            _ => new ParseFailed($"Неправильний формат рядка: {line}")
        };
    } 
    
    private abstract record ParseOutcome; 
    private sealed record ParseOkBook(BookDto Value) : ParseOutcome; 
    private sealed record ParseOkReader(ReaderDto Value) : ParseOutcome; 
    private sealed record ParseFailed(string Reason) : ParseOutcome; 
}