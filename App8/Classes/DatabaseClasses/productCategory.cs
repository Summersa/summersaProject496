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

namespace App8
{
    class ProductCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImgURL { get; set; }
        public string Business { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}