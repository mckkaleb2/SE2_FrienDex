using FrienDex.Models;
using System.Collections.Generic;


namespace FrienDex
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }


        public void OnNewButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";

            App.TestItemRepo.AddNewTestItem(newTestItem.Text);
            statusMessage.Text = App.TestItemRepo.StatusMessage;
        }

        public void OnGetButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";

            List<TestItem> testItems = App.TestItemRepo.GetAllTestItems();
            testItemsList.ItemsSource = testItems;
        }

    }
}
