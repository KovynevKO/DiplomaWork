using MedalsWebSystem.Models;
using MedalsWebSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MedalsWebSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        private FilterInformation filterInformation;
        private SortingInformation sortingInformation;

        public string[] RusCountries { get; } = new string[] {"Андорра", "Аргентина", "Австрия", "Беларусь", "Бельгия", "Болгария",
        "Канада", "Хорватия", "Куба", "Кипр", "Чешская Республика", "Дания", "Эквадор", "Эстония", "Финляндия", "Франция",
        "Германия", "Греция", "Гватемала", "Венгрия", "Ирландия", "Остров Мэн", "Италия", "Япония", "Казахстан", "Латвия",
        "Литва", "Люксембург", "Македония", "Мальта", "Мексика", "Молдавия", "Монако", "Нидерланды", "Никарагуа", "Норвегия",
        "Парагвай", "Перу", "Польша", "Португалия", "Румыния", "Россия", "Сан Марино", "Сербия", "Словакия", "Словения",
        "Южная Корея", "Испания", "Швеция", "Швейцария", "Турция", "Украина", "Великобритания", "США", "Ватикан"};
        public string[] RusMaterials { get; } = new string[] { "Золото", "Серебро", "Медь", "Бронза", "Латунь", "Томпак", "Алюминий",
        "Медноникель", "Нейзильбер", "Сталь", "Биметалл", "Другое"};
        public string[] RusCategories { get; } = new string[] {"Архитектура", "Искусство", "Спорт", "Наука", "Промышленность",
        "Транспорт", "Фауна", "Флора", "Освоение космоса", "История и личности", "Другое"};
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        [Authorize]
        public ViewResult MyCollection(int page = 1, string name = null, string category = null, string country = null, int? minYear = null, int? maxYear = null, string material = null,
            float? minWeight = null, float? maxWeight = null, float? minDiameter = null, float? maxDiameter = null,
            string byName = null, string byYear = null, string byCountry = null, string byCategory = null, string byMaterial = null, string byWeight = null, string byDiameter = null,
            string byDateTime=null)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            User user = db.Users.Include(c=>c.UserProducts).ThenInclude(cs=>cs.Product).FirstOrDefault(x => x.Email == User.Identity.Name);
            List<Product> allProducts = user.UserProducts.Select(sc => sc.Product).OrderByDescending(x => x.DateTime).ToList();
            ViewBag.CatalogCount = allProducts.Count;

            filterInformation = new FilterInformation
            {
                name = name,
                country = country,
                category = category,
                minYear = minYear,
                maxYear = maxYear,
                material = material,
                minWeight = minWeight,
                maxWeight = maxWeight,
                minDiameter = minDiameter,
                maxDiameter = maxDiameter
            };
            sortingInformation = new SortingInformation
            {
                byName = byName,
                byCountry = byCountry,
                byCategory = byCategory,
                byYear = byYear,
                byMaterial = byMaterial,
                byWeight = byWeight,
                byDiameter = byDiameter,
                byDateTime=byDateTime
            };

            allProducts = ApplyFilter(allProducts, filterInformation);
            allProducts = ApplySorting(allProducts, sortingInformation);
            if (filterInformation.name != null)
                allProducts = SearchByName(allProducts, filterInformation.name);

            int pageSize = 50;

            var count = allProducts.Count();
            ViewBag.SearchCount = count;
            var items = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CatalogViewModel viewModel = new CatalogViewModel
            {
                PageViewModel = pageViewModel,
                Products = items,
                FilterInformation = filterInformation,
                SortingInformation = sortingInformation
            };
            return View("MyCollection", viewModel);
        }

        public ViewResult MainCatalog(int page = 1, string name = null, string category = null, string country = null, int? minYear = null, int? maxYear = null, string material = null,
            float? minWeight = null, float? maxWeight = null, float? minDiameter = null, float? maxDiameter = null,
            string byName = null, string byYear = null, string byCountry = null, string byCategory = null, string byMaterial = null, string byWeight = null, string byDiameter = null,
            string byDateTime = null)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;
            User user = db.Users.Include(x=>x.UserProducts).FirstOrDefault(x => x.Email == User.Identity.Name);
            

            if (user == null)
                ViewBag.UserRole = "";
            else
                ViewBag.UserRole = user.Role;

            List<Product> allProducts = db.Products.Include(x=>x.UserAdder).Where(x=>x.isMainCatalog==true).OrderByDescending(x => x.DateTime).ToList();
            ViewBag.CatalogCount = allProducts.Count;

            filterInformation = new FilterInformation
            {
                name = name,
                country = country,
                category = category,
                minYear = minYear,
                maxYear = maxYear,
                material = material,
                minWeight = minWeight,
                maxWeight = maxWeight,
                minDiameter = minDiameter,
                maxDiameter = maxDiameter
            };
            sortingInformation = new SortingInformation
            {
                byName = byName,
                byCountry = byCountry,
                byCategory = byCategory,
                byYear = byYear,
                byMaterial = byMaterial,
                byWeight = byWeight,
                byDiameter = byDiameter,
                byDateTime = byDateTime
            };

            allProducts = ApplyFilter(allProducts, filterInformation);
            allProducts = ApplySorting(allProducts, sortingInformation);
            if (filterInformation.name != null)
                allProducts = SearchByName(allProducts, filterInformation.name);

            int pageSize = 50;

            var count = allProducts.Count();
            ViewBag.SearchCount = count;
            var items = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CatalogViewModel viewModel = new CatalogViewModel
            {
                PageViewModel = pageViewModel,
                Products = items,
                FilterInformation = filterInformation,
                SortingInformation = sortingInformation
            };
            return View("MainCatalog", viewModel);
        }

        [HttpGet]
        public ViewResult UserCatalog(int page=1, string name=null, string category=null, string country = null, int? minYear = null, int? maxYear = null, string material = null,
            float? minWeight = null, float? maxWeight = null, float? minDiameter = null, float? maxDiameter = null,
            string byName = null, string byYear = null, string byCountry = null, string byCategory=null, string byMaterial = null, string byWeight = null, string byDiameter = null,
            string byDateTime = null)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;


            List<Product> allProducts = db.Products.Include(c=>c.UserAdder).Where(x=>x.isMainCatalog==false).OrderByDescending(x => x.DateTime).ToList();
            ViewBag.CatalogCount = allProducts.Count;
            filterInformation = new FilterInformation { name = name, country = country, category = category, minYear = minYear, maxYear = maxYear,
                material = material, minWeight = minWeight, maxWeight = maxWeight, minDiameter = minDiameter, maxDiameter = maxDiameter };
            sortingInformation = new SortingInformation { byName = byName, byCountry = byCountry, byCategory = byCategory, byYear = byYear,
                byMaterial = byMaterial, byWeight = byWeight, byDiameter = byDiameter,
                byDateTime = byDateTime
            };

            allProducts = ApplyFilter(allProducts, filterInformation);
            allProducts = ApplySorting(allProducts, sortingInformation);
            if (filterInformation.name != null)
                allProducts = SearchByName(allProducts, filterInformation.name);

            int pageSize = 50;

            var count = allProducts.Count();
            ViewBag.SearchCount = count;
            var items = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CatalogViewModel viewModel = new CatalogViewModel
            {
                PageViewModel = pageViewModel,
                Products = items,
                FilterInformation = filterInformation,
                SortingInformation = sortingInformation
            };
            return View("UserCatalog", viewModel);
        }

        public ViewResult AboutUser (int userId, int page = 1, string name = null, string category = null, string country = null, int? minYear = null, int? maxYear = null, string material = null,
            float? minWeight = null, float? maxWeight = null, float? minDiameter = null, float? maxDiameter = null,
            string byName = null, string byYear = null, string byCountry = null, string byCategory = null, string byMaterial = null, string byWeight = null, string byDiameter = null,
            string byDateTime = null)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;
            User userAbout = db.Users.Include(c => c.UserProducts).ThenInclude(cs => cs.Product).FirstOrDefault(x => x.UserId == userId);

            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (user == null)
                ViewBag.UserRole = "";
            else
                ViewBag.UserRole = user.Role;

            List<Product> allProducts = userAbout.UserProducts.Select(sc => sc.Product).OrderByDescending(x=>x.DateTime).ToList();
            ViewBag.CatalogCount = allProducts.Count;

            filterInformation = new FilterInformation
            {
                name = name,
                country = country,
                category = category,
                minYear = minYear,
                maxYear = maxYear,
                material = material,
                minWeight = minWeight,
                maxWeight = maxWeight,
                minDiameter = minDiameter,
                maxDiameter = maxDiameter
            };
            sortingInformation = new SortingInformation
            {
                byName = byName,
                byCountry = byCountry,
                byCategory = byCategory,
                byYear = byYear,
                byMaterial = byMaterial,
                byWeight = byWeight,
                byDiameter = byDiameter,
                byDateTime = byDateTime
            };

            allProducts = ApplyFilter(allProducts, filterInformation);
            allProducts = ApplySorting(allProducts, sortingInformation);
            if (filterInformation.name != null)
                allProducts = SearchByName(allProducts, filterInformation.name);

            int pageSize = 50;

            var count = allProducts.Count();
            ViewBag.SearchCount = count;
            var items = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CatalogViewModel viewModel = new CatalogViewModel
            {
                PageViewModel = pageViewModel,
                Products = items,
                FilterInformation = filterInformation,
                SortingInformation = sortingInformation,
                User = userAbout
            };
            return View("AboutUser", viewModel);
        }

        private List<Product> ApplyFilter(List<Product> products, FilterInformation filter)
        {
            int? minYear = filter.minYear;
            int? maxYear = filter.maxYear;
            float? minDiameter = filter.minDiameter;
            float? maxDiameter = filter.maxDiameter;
            float? minWeight = filter.minWeight;
            float? maxWeight = filter.maxWeight;
            string country = filter.country;
            string category = filter.category;
            string material = filter.material;

            if (minYear == null)
                minYear = 0;
            if (maxYear == null)
                maxYear = 10000;
            products = products.Where(x => x.Year <= maxYear && x.Year >= minYear).ToList();
            
            if (minWeight == null)
                minWeight = 0;
            if (maxWeight == null)
                maxWeight = 10000;
            products = products.Where(x => x.Weight <= maxWeight && x.Weight >= minWeight).ToList();

            if (minDiameter == null)
                minDiameter = 0;
            if (maxDiameter == null)
                maxDiameter = 10000;
            products = products.Where(x => x.Diameter >= minDiameter && x.Year <= maxDiameter).ToList();

            if (country != null)
                products = products.Where(x => x.Country == country).ToList();
            if (category != null)
                products = products.Where(x => x.Category == category).ToList();
            if (material != null)
                products = products.Where(x => x.Material == material).ToList();

            return products;
        }
        private List<Product> ApplySorting(List<Product> products, SortingInformation sorter)
        {

            if (sorter.byDiameter == "По возрастанию")
                products = products.OrderBy(x => x.Diameter).ToList();
            else if (sorter.byDiameter == "По убыванию")
                products = products.OrderByDescending(x => x.Diameter).ToList();

            if (sorter.byWeight == "По возрастанию")
                products = products.OrderBy(x => x.Weight).ToList();
            else if (sorter.byWeight == "По убыванию")
                products = products.OrderByDescending(x => x.Weight).ToList();

            if (sorter.byMaterial == "По возрастанию")
                products = products.OrderBy(x => x.Material).ToList();
            else if (sorter.byMaterial == "По убыванию")
                products = products.OrderByDescending(x => x.Material).ToList();

            if (sorter.byYear == "По возрастанию")
                products = products.OrderBy(x => x.Year).ToList();
            else if (sorter.byYear == "По убыванию")
                products = products.OrderByDescending(x => x.Year).ToList();

            if (sorter.byCategory == "По возрастанию")
                products = products.OrderBy(x => x.Category).ToList();
            else if (sorter.byCategory == "По убыванию")
                products = products.OrderByDescending(x => x.Category).ToList();

            if (sorter.byCountry == "По возрастанию")
                products = products.OrderBy(x => x.Country).ToList();
            else if (sorter.byCountry == "По убыванию")
                products = products.OrderByDescending(x => x.Country).ToList();

            if (sorter.byName == "По возрастанию")
                products = products.OrderBy(x => x.Name).ToList();
            else if (sorter.byName == "По убыванию")
                products = products.OrderByDescending(x => x.Name).ToList();

            if (sorter.byDateTime == "По возрастанию")
                products = products.OrderBy(x => x.DateTime).ToList();
            else if (sorter.byDateTime == "По убыванию")
                products = products.OrderByDescending(x => x.DateTime).ToList();

            return products;
        }

        public List<Product> SearchByName(List<Product> products, string name)
        {
            return products.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
        }

        public IActionResult ChangeRoleUser(int userId, string newRole)
        {
            string curName = User.Identity.Name;
            User curUser = db.Users.FirstOrDefault(x => x.Email == curName);
            User user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (curUser.Role=="Администратор" && user.Role!="Администратор")
            {
                
                user.Role = newRole;
                db.Update(user);
                db.SaveChanges();
            }
            return RedirectToAction("AboutUser", new { userId = userId });
        }

        public ViewResult UserSearch(string userName)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            List<User> users = db.Users.Where(x => x.Login.ToLower().Contains(userName.ToLower())).ToList();
            return View(users);
        }
    }
}
