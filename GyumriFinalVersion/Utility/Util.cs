using Gyumri.Data.Models;
using GyumriFinalVersion.Models;
using Microsoft.IdentityModel.Tokens;

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
                    PlaceType.CAFE => "Սրճարան",
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

        public static string GetPdfCss()
        {
            return @"
                * {
                    margin: 0;
                    padding: 0;
                    box-sizing: border-box;
                    font-family: 'Montserrat', sans-serif;
                }
                .pdfLogo {
                    filter: brightness(0);
                }
                body {
                    color: #333;
                    line-height: 1.6;
                    padding-top: 76px;
                    min-height: 100vh;
                    display: flex;
                    flex-direction: column;
                }
                .plan-container {
                    margin: 0 auto;
                    background-color: white;
                    border-radius: 8px;
                    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    width: 100%;
                    max-width: 850px;
                    padding: 30px;
                    margin-bottom: 30px;
                }
                .plan-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: flex-start;
                    margin-bottom: 30px;
                }
                .logo {
                    width: 40px;
                    height: 40px;
                }
                .address {
                    text-align: right;
                    font-size: 12px;
                    color: #666;
                }
                .plan-title {
                    text-align: center;
                    margin-bottom: 20px;
                }
                .plan-title h1 {
                    font-size: 24px;
                    font-weight: 600;
                    margin-bottom: 5px;
                }
                .plan-title p {
                    font-size: 16px;
                    color: #666;
                }
                .plan-details {
                    display: flex;
                    justify-content: center;
                    gap: 20px;
                    margin-bottom: 30px;
                    padding-bottom: 20px;
                    border-bottom: 1px solid #eee;
                }
                .detail-item {
                    display: flex;
                    align-items: center;
                    font-size: 14px;
                }
                .plan-section {
                    margin-bottom: 25px;
                }
                .plan-section h2 {
                    font-size: 18px;
                    font-weight: 600;
                    margin-bottom: 15px;
                    color: #000;
                }
                .section-item {
                    display: flex;
                    align-items: center;
                    margin-bottom: 15px;
                    padding-bottom: 15px;
                    border-bottom: 1px solid #f5f5f5;
                }
                .section-item div{
                    flex:1;
                }
                .section-item:last-child {
                    margin-bottom: 0;
                    padding-bottom: 0;
                    border-bottom: none;
                }
                .item-icon {
                    width: 40px;
                    height: 40px;
                    background-color: #f5f5f5;
                    border-radius: 50%;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    margin-right: 15px;
                }
                .item-name {
                    flex: 1;
                    font-weight: 500;
                }
                .item-info {
                    display: flex;
                    align-items: center;
                    font-size: 14px;
                    color: #666;
                }
                .item-info i {
                    margin-left: 8px;
                }
                ";
        }

        public static string GetPdfHtml(TripInputInfo tripInfo, IWebHostEnvironment _env)
        {
            string placeName = tripInfo?.PlaceWhereToStay?.PlaceName;
            //currentCulture == "ru-RU" ? fullInfo.PlaceWhereToStay?.PlaceNameRu :
            //               currentCulture == "hy-AM" ? fullInfo.PlaceWhereToStay?.PlaceNameArm :
            //               fullInfo.PlaceWhereToStay?.PlaceName;

            string placeAddress = tripInfo?.PlaceWhereToStay?.Address;
            //currentCulture == "ru-RU" ? fullInfo.PlaceWhereToStay?.AddressRu :
            //                  currentCulture == "hy-AM" ? fullInfo.PlaceWhereToStay?.AddressArm :
            //                  fullInfo.PlaceWhereToStay?.Address;
            string logoPath = Path.Combine(_env.WebRootPath, "content", "Images", "pdfIcon.png");
            byte[] imageBytes = System.IO.File.ReadAllBytes(logoPath);
            string base64Image = Convert.ToBase64String(imageBytes);
            string logoDataUri = $"data:image/png;base64,{base64Image}";

            string logoPath2 = Path.Combine(_env.WebRootPath, "content", "Images", "car.png");
            byte[] imageBytes2 = System.IO.File.ReadAllBytes(logoPath2);
            string base64Image2 = Convert.ToBase64String(imageBytes2);
            string carDataUri = $"data:image/png;base64,{base64Image2}";

            string logoPath3 = Path.Combine(_env.WebRootPath, "content", "Images", "hotel.png");
            byte[] imageBytes3 = System.IO.File.ReadAllBytes(logoPath3);
            string base64Image3 = Convert.ToBase64String(imageBytes3);
            string hotelUri = $"data:image/png;base64,{base64Image3}";

            string logoPath4 = Path.Combine(_env.WebRootPath, "content", "Images", "whatToDo.png");
            byte[] imageBytes4 = System.IO.File.ReadAllBytes(logoPath4);
            string base64Image4 = Convert.ToBase64String(imageBytes4);
            string whatToDoUri = $"data:image/png;base64,{base64Image4}";


            string htmlContent = $@"
                <div class=""plan-container"">
                    <div class=""plan-header"">
                        <div class=""logo"">
                            <img class=""pdfLogo"" src=""{logoDataUri}"" alt=""Logo"" height=""30"">
                        </div>
                        <div class=""address"">
                            <p>{AppRes.AddressGyumri}</p>
                            <p>{AppRes.AddressGyumri2}</p>
                        </div>
                    </div>
                    <div class=""plan-title"">
                        <h1>{AppRes.TripToGyumri}</h1>
                        <p>{AppRes.Plan}</p>
                    </div>
                    <div class=""plan-details"">
                        <div class=""detail-item""><span>{tripInfo.Date}</span></div>
                        <div class=""detail-item""><span>{AppRes.Adults} - {tripInfo.AdultCount}</span></div>
                        <div class=""detail-item""><span> {AppRes.Children} - {tripInfo.ChildrenCount}</span></div>
                    </div>
                    <div class=""plan-section"">
                        <h2>{AppRes.GettingtoGyumri}</h2>
                        <div class=""section-item"">
                            <div class=""item-icon"">
                                <img class=""pdfLogo"" src=""{carDataUri}"" alt=""Logo"" height=""30"">
                            </div>
                            <div class=""item-name"">{tripInfo.TransportName}</div>
                            <div class=""item-info""><span>{tripInfo.TimePeriod} {AppRes.min}</span></div>
                        </div>
                    </div>
                    <div class=""plan-section"">
                        <h2>{AppRes.WheretoStay}</h2>
                        <div class=""section-item"">
                            <div class=""item-icon"">
                                <img class=""pdfLogo"" src=""{hotelUri}"" alt=""Logo"" height=""30"">
                            </div>
                            <div class=""item-name"">{placeName}</div>
                            <div class=""item-info"">{placeAddress}</div>
                        </div>
                    </div>
                ";
            if (tripInfo.PlaceWhatToDo.IsNullOrEmpty())
            {
                htmlContent += $@" <div class=""plan-section"">
                <h2>{AppRes.WhatToDo}</h2>";

                foreach (var item in tripInfo?.PlaceWhatToDo)
                {
                    htmlContent += $@"
                        <div class=""section-item d-flex"">
                            <div class=""item-icon"">
                                 <img class=""pdfLogo"" src=""{whatToDoUri}"" alt=""Logo"" height=""30"">
                            </div>
                            <div class=""item-name"">{item?.PlaceName}</div>
                            <div class=""item-info"">{item?.Address}</div>
                        </div>";
                }
            }

            htmlContent += $@" </div>
                            </div>
                            <style>{GetPdfCss()}</style>";

            return htmlContent;
        }
    }
}
