using Gyumri.Common.ViewModel.Place;

namespace GyumriFinalVersion.Models
{
    public class TripInputInfo
    {
        public string TransportName { get; set; }
        public string TransportAmount { get; set; }
        public string TimePeriod { get; set; }

        public string Date { get; set; }
        public string ChildrenCount { get; set; }
        public string AdultCount { get; set; }
        public PlacesViewModel PlaceWhereToStay { get; set; }
        public List<PlacesViewModel> PlaceWhatToDo { get; set; }
    }
}
