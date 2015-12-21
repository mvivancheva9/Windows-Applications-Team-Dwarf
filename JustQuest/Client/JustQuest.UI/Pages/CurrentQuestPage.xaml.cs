﻿using JustQuest.UI.Data;
using JustQuest.UI.DataModels.QuestDataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using JustQuest.UI.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JustQuest.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrentQuestPage : Page
    {
        private readonly HttpRequester httpClient;
        private Quest currentQuest;

        public CurrentQuestPage()
        {
            this.InitializeComponent();
            this.httpClient = new HttpRequester();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = e.Parameter as Quest;
            this.currentQuest = e.Parameter as Quest;
            base.OnNavigatedTo(e);

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var userCredentials = await SQLiteData.GetUserCredentials();

            var token = userCredentials.Token ?? "";

            var response = await httpClient.PutData(GuessBox.Text, "api/Quests?id=" + this.currentQuest.Id + "&answer=" + GuessBox.Text, token);
        }
    }
}