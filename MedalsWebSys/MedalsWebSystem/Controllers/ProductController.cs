using MedalsWebSystem.Models;
using MedalsWebSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace MedalsWebSystem.Controllers
{
    public class ProductController : Controller
    {

        private ApplicationContext db;
        public ProductController(ApplicationContext context)
        {
            db = context;
        }
        private string[] RusCountries { get; } = new string[] {"Андорра", "Аргентина", "Австрия", "Беларусь", "Бельгия", "Болгария",
        "Канада", "Хорватия", "Куба", "Кипр", "Чешская Республика", "Дания", "Эквадор", "Эстония", "Финляндия", "Франция",
        "Германия", "Греция", "Гватемала", "Венгрия", "Ирландия", "Остров Мэн", "Италия", "Япония", "Казахстан", "Латвия",
        "Литва", "Люксембург", "Македония", "Мальта", "Мексика", "Молдавия", "Монако", "Нидерланды", "Никарагуа", "Норвегия",
        "Парагвай", "Перу", "Польша", "Португалия", "Румыния", "Россия", "Сан Марино", "Сербия", "Словакия", "Словения",
        "Южная Корея", "Испания", "Швеция", "Швейцария", "Турция", "Украина", "Великобритания", "США", "Ватикан"};
        private string[] RusMaterials { get; } = new string[] { "Золото", "Серебро", "Медь", "Бронза", "Латунь", "Томпак", "Алюминий",
        "Медноникель", "Нейзильбер", "Сталь", "Биметалл", "Другое"};
        private string[] RusCategories { get; } = new string[] {"Архитектура", "Искусство", "Спорт", "Наука", "Промышленность",
        "Транспорт", "Фауна", "Флора", "Освоение космоса", "История и личности", "Другое"};


        [HttpGet]
        [Authorize]
        public ViewResult AddProduct()
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddProduct(AddProductModel product)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            
            if (product.Photo != null && product.Photo.Length >= 5242880)
            {
                ModelState.AddModelError("Photo", "Размер файла должен быть меньше 5мб");
            }
            if (ModelState.IsValid)
            {
                Product newPr = new Product
                {
                    Name = product.Name,
                    Country = product.Country,
                    Category = product.Category,
                    Year = product.Year,
                    Material = product.Material,
                    Weight = product.Weight,
                    Diameter = product.Diameter,
                    Description = product.Description,
                    UserAdder = user,
                    DateTime = DateTime.UtcNow
                };
                if (product.Photo != null)
                {
                    byte[] imageData = null;
                    byte[] minImageData = null;

                    using (var binaryReader = new BinaryReader(product.Photo.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)product.Photo.Length);
                    }

                    using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                        {
                            int h = 75;
                            int w = 150;

                            using (Bitmap b = new Bitmap(img, new Size(w, h)))
                            {
                                using (MemoryStream ms2 = new MemoryStream())
                                {
                                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    minImageData = ms2.ToArray();
                                }
                            }
                        }
                    }

                    newPr.Photo = imageData;
                    newPr.minPhoto = minImageData;
                }
                db.Add(newPr);
                db.SaveChanges();
                user.UserProducts.Add(new UserProduct { ProductId = newPr.ProductId, UserId = user.UserId });
                db.SaveChanges();
                return RedirectToAction("MyCollection", "Home");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ViewResult UpdateProduct(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;


            Product pr = db.Products.Include(u=>u.UserProducts).ThenInclude(uc=>uc.User).FirstOrDefault(x => x.ProductId == productId);
            List<User> us = pr.UserProducts.Select(uc => uc.User).ToList();

            AddProductModel prView = new AddProductModel
            {
                ProductId = pr.ProductId,
                Name = pr.Name,
                Country = pr.Country,
                Category = pr.Category,
                Year = pr.Year,
                Material = pr.Material,
                Weight = pr.Weight,
                Diameter = pr.Diameter,
                Description = pr.Description
            };

            bool key=false;
            foreach (User user in us)
            {
                if (user.Email == ViewBag.UserName)
                    key = true;
            }
            if ((key) && (us != null))
            {
                return View(prView);
            }
            else
                return View("AddProduct");
        }

        [HttpPost]
        public IActionResult UpdateProduct(AddProductModel product)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;
            if (product.Photo != null && product.Photo.Length >= 5242880)
            {
                ModelState.AddModelError("Photo", "Размер файла должен быть меньше 5мб");
            }
            if (ModelState.IsValid)
            {
                Product pr = db.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
                pr.Name = product.Name;
                pr.Year = product.Year;
                pr.Country = product.Country;
                pr.Category = product.Category;
                pr.Material = product.Material;
                pr.Weight = product.Weight;
                pr.Diameter = product.Diameter;
                pr.Description = product.Description;
                if (product.Photo != null)
                {
                    byte[] imageData = null;
                    byte[] minImageData = null;

                    using (var binaryReader = new BinaryReader(product.Photo.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)product.Photo.Length);
                    }

                    using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                        {
                            int h = 75;
                            int w = 150;

                            using (Bitmap b = new Bitmap(img, new Size(w, h)))
                            {
                                using (MemoryStream ms2 = new MemoryStream())
                                {
                                    b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    minImageData = ms2.ToArray();
                                }
                            }
                        }
                    }

                    pr.Photo = imageData;
                    pr.minPhoto = minImageData;
                }
                db.Update(pr);
                db.SaveChanges();
                return RedirectToAction("MyCollection", "Home");
            }
            return View(product);
        }

        [Authorize]
        public ViewResult DeleteProduct(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Product product = db.Products.Include(u => u.UserProducts).ThenInclude(uc => uc.User).FirstOrDefault(x => x.ProductId == productId);
            return View(product);
        }


        public IActionResult DeleteProductAction(int productId)
        {
            ViewBag.UserName = User.Identity.Name;
            Product product = db.Products.Include(u => u.UserAdder).FirstOrDefault(x => x.ProductId == productId);
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if ((product.UserAdder.Email== User.Identity.Name && product.isMainCatalog==false)
                || user.Role=="Администратор" || user.Role == "Модератор")
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("MyCollection", "Home");
        }

        public ViewResult ProductInfo(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.Include(xc=>xc.UserProducts).ThenInclude(xc=>xc.Product).FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.UserRole = user.Role;
            List<Product> curUserProducts = user.UserProducts.Select(x => x.Product).ToList();

            ViewBag.CanIAddIt = true;
            ViewBag.CanIDeleteIt = false;

            foreach (Product pr in curUserProducts)
            {
                if (pr.ProductId == productId)
                {
                    ViewBag.CanIAddIt = false;
                    ViewBag.CanIDeleteIt = true;
                } 
                    
            }
            Product product = db.Products.Include(x => x.UserProducts).ThenInclude(xs => xs.User).FirstOrDefault(x => x.ProductId == productId);
            if (product.isMainCatalog==false)
            {
                ViewBag.CanIAddIt = false;
                ViewBag.CanIDeleteIt = false;
            }
                
            List<User> users = product.UserProducts.Select(x => x.User).ToList();
            ProductInfoViewModel viewModel = new ProductInfoViewModel
            {
                Product = product,
                Users = users
            };
            return View(viewModel);
        }

        [Authorize]
        public IActionResult AddToMyCollection(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);
            User user = db.Users.Include(x=>x.UserProducts).ThenInclude(xc=>xc.Product).FirstOrDefault(x => x.Email == User.Identity.Name);
            bool key = false;
            foreach( Product pr in user.UserProducts.Select(x=>x.Product))
            {
                if (pr.ProductId==productId)
                {
                    key = true;
                }
            }
            if(!key && product.isMainCatalog)
            {
                user.UserProducts.Add(new UserProduct { UserId = user.UserId, ProductId = productId });
                db.SaveChanges();
            }
            
            return RedirectToAction("MyCollection", "Home");
        }

        [Authorize]
        public IActionResult DeleteFromMyCollection(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.Include(x => x.UserProducts).ThenInclude(xc => xc.Product).FirstOrDefault(x => x.Email == User.Identity.Name);
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            bool key = false;
            foreach (Product pr in user.UserProducts.Select(x => x.Product))
            {
                if (pr.ProductId == productId)
                {
                    key = true;
                }
            }
            if (key && product.isMainCatalog)
            {
                var userProduct = user.UserProducts.FirstOrDefault(x => x.ProductId == productId);
                user.UserProducts.Remove(userProduct);
                db.SaveChanges();
            }

            return RedirectToAction("MyCollection", "Home");
        }

        [Authorize]
        public IActionResult AddToMainCatalog(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.Include(x => x.UserProducts).ThenInclude(xc => xc.Product).FirstOrDefault(x => x.Email == User.Identity.Name);
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            if (!product.isMainCatalog && (user.Role=="Администратор" || user.Role =="Модератор"))
            {
                product.isMainCatalog = true;
                db.Products.Update(product);
                db.SaveChanges();
            }
            return RedirectToAction("MainCatalog", "Home");
        }

        [Authorize]
        public IActionResult DeleteFromMainCatalog(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.Include(x => x.UserProducts).ThenInclude(xc => xc.Product).FirstOrDefault(x => x.Email == User.Identity.Name);
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product.isMainCatalog && (user.Role == "Администратор" || user.Role == "Модератор"))
            {
                product.isMainCatalog = false;
                db.Products.Update(product);
                db.SaveChanges();
            }
            return RedirectToAction("MainCatalog", "Home");
        }

        [HttpGet]
        [Authorize]
        public ViewResult AddProductMC()
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            if (user.Role == "Администратор" || user.Role == "Модератор")
            {
                return View();
            }
            else return View("MainCatalog", "Home");
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddProductMC(AddProductModel product)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            if (product.Photo != null && product.Photo.Length >= 5242880)
            {
                ModelState.AddModelError("Photo", "Размер файла должен быть меньше 5мб");
            }
            if (ModelState.IsValid)
            {
                if (user.Role == "Администратор" || user.Role == "Модератор")
                {
                    Product newPr = new Product
                    {
                        Name = product.Name,
                        Country = product.Country,
                        Category = product.Category,
                        Year = product.Year,
                        Material = product.Material,
                        Weight = product.Weight,
                        Diameter = product.Diameter,
                        Description = product.Description,
                        UserAdder = user,
                        DateTime = DateTime.UtcNow,
                        isMainCatalog = true
                    };
                    if (product.Photo != null)
                    {
                        byte[] imageData = null;
                        byte[] minImageData = null;

                        using (var binaryReader = new BinaryReader(product.Photo.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)product.Photo.Length);
                        }

                        using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                        {
                            using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                            {
                                int h = 75;
                                int w = 150;

                                using (Bitmap b = new Bitmap(img, new Size(w, h)))
                                {
                                    using (MemoryStream ms2 = new MemoryStream())
                                    {
                                        b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        minImageData = ms2.ToArray();
                                    }
                                }
                            }
                        }

                        newPr.Photo = imageData;
                        newPr.minPhoto = minImageData;
                    }
                    db.Add(newPr);
                    db.SaveChanges();

                }

                return RedirectToAction("MainCatalog", "Home");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ViewResult UpdateProductMC(int productId)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            Array.Sort(RusCountries);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            Product pr = db.Products.Include(u => u.UserProducts).ThenInclude(uc => uc.User).FirstOrDefault(x => x.ProductId == productId);

            AddProductModel prView = new AddProductModel
            {
                ProductId = pr.ProductId,
                Name = pr.Name,
                Country = pr.Country,
                Category = pr.Category,
                Year = pr.Year,
                Material = pr.Material,
                Weight = pr.Weight,
                Diameter = pr.Diameter,
                Description = pr.Description
            };

            if ((user.Role == "Администратор" || user.Role == "Модератор") && pr.isMainCatalog)
            {
                return View("UpdateProductMC", prView);
            }
            else
                return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateProductMC(AddProductModel product)
        {
            ViewBag.UserAuth = User.Identity.IsAuthenticated;
            ViewBag.UserName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.Countries = RusCountries;
            ViewBag.Materials = RusMaterials;
            ViewBag.Categories = RusCategories;

            if (product.Photo != null && product.Photo.Length >= 5242880)
            {
                ModelState.AddModelError("Photo", "Размер файла должен быть меньше 5мб");
            }
            if (ModelState.IsValid)
            {
                if ((user.Role == "Администратор" || user.Role == "Модератор"))
                {
                    Product pr = db.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
                    pr.Name = product.Name;
                    pr.Year = product.Year;
                    pr.Country = product.Country;
                    pr.Category = product.Category;
                    pr.Material = product.Material;
                    pr.Weight = product.Weight;
                    pr.Diameter = product.Diameter;
                    pr.Description = product.Description;
                    if (product.Photo != null)
                    {
                        byte[] imageData = null;
                        byte[] minImageData = null;

                        using (var binaryReader = new BinaryReader(product.Photo.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)product.Photo.Length);
                        }

                        using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
                        {
                            using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                            {
                                int h = 75;
                                int w = 150;

                                using (Bitmap b = new Bitmap(img, new Size(w, h)))
                                {
                                    using (MemoryStream ms2 = new MemoryStream())
                                    {
                                        b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        minImageData = ms2.ToArray();
                                    }
                                }
                            }
                        }
                        pr.Photo = imageData;
                        pr.minPhoto = minImageData;
                    }
                    db.Update(pr);
                    db.SaveChanges();
                }
                return RedirectToAction("MainCatalog", "Home");
            }
            return View(product);    
        }
    }
}
