using System;

namespace SearchAcceleratorFramework
{
  public class SearchQuery<TResult> : IEquatable<SearchQuery<TResult>>
  {
    public SearchQuery(string searchTerm)
    {
      SearchTerm = searchTerm;
    }

    public string SearchTerm { get; }

    public Type SubjectType => typeof(TResult);

    public bool Equals(SearchQuery<TResult> other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;

      return string.Equals(SearchTerm, other.SearchTerm) && SubjectType == other.SubjectType;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((SearchQuery<TResult>) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((SearchTerm != null ? SearchTerm.GetHashCode() : 0)*397) ^ (SubjectType != null ? SubjectType.GetHashCode() : 0);
      }
    }

    public static bool operator ==(SearchQuery<TResult> left, SearchQuery<TResult> right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(SearchQuery<TResult> left, SearchQuery<TResult> right)
    {
      return !Equals(left, right);
    }
  }
}