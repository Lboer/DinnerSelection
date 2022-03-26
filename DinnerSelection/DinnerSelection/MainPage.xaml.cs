using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DinnerSelection
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var baseList = new List<string>();
            baseList.Add("Rice");
            baseList.Add("Potatoes");
            baseList.Add("Pasta/Noodles");
            baseList.Add("Salad");
            baseList.Add("Other");
            basePicker.ItemsSource = baseList;
            basePicker.SelectedIndex = 0;
            

            var typeList = new List<string>();
            typeList.Add("Pork");
            typeList.Add("Poultry");
            typeList.Add("Beef");
            typeList.Add("Fish");
            typeList.Add("Shellfish");
            typeList.Add("Vegetarian");
            typeList.Add("Vegan");
            typeList.Add("Other");
            typePicker.ItemsSource = typeList;
            typePicker.SelectedIndex = 0;

            var seasonList = new List<string>();
            seasonList.Add("Summer");
            seasonList.Add("Spring");
            seasonList.Add("Autumn");
            seasonList.Add("Winter");
            seasonList.Add("Entire year");
            seasonPicker.ItemsSource = seasonList;
            seasonPicker.SelectedIndex = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = await App.Database.GetDishesAsync();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(nameEntry.Text) && !string.IsNullOrWhiteSpace(scoreEntry.Text) /* Add pickers and other stuff*/)
            {
                // Add to database
                await App.Database.SaveDishAsync(new Dish
                {
                    Name = nameEntry.Text,
                    Score = Convert.ToDouble(scoreEntry.Text),
                    Base = (string)basePicker.SelectedItem,
                    Type = (string)typePicker.SelectedItem,
                    Season = (string)seasonPicker.SelectedItem
                });

                // Reset all input fields
                nameEntry.Text = string.Empty;
                scoreEntry.Text = string.Empty;
                basePicker.SelectedIndex = 0;
                typePicker.SelectedIndex = 0;
                seasonPicker.SelectedIndex = 0;

                // Update browseable dishes
                collectionView.ItemsSource = await App.Database.GetDishesAsync();
            }
        }
    }
}
