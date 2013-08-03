using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DevAgenda.WebApp.Models
{
  public class DevAgendaCtx : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<EventDuration> EventDurations { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Event>()
        .Property(e => e.Latitude)
        .HasPrecision(18, 10);

      modelBuilder.Entity<Event>()
        .Property(e => e.Longitude)
        .HasPrecision(18, 10);

      modelBuilder.Entity<Event>()
        .HasMany(e => e.Tags)
        .WithMany(t => t.Events)
        .Map(et =>
               {
                 et.MapLeftKey("EventId");
                 et.MapRightKey("TagId");
                 et.ToTable("EventTag");
               });

      modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
    }
  }
}