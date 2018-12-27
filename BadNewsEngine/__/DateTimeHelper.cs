using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadNewsEngine
{
    public static class DateTimeHelper
    {
        private static DateTimeFormatInfo zhTwDateTimeFormatInfo;

        static DateTimeHelper()
        {
            CultureInfo taiwanCI = new CultureInfo("zh-TW");
            taiwanCI.DateTimeFormat.Calendar = new TaiwanCalendar();
            zhTwDateTimeFormatInfo = taiwanCI.DateTimeFormat;
        }


        /// <summary>
        /// 由格式化字串取得時間類型資料
        /// </summary>
        /// <param name="DateTimeString">字串</param>
        /// <param name="format">yyyy/M/d HH:mm:ss.fffffff,CY表示民國年</param>
        /// <returns>時間DateTime</returns>
        public static DateTime GetDateTime(this string DateTimeString, string format)
        {
            DateTimeFormatInfo formatInfo;
            if (format.IndexOf("CY", StringComparison.Ordinal) >= 0)
            {
                format = format.Replace("0CY", "CY");
                if (format.StartsWith("CYMM", StringComparison.Ordinal))
                {
                    if (DateTimeString.Length == format.Length)
                    {
                        format = format.Insert(2, "/");
                        DateTimeString = DateTimeString.Insert(2, "/");
                    }
                    else if (DateTimeString.Length == format.Length + 1)
                    {
                        format = format.Insert(2, "/");
                        DateTimeString = DateTimeString.Insert(3, "/");
                    }
                }
                format = format.Replace("CY", "yy");
                formatInfo = zhTwDateTimeFormatInfo;
            }
            else
            {
                formatInfo = DateTimeFormatInfo.InvariantInfo;
            }
            return DateTime.ParseExact(DateTimeString, format, formatInfo, System.Globalization.DateTimeStyles.None);
        }

        /// <summary>
        /// 由格式化字串取得時間類型資料(針對null處理)
        /// </summary>
        /// <param name="DateTimeString">字串</param>
        /// <param name="format">yyyy/M/d HH:mm:ss.fffffff,CY表示民國年</param>
        /// <returns>時間DateTime</returns>
        public static DateTime? GetDateTimeNull(this string DateTimeString, string format)
        {
            DateTime outDateTime;
            DateTimeFormatInfo formatInfo;

            //主機空值會回傳0
            if (DateTimeString == "0" || string.IsNullOrWhiteSpace(DateTimeString))
            {
                return null;
            }

            if (format.IndexOf("CY", StringComparison.Ordinal) >= 0)
            {
                format = format.Replace("0CY", "CY");
                if (format.StartsWith("CYMM", StringComparison.Ordinal))
                {
                    if (DateTimeString.Length == format.Length)
                    {
                        format = format.Insert(2, "/");
                        DateTimeString = DateTimeString.Insert(2, "/");
                    }
                    else if (DateTimeString.Length == format.Length + 1)
                    {
                        format = format.Insert(2, "/");
                        DateTimeString = DateTimeString.Insert(3, "/");
                    }
                }
                format = format.Replace("CY", "yy");
                formatInfo = zhTwDateTimeFormatInfo;
            }
            else
            {
                formatInfo = DateTimeFormatInfo.InvariantInfo;
            }
            if (DateTime.TryParseExact(DateTimeString, format, formatInfo, System.Globalization.DateTimeStyles.None, out outDateTime))
            {
                return outDateTime;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 檢查small datetime range 範圍內才傳時間
        /// </summary>
        /// <param name="DateTimeString"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? GetSmallDateTimeNull(this string DateTimeString, string format)
        {
            DateTime dateMin = new DateTime(1900, 1, 1, 0, 0, 0);
            DateTime dateMax = new DateTime(2079, 6, 6, 23, 59, 59);
            DateTime? dt = GetDateTimeNull(DateTimeString, format);
            if (dt != null && DateTime.Compare(dt.Value, dateMin) >= 0 && DateTime.Compare(dt.Value, dateMax) <= 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 格式化時間字串
        /// </summary>
        /// <param name="dateTime">時間</param>
        /// <param name="format">yyyy/MM/dd HH:mm:ss.fffffff,CY表示民國年</param>
        /// <returns>字串</returns>
        public static string GetString(this DateTime dateTime, string format)
        {
            DateTimeFormatInfo formatInfo;
            if (format.IndexOf("CY", StringComparison.Ordinal) >= 0)
            {
                if (format.IndexOf("0CY", StringComparison.Ordinal) >= 0 && dateTime.Year < 1911 + 100)
                    format = format.Replace("0CY", dateTime.Year < 1911 + 100 ? "0yy" : "yy");
                else if (format.IndexOf("0CY", StringComparison.Ordinal) >= 0)
                    format = format.Replace("0CY", "yy");
                else
                    format = format.Replace("CY", "yy");
                formatInfo = zhTwDateTimeFormatInfo;
            }
            else
            {
                formatInfo = DateTimeFormatInfo.InvariantInfo;
            }
            return dateTime.ToString(format, formatInfo);
        }
        /// <summary>
        /// 格式化時間字串(針對nullable處理)
        /// </summary>
        /// <param name="dateTime">時間</param>
        /// <param name="format">yyyy/MM/dd HH:mm:ss.fffffff,CY表示民國年</param>
        /// <returns>字串</returns>
        public static string GetString(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? GetString(dateTime.Value, format) : null;
        }

        /// <summary>
        /// 取得兩日期的間隔期間(依照日期單位)
        /// </summary>
        /// <param name="interval">日期單位</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>間隔日期單位數</returns>
        public static long GetInterval(this DateTime startDate, DateTime endDate, DateTimeInterval interval)
        {
            TimeSpan span = endDate - startDate;
            long val = 0;
            switch (interval)
            {
                case DateTimeInterval.Second:
                    return (long)span.TotalSeconds;
                case DateTimeInterval.Minute:
                    return (long)span.TotalMinutes;
                case DateTimeInterval.Hour:
                    return (long)span.TotalHours;
                case DateTimeInterval.Day:
                    return (long)span.Days;
                case DateTimeInterval.Week:
                    return (long)(span.Days / 7);
                case DateTimeInterval.Month:
                    val = (long)(span.Days / 31);
                    while (startDate.AddMonths((int)val + 1) <= endDate)
                    {
                        val++;
                    }
                    return val;
                case DateTimeInterval.Quarter:
                    return (long)(GetInterval(startDate, endDate, DateTimeInterval.Month) / 3);
                case DateTimeInterval.Year:
                    val = (long)(span.Days / 366);
                    while (startDate.AddYears((int)val + 1) <= endDate)
                    {
                        val++;
                    }
                    return val;
                default:
                    throw new ArgumentException("interval非正確的DateTimeInterval類型");
            }
        }

        /// <summary>
        /// 取得兩日期的區間年月日
        /// </summary>
        /// <param name="beginDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns>開始日期加上區間日期後的時間。因為可能會有不足一天的時間，所以不一定會等於endDate。</returns>
        public static DateTime GetInterval(DateTime beginDate, DateTime endDate, ref int year, ref int month, ref int day)
        {
            year = (int)GetInterval(beginDate, endDate, DateTimeInterval.Year);
            beginDate = beginDate.AddYears(year);
            month = (int)GetInterval(beginDate, endDate, DateTimeInterval.Month);
            beginDate = beginDate.AddMonths(month);
            day = (int)GetInterval(beginDate, endDate, DateTimeInterval.Day);
            beginDate = beginDate.AddDays(day);
            return beginDate;
        }

        /// <summary>
        /// 取得兩日期的區間年月日
        /// </summary>
        /// <param name="beginDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="span">兩時間計算完間隔後剩餘不足一天的的時間。</param>
        public static void GetInterval(DateTime beginDate, DateTime endDate, ref int year, ref int month, ref int day, ref TimeSpan span)
        {
            span = endDate - GetInterval(beginDate, endDate, ref year, ref month, ref day);
        }
    }
}
