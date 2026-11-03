namespace Core.Dto;

public record ImportStatistics(int Accepted, int Skipped, int Total, double ErrorPercent)
{
    public static ImportStatistics Create(ImportResult result)
    {
        int accepted = result.Books.Count + result.Readers.Count;
        int skipped = result.Errors.Count;
        int total = accepted + skipped;
        double errorPercent = total == 0 ? 0 : (double)skipped / total * 100;

        return new ImportStatistics(accepted, skipped, total, errorPercent);
    }
}