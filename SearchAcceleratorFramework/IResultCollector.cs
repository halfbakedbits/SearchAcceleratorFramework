using System.Collections.Generic;

namespace SearchAcceleratorFramework
{
  public interface IResultCollector<TResult>
  {
    IEnumerable<WeightedItemResult<TResult>> GetWeightedItemsMatching(string searchTerm);
  }
}