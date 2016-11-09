using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using SearchAcceleratorFramework.Collectors;
using SearchAcceleratorFramework.Tests.Samples;
using Shouldly;
using Xunit;

namespace SearchAcceleratorFramework.Tests
{
  public class SimplePathTests
  {
    [Fact]
    public void BasicApiPath_NoLookupOneItemWithMultipleResults_TalliesWeight()
    {
      var searchTerm = "foobar";
      var sut = new SearchProcessor();

      var collector = A.Fake<IResultCollector>();
      sut.RegisterCollector<Customer>(collector);

      A.CallTo(() => collector.GetWeightedItemsMatching(searchTerm))
        .Returns(new List<WeightedItemResult>
        {
          new WeightedItemResult {Id = 1, Weight = 10},
          new WeightedItemResult {Id = 1, Weight = 2}
        });

      var results = sut.SearchFor(new SearchQuery<Customer>(searchTerm));
      results.Count().ShouldBe(1);
      var searchResult = results.First();
      searchResult.Id.ShouldBe(1);
      searchResult.Weight.ShouldBe(12);
      searchResult.Item.ShouldBeNull();
    }

    [Fact]
    public void BasicApiPath_NoLookupOneResult_ReturnsProperResult()
    {
      var searchTerm = "foobar";
      var sut = new SearchProcessor();

      var collector = A.Fake<IResultCollector>();
      sut.RegisterCollector<Customer>(collector);

      A.CallTo(() => collector.GetWeightedItemsMatching(searchTerm))
        .Returns(new List<WeightedItemResult> {new WeightedItemResult {Id = 1, Weight = 10}});

      var results = sut.SearchFor(new SearchQuery<Customer>(searchTerm));
      results.Count().ShouldBe(1);
      var searchResult = results.First();
      searchResult.Id.ShouldBe(1);
      searchResult.Weight.ShouldBe(10);
      searchResult.Item.ShouldBeNull();
    }

    [Fact]
    public void BasicApiPath_NoLookupTwoItemWithMultipleResults_ReturnsSortedGroupedAndTalliesWeight()
    {
      var searchTerm = "foobar";
      var sut = new SearchProcessor();

      var collector = A.Fake<IResultCollector>();
      sut.RegisterCollector<Customer>(collector);

      A.CallTo(() => collector.GetWeightedItemsMatching(searchTerm))
        .Returns(new List<WeightedItemResult>
        {
          new WeightedItemResult {Id = 1, Weight = 10},
          new WeightedItemResult {Id = 1, Weight = 2},
          new WeightedItemResult {Id = 3, Weight = 57},
          new WeightedItemResult {Id = 3, Weight = 1}
        });

      var results = sut.SearchFor(new SearchQuery<Customer>(searchTerm)).ToList();
      results.Count.ShouldBe(2);
      var firstResult = results.First();
      firstResult.Id.ShouldBe(3);
      firstResult.Weight.ShouldBe(58);
      firstResult.Item.ShouldBeNull();

      var secondResult = results.Last();
      secondResult.Id.ShouldBe(1);
      secondResult.Weight.ShouldBe(12);
      secondResult.Item.ShouldBeNull();
    }

    [Fact]
    public void BasicApiPath_NoResults()
    {
      var searchTerm = "foobar";
      var sut = new SearchProcessor();

      var collector = A.Fake<IResultCollector>();
      sut.RegisterCollector<Customer>(collector);

      A.CallTo(() => collector.GetWeightedItemsMatching(searchTerm))
        .Returns(Enumerable.Empty<WeightedItemResult>());

      var results = sut.SearchFor(new SearchQuery<Customer>(searchTerm));
      results.Count().ShouldBe(0);
    }

    [Fact]
    public void ProvidesExceptionWhenNoCollectors()
    {
      var sut = new SearchProcessor();

      Assert.Throws<InvalidOperationException>(() => sut.SearchFor(new SearchQuery<Customer>("foobar")));
    }
  }
}