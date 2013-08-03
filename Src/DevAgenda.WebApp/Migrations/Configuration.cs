using System.Linq;
using DevAgenda.Domain.Models;

namespace DevAgenda.WebApp.Migrations
{
  using System.Data.Entity.Migrations;

  internal sealed class Configuration : DbMigrationsConfiguration<DevAgendaCtx>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(DevAgendaCtx context)
    {
      context
        .Database
          .ExecuteSqlCommand(
            @"IF EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_LocationsQueryResults_Query') 
                DROP INDEX LocationsQueryResults.IX_LocationsQueryResults_Query
              CREATE INDEX IX_LocationsQueryResults_Query ON LocationsQueryResults (Query)");

      context
        .Database
          .ExecuteSqlCommand(
            @"IF EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Locations_Coords')
                DROP INDEX Locations.IX_Locations_Coords
              CREATE INDEX IX_Locations_Coords ON Locations (Latitude, Longitude)");

      if (!context.EventTypes.Any())
      {

        context.EventTypes.Add(new EventType { Name = "Conference" });
        context.EventTypes.Add(new EventType { Name = "UserGroup" });
        context.EventTypes.Add(new EventType { Name = "Workshop" });
        context.EventTypes.Add(new EventType { Name = "Other" });
      }

      base.Seed(context);
    }
  }
}
