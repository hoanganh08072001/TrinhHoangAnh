using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGV.Utils
{
    public class PaymentStripe
    {
        public Session paymentOnline(string name,int quantity)
        {
            StripeConfiguration.ApiKey = Constants.Constants.API_KEY;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = name,
                        Description =Constants.Constants.DESCRIPTION_BOOKING,
                        Amount = Constants.Constants.PRICE_TICKET,
                        Currency ="VND",
                        Quantity = quantity,
                        Images = new List<string>
                        {
                            HttpUtility.UrlPathEncode(Constants.Constants.IMAGE_URL)
                        }
                    },
                },
                SuccessUrl = Constants.Constants.SUCCESS_URL,
                CancelUrl = Constants.Constants.CANCEL_URL,
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                         {"Order_id","1234" },
                         {"sdsd","hello" },
                    }

                }
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }
}