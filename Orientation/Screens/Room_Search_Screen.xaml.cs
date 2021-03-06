﻿using System;
using System.Text.RegularExpressions;
using SQLite.Net;

using Xamarin.Forms;

namespace Orientation {
  public partial class Room_Search_Screen : ContentPage {
		public Room_Search_Screen()
		{
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
			NavigationPage.SetBackButtonTitle(this, "Search");
			bottomLayout.Children.Add(new TabMenu(3));

			roomNumber.WidthRequest = (int)(0.8 * ((Orientation.App)App.Current).getScreenSize().Width);
			buildingName.WidthRequest = (int)(0.8 * ((Orientation.App)App.Current).getScreenSize().Width);

			queryBuildings();

			setTheme();
		}

    public void setTheme() {
      stackLayout.BackgroundColor = Theme.getBackgroundColor();
      roomNumber.BackgroundColor = Theme.getEntryColor();
      roomNumber.PlaceholderColor = Theme.getEntryPlaceholderColor();
      buildingName.BackgroundColor = Theme.getEntryColor();
    }

    public void queryBuildings() {
      SQLiteConnection con = DependencyService.Get<IDatabaseHandler>().getDBConnection();
      var rooms = con.Table<Room>().OrderBy(r => r.buildingName);

      foreach (Room r in rooms) {
        if (!buildingName.Items.Contains(r.buildingName))
          buildingName.Items.Add(r.buildingName);
      }

      con.Close();
    }

    public void pressSearch(object sender, EventArgs args) {

      if (buildingName.SelectedIndex < 0)
        return;

      SQLiteConnection con = DependencyService.Get<IDatabaseHandler>().getDBConnection();
      var rooms = con.Table<Room>();
	    
      String building = buildingName.Items[buildingName.SelectedIndex];
      String number = roomNumber.Text;

      if (number == null || number.Length == 0) {
        con.Close();
        ((NavigationPage)App.Current.MainPage).PushAsync(new RoomListScreen(building));
        return;
      }

      number.ToUpper().Replace("-", "").Replace(" ", "");
        
  	  Room room = null;

      foreach (Room r in rooms) {
        if (r.buildingName.Equals(building) && r.roomNumber.ToLower().Trim().Equals(number.ToLower().Trim())) {
          room = r;
          break;
        }
      }

      if (room == null) {
        foreach (Room r in rooms) {
          if (r.buildingName.Equals(building) && r.roomNumber.ToLower().Trim().Contains(number.ToLower().Trim())) {
            room = r;
            break;
          }
        }
      }

      con.Close();

      if (room != null)
      {
        ((NavigationPage)App.Current.MainPage).PushAsync(new Room_Results_Screen(room));
      }
      else
        showErrorMessage();
    }

    public async void showErrorMessage() {
      await DisplayAlert("Sorry", "That room could not be found", "OK");
    }
  }
}

