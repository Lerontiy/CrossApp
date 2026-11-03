using Core.Dto;

namespace Core.Domain;

public sealed class BookCopy
{
    private bool _isIssued;

    public string Id { get; }
    public string Isbn { get; }
    public bool IsIssued => _isIssued;

    private BookCopy(string id, string isbn, bool isIssued)
    {
        Id = id;
        Isbn = isbn;
        _isIssued = isIssued;
    }

    public static BookCopy Create(string id, string isbn)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор примірника обов'язковий", nameof(id));
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN не може бути порожнім", nameof(isbn));

        return new BookCopy(id.Trim(), isbn.Trim().ToUpperInvariant(), false);
    }

    public void Issue()
    {
        if (_isIssued)
            throw new InvalidOperationException($"Примірник {Id} вже виданий, повторна видача неможлива");

        _isIssued = true;
    }

    public void ReturnCopy()
    {
        if (!_isIssued)
            throw new InvalidOperationException($"Примірник {Id} не виданий, його неможливо повернути");

        _isIssued = false;
    }

    public BookCopyDto ToDto() => new(Id, Isbn, IsIssued);

    public static BookCopy FromDto(BookCopyDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        BookCopy copy = Create(dto.Id, dto.Isbn);
        if (dto.IsIssued)
            copy.Issue();

        return copy;
    }

    public override string ToString() => $"{Id} [{Isbn}] - {(IsIssued ? "виданий" : "доступний")}";
}