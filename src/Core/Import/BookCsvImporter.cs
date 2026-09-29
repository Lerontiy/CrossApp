using Core.Dto; 
namespace Core.Import; 
public static class ProductCsvImporter 
{ 
    // Роздільник — крапка з комою: не конфліктує з комою в назвах товарів.  
    private const char Separator = ','; 
    public static ImportResult<BookDto> Load(string path) 
    { 
        var items = new List<BookDto>(); 
        var errors = new List<string>(); 
        string[] lines = File.ReadAllLines(path); 
        for (int i = 0; i < lines.Length; i++) 
        { 
            int number = i + 1; 
            string line = lines[i]; 
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#')) 
                continue; 
            if (number == 1 && line.StartsWith("id", StringComparison.OrdinalIgnoreCase))  
                continue; // рядок заголовків
            switch (ParseLine(line)) 
            { 
                case ParseOk ok: 
                    items.Add(ok.Value); 
                    break; 
                case ParseFailed failed: 
                    errors.Add($"рядок {number}: {failed.Reason}"); 
                    break; 
            } 
        } 
        return new ImportResult<BookDto>(items, errors); 
    } 
    private static ParseOutcome ParseLine(string line) 
    { 
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries); 
        return parts switch 
        { 
            { Length: < 4 } => new ParseFailed($"очікую 4 колонок, отримав {parts.Length}"),  [_, "", _, _] or [_, _, "", _] 
            => new ParseFailed("SKU або назва порожні"), 
            [_, _, _, var year] when !int.TryParse(year, out int y) || y < 1450 || y > DateTime.Now.Year  
            => new ParseFailed($"рік '{year}' поза допустимими межами") ,  [var Id, var Isbn, var Title, var Year] 
            => new ParseOk(new BookDto(Id, Isbn, Title, int.Parse(Year))),  _ => new ParseFailed($"занадто багато колонок: {parts.Length}")  
        }; 
    } 
    private abstract record ParseOutcome; 
    private sealed record ParseOk(BookDto Value) : ParseOutcome; 
    private sealed record ParseFailed(string Reason) : ParseOutcome; 
} 
