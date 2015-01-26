using System;
using System.Collections.Generic;

using Xamarin.Forms;

using HolisticWare.Core.Localization.Translate;

namespace HolisticWare.BabelFish
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			return;
		}

		private async void buttonTranslate_Clicked (object sender, EventArgs ea)
		{
			GoogleTranslatePrimitive gt = new GoogleTranslatePrimitive();

			string translation = await gt.Translate();

			return;
		}

	}
}

