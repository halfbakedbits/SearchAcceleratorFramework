using System;
using System.Collections.Generic;

namespace SearchAcceleratorFramework
{
  public interface ISearchProcessor<TResult>
  {
    /// <summary>
    ///   Registers a collector to perform search against a data source (e.g. database, file, website).
    /// </summary>
    /// <param name="collector">The collector.</param>
    void RegisterCollector(IResultCollector<TResult> collector);

    /// <summary>
    ///   Registers a function to lookup item result details
    /// </summary>
    /// <param name="itemLookupFunc">The item lookup function.</param>
    void RegisterLookup(Func<long, TResult> itemLookupFunc);

    /// <summary>
    ///   Searches for the supplied <paramref name="searchTerm" />
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="limit">The limit.</param>
    /// <returns>Weighted Search Results in descending order</returns>
    IEnumerable<WeightedSearchResult<TResult>> SearchFor(string searchTerm, int? offset = null, int? limit = null);
  }
}