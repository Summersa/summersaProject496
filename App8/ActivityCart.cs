using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;

namespace App8
{
    [Activity(Label = "Product Descriptions", Theme = "@style/CustomActionBarTheme")]
    public class ActivityCart : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Cart> cartTable = MobileService.GetTable<Cart>();
        private ListView view;
        private String businessName, newCategoryName;
        private ActionBarHelper topbar;
        private ImageButton gAddCategory;
        private Button gBtnPurchase;
        private TextView gTxtTotalPrice, gtxtInCart;
        private RadioButton gdeliveryCheckbox;
        private Double finalPrice;
        private int check;
        //private ListView listviewCategory;
        List<Cart> items;
        Users user;
        public static readonly int PickImageId = 1000;
        
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutCart);
            view = FindViewById<ListView>(Resource.Id.viewCategories);
            gBtnPurchase = FindViewById<Button>(Resource.Id.btnPurchase);
            gtxtInCart = FindViewById<TextView>(Resource.Id.txtInCart);
            gdeliveryCheckbox = FindViewById<RadioButton>(Resource.Id.radioButton1);
            businessName = Intent.GetStringExtra("businessName").ToString();
            user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            ActionBarHelper.GetPicture(this, topbar.GetUser());

            IMobileServiceTableQuery<Cart> query = cartTable.Where(Cart => Cart.userId == user.Id);
            items = await query.ToListAsync(); 
            cartAdapter adapter = new cartAdapter(this, items, businessName);
            view.Adapter = adapter;
            gTxtTotalPrice = FindViewById<TextView>(Resource.Id.txtTotalPrice);
            finalPrice = 0;
            int count = 0;
            foreach (Cart item in items)
            {
                finalPrice += (Convert.ToDouble(item.Price) * Convert.ToDouble(item.Quantity));
                count++;
            }
            gtxtInCart.Text = "Items In Cart (" + count + ")";
            gTxtTotalPrice.Text = "CDN$ "+ finalPrice.ToString();
            gBtnPurchase.Click += GBtnPurchase_Click;
            gdeliveryCheckbox.Click += deliveryBox_Click;
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
        }

        private void deliveryBox_Click(object sender, EventArgs e)
        {
            LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.linearLayoutHide);
            if (check==0)
            {
                finalPrice += 100;
                gTxtTotalPrice.Text = "CDN$ " + finalPrice.ToString() + " Delivery Charge Included $100";
                
                RunOnUiThread(() => { layout.Visibility = ViewStates.Visible; });
                check = 1;


            }
            else
            {
                finalPrice -= 100;
                gTxtTotalPrice.Text = "CDN$ " + finalPrice.ToString();
                RunOnUiThread(() => { layout.Visibility = ViewStates.Gone; });
                gdeliveryCheckbox.Checked = false;
                check = 0;

            }
        }

        private async void GBtnPurchase_Click(object sender, EventArgs e)
        {
            var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Test Product", Convert.ToDecimal(finalPrice), "CAD"), new Decimal(0));
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
                IMobileServiceTableQuery<Cart> query = cartTable.Where(Cart => Cart.userId == user.Id);
                items = await query.ToListAsync();
                foreach(Cart item in items)
                    await cartTable.DeleteAsync(item);
                items = null;
                cartAdapter adapter = new cartAdapter(this, items, businessName);
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
            try { PayPalManagerImplementation.Manager.Destroy(); }
            catch { }
        }
    }
}