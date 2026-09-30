namespace core;

public class Range
{

    public float? From { get; set; }
    public float? To { get; set; }

    public Range() { }
    public Range(float? f, float? t)
    {
        From = f;
        To = t;
    }

    public bool Equals(Range r)
    {
        return From == r.From && To == r.To;
    }
}

public class Ranges
{
    public List<Range> RangesList { get; private set; } = new();

    public Ranges(IEnumerable<Range> r)
    {
        RangesList.AddRange(r);
    }

    public void Merge()
    {
        if (!RangesList.Any())
            return;

        var sortedRanges = RangesList.OrderBy(r => r.From).ToList();

        var merged = new List<Range>();

        var current = sortedRanges[0];
        for (int i = 1; i < sortedRanges.Count; i++)
        {
            var next = sortedRanges[i];

            // Check continuity
            if (next.From == current.To) current = new Range(current.From, next.To);
            else
            {
                merged.Add(current);
                current = next;
            }
        }
        merged.Add(current);
        RangesList = merged;
    }
}


public record Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(string error) { IsSuccess = false; Error = error; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}
