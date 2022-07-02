using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
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
            baseList.Add("No Filter");
            baseFilter.ItemsSource = baseList;
            baseFilter.SelectedIndex = 5;

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
            typeList.Add("No Filter");
            typeFilter.ItemsSource = typeList;
            typeFilter.SelectedIndex = 8;

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
            seasonList.Add("No Filter");
            seasonFilter.ItemsSource = seasonList;
            seasonFilter.SelectedIndex = 5;
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

            SelectName.Text = "";
            SelectScore.Text = "";
            SelectBase.Text = "";
            SelectType.Text = "";
            SelectSeason.Text = "";

            // apply filters
            if (baseFilter.IsVisible)
            {
                items = ApplyFilters(items);
            }

            if (items.Count > 0)
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

        private async void Weighted_Random_Button_Clicked(object sender, EventArgs e)
        {
            var items = await App.Database.GetDishesAsync();

            SelectName.Text = "";
            SelectScore.Text = "";
            SelectBase.Text = "";
            SelectType.Text = "";
            SelectSeason.Text = "";

            // apply filters
            if (baseFilter.IsVisible)
            {
                items = ApplyFilters(items);
            }

            // check if there are items after the filter
            if (items.Count > 0)
            {
                Random rnd = new Random();
                Dish SelectedDish = null;
                int total = 0;

                // get total score & generate number based on score.
                foreach(Dish dish in items)
                {
                    total += (int)(dish.Score*10);
                }
                int randomSelect = rnd.Next(total);

                // go through the dishes until randomSelect reaches or goes below 0.
                foreach (Dish dish in items)
                {
                    randomSelect -= (int)(dish.Score * 10);
                    if(randomSelect <= 0)
                    {
                        SelectedDish = dish;
                        break;
                    }
                }
                
                // show dish
                SelectName.Text = SelectedDish.Name;
                SelectScore.Text = SelectedDish.Score.ToString();
                SelectBase.Text = SelectedDish.Base;
                SelectType.Text = SelectedDish.Type;
                SelectSeason.Text = SelectedDish.Season;
            }
        }

        private List<Dish> ApplyFilters(List<Dish> items)
        {
            // filter on base
            if((string)baseFilter.SelectedItem != "No Filter")
            {
                items.RemoveAll(a => a.Base != (string)baseFilter.SelectedItem);
            }
            // filter on type
            if ((string)typeFilter.SelectedItem != "No Filter")
            {
                items.RemoveAll(a => a.Type != (string)typeFilter.SelectedItem);
            }
            // filter on season
            if ((string)seasonFilter.SelectedItem != "No Filter")
            {
                items.RemoveAll(a => a.Season != (string)typeFilter.SelectedItem && a.Season != "Entire year");
            }
            // filter on score
            if (scoreFilter.Text != null && scoreFilter.Text != "")
            {
                items.RemoveAll(a => a.Score < Convert.ToDouble(scoreFilter.Text));
            }

            return items;
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

        private void Toggle_Filter_Button_Clicked(object sender, EventArgs e)
        {
            if (!baseFilter.IsVisible)
            {
                baseFilter.IsVisible = true;
                typeFilter.IsVisible = true;
                seasonFilter.IsVisible = true;
                scoreFilter.IsVisible = true;
            }
            else
            {
                baseFilter.IsVisible = false;
                typeFilter.IsVisible = false;
                seasonFilter.IsVisible = false;
                scoreFilter.IsVisible = false;
            }
        }
    }
}
