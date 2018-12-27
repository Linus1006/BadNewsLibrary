using System;
using System.Collections.Generic;
using System.Linq;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;

namespace BadNewsEngine.Service
{
    public class JiebaHelper
    {

        private static JiebaSegmenter _segmenter;
        private static PosSegmenter _posSegenter;

        /// <summary>
        /// 結巴的相關設定放在建構子.
        /// </summary>
        static JiebaHelper(){
            _segmenter = new JiebaSegmenter();
            _segmenter.LoadUserDict("/Users/esb11964/Projects/BadNewsEngine/packages/jieba.NET.0.39.1/Resources/dict.txt.big.txt");
            _posSegenter = new PosSegmenter(_segmenter);
        }

        /// <summary>
        /// 結巴分詞
        /// </summary>
        /// <returns>分詞結果</returns>
        /// <param name="content">Content.</param>
        /// <param name="cutAll">全模式</param>
        /// <param name="hmm">精確模式</param>
        /// <param name="search">搜索引擎模式</param>
        public static List<string> Participle(string content, bool cutAll = true, bool hmm = true, bool search = false){

			return (search)? _segmenter.Cut(text:content, cutAll:cutAll, hmm:hmm).ToList() : _segmenter.CutForSearch(content, hmm:hmm).ToList();
        }

        /// <summary>
        /// 詞性標註
        /// </summary>
        /// <returns>詞性標註結果</returns>
        /// <param name="content">Content.</param>
        /// <param name="hmm">精準模式</param>
        public static List<Pair> PosTagging(string content, bool hmm = true) =>  _posSegenter.Cut(content, hmm).ToList();


        /// <summary>
        /// 去除重複的關鍵字
        /// </summary>
        /// <returns>The distinct.</returns>
        /// <param name="pairs">Pairs. Flag,Word 字與詞性</param>
        public static List<Pair> PairDistinct(IEnumerable<Pair> pairs){

            return pairs.GroupBy(n => n.Word).ToDictionary(o => o.Key, o => o.ToList()).Select(m => {
                
                return m.Value.FirstOrDefault();

            }).ToList();
        }

        /// <summary>
        /// 找出姓名
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="pairs">Pairs.</param>
        public static List<string> GetName(IEnumerable<Pair> pairs) => pairs.Where(n => n.Flag.Contains("nr") && n.Word.Length >1).Select(m => { return m.Word; }).ToList();

        /// <summary>
        /// 找出地名
        /// </summary>
        /// <returns>The location.</returns>
        /// <param name="pairs">Pairs.</param>
        public static List<string> GetLocation(IEnumerable<Pair> pairs) => pairs.Where(n => n.Flag.Contains("ns")).Select(m => { return m.Word; }).ToList();

        /// <summary>
        /// 找出並比對負面新聞(可能要調整badWords)
        /// </summary>
        /// <returns>The bad word.</returns>
        /// <param name="pairWords">Pair words.</param>
        /// <param name="badWords">Bad words.</param>
        public static List<string> GetBadWord(IEnumerable<string> pairWords, IEnumerable<string> badWords) => pairWords.Intersect(badWords).ToList();

    }
}
