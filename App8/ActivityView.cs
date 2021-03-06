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
namespace App8
{
    [Activity(Label = "Product Items", Theme = "@style/CustomActionBarTheme")]
    public class ActivityView : Activity
    {
        public static MobileServiceClient MobileService = new MobileServiceClient("https://mobileappdatabase666.azurewebsites.net");
        private IMobileServiceTable<Product> productTable = MobileService.GetTable<Product>();
        private ActionBarHelper topbar;
        private ImageButton gproductImage;
        private TextView gtxtDescription;
        private TextView gtxtPrice;
        private Product product;
        private EditText gQuantity;
        private Button gAddToCart, gProceedToCart;
        private string businessName;
        public static readonly int PickImageId = 1000;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layoutProductView);
            gproductImage = FindViewById<ImageButton>(Resource.Id.image);
            gtxtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            gtxtPrice = FindViewById<TextView>(Resource.Id.txtPrice);
            gAddToCart = FindViewById<Button>(Resource.Id.btnAddToCart);
            gProceedToCart = FindViewById<Button>(Resource.Id.btnProceedToCart);
            string txtproduct = Intent.GetStringExtra("product").ToString();
            IMobileServiceTableQuery<Product> query = productTable.Where(Product => Product.Name == txtproduct).Take(1);
            List<Product> items = await query.ToListAsync();
            product = items[0];
            Users user = JsonConvert.DeserializeObject<Users>(Intent.GetStringExtra("user"));
            businessName = Intent.GetStringExtra("businessName").ToString();
            gproductImage.SetImageBitmap(OnlinePicture.Stream(String.Format("https://storagedatabase666.blob.core.windows.net/{0}/product{1}", businessName.Replace(" ", "").ToLower(), product.Name.Replace(" ", ""))));
            gtxtDescription.Text = product.description;
            gtxtPrice.Text = product.price;
            topbar = new ActionBarHelper(this, user);
            topbar.Start();
            int subLength = 18;
            if (txtproduct.Length < 18) subLength = txtproduct.Length;
            topbar.textViewChange("C > P > " + txtproduct.Substring(0, subLength));
            ActionBarHelper.GetPicture(this, topbar.GetUser());
            gQuantity = FindViewById<EditText>(Resource.Id.txtQuantity);
            gAddToCart.Click += Click_AddToCart;
            gProceedToCart.Click += Click_ProceedToCart;
            Button btnMinus = FindViewById<Button>(Resource.Id.buttonMinus);
            Button btnPlus = FindViewById<Button>(Resource.Id.buttonPlus);
            btnMinus.Click += minus_click;
            btnPlus.Click += plus_click;
        }

        private void plus_click(object sender, EventArgs e)
        {
            gQuantity.Text = (Convert.ToInt32(gQuantity.Text) + 1).ToString();
        }

        private void minus_click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(gQuantity.Text)>1)
                gQuantity.Text = (Convert.ToInt32(gQuantity.Text) - 1).ToString();
        }
        private async void Click_ProceedToCart(object sender, EventArgs e)
        {
            Cart item = new Cart { userId = topbar.GetUser().Id, Name = product.Name, Price = product.price, Quantity = gQuantity.Text };
            await MobileService.GetTable<Cart>().InsertAsync(item);
            var intent = new Intent(this, typeof(ActivityCart));
            topbar.CartImg();
            intent.PutExtra("businessName", businessName);
            intent.PutExtra("user", JsonConvert.SerializeObject(topbar.GetUser()));
            StartActivity(intent);
        }

        private async void Click_AddToCart(object sender, EventArgs e)
        {
            Cart item = new Cart { userId = topbar.GetUser().Id, Name = product.Name, Price = product.price, Quantity = gQuantity.Text };
            await MobileService.GetTable<Cart>().InsertAsync(item);
            topbar.CartImgPlus();
        }
    }
}
