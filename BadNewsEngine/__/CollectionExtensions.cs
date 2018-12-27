using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadNewsEngine
{
    public static class CollectionExtensions
    {
        /// <summary>取得某項目的值或是預設值</summary>
        /// <typeparam name="TKey">IDictionary的Key類型</typeparam>
        /// <typeparam name="TValue">IDictionary的Value類型</typeparam>
        /// <param name="dict">字典</param>
        /// <param name="key">鍵值</param>
        /// <param name="defaultValue">若抓不到鍵值時所回傳的預設值</param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }


        /// <summary>取值或是設定</summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="valueGetter"></param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueGetter)
        {
            var concurrentDict = dict as ConcurrentDictionary<TKey, TValue>;
            if (concurrentDict != null) return concurrentDict.GetOrAdd(key, valueGetter);
            TValue value;
            if (!dict.TryGetValue(key, out value)) dict[key] = value = valueGetter(key);
            return value;
        }

        /// <summary>設定Dictionary的多個項目</summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="entries"></param>
        public static void SetEntries<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> entries)
        {
            foreach (var n in entries)
            {
                dict[n.Key] = n.Value;
            }
        }

        /// <summary>在集合的每個元素上執行某個動作</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collections"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> collections, Action<T> action)
        {
            foreach (var n in collections)
            {
                action(n);
            }
        }

        /// <summary>在集合的每個元素上執行某個動作</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collections"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> collections, Action<T, int> action)
        {
            var i = 0;
            foreach (var n in collections)
            {
                action(n, i++);
            }
        }
    }
}
