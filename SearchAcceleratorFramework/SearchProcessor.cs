using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchAcceleratorFramework
{
  public class SearchProcessor<TResult> : ISearchProcessor<TResult>
  {
    private readonly IList<IResultCollector<TResult>> _collectors = new List<IResultCollector<TResult>>();

    private Func<long, TResult> _itemLookupFunc;

    public void RegisterCollector(IResultCollector<TResult> collector)
    {
      _collectors.Add(collector);
    }

    public void RegisterLookup(Func<long, TResult> itemLookupFunc)
    {
      _itemLookupFunc = itemLookupFunc;
    }

    public IEnumerable<WeightedSearchResult<TResult>> SearchFor(string searchTerm, int? offset = null, int? limit = null)
    {
      if (!_collectors.Any())
      {
        throw new InvalidOperationException("You must supply at least one collector before issuing search");
      }

      var collectorResults = new List<WeightedItemResult<TResult>>();
      foreach (var collector in _collectors)
      {
        collectorResults.AddRange(collector.GetWeightedItemsMatching(searchTerm));
      }

      var itemResults = collectorResults
        .Select(s => s.Id)
        .Distinct()
        .Select(s => new
        {
          Id = s,
          Weight = collectorResults.Sum(sm => sm.Weight)
        })
        .OrderByDescending(o => o.Weight)
        .ToList();

      var skip = offset.GetValueOrDefault(0);
      var take = limit.GetValueOrDefault(int.MaxValue);

      return itemResults
        .Skip(skip)
        .Take(take)
        .Select(s => new WeightedSearchResult<TResult>
        {
          Id = s.Id,
          Weight = s.Weight,
          Item = _itemLookupFunc == null ? default(TResult) : _itemLookupFunc(s.Id)
        });
    }
  }
}