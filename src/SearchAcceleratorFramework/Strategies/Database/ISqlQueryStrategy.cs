namespace SearchAcceleratorFramework.Strategies.Database
{
  public interface ISqlQueryStrategy : ISearchStrategy
  {
    string SqlStatement { get; }
  }
}