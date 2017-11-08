using System.Collections.Generic;
using System.Dynamic;

namespace ClassLibraryA
{
    public static class MessageId
    {
        //public static Dictionary<string string> GetAllMessageId()
        //{
        //    var results = new Dictionary<string string>();
        //    results.Add();
        //    return results;
        //}


        public const string ErrorActivateUser = "ErrorActivateUser";//1)activate user
        public const string ErrorAirportNotFound = "ErrorAirportNotFound";//1)MasterDataCacheService GetNearestAirport //2)MasterDataDbService GetNearestAirport
        public const string ErrorBookedItineraryNotFound = "ErrorBookedItineraryNotFound";//1)ItineraryController UndoChanges GetBookedById
        public const string ErrorCancellingHotel = "ErrorCancellingHotel"; //1)2)BkHotelWebService MapCancellationResponse 3)Hotel EnsureHotelCancellationSuccessStatus 4)HotelChangeprocessor Cancel
        public const string ErrorCancellingPNR = "ErrorCancellingPnr";//1)FlightUpdateWebService CancelPnr -Should move
        public const string ErrorCannotCancelAlreadyCancelledFlight = "ErrorCannotCancelAlreadyCancelledFlight";// 1)ItineraryController CancelFlightBooking
        public const string ErrorCannotCancelAlreadyCancelledHotel = "ErrorCannotCancelAlreadyCancelledHotel";// 1)ItineraryController CancelHotelBooking
        public const string ErrorCannotCancelTestBooking = "ErrorCannotCancelTestBooking";// 1)ItineraryController CancelHotelBooking
        public const string ErrorCantRemovePreferredCardForPaidMembers = "ErrorCantRemovePreferredCardForPaidMembers";//1)CardController RemoveToken
        public const string ErrorCAPTCHAFailed = "ErrorCAPTCHAFailed";//renaming ErrorCAPTCHAValueNotFound 1) UserProfileController PasswordReset
        public const string ErrorCardDetailsNeededForPaidMembers = "ErrorCardDetailsNeededForPaidMembers";//1)2) UserProfileController UpdateUserType Post
        public const string ErrorCityStateCounrtyMatchNotFound = "ErrorCityStateCounrtyMatchNotFound";//1)2)3)4) UserProfileDbService UpdateUser UpdateUserAccountSettings UpdateAnonymousToRegularUser CreateProfileAndCommit
        public const string ErrorCompanionAlreadyAssociated = "ErrorCompanionAlreadyAssociated";//1) CompanionController AddCompanion
        public const string ErrorCompanionNotFound = "ErrorCompanionNotFound";//1) CompanionController Put 2) HotelController AddOccupants
        public const string ErrorCompanionRejectFailed = "ErrorCompanionRejectFailed";//renaming ErrorCompanionConfirmationFailed 1)CompnaionController Reject
        public const string ErrorContentUnavailable = "ErrorContentUnavailable";//1)Scheduler GetFlightAlternatives
        public const string ErrorCountryNotFound = "ErrorCountryNotFound";//1) HttpCountryFetcher GetCountry
        public const string ErrorEmptyHighlightId = "ErrorEmptyHighlightId";//1)ItineraryController GetResolvedAttributes
        public const string ErrorFeePaymentProcessing = "ErrorFeePaymentProcessing";//1)BookingHandler Book TODO: why use this?
        public const string ErrorFetchingRequestedUserType = "ErrorFetchingRequestedUserType";//1)UserProfileDbService UpdateUserType
        public const string ErrorFlightNotBooked = "ErrorFlightNotBooked";//1)2)3)4)BookingController GetFlightConfirmation GetFlightConfirmationPdf GetFlightETicket GetFlightETicketPdf
        public const string ErrorFlightNotFoundInBookedSolution = "ErrorFlightNotFoundInBookedSolution";//1)FlightController UpdateTicketInfo
        public const string ErrorFlightNotFoundInSolution = "ErrorFlightNotFoundInSolution";//1)2)3)4) BookingController GetFlightConfirmation GetFlightConfirmationPdf GetFlightETicket GetFlightETicketPdf //5) FlightController Put(SetSeat) 6) ItineraryController CancelFlightBooking
        public const string ErrorFlightTimeTooCloseToNow = "ErrorFlightTimeTooCloseToNow";//renaming ErrorInvalidFlightTime 1)CheckOutController ValidateFlightTime
        public const string ErrorFreeUsageMaxReached = "ErrorFreeUsageMaxReached";//1) SessionController AnonymousAuthentication
        public const string ErrorGetAlternativeHotels = "ErrorGetAlternativeHotels";//1)HotelController Get
        public const string ErrorGetCountries = "ErrorGetCountries";//renaming ErrorNoCountriesFound 1)HttpCountryFetcher GetAllCountries
        public const string ErrorGetCreditCardDetails = "ErrorGetCreditCardDetails";//1)CardWebService GetCardDetails
        public const string ErrorGetCreditCardSessionToken = "ErrorGetCreditCardSessionToken";//1)CardWebService GetSession
        public const string ErrorGetPromoHotels = "ErrorGetPromoHotels";//1)HotelController Get
        public const string ErrorGetProvinces = "ErrorGetProvinces";//renaming ErrorNoProvincesFound 1)HttpProvinceFetcher GetAllProvinces
        public const string ErrorGetServiceBroker = "ErrorGetServiceBroker";//renaming ErrorGetServiceBrokerError 1)NotificationDbService SendPushNotification
        public const string ErrorGroupUserAlreadyInvited = "ErrorGroupUserAlreadyInvited";//1)UserGroupDbService AddMember
        public const string ErrorGroupUserLimitReached = "ErrorGroupUserLimitReached";//1)UserGroupDbService AddMember
        public const string ErrorHotelAvailCheckFailed = "ErrorHotelAvailCheckFailed";//1)BkHotelWebService GetHotelAvailability 2))PlHotelWebService GetHotelAvailability 3)HotelController Put
        public const string ErrorHotelConflict = "ErrorHotelConflict";//1) HotelController AddOccupants
        public const string ErrorHotelNotBooked = "ErrorHotelNotBooked";//1)2)3)4)BookingController GetHotelConfirmation GetHotelConfirmationPdf
        public const string ErrorHotelNotFoundInSolution = "ErrorHotelNotFoundInSolution";//1)2)BookingController GetHotelConfirmation GetHotelConfirmationPdf  //3)4)5)6)HotelController AddOccupants(x4) 7）ItineraryController CancelHotelBooking
        public const string ErrorInactiveGroup = "ErrorInactiveGroup";//1)UserGroupDbService AddMember 2)UserGroupController AcceptGroupInvitation
        public const string ErrorInsecureNotAllowed = "ErrorInsecureNotAllowed";//1)AuthorizationMiddleware Invoke
        public const string ErrorInvalidAge = "ErrorInvalidAge";//1)UpdateUserProfileRequestValidator
        public const string ErrorInvalidAirlinePointsProgramId = "ErrorInvalidAirlinePointsProgramId";//1)AirlinePointsProgramController
        public const string ErrorInvalidAttributeId = "ErrorInvalidAttributeId";//1) TravelPreferenceController GetTravelPreferenceValues
        public const string ErrorInvalidCategoryCode = "ErrorInvalidCategoryCode";//1) TravelPreferenceController GetTravelPreferenceValues
        public const string ErrorInvalidConfirmationId = "ErrorInvalidConfirmationId";//1)2) CompanionController Confirm Reject
        public const string ErrorInvalidCorrelationQuoteId = "ErrorInvalidCorrelationQuoteId";//1)FlightDirectSell EnsureDirectSellCallSuccessStatus -Should move
        public const string ErrorInvalidCountryCode = "ErrorInvalidCountryCode";//1)StaticsController GetProvinceByCountryCode
        public const string ErrorInvalidCreditCard = "ErrorInvalidCreditCard";//1)BookingController PopulateCardDetails 2)3) UserProfileController Post(x2)
        public const string ErrorInvalidEchoToken = "ErrorInvalidEchoToken";//1)FlightDirectSell EnsureDirectSellCallSuccessStatus -Should move
        public const string ErrorInvalidEchoTokenAndQuoteId = "ErrorInvalidEchoTokenAndQuoteId";//1)FlightDirectSell EnsureDirectSellCallSuccessStatus -Should move
        public const string ErrorInvalidEmail = "ErrorInvalidEmail";//1)EmailRequiredValidator IsValid 2)CompanionController AddCompanion  //3)UserProfileController ResetPassword 4)AddUserProfileRequest 5)AddUsernameRequest
        public const string ErrorInvalidFlightId = "ErrorInvalidFlightId";//1)2)3)4)Scheduler GetFlgihtAlternatives SetAlternateFlight 5)ItineraryController CancelFlightBooking
        public const string ErrorInvalidFlightSelected = "ErrorInvalidFlightSelected";//1) FlightController Put
        public const string ErrorInvalidGroup = "ErrorInvalidGroup";//1)2)3)UserGroupDbService AddMember GetMembers DeclineMemberInvite //4)5)6)7)8) UserGroupController RenameGroup GetGroupMembers AddGroupMember RemoveGroupMember AcceptGroupInvitation
        public const string ErrorInvalidGroupInvite = "ErrorInvalidGroupInvite";//1)UserGroupDbService DeclineMemberInvite 2)UserGroupController AcceptGroupInvitation
        public const string ErrorInvalidGroupMember = "ErrorInvalidGroupMember";//1)UserGroupDbService RemoveMember
        public const string ErrorInvalidHotelId = "ErrorInvalidHotelId";//1)2)3)4) HotelController Put Search(x2) Delete 5) ItineraryController CancelHotelBooking
        public const string ErrorInvalidHotelRoomId = "ErrorInvalidHotelRoomId";//1)HotelRoomController Put
        public const string ErrorInvalidInputs = "ErrorInvalidInputs";//1)BookingHandler Book 2)EnforceModelStateValidAttribute OnActionExecuting
        public const string ErrorInvalidInvitation = "ErrorInvalidInvitation";//1)UserProfileController Post
        public const string ErrorInvalidItemToBook = "ErrorInvalidItemToBook";//1)BkHotelWebService BookHotel 2)BookingController Validate
        public const string ErrorInvalidItemToCheckout = "ErrorInvalidItemToCheckout";//1)CheckoutController Post(x2)
        public const string ErrorInvalidLength = "ErrorInvalidLength";//1)2)CompanionController Put Post 3)many places in validators
        public const string ErrorInvalidOccupant = "ErrorInvalidOccupant";//1)HotelController DeleteOccupants
        public const string ErrorInvalidPhoneNumber = "ErrorInvalidPhoneNumber";//many places in validators
        public const string ErrorInvalidPostalCode = "ErrorInvalidPostalCode";//PostalCodeValidator
        public const string ErrorInvalidProfileId = "ErrorInvalidProfileId"; //1)2)3)4) TravelPreferenceController GetTravelPreferenceProfileById SetTravelPreferenceProfiles GetUserProfilesWithCategoriesWithAttributesWithValues ValidateUserProfile
        public const string ErrorInvalidProviderFlightBookingId = "ErrorInvalidProviderFlightBookingId";//1)FlightController UpdateTicketInfo
        public const string ErrorInValidProvinceAsPerLegal = "ErrorInValidProvinceAsPerLegal";//1)CheckOutcontroller Post
        public const string ErrorInvalidQuery = "ErrorInvalidQuery";//1)HighlightController HighlightSearch
        public const string ErrorInvalidTitle = "ErrorInvalidTitle";//1)TitleValidator 2)PaxDetailValidator
        public const string ErrorInvalidUser = "ErrorInvalidUser"; //1)2)BookingController Validate(x2) 3)4)5)CompanionController Reject Put Confirm 6)7)FlightController Get Put  //8)9)HotelController Put DeleteHotel 10)HotelRoomController Put 11)12))13)14)ItineraryController Put Delete CancelFlightBooking UndoChanges
        public const string ErrorInvalidUserAirlinePointsProgramId = "ErrorInvalidUserAirlinePointsProgramId";//1)AirlinePointsProgramController
        public const string ErrorItineraryIdNullOrDefault = "ErrorItineraryIdNullOrDefault";//renaming ErrorInvalidSolutionId 1)BookingController 2)ItineraryController
        public const string ErrorItineraryNotFound = "ErrorItineraryNotFound";
        public const string ErrorMaxRoomOccupancyExceeded = "ErrorMaxRoomOccupancyExceeded";//1)HotelController SetOccupantsValue

