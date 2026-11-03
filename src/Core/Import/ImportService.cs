using Core.Dto;
using System.IO;

namespace Core.Import;

public class ImportService
{
    public ImportResult ImportFile(string path)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();

        return extension switch
        {
            ".csv" => BookCsvImporter.Load(path), 
            ".json" => BookJsonImporter.Load(path),
            _ => new ImportResult([], [], [$"Непідтримуваний формат файлу: {extension}"])
        };
    }
}