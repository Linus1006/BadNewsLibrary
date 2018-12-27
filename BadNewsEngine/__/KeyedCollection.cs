using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BadNewsEngine
{
    [Serializable, CollectionDataContract]
    public class KeyedCollection<TKey, TItem> : System.Collections.ObjectModel.KeyedCollection<TKey, TItem>
    {
        private readonly Func<TItem, TKey> keyGetter;
        public KeyedCollection(Func<TItem, TKey> keyGetter, IEqualityComparer<TKey> comparer = null, int? dictionaryCreationThreshold = null) : base(comparer, dictionaryCreationThreshold ?? 0)
        {
            this.keyGetter = keyGetter;
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return keyGetter(item);
        }

        public bool TryGetValue(TKey key, out TItem value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

    }
}
