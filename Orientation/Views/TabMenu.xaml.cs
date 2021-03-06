﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Orientation
{
	public partial class TabMenu : ContentView
	{
		public TabMenu (int selectedId)
		{	
			InitializeComponent ();
			grid.Children.Add (new MenuButton(this, "Home", "home"), 0, 0);
			grid.Children.Add (new MenuButton(this, "Services", "services"), 1, 0);
			grid.Children.Add (new MenuButton(this, "Favorites", "favorite"), 2, 0);
			grid.Children.Add (new MenuButton(this, "Rooms", "rooms"), 3, 0);
			grid.Children.Add (new MenuButton(this, "Scavenger", "scavengerHunt"), 4, 0);

			((MenuButton)grid.Children [selectedId]).setSelected(true);

			setTheme();
		}

		public void pressMenuButton(string name, bool selected)
		{
			if (!selected && name.Equals ("Home")) {
				changeVisiblePage (new Home_Screen());
			}
			else if (!selected && name.Equals ("Services")) {
				changeVisiblePage (new Service_Search_Screen(null));
			}
      else if (selected && name.Equals("Services")) {
        if (((Service_Search_Screen)Navigation.NavigationStack[Navigation.NavigationStack.Count-1]).getCategory() != null)
          Navigation.PopAsync();
      }
			else if (!selected && name.Equals ("Favorites")) {
				changeVisiblePage (new Favorites_Screen ());
			}
			else if (!selected && name.Equals ("Rooms")) {
				changeVisiblePage (new Room_Search_Screen());
			}
			else if (!selected && name.Equals ("Scavenger")) {
				changeVisiblePage (new Scavenger_Hunt_Screen());
			}
		}

		public void changeVisiblePage(ContentPage page)
		{
			NavigationPage navPage = new NavigationPage(page);
			navPage.BarBackgroundColor = Theme.getBackgroundColor ();
			navPage.BarTextColor = Theme.getTextColor();
			App.Current.MainPage = navPage;
		}

		public void setTheme()
		{
			grid.BackgroundColor = Theme.getBackgroundColor();
		}
	}
}

