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
    class Orders
    {
        public string Id { get; set; }
        public string userId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Products { get; set; }
        public string Prices { get; set; }
        public string Quantities { get; set; }
        public string Completed { get; set; }
        public override string ToString()
        {
            return Id;
        }

    }
}