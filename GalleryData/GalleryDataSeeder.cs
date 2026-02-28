using GalleryData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GalleryData
{
    public class GalleryDataSeeder
    {
        private readonly AppDbContext _db;
        public GalleryDataSeeder(AppDbContext db)
        {
            this._db = db;
        }
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            if (!context.Authors.Any())
            {
                var authors = new List<Author>
            {
                new Author { Name = "TRAN thị huyền trân" },
                new Author { Name = "Jean Dupont" },
                new Author { Name = "Anna Müller" },
                new Author { Name = "Liu Wei" },
                new Author { Name = "Carlos Silva" },
                new Author { Name = "Fatima Zahra" },
                new Author { Name = "John Smith" }
            };
                context.Authors.AddRange(authors);
                await context.SaveChangesAsync();

                var defaultSizes = new List<FrameSize>
            {
                new() { Height = 40, Width = 50, Price = 45 },
                new() { Height = 50, Width = 50, Price = 55 },
                new() { Height = 50, Width = 70, Price = 65 },
                new() { Height = 60, Width = 60, Price = 70 },
                new() { Height = 60, Width = 70, Price = 80 },
                new() { Height = 70, Width = 60, Price = 85 },
                new() { Height = 60, Width = 80, Price = 95 }
            };

                var frames = new List<Frame>
            {
                new Frame { Material = "Wood", Color = "Black" ,ImageUrl = "/images/frames/frame-woodBlack.png",Sizes = defaultSizes.Select(s => new FrameSize
        {
            Height = s.Height,
            Width = s.Width,
            Price = s.Price
        }).ToList()},
                new Frame { Material = "Metal", Color = "Silver",ImageUrl = "/images/frames/frame-metalSilver.png",Sizes = defaultSizes.Select(s => new FrameSize
        { Height = s.Height,Width = s.Width, Price = s.Price + 10 }).ToList() },
                new Frame { Material = "Plastic", Color = "White" ,ImageUrl = "/images/frames/frame-plasticWhite.png",Sizes = defaultSizes.Select(s => new FrameSize
        {
            Height = s.Height,
            Width = s.Width,
            Price = s.Price - 10 
        }).ToList()},
                new Frame { Material = "Wood", Color = "Brown" ,ImageUrl = "/images/frames/frame-woodBrown.png",  Sizes = defaultSizes.Select(s => new FrameSize
        {
            Height = s.Height,
            Width = s.Width,
            Price = s.Price + 5
        }).ToList()
    },
                new Frame { Material = "Metal", Color = "Gold",ImageUrl = "/images/frames/frame-metalGold.png" ,Sizes = defaultSizes.Select(s => new FrameSize
        {
            Height = s.Height,
            Width = s.Width,
            Price = s.Price + 20 
        }).ToList()}
            };           

                context.Frames.AddRange(frames);
                await context.SaveChangesAsync();

                var artworks = new List<Artwork>
            {
                new Artwork { Title = "Les Ailes des Anges",Price = 1000 + 1 * 50, Technique = "Acrylic", Signature = "Hand signed", CountryOfOrigin = "Vietnam", Period = "Modern", Year = 2025, SoldWithFrame = false, Condition = "Excellent", Height = 50, Width = 50, Style = "Contemporary", AuthorId = 1,ImageUrl = "/images/artworks/LesAilesdesAnges.jpg",},
                new Artwork { Title = "Le Soleil Couchant",Price = 1000 + 2 * 50, Technique = "Oil", Signature = "Signed", CountryOfOrigin = "France", Period = "Impressionism", Year = 2023, SoldWithFrame = true, Condition = "Very Good", Height = 70, Width = 60, Style = "Impressionism", AuthorId = 2, ImageUrl = "/images/artworks/LeSoleilCouchant.jpg"},
                new Artwork { Title = "Night Sky",Price = 1000 + 3 * 50, Technique = "Watercolor", Signature = "Signed", CountryOfOrigin = "Germany", Period = "Contemporary", Year = 2022, SoldWithFrame = false, Condition = "Good", Height = 40, Width = 50, Style = "Abstract", AuthorId = 3 ,ImageUrl = "/images/artworks/NightSky.png"},
                new Artwork { Title = "Ocean Waves",Price = 1000 + 4 * 50, Technique = "Acrylic", Signature = "Hand signed", CountryOfOrigin = "China", Period = "Realism", Year = 2021, SoldWithFrame = true, Condition = "Excellent", Height = 60, Width = 80, Style = "Realism", AuthorId = 4,ImageUrl = "/images/artworks/OceanWaves.png" },
                new Artwork { Title = "Mountain View", Price = 1000 + 5 * 50, Technique = "Oil", Signature = "Signed", CountryOfOrigin = "Brazil", Period = "Landscape", Year = 2020, SoldWithFrame = false, Condition = "Very Good", Height = 50, Width = 70, Style = "Landscape", AuthorId = 5 , ImageUrl = "/images/artworks/MountainView.png"},
                new Artwork { Title = "City Lights",Price = 1000 + 6 * 50, Technique = "Acrylic", Signature = "Hand signed", CountryOfOrigin = "Morocco", Period = "Modern", Year = 2024, SoldWithFrame = true, Condition = "Excellent", Height = 60, Width = 60, Style = "Modern", AuthorId = 6 ,ImageUrl = "/images/artworks/CityLights.png"},
                new Artwork { Title = "Floral Dreams",Price = 1000 + 7 * 50, Technique = "Oil", Signature = "Signed", CountryOfOrigin = "USA", Period = "Still Life", Year = 2023, SoldWithFrame = true, Condition = "Good", Height = 40, Width = 50, Style = "Still Life", AuthorId = 7 , ImageUrl = "/images/artworks/FloralDreams.png"},
                new Artwork { Title = "Abstract Lines",Price = 1000 + 8 * 50, Technique = "Acrylic", Signature = "Hand signed", CountryOfOrigin = "Vietnam", Period = "Abstract", Year = 2025, SoldWithFrame = false, Condition = "Excellent", Height = 50, Width = 50, Style = "Abstract", AuthorId = 1 ,ImageUrl = "/images/artworks/AbstractLines.png"},
                new Artwork { Title = "Sunrise Glow",Price = 1000 + 9 * 50, Technique = "Oil", Signature = "Signed", CountryOfOrigin = "France", Period = "Landscape", Year = 2024, SoldWithFrame = true, Condition = "Very Good", Height = 60, Width = 70, Style = "Landscape", AuthorId = 2,ImageUrl = "/images/artworks/SunriseGlow.png" },
                new Artwork { Title = "Winter Forest",Price = 1000 + 10 * 50, Technique = "Watercolor", Signature = "Signed", CountryOfOrigin = "Germany", Period = "Realism", Year = 2022, SoldWithFrame = false, Condition = "Good", Height = 50, Width = 50, Style = "Realism", AuthorId = 3 , ImageUrl = "/images/artworks/WinterForest.png"},
                new Artwork { Title = "Golden Path",Price = 1000 + 11 * 50, Technique = "Acrylic", Signature = "Hand signed", CountryOfOrigin = "China", Period = "Contemporary", Year = 2021, SoldWithFrame = true, Condition = "Excellent", Height = 60, Width = 80, Style = "Contemporary", AuthorId = 4 ,ImageUrl = "/images/artworks/GoldenPath.png"},
                new Artwork { Title = "Red Horizon",Price = 1000 + 12 * 50, Technique = "Oil", Signature = "Signed", CountryOfOrigin = "Brazil", Period = "Modern", Year = 2020, SoldWithFrame = false, Condition = "Very Good", Height = 50, Width = 70, Style = "Modern", AuthorId = 5 , ImageUrl = "/images/artworks/RedHorizon.png"},
                new Artwork { Title = "Silent River",Price = 1000 + 13 * 50, Technique = "Watercolor", Signature = "Signed", CountryOfOrigin = "Morocco", Period = "Landscape", Year = 2023, SoldWithFrame = true, Condition = "Good", Height = 40, Width = 50, Style = "Landscape", AuthorId = 6 ,ImageUrl = "/images/artworks/SilentRiver.png"},
                new Artwork { Title = "Dreamscape", Price = 1000 + 14 * 50, Technique = "Acrylic", Signature = "Hand signed", CountryOfOrigin = "USA", Period = "Abstract", Year = 2025, SoldWithFrame = false, Condition = "Excellent", Height = 50, Width = 50, Style = "Abstract", AuthorId = 7 ,ImageUrl = "/images/artworks/Dreamscapee.png"},
                new Artwork { Title = "Mystic Mountains",Price = 1000 + 15 * 50, Technique = "Oil", Signature = "Signed", CountryOfOrigin = "Vietnam", Period = "Realism", Year = 2024, SoldWithFrame = true, Condition = "Very Good", Height = 60, Width = 70, Style = "Realism", AuthorId = 1 ,ImageUrl = "/images/artworks/MysticMountains.png"},
                new Artwork { Title = "Calm Sea",Price = 1000 + 16 * 50, Technique = "Watercolor", Signature = "Signed", CountryOfOrigin = "France", Period = "Landscape", Year = 2023, SoldWithFrame = false, Condition = "Good", Height = 50, Width = 50, Style = "Landscape", AuthorId = 2 ,ImageUrl = "/images/artworks/CalmSea.png"}
};

                context.Artworks.AddRange(artworks);
                await context.SaveChangesAsync();

                var user = await context.Users.FirstOrDefaultAsync();
                if (user != null)
                {
                    foreach (var art in artworks.Take(10))
                    {
                        context.Bids.Add(new Bid
                        {
                            ArtworkId = art.Id,
                            UserId = user.Id,
                            Price = 1000 + art.Id * 50,
                            Date = DateTime.Now
                        });

                        context.UserLikedArtworks.Add(new UserLikedArtwork
                        {
                            ArtworkId = art.Id,
                            UserId = user.Id
                        });
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
