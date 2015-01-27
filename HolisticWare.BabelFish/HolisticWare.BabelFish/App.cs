using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace HolisticWare.BabelFish
{
	public class App : Application // superclass new in 1.3
	{
	    public App ()
	    {
	        // The root page of your application
	        MainPage = App.GetMainPage(); // property new in 1.3

	        return;
	    }

		public static Page GetMainPage()
		{
			return new MainPage();
			/*
			new ContentPage
			{
				Content = 


				new Label
				{
					Text = "Hello, Forms !",
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				},
			};
			*/
		}
	}
}
