using System;
using System.Numerics;


namespace Tickets
{
    public class TicketsTask
    {
        public static BigInteger Solve(int length, int sum)
        {
            if (sum % 2 != 0) return 0;
            var halfOfSum = sum * 1 / 2;
            var happyTicketsTable = new BigInteger[1000, 1000];
            for (var i = 0; i < 1000; i++)
                for (var j = 0; j < 1000; j++)
                    happyTicketsTable[i, j] = int.MinValue;
            var halfOfResult = CountTicket(happyTicketsTable, length, halfOfSum);
            return halfOfResult * halfOfResult;
        }

        private static BigInteger CountTicket(BigInteger[,] happyTickets, int length, int sum)
        {
            if (happyTickets[length, sum] >= 0) return happyTickets[length, sum];
            if (sum == 0) return 1;
            if (length == 0) return 0;
            happyTickets[length, sum] = 0;
            for (var i = 0; i < 10; i++)
                if (sum - i >= 0)
                    happyTickets[length, sum] += CountTicket(happyTickets, length - 1, sum - i);
            return happyTickets[length, sum];
        }
    }
}