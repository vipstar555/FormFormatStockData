using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FormFormatStockData
{
    public class YahooFInanceConToViewData
    {
        static public SortableBindingList<ViewItem> TradeIndexsWhereDatetime(DateTime datetime)
        {
            var yahooCon = new YahooFinanceDbContext();
            var result = new SortableBindingList<ViewItem>();
            foreach(var tradeIndex in yahooCon.TradeIndexs.Where(x => x.date == datetime.Date))
            {
                result.Add
                    (
                        ToViewItem(tradeIndex)
                    );
            }
            return result;
        }

        static public SortableBindingList<ViewItem> TradeIndexsWhereCode(int code)
        {
            var yahooCon = new YahooFinanceDbContext();
            var result = new SortableBindingList<ViewItem>();
            foreach (var tradeIndex in yahooCon.TradeIndexs.Where(x => x.code == code))
            {
                result.Add
                    (
                        ToViewItem(tradeIndex)
                    );
            }
            return result;
        }
        
        static private ViewItem ToViewItem(TradeIndex tradeIndex)
        {
            return new ViewItem
            {
                Datetime = tradeIndex.date,
                Name = tradeIndex.price.codeList.name,
                OpenPrice = tradeIndex.price.openPrice,
                ClosePrice = tradeIndex.price.closePrice,
                HighPrice = tradeIndex.price.highPrice,
                LowPrice = tradeIndex.price.lowPrice,
                Volume = tradeIndex.price.volume,
                Code = tradeIndex.code,
                Torihiki = TorihikiCalc(tradeIndex.price.volume, tradeIndex.outstandingShares),
                TR = TRCalc(tradeIndex.price.highPrice, tradeIndex.price.lowPrice, tradeIndex.price.lastClosePrice),
                TRPercent = TRPercentCalc(tradeIndex.price.highPrice, tradeIndex.price.lowPrice, tradeIndex.price.lastClosePrice),
                Vora = tradeIndex.price.highPrice - tradeIndex.price.lowPrice,
                VoraPercent = (tradeIndex.price.highPrice - tradeIndex.price.lowPrice) / tradeIndex.price.lowPrice*100,
            };
        }
        //TR%計算
        static private double? TRPercentCalc(double? highPrice, double? lowPrice, double? lastClosePrice)
        {
            var TRList = new List<double?>
                {
                    (highPrice - lowPrice) / lowPrice * 100,
                    (highPrice - lastClosePrice) / lastClosePrice * 100,
                    (lastClosePrice - lowPrice) / lowPrice * 100
                };
            return TRList.Max();
        }
        //TR計算
        static private double? TRCalc(double? highPrice, double? lowPrice, double? lastClosePrice)
        {
            var TRList = new List<double?>
                {
                    highPrice - lowPrice,
                    highPrice - lastClosePrice,
                    lastClosePrice - lowPrice
                };
            return TRList.Max();
        }
        //取引比率計算
        static private double? TorihikiCalc(long volume, long outstandingShares)
        {
            if(volume == 0 || outstandingShares == 0)
            {
                return 0;
            }
            return (double?)volume / (double?)outstandingShares;
        }
    }
    public class ViewItem
    {
        //日付
        public DateTime Datetime { get; set; }
        //コード
        public int Code { get; set; }
        //会社名
        public string Name { get; set; }
        //ボラ%(値幅%)
        public double? VoraPercent { get; set; }
        //TR%
        public double? TRPercent { get; set; }
        //取引比率
        public double? Torihiki { get; set; }
        //ボラ(値幅)
        public double? Vora { get; set; }
        //TR
        public double? TR { get; set; }
        //4本値
        public double? OpenPrice { get; set; }
        public double? ClosePrice { get; set; }
        public double? HighPrice { get; set; }
        public double? LowPrice { get; set; }
        //出来高
        public long Volume { get; set; }
    }
}
