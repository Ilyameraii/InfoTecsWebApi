using Entities;

namespace Services.Contracts;

/// <summary>
/// Предоставляет функциональность разбора CSV-файла с результатами измерений
/// в коллекцию строгих доменных объектов <see cref="ValueRecord"/>.
/// </summary>
public interface ICsvParser
{
    /// <summary>
    /// Асинхронно разбирает CSV-поток в коллекцию записей <see cref="ValueRecord"/>.
    /// </summary>
    /// <param name="csvStream">Поток с содержимым CSV-файла в формате "Date;ExecutionTime;Value".</param>
    /// <param name="fileName">Имя файла, которое будет присвоено каждой распарсенной записи.</param>
    /// <returns>Коллекция распарсенных записей <see cref="ValueRecord"/>.</returns>
    /// <exception cref="CsvValidationException">
    /// Выбрасывается, если строка содержит неверное количество полей, отсутствует одно из значений,
    /// либо дата, время выполнения или значение показателя не соответствуют ожидаемому формату/типу.
    /// </exception>
    /// <remarks>
    /// Первая строка файла считается заголовком и пропускается. Пустые строки игнорируются.
    /// Формат даты: <c>yyyy-MM-ddTHH-mm-ss.ffffZ</c>.
    /// </remarks>
    Task<IReadOnlyCollection<ValueRecord>> ParseAsync(Stream csvStream, string fileName);
}