using System.Collections.Generic;

namespace PlayGround.Model
{
    public class DepartureAirport
    {
        public string LocationCode { get; set; }
    }

    public class ArrivalAirport
    {
        public string LocationCode { get; set; }
    }

    public class MarketingAirline
    {
        public string Code { get; set; }
        public string CompanyShortName { get; set; }
    }

    public class OperatingAirline
    {
        public string Code { get; set; }
        public string CompanyShortName { get; set; }
    }

    public class FlightSegment
    {
        public DepartureAirport DepartureAirport { get; set; }
        public string FlightNumber { get; set; }
        public ArrivalAirport ArrivalAirport { get; set; }
        public string ArrivalDateTime { get; set; }
        public string JourneyDuration { get; set; }
        public MarketingAirline MarketingAirline { get; set; }
        public string DepartureDateTime { get; set; }
        public OperatingAirline OperatingAirline { get; set; }
    }

    public class OriginDestinationOption
    {
        public List<FlightSegment> FlightSegment { get; set; }
    }

    public class OriginDestinationOptions
    {
        public List<OriginDestinationOption> OriginDestinationOption { get; set; }
    }

    public class AirItinerary
    {
        public OriginDestinationOptions OriginDestinationOptions { get; set; }
        public string DirectionInd { get; set; }
    }

    public class AirItineraryEncode
    {
        public string enc { get; set; }
    }

    public class Fee
    {
        public string Amount { get; set; }
        public string FeeCode { get; set; }
    }

    public class Fees
    {
        public List<Fee> Fee { get; set; }
    }

    public class BaseFare
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string DecimalPlaces { get; set; }
    }

    public class Tax
    {
        public string Amount { get; set; }
    }

    public class Taxes
    {
        public Tax Tax { get; set; }
    }

    public class TotalFare
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string DecimalPlaces { get; set; }
    }

    public class ItinTotalFare
    {
        public Fees Fees { get; set; }
        public BaseFare BaseFare { get; set; }
        public Taxes Taxes { get; set; }
        public TotalFare TotalFare { get; set; }
    }

    public class BaseFare2
    {
        public double Amount { get; set; }
    }

    public class Tax2
    {
        public string Amount { get; set; }
    }

    public class SubTax
    {
        public string Amount { get; set; }
        public string Name { get; set; }
    }

    public class BreakDown
    {
        public List<SubTax> SubTax { get; set; }
    }

    public class Fee2
    {
        public string Amount { get; set; }
        public string FeeCode { get; set; }
    }

    public class Fees2
    {
        public List<Fee2> Fee { get; set; }
    }

    public class Taxes2
    {
        public Tax2 Tax { get; set; }
        public BreakDown BreakDown { get; set; }
        public Fees2 Fees { get; set; }
    }

    public class TotalFare2
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string DecimalPlaces { get; set; }
    }

    public class PassengerFare
    {
        public BaseFare2 BaseFare { get; set; }
        public Taxes2 Taxes { get; set; }
        public TotalFare2 TotalFare { get; set; }
    }

    public class PassengerTypeQuantity
    {
        public string Code { get; set; }
        public string Quantity { get; set; }
    }

    public class PTCFareBreakdown
    {
        public PassengerFare PassengerFare { get; set; }
        public PassengerTypeQuantity PassengerTypeQuantity { get; set; }
    }

    public class PTCFareBreakdowns
    {
        public List<PTCFareBreakdown> PTC_FareBreakdown { get; set; }
    }

    public class TPAExtensions
    {
        public string ProviderCode { get; set; }
    }

    public class FareInfo
    {
        public TPAExtensions TPA_Extensions { get; set; }
    }

    public class FareInfos
    {
        public FareInfo FareInfo { get; set; }
    }

    public class AirItineraryPricingInfo
    {
        public string QuoteID { get; set; }
        public ItinTotalFare ItinTotalFare { get; set; }
        public PTCFareBreakdowns PTC_FareBreakdowns { get; set; }
        public FareInfos FareInfos { get; set; }
    }

    public class PricedItinerary
    {
        public AirItinerary AirItinerary { get; set; }
        public int SequenceNumber { get; set; }
        public AirItineraryEncode AirItineraryEncode { get; set; }
        public AirItineraryPricingInfo AirItineraryPricingInfo { get; set; }
    }

    public class OTAAirLowFareSearchRS
    {
        public string EchoToken { get; set; }
        public string Target { get; set; }
        public string Version { get; set; }
        public List<PricedItinerary> PricedItineraries { get; set; }
    }

    public class SearchOneWayResponse
    {
        public OTAAirLowFareSearchRS OTA_AirLowFareSearchRS { get; set; }
    }
}

