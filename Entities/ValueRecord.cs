namespace Entities;

/// <summary>
/// Одна строка данных, распарсенная из CSV-файла (таблица Values).
/// Соответствует формату Date;ExecutionTime;Value; при повторной обработке
/// файла с тем же именем прежние строки удаляются и заменяются новыми.
/// </summary>
public class ValueRecord
{
    /// <summary>
    /// Идентификатор записи в базе данных.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя CSV-файла, из которого получено значение.
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    /// Время начала операции (Date) в формате ГГГГ-ММ-ДДTчч-мм-сс.ммммZ
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Время выполнения операции в секундах (ExecutionTime)
    /// </summary>
    public double ExecutionTime { get; set; }

    /// <summary>
    /// Значение показателя в виде числа с плавающей запятой (Value)
    /// </summary>
    public double Value { get; set; }
}