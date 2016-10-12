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
using Java.Lang;

namespace App8
{
    class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgURL { get; set; }
        public string Business { get; set; }
        public string price { get; set; }
        public string description { get; set; }
        public string productCategory { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}