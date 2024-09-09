namespace mohaymen_codestar_Team02_XUnitTest.CleanArch1;

    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly List<TElement> _elements;

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            Key = key;
            _elements = elements.ToList();
        }

        public TKey Key { get; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
