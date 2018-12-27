using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadNewsEngine
{
    public static class StreamExtensions
    {
        /// <summary>讀取來源串流資料並寫到目標串流去</summary>
        /// <param name="destination">目標串流</param>
        /// <param name="source">來源串流</param>
        /// <param name="count">讀取並寫入的byte長度上限，null表示直到來源串流結束</param>
        /// <param name="offset">來源串流開始讀取的位置，null表示從目前位置開始。來源串流必須支援seek才可指定offset值。</param>
        /// <param name="pinSourcePosition">false表示來源串流會隨著讀取而一直將位置前移；true表示來源串流的位置固定不動，但來源串流必須有支援seek才能使用此功能。預設為false。</param>
        /// <returns>讀取並寫入的實際byte長度</returns>
        public static long Write(this Stream destination, Stream source, long? count = null, long? offset = null, bool pinSourcePosition = false)
        {
            //因為要抓實際處理長度, 若要判斷source和destination是否有CanSeek太過麻煩(有seek才能抓定位), 所以不用內建的Stream.CopyTo
            if ((offset.HasValue || pinSourcePosition) && !source.CanSeek) throw new NotSupportedException("來源串流不支援Seek");
            var originalPosition = pinSourcePosition ? source.Position : (long?)null;   //抓原本的位置
            if (offset.HasValue) source.Position = offset.Value;    //定位到開始讀取的位置
            long length = 0;    //實際處理的byte長度
            var buffer = new byte[Math.Min(count ?? int.MaxValue, 81920)]; //設定buffer, 最大長度為81920
            try
            {
                while (true)
                {
                    int partLengthMax = (int)Math.Min(count.GetValueOrDefault(long.MaxValue) - length, buffer.Length);  //因為buffer最大長度為81920, 所以結果一定能轉為int
                    var partLength = source.Read(buffer, 0, partLengthMax);
                    if (partLength == 0) break; //長度為0的話表示已無資料
                    destination.Write(buffer, 0, partLength);
                    length += partLength;   //這邊long溢位的話自然會有錯誤, 所以不多做溢位判斷
                    if (count.HasValue && length >= count) break;
                }
            }
            finally
            {
                if (originalPosition.HasValue) source.Position = originalPosition.Value;    //回歸到原本的位置
            }
            return length;
        }

        /// <summary>將串流的資料寫入位元組陣列並傳回</summary>
        /// <param name="stream"></param>
        /// <param name="count">讀取的byte長度上限，null表示直到結束</param>
        /// <param name="offset">開始讀取的位置，null表示從目前位置開始。必須支援seek才可指定offset值。</param>
        /// <param name="pinPosition">false表示串流會隨著讀取而一直將位置前移；true表示串流的位置固定不動，但必須有支援seek才能使用此功能。預設為false。</param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream, long? count = null, long? offset = null, bool pinPosition = false)
        {
            //因為framework 4.5之後的BinaryReader建構式才有leaveOpen參數, 所以只能先用MemoryStream來處理
            using (var tmpStream = new MemoryStream())
            {
                tmpStream.Write(stream, count, offset, pinPosition);
                return tmpStream.ToArray();
            }
        }

    }
}
