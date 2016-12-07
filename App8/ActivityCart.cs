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
using SendGrid;
using System.Net.Mail;
using System.Net;

namespace App8
{
    [Activity(Label = "Product Descriptions", Theme = "@style/CustomActionBarTheme")]
    public class ActivityCart : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Cart> cartTable = MobileService.GetTable<Cart>();
        private ListView view;
        private String businessName, newCategoryName, products, prices, quantities;
        private ActionBarHelper topbar;
        private ImageButton gAddCategory;
        private Button gBtnPurchase;
        private TextView gTxtTotalPrice, gtxtInCart;
        private EditText gTxtAddress, gTxtPhone;
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
            topbar.CartImg();
            topbar.textViewChange("Cart");
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
                MerchantName = "DanteVFenris Games",
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
            if (items.Count == 0)
                return;
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
                products = String.Join(".", items.Select(c => c.Name));
                //products = String.Join('.', item.Name,1);
                //products.Join(".", item.Name);
                prices = String.Join(".", items.Select(c => c.Price));
                quantities = String.Join(".", items.Select(c => c.Quantity));
                Console.WriteLine("PURCHASE ACCEPTED");
                Console.WriteLine(result.ServerResponse.Response.Id);
                IMobileServiceTableQuery<Cart> query = cartTable.Where(Cart => Cart.userId == user.Id);
                items = await query.ToListAsync();
                foreach (Cart item in items)
                {
                    await cartTable.DeleteAsync(item);

                }

                items.Clear();
                cartAdapter adapter = new cartAdapter(this, items, businessName);
                view.Adapter = adapter;
                gtxtInCart.Text = "Items In Cart (" + "0" + ")";
                gTxtTotalPrice.Text = "CDN$ " + "0.00";
                int count = 0;
                foreach (Cart item in items)
                {
                   
                   // prices += item.Price + ".";
                    //quantities += item.Quantity + ".";
                    finalPrice += (Convert.ToDouble(item.Price) * Convert.ToDouble(item.Quantity));
                    count++;
                }

                Console.WriteLine("ProductCheck{0}", products);
                if (gdeliveryCheckbox.Checked)
                {
                    Orders order = new Orders { userId = topbar.GetUser().Id, Phone = null, Address = null, Products = products, Prices = prices, Quantities = quantities, Completed = "1" };
                    await MobileService.GetTable<Orders>().InsertAsync(order);
                }
                else
                {
                    gTxtAddress = FindViewById<EditText>(Resource.Id.editAddress);
                    gTxtPhone = FindViewById<EditText>(Resource.Id.editPhone);
                    Orders order = new Orders { userId = topbar.GetUser().Id, Phone = gTxtPhone.Text, Address = gTxtAddress.Text, Products = products, Prices = prices, Quantities = quantities, Completed = "1" };
                    await MobileService.GetTable<Orders>().InsertAsync(order);
                }
                SendGridMessage myMessage = new SendGridMessage();
                myMessage.AddTo("summersa3@mailinator.com");
                myMessage.From = new MailAddress("anthonytyran@hotmail.com", "Anthony Summers");
                myMessage.Subject = "Verification Delivery App!!!";

 
                myMessage.Text = "Hello thank you for your purchase of the windsor plywood app. Please go to the order section of the app for full details.";
                // Create credentials, specifying your user name and password.
                var credentials = new NetworkCredential("azure_d994d44538e7b536651c95092059223d@azure.com", "Bleach332!!");

                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email, which returns an awaitable task.
                await transportWeb.DeliverAsync(myMessage);
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