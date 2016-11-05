using System;
using System.Linq;
using FakeItEasy;
using SearchAcceleratorFramework.Tests.Samples;
using Shouldly;
using Xunit;

namespace SearchAcceleratorFramework.Tests
{
  public class SimplePathTests
  {
    [Fact]
    public void BasicApiPath_NoResults()
    {
      var searchTerm = "foobar";
      var sut = new SearchProcessor<Customer>();

      var collector = A.Fake<IResultCollector<Customer>>();
      sut.RegisterCollector(collector);

      A.CallTo(() => collector.GetWeightedItemsMatching(searchTerm))
        .Returns(Enumerable.Empty<WeightedItemResult<Customer>>());

      var results = sut.SearchFor(searchTerm);
      results.Count().ShouldBe(0);
    }

    [Fact]
    public void ProvidesExceptionWhenNoCollectors()
    {
      var sut = new SearchProcessor<Customer>();

      Assert.Throws<InvalidOperationException>(() => sut.SearchFor("foobar"));
    }
  }
}