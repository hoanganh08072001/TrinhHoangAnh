using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Model
{
    public partial class MyDB : DbContext
    {
        public MyDB()
            : base("name=MyDB")
        {
        }

        public virtual DbSet<booking> bookings { get; set; }
        public virtual DbSet<category_film> category_film { get; set; }
        public virtual DbSet<category_post> category_post { get; set; }
        public virtual DbSet<film> films { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<rating> ratings { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<room> rooms { get; set; }
        public virtual DbSet<schedule> schedules { get; set; }
        public virtual DbSet<seat> seats { get; set; }
        public virtual DbSet<showtime> showtimes { get; set; }
        public virtual DbSet<usercgv> usercgvs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<category_film>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<category_film>()
                .HasMany(e => e.films)
                .WithRequired(e => e.category_film)
                .HasForeignKey(e => e.id_cfilm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<category_post>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<category_post>()
                .HasMany(e => e.posts)
                .WithRequired(e => e.category_post)
                .HasForeignKey(e => e.id_cpost)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<film>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<film>()
                .Property(e => e.director)
                .IsUnicode(false);

            modelBuilder.Entity<film>()
                .Property(e => e.actor)
                .IsUnicode(false);

            modelBuilder.Entity<film>()
                .Property(e => e.duration);
             

            modelBuilder.Entity<film>()
                .Property(e => e.film_name)
                .IsUnicode(false);

            modelBuilder.Entity<film>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<film>()
                .Property(e => e.trailer)
                .IsUnicode(false);

            modelBuilder.Entity<film>()
                .HasMany(e => e.bookings)
                .WithRequired(e => e.film)
                .HasForeignKey(e => e.film_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<film>()
                .HasMany(e => e.ratings)
                .WithOptional(e => e.film)
                .HasForeignKey(e => e.film_id);

            modelBuilder.Entity<film>()
                .HasMany(e => e.schedules)
                .WithOptional(e => e.film)
                .HasForeignKey(e => e.film_id);

            modelBuilder.Entity<post>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<post>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<post>()
                .Property(e => e.image)
                .IsUnicode(false);

            modelBuilder.Entity<rating>()
                .Property(e => e.rate)
                .IsUnicode(false);

            modelBuilder.Entity<rating>()
                .Property(e => e.name_user)
                .IsUnicode(false);

            modelBuilder.Entity<role>()
                .Property(e => e.role_name)
                .IsUnicode(false);

            modelBuilder.Entity<role>()
                .HasMany(e => e.usercgvs)
                .WithOptional(e => e.role)
                .HasForeignKey(e => e.role_id);

            modelBuilder.Entity<room>()
                .Property(e => e.room_name)
                .IsUnicode(false);

            modelBuilder.Entity<schedule>()
                .HasMany(e => e.bookings)
                .WithRequired(e => e.schedule)
                .HasForeignKey(e => e.schedule_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<schedule>()
                .HasMany(e => e.showtimes)
                .WithRequired(e => e.schedule)
                .HasForeignKey(e => e.schedule_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<seat>()
                .Property(e => e.seat_name)
                .IsUnicode(false);

            modelBuilder.Entity<showtime>()
                .HasMany(e => e.bookings)
                .WithRequired(e => e.showtime)
                .HasForeignKey(e => e.showtime_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usercgv>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<usercgv>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<usercgv>()
                .Property(e => e.phonenumber)
                .IsUnicode(false);

            modelBuilder.Entity<usercgv>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<usercgv>()
                .HasMany(e => e.bookings)
                .WithRequired(e => e.usercgv)
                .HasForeignKey(e => e.id_user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<usercgv>()
                .HasMany(e => e.ratings)
                .WithOptional(e => e.usercgv)
                .HasForeignKey(e => e.id_user);
        }
    }
}
