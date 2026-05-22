using DeliveryShopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryShopApp.Data;

public static class DbInitializer
{
    public static void Seed(DataContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Client" }
            );
            context.SaveChanges();
        }

        if (!context.OrderStatuses.Any())
        {
            context.OrderStatuses.AddRange(
                new OrderStatus { Name = "Нове" },
                new OrderStatus { Name = "Підтверджено" },
                new OrderStatus { Name = "Комплектується" },
                new OrderStatus { Name = "В дорозі" },
                new OrderStatus { Name = "Доставлено" },
                new OrderStatus { Name = "Скасовано" }
            );
            context.SaveChanges();
        }

        if (!context.Users.Any(u => u.Email == "admin@delivery.com"))
        {
            var adminRole = context.Roles.First(r => r.Name == "Admin");
            context.Users.Add(new User
            {
                FirstName = "Адмін",
                LastName = "Магазину",
                Email = "admin@delivery.com",
                Phone = "+380000000000",
                PasswordHash = "admin123",
                RoleId = adminRole.Id
            });
            context.SaveChanges();
        }

        if (!context.Units.Any())
        {
            context.Units.AddRange(
                new Unit { Name = "шт" },
                new Unit { Name = "кг" }
            );
            context.SaveChanges();
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Овочі та фрукти" },
                new Category { Name = "М'ясо, птиця" },
                new Category { Name = "Риба та морепродукти" },
                new Category { Name = "Яйця та молочні продукти" },
                new Category { Name = "Пекарня та хліб" },
                new Category { Name = "Бакалія" },
                new Category { Name = "Соуси та спеції" },
                new Category { Name = "Консерви та паштети" },
                new Category { Name = "Солодощі та десерти" },
                new Category { Name = "Cнеки" },
                new Category { Name = "Гарячі напої" },
                new Category { Name = "Соки та безалкогольні напої" },
                new Category { Name = "Заморожені продукти" }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var pcsUnit = context.Units.First(u => u.Name == "шт");
            var kgUnit = context.Units.First(u => u.Name == "кг");

            var vegCat = context.Categories.First(c => c.Name == "Овочі та фрукти");
            var meatCat = context.Categories.First(c => c.Name == "М'ясо, птиця");
            var fishCat = context.Categories.First(c => c.Name == "Риба та морепродукти");
            var dairyCat = context.Categories.First(c => c.Name == "Яйця та молочні продукти");
            var bakeryCat = context.Categories.First(c => c.Name == "Пекарня та хліб");
            var groceryCat = context.Categories.First(c => c.Name == "Бакалія");
            var sauceCat = context.Categories.First(c => c.Name == "Соуси та спеції");
            var cansCat = context.Categories.First(c => c.Name == "Консерви та паштети");
            var sweetsCat = context.Categories.First(c => c.Name == "Солодощі та десерти");
            var snacksCat = context.Categories.First(c => c.Name == "Cнеки");
            var hotDrinksCat = context.Categories.First(c => c.Name == "Гарячі напої");
            var coldDrinksCat = context.Categories.First(c => c.Name == "Соки та безалкогольні напої");
            var frozenCat = context.Categories.First(c => c.Name == "Заморожені продукти");

            context.Products.AddRange(
                new Product
                {
                    Name = "Банани імпортні",
                    Description = "Свіжі стиглі банани, багаті на калій.",
                    Price = 59.00m, 
                    CategoryId = vegCat.Id,
                    UnitId = kgUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1571771894821-ce9b6c11b08e?w=500&q=80",
                    StockQuantity = 120,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Томати Черрі",
                    Description = "Маленькі, солодкі та соковиті томати черрі на гілці.",
                    Price = 125.00m,
                    CategoryId = vegCat.Id,
                    UnitId = kgUnit.Id,
                    ImageUrl = "https://www.google.com/url?sa=t&source=web&rct=j&url=https%3A%2F%2Ffruit-time.ua%2Fproduct%2Fcerri-imp.html&ved=0CBYQjRxqFwoTCIiqvYyLzZQDFQAAAAAdAAAAABAG&opi=89978449",
                    StockQuantity = 40,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Філе куряче охолоджене",
                    Description = "Ніжне дієтичне м'ясо птиці, свіжа обробка.",
                    Price = 165.00m,
                    CategoryId = meatCat.Id,
                    UnitId = kgUnit.Id, 
                    ImageUrl = "https://images.unsplash.com/photo-1604503468506-a8da13d82791?w=500&q=80",
                    StockQuantity = 45,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Стейк із лосося",
                    Description = "Свіжоморожений стейк атлантичного лосося.",
                    Price = 620.00m,
                    CategoryId = fishCat.Id,
                    UnitId = kgUnit.Id,
                    ImageUrl = "https://images.silpo.ua/v2/products/1200x1200/webp/0b60fe92-c678-4d31-9808-f5095d898ff6.png",
                    StockQuantity = 15,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Молоко 2.5%",
                    Description = "Пастеризоване коров'яче молоко в пакеті, 900 г.",
                    Price = 38.50m,
                    CategoryId = dairyCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1550583724-b2692b85b150?w=500&q=80",
                    StockQuantity = 60,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Сир Твердий Гауда",
                    Description = "Класичний напівтвердий сир із ніжним вершковим смаком.",
                    Price = 310.00m,
                    CategoryId = dairyCat.Id,
                    UnitId = kgUnit.Id,
                    ImageUrl = "https://foodfestival.com.ua/image/cache/catalog/000col/00329-1000x1000.jpg",
                    StockQuantity = 25,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Круасан з шоколадом",
                    Description = "Свіжоспечений хрусткий круасан з французького тіста з шоколадною начинкою.",
                    Price = 32.00m,
                    CategoryId = bakeryCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=500&q=80",
                    StockQuantity = 15,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Багет французький",
                    Description = "Традиційний хліб із хрусткою скоринкою та м'якою серединою.",
                    Price = 24.00m,
                    CategoryId = bakeryCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.silpo.ua/v2/products/1200x1200/webp/a28e99b6-db30-4d28-b97d-337140f5c3c7.png",
                    StockQuantity = 30,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Рис Басматі",
                    Description = "Довгозернистий ароматний рис вищого ґатунку, 1 кг.",
                    Price = 85.00m,
                    CategoryId = groceryCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1586201375761-83865001e31c?w=500&q=80",
                    StockQuantity = 100,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Кетчуп Лагідний",
                    Description = "Класичний томатний соус зі спеціями, 300 г.",
                    Price = 42.00m,
                    CategoryId = sauceCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://i.ebayimg.com/images/g/T74AAeSwm6Zp86~V/s-l1600.jpg",
                    StockQuantity = 70,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Шпроти в олії",
                    Description = "Консервована копчена риба в соняшниковій олії, 160 г.",
                    Price = 65.00m,
                    CategoryId = cansCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://kuldim.com/wa-data/public/shop/products/55/66/6655/images/35488/35488.970.jpg",
                    StockQuantity = 90,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Шоколад чорний 70%",
                    Description = "Плитка класичного гіркого шоколаду з добірних какао-бобів.",
                    Price = 48.00m,
                    CategoryId = sweetsCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44015466834?w=500&q=80",
                    StockQuantity = 80,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Картопляні чіпси з сіллю",
                    Description = "Хрусткі скибочки добірної картоплі з морською сіллю.",
                    Price = 55.00m,
                    CategoryId = snacksCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://dpairgas.com.ua/wp-content/uploads/2014/01/чипсы.jpg",
                    StockQuantity = 110,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Кава в зернах Арабіка",
                    Description = "Кава натуральна смажена в зернах середнього обсмаження, 250 г.",
                    Price = 180.00m,
                    CategoryId = hotDrinksCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1447933601403-0c6688de566e?w=500&q=80",
                    StockQuantity = 40,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Сік Апельсиновий",
                    Description = "100% натуральний відновлений сік із м'якоттю, 1 л.",
                    Price = 62.00m,
                    CategoryId = coldDrinksCat.Id,
                    UnitId = pcsUnit.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=500&q=80",
                    StockQuantity = 85,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
            context.SaveChanges();
        }
    }
}