# CrossApp
Наскрізний проєкт з крос-платформного програмування

Предметна область: Бібліотека. Сутності: Book, BookCopy, Reader, Loan

Призначення: облік видач примірників читачам і повернень.
## Запуск
dotnet build

dotnet run --project src/Cli

## Пабліш
dotnet publish src/Cli -c Release -r win-x64 --self-contained true

src\Cli\bin\Release\net10.0\win-x64\Cli.exe
## Середовище
.NET SDK 10.0, Windows 11 x64

|  RID  |--self-contained|розмір(мб)|чи потрібен встановлений runtime|
|---|---|---|---|
|win-x64|true|76.68|-|
|win-x64|false|0.19|+|
|linux-x64|true|78,81|-|
|linux-x64|false|0.11|+|


## Команда для запуску в докер контейнері
docker run --rm -v ${PWD}:/src -w /src mcr.microsoft.com/dotnet/runtime-deps:10.0 ./src/Cli/bin/Release/net10.0/linux-x64/publish/Cli