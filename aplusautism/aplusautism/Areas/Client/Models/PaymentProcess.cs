using Stripe;
using Nancy.Json;
using aplusautism.Data.Models;
using aplusautism.Bal.DTO;
using Stripe.FinancialConnections;

namespace aplusautism.Areas.Client.Payment
{
    public class PaymentProcess
    {

        public static async Task<dynamic> PayAsync(PaymentsDTO payModel)
        {
            try
            {
                //  This Key is used for Live Payment
                //StripeConfiguration.ApiKey = "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";

                // This Key is used for Test Mode payment
                StripeConfiguration.ApiKey = "sk_test_51KKCylCTglflRx9aDVWTTkWeVxnMOp3IJF4Zw8XV1Y7SplOYKrqbSWBacJ0xpktj9R7xxsGi31AYZtXCGF99cQia00d2jdOzLL";
                decimal amount = Convert.ToDecimal(payModel.AmountPaid);
                decimal SavedAmount = amount * 100;
                var options = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = payModel.CardNumder,
                        ExpMonth = payModel.Month.ToString(),
                        ExpYear = payModel.Year.ToString(),
                        Cvc = payModel.CVC,

                    },


                };

                //var serviceToken = new TokenService();
                //Token stripeToken = await serviceToken.CreateAsync(options);
                //var optionscusDel = new CustomerDeleteOptions
                //{

                //};
                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(options);

                var chargeOptions = new ChargeCreateOptions
                {
                    //    Amount =Convert.ToInt64(SavedAmount),
                    //   // Amount =600,
                    //    Currency = "inr",
                    //    SetupFutureUsage = "off_session",
                    //    PaymentMethodTypes = new List<string> { "card" },
                    //    StatementDescriptor = "Custom descriptor",
                    //Customer = "Amitr",
                    // Amount = payModel.Amount,
                    Amount = Convert.ToInt64(SavedAmount),
                    Currency = "usd",
                    Description = payModel.Description,
                    Source = stripeToken.Id

                };
                var chargeService = new ChargeService();
                Charge charge = await chargeService.CreateAsync(chargeOptions);

                if (charge.Paid)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            

               // var service = new PaymentIntentService();
               // var paymentIntent = service.Create(Paymentoptions);
               // var clientSecert = paymentIntent.Id.ToString();
               // var Confirmoptions = new PaymentIntentConfirmOptions
               // {
               //     PaymentMethod = "pm_card_visa",
               // };


            // var Confirmservice = new PaymentIntentService();
            // Confirmservice.Confirm(
            //   clientSecert,
            //   Confirmoptions);


            // var SetupIntentoptions = new SetupIntentCreateOptions
            // {
            //     PaymentMethodTypes = new List<string>
            //       {
            //         "card",
            //       },
            // };
            // var SetupIntentservice = new SetupIntentService();
            //var ServiceIntent =  SetupIntentservice.Create(SetupIntentoptions);



            // SetupIntentservice.Get(ServiceIntent.Id.ToString());



            //var setupIntentUpdateoptions = new SetupIntentUpdateOptions
            //{
            //    Metadata = new Dictionary<string, string>
            //    {
            //    { "user_id", "cus_NJ70PfypdKYurs" },
            //    },
            //};
            // var setupIntentUpdateservice = new SetupIntentService();
            //var setupIntentResponse =  setupIntentUpdateservice.Update(
            //   ServiceIntent.Id.ToString(),
            //   setupIntentUpdateoptions);

            // var setupIntentConfirmoptions = new SetupIntentConfirmOptions
            // {
            //     PaymentMethod = "pm_card_visa",
            // };
            // var SetupIntentConfirmservice = new SetupIntentService();
            //var confirmResponse = SetupIntentConfirmservice.Confirm(
            //  ServiceIntent.Id.ToString(),
            //   setupIntentConfirmoptions);

            // return paymentIntent.ToString();
            }
            catch (ApplicationException ex)
            {
                return ex.Message;
            }
        }
    }
}
