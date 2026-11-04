# CrossApp
Наскрізний проєкт з крос-платформного програмування

Предметна область: Бібліотека. Сутності: Book, BookCopy, Reader, Loan

Призначення: облік видач примірників читачам і повернень.

## Доменна модель
`Loan` — головна сутність процесу видачі: вона зберігає ідентифікатори примірника й читача та дати видачі/повернення. `BookCopy` — пов'язана сутність, що захищає стан конкретного примірника. Читач поки задається через `ReaderId` із `ReaderDto`, без окремої доменної сутності `Reader`.

Позика відкривається через `Loan.Open`, що видає пов'язаний примірник, і закривається через `Loan.Close`, що повертає його. `BookCopyDto` та `LoanDto` переносять дані, але не є доменними сутностями.

Інваріанти `BookCopy`:
- Ідентифікатор примірника не може бути порожнім (`ArgumentException`, `Create`).
- ISBN не може бути порожнім (`ArgumentException`, `Create`).
- Виданий примірник не можна видати повторно (`InvalidOperationException`, `Issue`).
- Невиданий примірник не можна повернути (`InvalidOperationException`, `ReturnCopy`).

Інваріанти `Loan`:
- ID позики, ID примірника та ID читача обов'язкові (`ArgumentException`, `Open` / `FromDto`).
- Не можна повернути примірник, який не належить позиці (`ArgumentException`, `Close`).
- Дата повернення не може бути раніше дати видачі (`ArgumentOutOfRangeException`, `Close` / `FromDto`).
- Закриту позику не можна закрити повторно (`InvalidOperationException`, `Close`).
- `FromDto` проходить ті самі перевірки даних, що й створення позики.

## Запуск
dotnet build

dotnet run --project src/Cli

## Пабліш
dotnet publish src/Cli -c Release -r win-x64 --self-contained true

src\Cli\bin\Release\net10.0\win-x64\Cli.exe
## Середовище
.NET SDK 10.0, Windows 11 x64

|RID|режим|розмір(мб)|чи потрібен встановлений runtime|
|---|---|---|---|
|win-x64|--self-contained|76.68|-|
|win-x64||0.19|+|
|linux-x64|--self-contained|78,81|-|
|linux-x64||0.11|+|
|win-x64|PublishSingleFile,self-contained|70.14|-|
|win-x64|PublishTrimmed,self-contained|19.1|-|
|win-x64|PublishTrimmed|19.3|+|
|win-x64|PublishSingleFile|70.15|+|



## Команда для запуску в докер контейнері
docker run --rm -v ${PWD}:/src -w /src mcr.microsoft.com/dotnet/runtime-deps:10.0 ./src/Cli/bin/Release/net10.0/linux-x64/publish/Cli