using Entities;
using Services.Contracts;

namespace Services;

/// <summary>
/// Реализация <see cref="ICsvAggregator"/>.
/// Вычисляет интегральные показатели (дельту времени, среднее время выполнения,
/// среднее, медианное, минимальное и максимальное значения) по коллекции строк,
/// сортируя значения показателя для расчёта медианы, минимума и максимума.
/// </summary>
public class CsvAggregator : ICsvAggregator
{
    /// <inheritdoc/>
    public ResultRecord Calculate(string fileName, IReadOnlyCollection<ValueRecord> rows)
    {
        var minDate = rows.Min(r => r.Date);
        var maxDate = rows.Max(r => r.Date);

        var values = rows.Select(r => r.Value).OrderBy(v => v).ToList();

        return new ResultRecord
        {
            FileName = fileName,
            DeltaSeconds = (maxDate - minDate).TotalSeconds,
            MinDate = minDate,
            AverageExecutionTime = rows.Average(r => r.ExecutionTime),
            AverageValue = values.Average(),
            MedianValue = CalculateMedian(values),
            MaxValue = values[^1],
            MinValue = values[0]
        };
    }

    private static double CalculateMedian(IReadOnlyList<double> sortedValues)
    {
        var count = sortedValues.Count;
        var mid = count / 2;

        return count % 2 == 0
            ? (sortedValues[mid - 1] + sortedValues[mid]) / 2.0
            : sortedValues[mid];
    }
}