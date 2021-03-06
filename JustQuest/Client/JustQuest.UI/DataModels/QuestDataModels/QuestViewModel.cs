﻿namespace JustQuest.UI.DataModels.QuestDataModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Data;
    using Extensions;
    using Helpers;
    using HintDataModels;
    using Newtonsoft.Json;
    using Pages;
    public class QuestViewModel : ViewModelBase, IContentViewModel
    {
        private ICommand removeQuestCommand;
        private ICommand addHintCommand;
        public static List<Hint> hintsToAdd = new List<Hint>();
        private ObservableCollection<Hint> hints;
        private ICollection<Quest> quests;
        private readonly HttpRequester httpClient;

        public QuestViewModel()
        {
            httpClient = new HttpRequester();
        }

        public ICollection<Quest> Quests
        {
            get
            {
                if (this.quests == null)
                {
                    this.quests = new ObservableCollection<Quest>();
                }

                return this.quests;
            }
            set
            {
                if (this.quests == null)
                {
                    this.quests = new ObservableCollection<Quest>();
                }

                this.quests.Clear();

                foreach (var questToAdd in value)
                {
                    this.quests.Add(questToAdd);
                }
            }
        }

        public ICollection<Hint> Hints
        {
            get
            {
                if (this.hints == null)
                {
                    this.hints = new ObservableCollection<Hint>();
                }

                return this.hints;
            }
            set
            {
                if (this.hints == null)
                {
                    this.hints = new ObservableCollection<Hint>();
                }

                this.hints.Clear();
                foreach (var hintToAdd in hintsToAdd)
                {
                    this.hints.Add(hintToAdd);
                }
                value.ForEach(this.hints.Add);
            }
        }

        public ICommand AddQuest
        {
            get
            {
                if (this.removeQuestCommand == null)
                {
                    this.removeQuestCommand = new DelegateCommand<Quest>(async (quest) =>
                    {
                        foreach (var hint in hintsToAdd)
                        {
                            quest.Hints.Add(hint);
                        }
                        var userCredentials = await SQLiteData.GetUserCredentials();

                        var token = userCredentials.Token ?? "";

                        var response = await httpClient.PostData(quest, "api/Quests", token);
                        hintsToAdd.Clear();

                        if (response.IsSuccessStatusCode)
                        {
                            // TODO: Add messageBox
                            ((Window.Current.Content as AppShell).AppFrame as Frame).Navigate(typeof(MainPage));
                        }
                    });
                }
                return this.removeQuestCommand;
            }

        }
        public ICommand AddHint
        {
            get
            {
                if (this.addHintCommand == null)
                {

                    this.addHintCommand = new DelegateCommand<Hint>((hint) =>
                    {
                        hintsToAdd.Add(hint);
                        ((Window.Current.Content as AppShell).AppFrame as Frame).GoBack();
                    });

                }
                return this.addHintCommand;
            }

        }
    }
}
