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
using BCMWeb.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(BCMWeb.Droid.IDownloadPathFolder))]
namespace BCMWeb.Droid
{
    public class IDownloadPathFolder : IIDownloadPathService
    {
        public string GetDownloadFolder()
        {
            string _path;

            _path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
            return _path;

        }
    }
}