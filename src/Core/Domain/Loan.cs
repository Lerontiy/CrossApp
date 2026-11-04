using Core.Dto;

namespace Core.Domain;

public sealed class Loan
{
    public string Id { get; }
    public string BookCopyId { get; }
    public string ReaderId { get; }
    public DateOnly IssuedOn { get; }
    public DateOnly? ReturnedOn { get; private set; }

    private Loan(string id, string bookCopyId, string readerId, DateOnly issuedOn, DateOnly? returnedOn)
    {
        Id = id;
        BookCopyId = bookCopyId;
        ReaderId = readerId;
        IssuedOn = issuedOn;
        ReturnedOn = returnedOn;
    }

    public static Loan Open(string id, BookCopy copy, string readerId, DateOnly issuedOn)
    {
        ArgumentNullException.ThrowIfNull(copy);

        Loan loan = Create(id, copy.Id, readerId, issuedOn, null);
        copy.Issue();
        return loan;
    }

    public void Close(BookCopy copy, DateOnly returnedOn)
    {
        if (ReturnedOn.HasValue)
            throw new InvalidOperationException($"Позика {Id} вже закрита");
        ArgumentNullException.ThrowIfNull(copy);
        if (!string.Equals(copy.Id, BookCopyId, StringComparison.Ordinal))
            throw new ArgumentException($"Примірник {copy.Id} не належить позиці {Id}", nameof(copy));
        if (returnedOn < IssuedOn)
            throw new ArgumentOutOfRangeException(nameof(returnedOn), returnedOn,
                "Дата повернення не може бути раніше дати видачі");

        copy.ReturnCopy();
        ReturnedOn = returnedOn;
    }

    public LoanDto ToDto() => new(Id, BookCopyId, ReaderId, IssuedOn, ReturnedOn);

    public static Loan FromDto(LoanDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return Create(dto.Id, dto.BookCopyId, dto.ReaderId, dto.IssuedOn, dto.ReturnedOn);
    }

    private static Loan Create(string id, string bookCopyId, string readerId, DateOnly issuedOn, DateOnly? returnedOn)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор позики обов'язковий", nameof(id));
        if (string.IsNullOrWhiteSpace(bookCopyId))
            throw new ArgumentException("Ідентифікатор примірника обов'язковий", nameof(bookCopyId));
        if (string.IsNullOrWhiteSpace(readerId))
            throw new ArgumentException("Ідентифікатор читача обов'язковий", nameof(readerId));
        if (returnedOn < issuedOn)
            throw new ArgumentOutOfRangeException(nameof(returnedOn), returnedOn,
                "Дата повернення не може бути раніше дати видачі");

        return new Loan(id.Trim(), bookCopyId.Trim(), readerId.Trim(), issuedOn, returnedOn);
    }

    public override string ToString() =>
        $"{Id}: примірник {BookCopyId}, читач {ReaderId}, видано {IssuedOn:yyyy-MM-dd}, " +
        (ReturnedOn is DateOnly returnedOn ? $"повернено {returnedOn:yyyy-MM-dd}" : "ще не повернено");
}