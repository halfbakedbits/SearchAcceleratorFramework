using System.Collections.Generic;

namespace SearchAcceleratorFramework.Collectors
{
  public interface IResultCollector
  {
    IEnumerable<WeightedItemResult> GetWeightedItemsMatching(string searchTerm);
  }
}