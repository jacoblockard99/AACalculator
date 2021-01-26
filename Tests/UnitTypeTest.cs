using System.Linq;
using AACalculator;
using Xunit;

namespace Tests
{
    public class UnitTypeTest
    {
        public static object[][] Constructor_AddsToList_Values =
        {
            new object[] { UnitType.Infantry },
            new object[] { UnitType.Tank },
            new object[] { UnitType.Fighter }
        };
        
        [Theory]
        [MemberData(nameof(Constructor_AddsToList_Values))]
        public static void Constructor_AddsToList(UnitType type)
        {
            Assert.Contains(type, UnitType.Values);
        }
    }
}