        public const string ErrorMissingCreditCard = "ErrorMissingCreditCard";//1)BookingController Validate
        public const string ErrorMissingPaxDetail = "ErrorMissingPaxDetail";//1)BookingController Validate
        public const string ErrorMissingRequiredInput = "ErrorMissingRequiredInput";//1)EnforceModelStateValidAttribute OnActionExecuting 2)many places in validators
        public const string ErrorNoAlternativeFlightsFoundForPassenger = "ErrorNoAlternativeFlightsFoundForPassenger";//1)Scheduler GetFlightAlternatives
        public const string ErrorNoHotelsFoundInSolution = "ErrorNoHotelsFoundInSolution";//1)2)3)4) HotelController Get Put(x2) DeleteHotel 5)HotelRoomController Get
        public const string ErrorNoHotelsFoundNearby = "ErrorNoHotelsFoundNearby";//1) HotelController Search
        public const string ErrorNoItemRemainsInItinerary = "ErrorNoItemRemainsInItinerary";//1)HotelController DeleteHotel
        public const string ErrorNoItemSpecifiedForCheckout = "ErrorNoItemSpecifiedForCheckout";//1)BookingInputParametersValidator
        public const string ErrorNoProvincesFoundForCountry = "ErrorNoProvincesFoundForCountry";//1)HttpProvinceFetcher GetProvinceByCountryCode
        public const string ErrorNotGroupAdmin = "ErrorNotGroupAdmin";//1)2)3)4)5)6)7) UserGroupController AddGroup RenameGroup(x2) AddGroupMember(x2) RemoveGroupMember(x2)
        public const string ErrorNotStrongPassword = "ErrorNotStrongPassword";//1)AddUserProfileRequest
        public const string ErrorNoUserTravelPreferenceProfilesFound = "ErrorNoUserTravelPreferenceProfilesFound";//1)TravelPreferenceController GetTravelPreferenceProfiles
        public const string ErrorPassengerNotFoundInSolution = "ErrorPassengerNotFoundInSolution";//1)2)HotelController Get Put 3)HotelRoomController Get
        public const string ErrorPassengerNotRegisteredToRoom = "ErrorPassengerNotRegisteredToRoom";//1)HotelRoomController Put
        public const string ErrorPNRNotFound = "ErrorPNRNotFoundForFlightToBeCancelled";//1)FlightUpdateWebService CancelFlight -Should move
        public const string ErrorProcessBooking = "ErrorProcessBooking";//1)Process booking with MCS 2)3) FlightBookingProcessor Book
        public const string ErrorProcessFlightCancellation = "ErrorProcessFlightCancellation";//1)FlightChangeProcessor Cancel
        public const string ErrorProcessFlightExchange = "ErrorProcessFlightExchange";//1)FlightChangeProcessor Exchange
        public const string ErrorProcessHotelBooking = "ErrorProcessHotelBooking";//1)MapBookingResonse for hotel 2)3)HotelBookingProcessor Book
        public const string ErrorPropertyIsInvalidDate = "ErrorPropertyIsInvalidDate";//1)PaxDetails 2)ExpiryDateValidator
        public const string ErrorProvinceNotFound = "ErrorProvinceNotFound";//HttpProvinceFetcher GetProvinceByProvinceCode
        public const string ErrorRetrievingHotelBookingDetails = "ErrorRetrievingHotelBookingDetails";//1)PlHotelWebService GetHotelDetails
        public const string ErrorSearchFlight = "ErrorSearchFlight";//1)2)FlightSearchProcessor Search
        public const string ErrorSearchHotel = "ErrorSearchHotel";//1)ExHotelWebService SearchHotel 2)HotelSearchProcessor Search
        public const string ErrorTicketNotFound = "ErrorTicketNotFound";//1)2)BookingController GetFlightETicket GetFlightETicketPdf
        public const string ErrorTicketNotFoundOrHasBeenIssued = "ErrorTicketNotFoundOrHasBeenIssued";//1)FlightController UpdateTicketInfo
        public const string ErrorTwoUnderageUsersCannotBeAddedAsCompanion = "ErrorTwoUnderageUsersCannotBeAddedAsCompanion";//renaming ErrorInvalidDateOfBirth 1)CompanionController AddCompanion
        public const string ErrorUndoItinerary = "ErrorUndoItinerary";//1)ItineraryController CancelFlightBooking
        public const string ErrorUserAlreadyActivated = "ErrorUserAlreadyActivated";//1)InvitaionController Sendinvitation 2)3) UserProfileController Post ResendWelcomeEmail
        public const string ErrorUserAlreadyGroupMember = "ErrorUserAlreadyGroupMember";//1)UserGroupDbService CreateUserGroup
        public const string ErrorUserDeviceNotFound = "ErrorUserNotFound";//1)UserProfileDbService UpdateUserDevice  //2)3)4) PushNotificationsController EnablePushNotifications DisablePushNotifications UpdateBadgeValue 5)SessionController Logout
        public const string ErrorUserHasNoAddress = "ErrorUserHasNoAddress";//1)UserProfileController ConverAnonymousToRegularUser
        public const string ErrorUserNotAllowedForCheckout = "ErrorUserNotAllowedForCheckout";//1)CheckOutController Post
        public const string ErrorUserNotFound = "ErrorUserNotFound";//1)SessionTokenDbService AuthenticateUser  //2)3)4)UserProfileDbService ChangePassword UpdateAvatarUrl UpdateUserType 5)6)UserProfileController ResendWelcomeEmail ResetPassword
        public const string ErrorUserPasswordMismatch = "ErrorUserPasswordMismatch";//1)SessionTokenDbService AuthenticateUser 2) UserProfildDbService ChangePassword
        public const string ErrorUserProfileNotCreated = "ErrorUserProfileNotCreated";//1)SessionController AnonymousAuthentication 2)UserProfileController Post
        public const string ErrorUserUnderAgeNotAllowed = "ErrorUserUnderAgeNotAllowed";//1)AgeValidator
        public const string ErrorUserVerificationRequired = "ErrorUserVerificationRequired";//1)SessionTokenDbService AuthenticateUser
        public const string ErrorVoidingTicket = "ErrorVoidingTheTicket";//1)2)FlightUpdateWebService VoidBsp VoidTicket -Should move
        public const string ErrorWithDirectSell = "ErrorWithDirectSell";//1)FlightBookingProcessor book
        public const string FlightTicketReceived = "FlightTicketReceived";//1)NotificationDbService SendNotification TODO: why use this?

        public const string InvalidUserTypeSetupContactHGB = "InvalidUserTypeSetupContactHGB";//1)CheckOutController Post 2)ItineraryController CalculateAndApplyCMFee

        public const string PaymentAuthConsentNotGiven = "PaymentAuthConsentNotGiven";//1)2)BookingHandler Validate(x2)


        public const string TravelCompanionRequestReceived = "TravelCompanionRequestReceived";
        public const string TravelCompanionRequestAccepted = "TravelCompanionRequestAccepted";
    }
}