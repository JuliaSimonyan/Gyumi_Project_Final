using Gyumri.Data.Models;

namespace Gyumri.Common.Utility
{
    public static class Util
    {
        public static string GetPlaceTypeNameByEnumType(PlaceType placeType)
        {
            return placeType switch
            {
                PlaceType.HOTEL => AppRes.Hotel,
                PlaceType.GUESTHOUSE => AppRes.Guesthouse,
                PlaceType.HOSTEL => AppRes.Hostel,
                PlaceType.MUSEUM => AppRes.Museum,
                PlaceType.CRAFTS => AppRes.Crafts,
                PlaceType.HISTORY => AppRes.History,
                PlaceType.OUTDOOR => AppRes.Outdoor,
                PlaceType.CAFE => AppRes.Cafe,
                PlaceType.RESTAURANT => AppRes.Restaurant,
                _ => string.Empty
            };
        }

        public static List<PlaceType> GetEnumForFirstPage()
        {
            return new List<PlaceType> { PlaceType.HOTEL, PlaceType.GUESTHOUSE, PlaceType.HOSTEL };
        }
        public static List<PlaceType> GetEnumForSecondPage()
        {
            return new List<PlaceType> { PlaceType.MUSEUM, PlaceType.CRAFTS, PlaceType.HISTORY, PlaceType.OUTDOOR, PlaceType.CAFE, PlaceType.RESTAURANT };
        }
    }
}
