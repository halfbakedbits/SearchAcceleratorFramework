using System.Collections.Generic;

namespace SearchAcceleratorFramework.Collectors.Database
{
  public interface IDatabaseGateway
  {
    IEnumerable<WeightedItemResult> GetItemResults(string sqlQuery, string searchTerm);
  }
}