using System.Collections.Generic;

namespace SearchAcceleratorFramework
{
  public interface IResultCollector
  {
    IEnumerable<WeightedItemResult> GetWeightedItemsMatching(string searchTerm);
  }
}