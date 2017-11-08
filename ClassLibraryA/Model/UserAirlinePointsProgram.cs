using System;

namespace ClassLibraryA.Model
{
    public class UserAirlinePointsProgram
    {
        public virtual long Id { get; set; }
        public virtual Guid UserProfileId { get; set; }
        public virtual AirlinePointsProgram Program { get; set; }
        public virtual string ProgramNumber { get; set; }
    }
}
