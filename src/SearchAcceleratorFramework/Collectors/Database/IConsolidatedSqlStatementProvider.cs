using SearchAcceleratorFramework.Strategies.Database;

namespace SearchAcceleratorFramework.Collectors.Database
{
  /// <summary>
  ///   Consolidates multiple SQL strategies into singular statement
  /// </summary>
  public interface IConsolidatedSqlStatementProvider
  {
    /// <summary>
    ///   Joins the SQL statements provided in the search strategies.
    /// </summary>
    /// <param name="searchStrategies">The search strategies.</param>
    /// <returns>Properly formatted SQL statement</returns>
    string CreateSqlSearchStatement(ISqlQueryStrategy[] searchStrategies);
  }
}