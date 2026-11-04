namespace Core.Dto;

public record LoanDto(string Id, string BookCopyId, string ReaderId, DateOnly IssuedOn, DateOnly? ReturnedOn);