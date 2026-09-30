namespace core;

public class Paginated<T>
{
    public List<T> Items { get; set; }
    public int Total { get; set; }
}
