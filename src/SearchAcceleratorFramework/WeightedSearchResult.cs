namespace SearchAcceleratorFramework
{
  public class WeightedSearchResult<TResult>
  {
    public long Id { get; set; }
    public decimal Weight { get; set; }
    public TResult Item { get; set; }
  }
}