using Gyumri.Data.Models;

namespace Gyumri.Common.Utility
{
    public static class Util
    {
            public static string GetPlaceTypeNameByEnumType(PlaceType placeType, string currentCulture)
            {
                return currentCulture switch
                {
                    "ru-RU" => placeType switch
                    {
                        PlaceType.HOTEL => "Отель",
                        PlaceType.GUESTHOUSE => "Гостевой дом",
                        PlaceType.HOSTEL => "Хостел",
                        PlaceType.MUSEUM => "Музей",
                        PlaceType.CRAFTS => "Ремесла",
                        PlaceType.HISTORY => "История",
                        PlaceType.OUTDOOR => "На открытом воздухе",
                        PlaceType.CAFE => "Кафе",
                        PlaceType.RESTAURANT => "Ресторан",
                        _ => string.Empty
                    },
                    "hy-AM" => placeType switch
                    {
                        PlaceType.HOTEL => "Հյուրանոց",
                        PlaceType.GUESTHOUSE => "Հյուրատուն",
                        PlaceType.HOSTEL => "Հոսթել",
                        PlaceType.MUSEUM => "Թանգարան",
                        PlaceType.CRAFTS => "Արվեստներ",
                        PlaceType.HISTORY => "Պատմություն",
                        PlaceType.OUTDOOR => "Բացօթյա",
                        PlaceType.CAFE => "Կաֆե",
                        PlaceType.RESTAURANT => "Ռեստորան",
                        _ => string.Empty
                    },
                    _ => placeType switch
                    {
                        PlaceType.HOTEL => "Hotel",
                        PlaceType.GUESTHOUSE => "Guesthouse",
                        PlaceType.HOSTEL => "Hostel",
                        PlaceType.MUSEUM => "Museum",
                        PlaceType.CRAFTS => "Crafts",
                        PlaceType.HISTORY => "History",
                        PlaceType.OUTDOOR => "Outdoor",
                        PlaceType.CAFE => "Cafe",
                        PlaceType.RESTAURANT => "Restaurant",
                        _ => string.Empty
                    }
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
