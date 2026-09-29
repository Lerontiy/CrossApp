using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class BookJsonImporter
{
    public static ImportResult Load(string path)
    {
        var books = new List<BookDto>();
        var readers = new List<ReaderDto>();
        var errors = new List<string>();

        try
        {
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            using JsonDocument doc = JsonDocument.Parse(json);
            
            int index = 0;
            foreach (JsonElement element in doc.RootElement.EnumerateArray())
            {
                index++;
                try
                {
                    string? type = element.TryGetProperty("Type", out var typeProp) 
                        ? typeProp.GetString() 
                        : null;

                    if (type == "B")
                    {
                        var book = element.Deserialize<BookDto>(options);
                        if (book != null) books.Add(book);
                    }
                    else if (type == "R")
                    {
                        var reader = element.Deserialize<ReaderDto>(options);
                        if (reader != null) readers.Add(reader);
                    }
                    else
                    {
                        errors.Add($"елемент {index}: Невідомий тип запису '{type ?? "null"}'");
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"елемент {index}: Помилка парсингу - {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            errors.Add($"Критична помилка читання JSON: {ex.Message}");
        }

        return new ImportResult(books, readers, errors);
    }
}