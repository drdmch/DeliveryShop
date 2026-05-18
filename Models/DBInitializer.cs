using DeliveryShopApp.Models;

namespace DeliveryShopApp.Data;

public static class DbInitializer
{
    public static void Seed(DataContext context)
    {
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
                new Category { Name = "М'ясо, птиця та ковбаси" },
                new Category { Name = "Риба та морепродукти" },
                new Category { Name = "Яйця та молочні продукти" },
                new Category { Name = "Пекарня та хліб" },
                new Category { Name = "Бакалія" },
                new Category { Name = "Соуси та спеції" },
                new Category { Name = "Консерви та паштети" },
                new Category { Name = "Солодощі та десерти" },
                new Category { Name = "Чіпси, сухарики та снеки" },
                new Category { Name = "Гарячі напої (чай, кава)" },
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
            var meatCat = context.Categories.First(c => c.Name == "М'ясо, птиця та ковбаси");
            var dairyCat = context.Categories.First(c => c.Name == "Яйця та молочні продукти");
            var bakeryCat = context.Categories.First(c => c.Name == "Пекарня та хліб");
            var sweetsCat = context.Categories.First(c => c.Name == "Солодощі та десерти");

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
                }
            );
            context.SaveChanges();
        }
    }
}