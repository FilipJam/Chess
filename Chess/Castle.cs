using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal static class Castle
    {
        private static List<int[]> rangeList = new List<int[]>
        {
            new int[] { 0, 4},
            new int[] { 4, 7}
        };

        private static bool[] castleAvailability = { true, true };

        public static List<int[]> RangeList { get => rangeList; }
        public static bool[] Availability { get => castleAvailability; set => castleAvailability = value; }

        public static bool CheckCastleAvailable()
        {
            return false;
        }
    }
}
