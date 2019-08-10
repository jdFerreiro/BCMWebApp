using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BCMWeb.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(BCMWeb.Droid.IPAddressManager))]
namespace BCMWeb.Droid
{
    class IPAddressManager : IIPAddressManager
    {

        public string GetIpAddress()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());

            if (adresses != null && adresses[0] != null)
            {
                return adresses[0].ToString();
            }
            else
            {
                return null;
            }
        }


    }
}