using System.Data.Entity;

namespace DevAgenda.Domain.Models
{
  public class DevAgendaCtx : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Action> Actions { get; set; }
    public DbSet<EventsFilter> EventsFilters { get; set; }
    public DbSet<SearchedArea> SearchedAreas { get; set; }
    public DbSet<LocationsQueryResult> LocationsQueryResults { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder
        .Entity<SearchedArea>()
        .Property(sa => sa.NorthEastLatitude)
        .HasPrecision(18, 10);

      modelBuilder
        .Entity<SearchedArea>()
        .Property(sa => sa.NorthEastLongitude)
        .HasPrecision(18, 10);

      modelBuilder
        .Entity<SearchedArea>()
        .Property(sa => sa.SouthWestLatitude)
        .HasPrecision(18, 10);

      modelBuilder
        .Entity<SearchedArea>()
        .Property(sa => sa.SouthWestLongitude)
        .HasPrecision(18, 10);

      modelBuilder
        .Entity<Event>()
        .HasMany(e => e.Tags)
        .WithMany(t => t.Events)
        .Map(et =>
               {
                 et.MapLeftKey("EventId");
                 et.MapRightKey("TagId");
                 et.ToTable("EventTags");
               });

      modelBuilder
        .Entity<Event>()
        .HasMany(e => e.Attendees)
        .WithMany(u => u.Events)
        .Map(r =>
               {
                 r.MapLeftKey("EventId");
                 r.MapRightKey("UserId");
                 r.ToTable("RSVPs");
               });


      modelBuilder
        .Entity<Location>()
        .Property(l => l.Longitude)
        .HasPrecision(18, 10);

      modelBuilder
        .Entity<Location>()
        .Property(l => l.Latitude)
        .HasPrecision(18, 10);


      //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
    }
  }
}