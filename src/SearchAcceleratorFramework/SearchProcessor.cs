using System;
using System.Collections.Generic;
using System.Linq;
using SearchAcceleratorFramework.Collectors;

namespace SearchAcceleratorFramework
{
  public class SearchProcessor : ISearchProcessor
  {
    private readonly Dictionary<Type, List<IResultCollector>> _collectorMap = new Dictionary<Type, List<IResultCollector>>();
    private readonly Dictionary<Type, Func<long, object>> _lookupMap = new Dictionary<Type, Func<long, object>>();

    public void RegisterCollector<TResult>(IResultCollector collector)
    {
      List<IResultCollector> collectors;
      var resultType = typeof(TResult);
      if (!_collectorMap.TryGetValue(resultType, out collectors))
      {
        collectors = new List<IResultCollector>();
      }

      collectors.Add(collector);

      _collectorMap[resultType] = collectors;
    }


    public void RegisterLookup<TResult>(Func<long, object> itemLookupFunc)
    {
      Func<long, object> func;
      var resultType = typeof(TResult);
      if (!_lookupMap.TryGetValue(resultType, out func))
      {
        _lookupMap.Add(resultType, itemLookupFunc);

        return;
      }

      _lookupMap[resultType] = itemLookupFunc;
    }

    public IEnumerable<WeightedSearchResult<TResult>> SearchFor<TResult>(SearchQuery<TResult> query, int? offset = null, int? limit = null)
    {
      var resultType = typeof(TResult);
      List<IResultCollector> collectors;
      if (!_collectorMap.TryGetValue(resultType, out collectors) || !collectors.Any())
      {
        throw new InvalidOperationException("You must supply at least one collector before issuing search");
      }
      var collectorResults = new List<WeightedItemResult>();
      foreach (var collector in collectors)
      {
        collectorResults.AddRange(collector.GetWeightedItemsMatching(query.SearchTerm));
      }

      var itemResults = collectorResults
        .Select(s => s.Id)
        .Distinct()
        .Select(s => new
        {
          Id = s,
          Weight = collectorResults.Where(w => w.Id == s).Sum(sm => sm.Weight)
        })
        .OrderByDescending(o => o.Weight)
        .ToList();

      var skip = offset.GetValueOrDefault(0);
      var hasLimit = limit.HasValue;

      Func<long, object> lookupFunc;
      if (!_lookupMap.TryGetValue(resultType, out lookupFunc))
      {
        if (hasLimit)
          return itemResults
            .Skip(skip)
            .Take(limit.Value)
            .Select(s => new WeightedSearchResult<TResult>
            {
              Id = s.Id,
              Weight = s.Weight
            });

        return itemResults
          .Skip(skip)
          .Select(s => new WeightedSearchResult<TResult>
          {
            Id = s.Id,
            Weight = s.Weight
          });
      }

      if (hasLimit)
      {
        return itemResults
          .Skip(skip)
          .Take(limit.Value)
          .Select(s => new WeightedSearchResult<TResult>
          {
            Id = s.Id,
            Weight = s.Weight,
            Item = (TResult) lookupFunc.Invoke(s.Id)
          });
      }

      return itemResults
        .Skip(skip)
        .Select(s => new WeightedSearchResult<TResult>
        {
          Id = s.Id,
          Weight = s.Weight,
          Item = (TResult) lookupFunc.Invoke(s.Id)
        });
    }
  }
}