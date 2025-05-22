using Gyumri.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


    }
}
