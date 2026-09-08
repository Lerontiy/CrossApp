# CrossApp
Наскрізний проєкт з крос-платформного програмування

Предметна область: Бібліотека. Сутності: Book, BookCopy, Reader, Loan

Призначення: облік видач примірників читачам і повернень.
## Запуск
dotnet build

dotnet run --project src/Cli
## Середовище
.NET SDK 10.0, Windows 11 x64

## Розмір каталогів publish для різних RID
win-x64 76,9Мб

linux-x64 79,1Мб

## Команда для запуску в докер контейнері
docker run --rm -v ${PWD}:/src -w /src mcr.microsoft.com/dotnet/sdk:10.0 dotnet run --project src/Cli --json