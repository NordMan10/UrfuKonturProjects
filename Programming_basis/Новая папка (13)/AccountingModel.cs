    using System;

    namespace HotelAccounting
    {
        public class AccountingModel : ModelBase
        {
            public double Price { get; set; }
            public double NightsCount { get; set; }
            public double Discount { get; set; }
            public double Total { get; set; }

            public AccountingModel(double price, double nightsCount, double discount)
            {
                if (price < 0 || nightsCount <= 0 || discount > 100) throw new ArgumentException();
                Price = price;
                Notify(nameof(Price));
                NightsCount = nightsCount;
                Notify(nameof(NightsCount));
                Discount = discount;
                Notify(nameof(Discount));    
                Total = Price * NightsCount * (1 - Discount / 100);
                Notify(nameof(Total));
            }

            public AccountingModel(double total)
            {
                if (total < 0) throw new ArgumentException();
                Total = total;
                Notify(nameof(Total));
                Discount = 100 - (100 * total) / (Price * NightsCount);
                Notify(nameof(Discount));
            }

            public AccountingModel() { }
        }
    }
