using System;
using System.Collections.Generic;
using SearchAcceleratorFramework.Collectors;

namespace SearchAcceleratorFramework
{
  public interface ISearchProcessor
  {
    /// <summary>
    ///   Registers a collector to perform search against a data source (e.g. database, file, website).
    /// </summary>
    /// <param name="collector">The collector.</param>
    void RegisterCollector<TResult>(IResultCollector collector);

    /// <summary>
    ///   Registers a function to lookup item result details
    /// </summary>
    /// <param name="itemLookupFunc">The item lookup function.</param>
    void RegisterLookup<TResult>(Func<long, object> itemLookupFunc);

    /// <summary>
    ///   Searches for the supplied <paramref name="query" />
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="query">The search query.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="limit">The limit.</param>
    /// <returns>
    ///   Weighted Search Results in descending order
    /// </returns>
    IEnumerable<WeightedSearchResult<TResult>> SearchFor<TResult>(SearchQuery<TResult> query, int? offset = null, int? limit = null);
  }
}