using ClassLibraryA.Model;
using FluentNHibernate.Mapping;

namespace ClassLibraryA.Mapping
{
    public class AirlinePointsProgramMap : ClassMap<AirlinePointsProgram>
    {
        public AirlinePointsProgramMap()
        {
            Table("[MasterData].[AirlinePointsProgram]");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.ProgramName);
            Map(x => x.AirlineIATACode);
            Map(x => x.Country);
        }
    }
}
