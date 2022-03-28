using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DinnerSelection
{
    public partial class MainPage : ContentPage
    {
        public int selectedBrowseIndex;
        public MainPage()
        {
            InitializeComponent();

            var baseList = new List<string>
            {
                "Rice",
                "Potatoes",
                "Pasta/Noodles",
                "Salad",
                "Other"
            };
            basePicker.ItemsSource = baseList;
            basePicker.SelectedIndex = 0;


            var typeList = new List<string>
            {
                "Pork",
                "Poultry",
                "Beef",
                "Fish",
                "Shellfish",
                "Vegetarian",
                "Vegan",
                "Other"
            };
            typePicker.ItemsSource = typeList;
            typePicker.SelectedIndex = 0;

            var seasonList = new List<string>
            {
                "Summer",
                "Spring",
                "Autumn",
                "Winter",
                "Entire year"
            };
            seasonPicker.ItemsSource = seasonList;
            seasonPicker.SelectedIndex = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = await App.Database.GetDishesAsync();
        }

        async void Add_Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(nameEntry.Text) && !string.IsNullOrWhiteSpace(scoreEntry.Text) /* Add pickers and other stuff*/)
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
                UpdateFromDatabase();
            }
        }

        private async void Random_Button_Clicked(object sender, EventArgs e)
        {
            var items = await App.Database.GetDishesAsync();
            if(items.Count > 0)
            {
                Random rnd = new Random();
                int randomSelect = rnd.Next(items.Count);
                SelectName.Text = items[randomSelect].Name;
                SelectScore.Text = items[randomSelect].Score.ToString();
                SelectBase.Text = items[randomSelect].Base;
                SelectType.Text = items[randomSelect].Type;
                SelectSeason.Text = items[randomSelect].Season;
            }
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBrowseIndex = (int)(e.CurrentSelection.FirstOrDefault() as Dish)?.Id;
        }

        private async void UpdateFromDatabase()
        {
            collectionView.ItemsSource = await App.Database.GetDishesAsync();
        }

        private async void Delete_Button_Clicked(object sender, EventArgs e)
        {
            await App.Database.DeleteDishAsync(selectedBrowseIndex);
            UpdateFromDatabase();
        }
    }
}
