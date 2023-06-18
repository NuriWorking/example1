using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question2
{
    public class InspData
    {
        public string InspStart { get; set; }
        public string InspEnd { get; set; }
        private List<CardData> cardDatas;

        public List<CardData> CardDatas
        {
            get { return cardDatas; }
        }

        public InspData()
        {
            //BusId = busId;
            cardDatas = new List<CardData>();
        }
        public void AddCardData(string meta)
        {
            //[카드ID(8)][최근탑승버스ID(7)][최근탑승승차/하차 코드(1)][최근 승차시각(14)]
            string cardId = meta.Substring(0, 8);
            string busId = meta.Substring(8, 7);
            string rideCode = meta.Substring(15, 1);
            string recentRideTime = meta.Substring(16, 14);
            cardDatas.Add(new CardData(meta, cardId, busId, rideCode, recentRideTime, DateTime.Now.ToString("yyyyMMddHHmmss")));
        }
    }
    public class CardData
    {
        public string Meta { get; set; }
        public string CardId { get; set; }
        public string RecentRideBusId { get; set; }
        public string RecentRideCode { get; set; }
        public string RecentRideTime { get; set; }
        public string CheckTime { get; set; }
        public string InspectionCode { get; set; }

        public CardData(string meta, string cardId, string recentRideBusId, string recentRideCode, string recentRideTime, string checkTime)
        {
            Meta = meta;
            CardId = cardId;
            RecentRideBusId = recentRideBusId;
            RecentRideCode = recentRideCode;
            RecentRideTime = recentRideTime;
            CheckTime = checkTime;
        }

    }
}
