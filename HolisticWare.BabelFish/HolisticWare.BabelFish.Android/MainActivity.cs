﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;

namespace HolisticWare.BabelFish.Droid
{
	[Activity(Label = "HolisticWare.BabelFish", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity 
		: 
		// AndroidActivity
		global::Xamarin.Forms.Platform.Android.FormsApplicationActivity // superclass new in 1.3
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

	        LoadApplication (new App ()); // method is new in 1.3

	        return;

			/*
			Xamarin.Forms.Forms.Init(this, bundle);

			SetPage(App.GetMainPage());

			return;
			*/
		}
	}
}

