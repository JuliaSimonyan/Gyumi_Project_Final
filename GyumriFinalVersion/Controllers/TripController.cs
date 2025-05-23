using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Gyumri.Common.ViewModel.Place;
using GyumriFinalVersion.Models;
using System.Threading.Tasks;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private static TripInputInfo tripInfo= new();
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly ISubcategory _subCategoryService;
        private readonly IPlace _placeService;

        private readonly string fromEmail = "inesamkrtchyan9999@gmail.com";
        private readonly string password = "lhwd zttk xque sajp";
        List<PlacesViewModel> places = new List<PlacesViewModel>
        {
            new PlacesViewModel { Id = 1, PlaceName = "Grand Hotel", PlaceNameArm = "Գրանդ Հյուրանոց", PlaceNameRu = "Гранд Отель", Description = "Luxury stay", DescriptionArm = "Հարմարավետ հանգիստ", DescriptionRu = "Роскошное пребывание", Photo = "29653a73-511c-4a38-b8e3-4c872a271c73.jpeg", MinPrice = 10000, MaxPrice = 20000, Raiting = 5, SubcategoryId = 1, PlaceType = PlaceType.HOTEL, Address = "123 Main St", AddressArm = "Մայրուղի 123", AddressRu = "Главная улица 123" },
            new PlacesViewModel { Id = 2, PlaceName = "Sunny Guesthouse", PlaceNameArm = "Արևոտ Հյուրընկալարան", PlaceNameRu = "Солнечный Гостевой Дом", Description = "Cozy guesthouse", DescriptionArm = "Հարմարավետ հյուրատուն", DescriptionRu = "Уютный гостевой дом", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 5000, MaxPrice = 8000, Raiting = 4, SubcategoryId = 2, PlaceType = PlaceType.GUESTHOUSE, Address = "45 Lake Rd", AddressArm = "Լճի փողոց 45", AddressRu = "Озёрная дорога 45" },
            new PlacesViewModel { Id = 3, PlaceName = "City Hostel", PlaceNameArm = "Քաղաքի Հոսթել", PlaceNameRu = "Городской Хостел", Description = "Affordable hostel", DescriptionArm = "Սպասարկման հոսթել", DescriptionRu = "Доступный хостел", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 3000, MaxPrice = 5000, Raiting = 3, SubcategoryId = 3, PlaceType = PlaceType.HOSTEL, Address = "9 Center Ave", AddressArm = "Կենտրոնի 9 փողոց", AddressRu = "Центральный проспект 9" },
            new PlacesViewModel { Id = 4, PlaceName = "History Museum", PlaceNameArm = "Պատմության Թանգարան", PlaceNameRu = "Исторический Музей", Description = "Explore history", DescriptionArm = "Բացահայտեք պատմությունը", DescriptionRu = "Изучайте историю", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 1500, MaxPrice = 2000, SubcategoryId = 4, PlaceType = PlaceType.MUSEUM, Address = "12 Museum St", AddressArm = "Թանգարանի փողոց 12", AddressRu = "Улица Музея 12" },
            new PlacesViewModel { Id = 5, PlaceName = "Crafts Center", PlaceNameArm = "Արվեստների Կենտրոն", PlaceNameRu = "Центр Ремёсел", Description = "Local crafts", DescriptionArm = "Տեղական արհեստներ", DescriptionRu = "Местные ремёсла", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 1000, MaxPrice = 2500, SubcategoryId = 5, PlaceType = PlaceType.CRAFTS, Address = "5 Art St", AddressArm = "Արվեստի փողոց 5", AddressRu = "Улица Искусств 5" },
            new PlacesViewModel { Id = 6, PlaceName = "Ancient Ruins", PlaceNameArm = "Հին Կալվածքներ", PlaceNameRu = "Древние Руины", Description = "Historic site", DescriptionArm = "Պատմական վայր", DescriptionRu = "Историческое место", Photo = "29653a73-511c-4a38-b8e3-4c872a271c73.jpeg", MinPrice = 2000, MaxPrice = 3000, SubcategoryId = 6, PlaceType = PlaceType.HISTORY, Address = "Ruins Rd", AddressArm = "Կալվածքների փողոց", AddressRu = "Дорога Руин" },
            new PlacesViewModel { Id = 7, PlaceName = "Mountain Hike", PlaceNameArm = "Լեռնային Հեծանվավարություն", PlaceNameRu = "Горный Поход", Description = "Outdoor adventure", DescriptionArm = "Բացօթյա արկած", DescriptionRu = "Приключение на природе", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 1000, MaxPrice = 3000, SubcategoryId = 7, PlaceType = PlaceType.OUTDOOR, Address = "Trail 7", AddressArm = "Ուղին 7", AddressRu = "Тропа 7" },
            new PlacesViewModel { Id = 8, PlaceName = "Luna Cafe", PlaceNameArm = "Լունա Կաֆե", PlaceNameRu = "Кафе Луна", Description = "Relax with coffee", DescriptionArm = "Հանգստացեք սուրճի հետ", DescriptionRu = "Отдохните с кофе", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 2000, MaxPrice = 4000, SubcategoryId = 8, PlaceType = PlaceType.CAFE, Address = "Coffee St 10", AddressArm = "Սուրճի փողոց 10", AddressRu = "Кофейная улица 10" },
            new PlacesViewModel { Id = 9, PlaceName = "Blue Restaurant", PlaceNameArm = "Կապույտ Ռեստորան", PlaceNameRu = "Синий Ресторан", Description = "Delicious food", DescriptionArm = "Համեղ սնունդ", DescriptionRu = "Вкусная еда", Photo = "29653a73-511c-4a38-b8e3-4c872a271c73.jpeg", MinPrice = 5000, MaxPrice = 15000, SubcategoryId = 9, PlaceType = PlaceType.RESTAURANT, Address = "Ocean Ave 15", AddressArm = "Ծովի փողոց 15", AddressRu = "Океанский проспект 15" },
            new PlacesViewModel { Id = 10, PlaceName = "Classic Hotel", PlaceNameArm = "Կլասիկ Հյուրանոց", PlaceNameRu = "Классический Отель", Description = "Comfortable rooms", DescriptionArm = "Հարմարավետ սենյակներ", DescriptionRu = "Комфортные номера", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 9000, MaxPrice = 18000, SubcategoryId = 1, PlaceType = PlaceType.HOTEL, Address = "22 King St", AddressArm = "Թագավորի փողոց 22", AddressRu = "Королевская улица 22" },
            new PlacesViewModel { Id = 11, PlaceName = "Village Guesthouse", PlaceNameArm = "Գյուղական Հյուրընկալարան", PlaceNameRu = "Деревенский Гостевой Дом", Description = "Local experience", DescriptionArm = "Տեղական փորձ", DescriptionRu = "Местный опыт", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 4000, MaxPrice = 7000, SubcategoryId = 2, PlaceType = PlaceType.GUESTHOUSE, Address = "Village Rd 8", AddressArm = "Գյուղի փողոց 8", AddressRu = "Деревенская дорога 8" },
            new PlacesViewModel { Id = 12, PlaceName = "Backpackers Hostel", PlaceNameArm = "Ճամփորդների Հոսթել", PlaceNameRu = "Хостел для Путешественников", Description = "Youth stay", DescriptionArm = "Երիտասարդների հանգիստ", DescriptionRu = "Место для молодежи", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 2000, MaxPrice = 3500, SubcategoryId = 3, PlaceType = PlaceType.HOSTEL, Address = "Backpackers St", AddressArm = "Ճամփորդների փողոց", AddressRu = "Улица Путешественников" },
            new PlacesViewModel { Id = 13, PlaceName = "Art Museum", PlaceNameArm = "Արվեստի Թանգարան", PlaceNameRu = "Музей Искусств", Description = "Modern art", DescriptionArm = "Ժամանակակից արվեստ", DescriptionRu = "Современное искусство", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 1200, MaxPrice = 1800, SubcategoryId = 4, PlaceType = PlaceType.MUSEUM, Address = "Art St 14", AddressArm = "Արվեստի փողոց 14", AddressRu = "Улица Искусств 14" },
            new PlacesViewModel { Id = 14, PlaceName = "Handmade Crafts", PlaceNameArm = "Ձեռքով Արվեստներ", PlaceNameRu = "Ручные Ремёсла", Description = "Armenian culture", DescriptionArm = "Հայկական մշակույթ", DescriptionRu = "Армянская культура", Photo = "29653a73-511c-4a38-b8e3-4c872a271c73.jpeg", MinPrice = 1500, MaxPrice = 3000, SubcategoryId = 5, PlaceType = PlaceType.CRAFTS, Address = "Crafts St 3", AddressArm = "Արվեստների փողոց 3", AddressRu = "Улица Ремёсел 3" },
            new PlacesViewModel { Id = 15, PlaceName = "Old Fortress", PlaceNameArm = "Հին Բերդ", PlaceNameRu = "Старая Крепость", Description = "Medieval history", DescriptionArm = "Միջնադարյան պատմություն", DescriptionRu = "Средневековая история", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 2500, MaxPrice = 3500, SubcategoryId = 6, PlaceType = PlaceType.HISTORY, Address = "Fortress Hill", AddressArm = "Բերդի բարձունքը", AddressRu = "Крепостной холм" },
            new PlacesViewModel { Id = 16, PlaceName = "Forest Walk", PlaceNameArm = "Ճանապարհ Ձորում", PlaceNameRu = "Прогулка по Лесу", Description = "Nature walk", DescriptionArm = "Բնության զբոսանք", DescriptionRu = "Прогулка на природе", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 800, MaxPrice = 1800, SubcategoryId = 7, PlaceType = PlaceType.OUTDOOR, Address = "Forest Rd 11", AddressArm = "Ճանապարհ 11", AddressRu = "Лесная дорога 11" },
            new PlacesViewModel { Id = 17, PlaceName = "Coffee House", PlaceNameArm = "Սուրճի Տուն", PlaceNameRu = "Кофейный Дом", Description = "Best coffee", DescriptionArm = "Լավագույն սուրճը", DescriptionRu = "Лучший кофе", Photo = "29653a73-511c-4a38-b8e3-4c872a271c73.jpeg", MinPrice = 1500, MaxPrice = 3500, SubcategoryId = 8, PlaceType = PlaceType.CAFE, Address = "Coffee St 18", AddressArm = "Սուրճի փողոց 18", AddressRu = "Кофейная улица 18" },
            new PlacesViewModel { Id = 18, PlaceName = "Gourmet Restaurant", PlaceNameArm = "Գուրմե Ռեստորան", PlaceNameRu = "Гурме Ресторан", Description = "Fine dining", DescriptionArm = "Բարձրակարգ սնունդ", DescriptionRu = "Высокая кухня", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 10000, MaxPrice = 25000, SubcategoryId = 9, PlaceType = PlaceType.RESTAURANT, Address = "Gourmet Ave 20", AddressArm = "Գուրմե փողոց 20", AddressRu = "Гурме проспект 20" },
            new PlacesViewModel { Id = 19, PlaceName = "Seaside Hotel", PlaceNameArm = "Ծովային Հյուրանոց", PlaceNameRu = "Прибрежный Отель", Description = "Ocean view", DescriptionArm = "Ծովի տեսարան", DescriptionRu = "Вид на океан", Photo = "29653a73-511c-4a38-b8e3-4c872a271c73.jpeg", MinPrice = 11000, MaxPrice = 22000, SubcategoryId = 1, PlaceType = PlaceType.HOTEL, Address = "Seaside Blvd 2", AddressArm = "Ծովի փողոց 2", AddressRu = "Набережная 2" },
            new PlacesViewModel { Id = 20, PlaceName = "Countryside Guesthouse", PlaceNameArm = "Գյուղական Հյուրընկալարան", PlaceNameRu = "Загородный Гостевой Дом", Description = "Peaceful stay", DescriptionArm = "Անհոգ հանգիստ", DescriptionRu = "Спокойный отдых", Photo = "7ba63a8d-8002-4274-b427-5b66b4d147aa.jpeg", MinPrice = 3500, MaxPrice = 6000, SubcategoryId = 2, PlaceType = PlaceType.GUESTHOUSE, Address = "Country Rd 5", AddressArm = "Գյուղական փողոց 5", AddressRu = "Загородная дорога 5" }
        };

        public TripController(ICategory categoryService, ApplicationContext context, IPlace placeService, ISubcategory subcategoryService)
        {
            _categoryService = categoryService;
            _context = context;
            _subCategoryService = subcategoryService;
            _placeService = placeService;
        }

        private void SetCulture()
        {
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string dates, string adult, string children)
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();

            if (string.IsNullOrEmpty(dates) && string.IsNullOrEmpty(adult) && string.IsNullOrEmpty(children))
            {
                return View();
            }
            tripInfo.Date = dates;
            tripInfo.AdultCount = adult;
            tripInfo.ChildrenCount = children;
            TempData["Dates"] = dates;
            TempData["AdultCount"] = adult;
            TempData["ChildrenCount"] = children;

            return RedirectToAction("FirstStep");
        }

        [HttpGet]
        public async Task<IActionResult> FirstStep()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        [HttpPost]
        public IActionResult FirstStep(string transport)
        {
            TempData["Transport"] = transport;
            return RedirectToAction("WheretoStay");
        }

        [HttpGet]
        public async Task<IActionResult> WheretoStay(PlaceType selectedPlaceType = PlaceType.HOTEL, int currentPage = 1)
        {
            SetCulture();

            //var places = await _placeService.GetPlacesByPlaceType(selectedPlaceType);
            var Places = places.Where(p => p.PlaceType == selectedPlaceType).ToList();

            ViewBag.PagesCount = (int)Math.Ceiling((double)Places.Count() / 5);
            ViewBag.CurrentPlaceType = selectedPlaceType;
            ViewBag.Places = Places.Skip((currentPage - 1) * 5).Take(5).ToList();
            ViewBag.CurrentPage = currentPage;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WheretoStay(int selectedPlaceId)
        {
            TempData["Place1Id"] = selectedPlaceId;
            tripInfo.PlaceWhereToStay = places.First(p=> p.Id == selectedPlaceId);
            //tripInfo.PlaceWhereToStay = await _placeService.GetPlaceById(selectedPlaceId);
            return RedirectToAction("WhatToDo");
        }

        [HttpGet]
        public async Task<IActionResult> WhatToDo()
        {
            var allPlaces = places;//await _placeService.GetPlacesWithPlaceType();
            ViewBag.Places = allPlaces;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WhatToDo(List<int> selectedPlaceIds)
        {
            if (selectedPlaceIds == null || selectedPlaceIds.Count == 0)
            {
                ModelState.AddModelError("", "Խնդրում ենք ընտրել առնվազն մեկ տեղ։");
                return View();
            }
            foreach (int id in selectedPlaceIds)
            {
                tripInfo.PlaceWhatToDo.Add(places.First(p => p.Id == id));
                //var p = await _placeService.GetPlaceById(id);
                //tripInfo.PlaceWhatToDo.Add(p);

            }
            return RedirectToAction("ForthStep"); // or wherever you want to go
        }

        [HttpGet]
        public async Task<IActionResult> ForthStep()
        {
            ViewBag.FullInfo = tripInfo;
            //var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            //var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            //Thread.CurrentThread.CurrentCulture = cultureInfo;
            //Thread.CurrentThread.CurrentUICulture = cultureInfo;
            //ViewBag.Categories = await _categoryService.GetAllCategories();

            //if (!TempData.TryGetValue("Place1Id", out var place1IdObj) || !TempData.TryGetValue("Place2Id", out var place2IdObj))
            //{
            //    return RedirectToAction("Index");
            //}

            //int place1Id = Convert.ToInt32(place1IdObj);
            //int place2Id = Convert.ToInt32(place2IdObj);

            //var place1 = await _placeService.GetPlaceById(place1Id);
            //var place2 = await _placeService.GetPlaceById(place2Id);
            //ViewBag.CurrentCulture = string.IsNullOrEmpty(currentCulture) ? "en" : currentCulture;
            //ViewBag.Place1 = place1;
            //ViewBag.Place2 = place2;

            return View();
        }

        public async Task<IActionResult> Final()
        {
            SetCulture();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        // ______________________ Send Email ____________________
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(fromEmail, "Gyumri Trip Planner");
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

        [HttpPost]
        public async Task<IActionResult> SendWelcomeEmail(string email)
        {
            SetCulture();

            if (!TempData.TryGetValue("Place1Id", out var place1IdObj) || !TempData.TryGetValue("Place2Id", out var place2IdObj))
            {
                return RedirectToAction("Index");
            }

            int place1Id = Convert.ToInt32(place1IdObj);
            int place2Id = Convert.ToInt32(place2IdObj);

            var place1 = await _placeService.GetPlaceById(place1Id);
            var place2 = await _placeService.GetPlaceById(place2Id);

            var subject = AppRes.WelcomeGyumri;
            string body;

            string currentCulture = Request.Cookies["UserCulture"] ?? "en";

            if (currentCulture == "ru-RU")
            {
                body = $@"
        <h1>{AppRes.TripPlan}</h1>
        <h3>{AppRes.WheretoStay}</h3>
        <p>{(place1?.PlaceNameRu ?? "N/A")}</p>
        <br/>
        <h3>{AppRes.WhatToDo}</h3>
        <p>{(place2?.PlaceNameRu ?? "N/A")}</p>";
            }
            else if (currentCulture == "hy-AM")
            {
                body = $@"
        <h1>{AppRes.TripPlan}</h1>
        <h3>{AppRes.WheretoStay}</h3>
        <p>{(place1?.PlaceNameArm ?? "N/A")}</p>
        <br/>
        <h3>{AppRes.WhatToDo}</h3>
        <p>{(place2?.PlaceNameArm ?? "N/A")}</p>";
            }
            else
            {
                body = $@"
        <h1>{AppRes.TripPlan}</h1>
        <h3>{AppRes.WheretoStay}</h3>
        <p>{(place1?.PlaceName ?? "N/A")}</p>
        <br/>
        <h3>{AppRes.WhatToDo}</h3>
        <p>{(place2?.PlaceName ?? "N/A")}</p>";
            }


            SendEmail(email, subject, body);

            return RedirectToAction("Final");
        }
    }
}
