namespace Core.Dto; 

public sealed record ImportResult(
    IReadOnlyList<BookDto> Books, 
    IReadOnlyList<ReaderDto> Readers, 
    IReadOnlyList<string> Errors
);