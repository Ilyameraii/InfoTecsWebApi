namespace Entities;

public class ValueRecord
{
    public long Id { get; set; }

    public required string FileName { get; set; }

    public DateTime Date { get; set; }

    public double ExecutionTime { get; set; }

    public double Value { get; set; }
}