using GalleryData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryData
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<FrameSize> FrameSizes { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<UserLikedArtwork> UserLikedArtworks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLikedArtwork>()
                .HasKey(x => new { x.UserId, x.ArtworkId });

            builder.Entity<UserLikedArtwork>()
                .HasOne(x => x.User)
                .WithMany(u => u.UserLikedArtworks)
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserLikedArtwork>()
                .HasOne(x => x.Artwork)
                .WithMany(a => a.UserLikedArtworks)
                .HasForeignKey(x => x.ArtworkId);
        }
    }
}
