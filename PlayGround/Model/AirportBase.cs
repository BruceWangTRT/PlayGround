namespace PlayGround.Model
{
    public class AirportBase
    {
        public virtual int AirportCityMapID { get; set; }
        public virtual string AirportCode { get; set; }
        public virtual string AirportName { get; set; }
        public virtual string CityName { get; set; }
        public virtual string CityCode { get; set; }
        public virtual bool IsMetroCode { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual int SelectionOrder { get; set; }
        public virtual int SkipRow { get; set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
        public virtual bool DefaultDisplay { get; set; }
        public virtual bool HilightCountry { get; set; }
        public virtual string RegionCode { get; set; }
        public virtual string RegionName { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual string CategoryCountryCode { get; set; }
        public virtual string DestinationCategory { get; set; }
        public virtual string Landscape { get; set; }

    }
}
