namespace Entities;

public class ResultRecord
{
    public long Id { get; set; }

    public required string FileName { get; set; }

    /// <summary>
    /// Дельта времени Date в секундах (максимальное Date – минимальное Date)
    /// </summary>
    public double DeltaSeconds { get; set; }

    /// <summary>
    /// Минимальная дата и время — момент запуска первой операции
    /// </summary>
    public DateTime MinDate { get; set; }

    /// <summary>
    /// Среднее время выполнения (ExecutionTime)
    /// </summary>
    public double AverageExecutionTime { get; set; }

    /// <summary>
    /// Среднее значение по показателям (Value)
    /// </summary>
    public double AverageValue { get; set; }

    /// <summary>
    /// Медиана по показателям (Value)
    /// </summary>
    public double MedianValue { get; set; }

    /// <summary>
    /// Максимальное значение показателя (Value)
    /// </summary>
    public double MaxValue { get; set; }

    /// <summary>
    /// Минимальное значение показателя (Value)
    /// </summary>
    public double MinValue { get; set; }
}