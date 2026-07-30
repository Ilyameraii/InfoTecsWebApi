using System.Globalization;
using Entities;
using Services.Contracts;
using Services.Contracts.Exceptions;

namespace Services;

/// <summary>
/// Реализация <see cref="ICsvParser"/>.
/// Построчно разбирает CSV-поток формата "Date;ExecutionTime;Value", пропуская заголовок
/// и пустые строки, и выбрасывает <see cref="CsvValidationException"/> при несоответствии
/// количества полей, отсутствии значений или ошибках преобразования типов.
/// </summary>
public class CsvParser : ICsvParser
{
    private const string DateFormat = "yyyy-MM-ddTHH-mm-ss.ffffZ";

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<ValueRecord>> ParseAsync(Stream csvStream, string fileName)
    {
        var rows = new List<ValueRecord>();
        using var reader = new StreamReader(csvStream);

        var lineNumber = 0;
        var isFirstLine = true;

        while (await reader.ReadLineAsync() is { } line)
        {
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            var parts = line.Split(';');

            if (parts.Length != 3)
            {
                throw new CsvValidationException(
                    $"Строка {lineNumber}: ожидалось 3 поля, получено {parts.Length}");
            }

            var dateRaw = parts[0].Trim();
            var executionTimeRaw = parts[1].Trim();
            var valueRaw = parts[2].Trim();

            if (string.IsNullOrEmpty(dateRaw) ||
                string.IsNullOrEmpty(executionTimeRaw) ||
                string.IsNullOrEmpty(valueRaw))
            {
                throw new CsvValidationException(
                    $"Строка {lineNumber}: отсутствует одно из значений");
            }

            if (!DateTime.TryParseExact(
                    dateRaw,
                    DateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var date))
            {
                throw new CsvValidationException(
                    $"Строка {lineNumber}: некорректный формат даты '{dateRaw}'");
            }

            if (!double.TryParse(
                    executionTimeRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var executionTime))
            {
                throw new CsvValidationException(
                    $"Строка {lineNumber}: некорректное время выполнения '{executionTimeRaw}'");
            }

            if (!double.TryParse(
                    valueRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                throw new CsvValidationException(
                    $"Строка {lineNumber}: некорректное значение показателя '{valueRaw}'");
            }

            rows.Add(new ValueRecord
            {
                FileName = fileName,
                Date = date,
                ExecutionTime = executionTime,
                Value = value
            });
        }

        return rows;
    }
}