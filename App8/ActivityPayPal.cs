using System;
using Android.App;
using Android.Content;
using Android.OS;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;

namespace App8
{
    [Activity(Label = "Product Descriptions", Theme = "@style/CustomActionBarTheme")]
    public class ActivityPayPal : Activity
    {

        private string itemPrice;
        protected override async void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            //SetContentView(Resource.Layout.layoutCart);
            itemPrice = Intent.GetStringExtra("itemPrice").ToString();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossPayPalManager.Init(new PayPalConfiguration(
                               PayPalEnvironment.NoNetwork,
                               "dantevfenris-facilitator@hotmail.com"
                               )
            {
                //If you want to accept credit cards
                AcceptCreditCards = true,
                //Your business name
                MerchantName = "Test Store",
                //Your privacy policy Url
                MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
                //Your user agreement Url
                MerchantUserAgreementUri = "https://www.example.com/legal",

                // OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
                ShippingAddressOption = ShippingAddressOption.Both,

                // OPTIONAL - Language: Default languege for PayPal Plug-In
                Language = "en",

                // OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
                PhoneCountryCode = "52",
            }
                       );//CAD
            var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Test Product", Convert.ToDecimal(itemPrice), "USD"), new Decimal(0));
            if (result.Status == PayPalStatus.Cancelled)
            {
                Console.WriteLine("Cancelled");
            }
            else if (result.Status == PayPalStatus.Error)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            else if (result.Status == PayPalStatus.Successful)
            {
                Console.WriteLine(result.ServerResponse.Response.Id);
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PayPalManagerImplementation.Manager.Destroy();
        }

    }
}

