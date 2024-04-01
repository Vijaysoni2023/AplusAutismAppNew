namespace aplusautism.Common.Enums
{
    public enum SPROC_Names
    {
        sp_getusers = 1,
        sp_getGlobalCode = 2,
        sp_getLesson = 3,
        sp_getLanguages = 4,
        USP_GetGlobalDataByCategoryIds = 5,
        USP_UpdateLessonSetup,
        sp_getLesson_forclientbycategory,
        SpUpdateStripeAPIResponse,
        USP_GetLatestSubscriptionData,
        USP_GetClientDetails,
        USP_GetPaymentDetails,
        USP_GetClientStatusList, 
        SpUpdateClientStatus,
        SpSaveContactLog,
        SpSaveDeviceTracking,
        sp_getusersPreferedLanguage,
        sp_GetUserDeviceIdCount,
        USP_GetPlanName,
        USP_GetClientDetail,
        USP_GetUserPaymentStatus
    }

    public enum AddressType
    {
        BillingAddress=1

    }

    public enum Status
    {
        Active = 1,
        InActive = 2,
        Trial = 3,
        TrialEndedNotSubscribed = 4,
        Suspend = 5

    }

    public enum ContactTypes
    {
        ContactType=1,
        ContactTypeTopic=2,
        ContactMethod=3,
        Suspend=4,
        Active=5,
        Trial=6 ,
        Contactus=7 ,
        Forgetpassword=8 ,
    }
}
