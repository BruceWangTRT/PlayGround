using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryA.Model;
using FluentNHibernate.Mapping;

namespace ClassLibraryA.Mapping
{
    public class UserAirlinePointsProgramMap : ClassMap<UserAirlinePointsProgram>
    {
        public UserAirlinePointsProgramMap()
        {
            Table("[Users].[UserAirlinePointsProgram]");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.ProgramNumber);
            Map(x => x.UserProfileId);
            References(x => x.Program, "ProgramId");
            //References(x => x.UserProfileId, "UserProfileID").Class<UserProfile>();
        }
    }
}
