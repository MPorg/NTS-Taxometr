﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System.Diagnostics;

namespace TaxometrMauiMvvm
{
    [Activity(Theme = "@style/Theme.FullScreen", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}
