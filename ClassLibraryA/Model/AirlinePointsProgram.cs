namespace ClassLibraryA.Model
{
    public class AirlinePointsProgram
    {
        public virtual long Id { get; set; }
        public virtual string ProgramName { get; set; }
        public virtual string AirlineIATACode { get; set; }
        public virtual string Country { get; set; }
    }
}
