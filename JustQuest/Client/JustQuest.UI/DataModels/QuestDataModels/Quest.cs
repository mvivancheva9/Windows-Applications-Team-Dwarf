﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustQuest.UI.DataModels
{
    public class Quest : ViewModelBase
    {
        public Quest()
        : this(string.Empty, string.Empty, string.Empty)
        {

        }


        public Quest(Quest quest)
      : this(quest.Name, quest.Task, quest.PossibleAnswers)
        {

        }

        public Quest(string name, string task, string possibleAnswers)
        {
            this.Name = name;
            this.Task = task;
            this.PossibleAnswers = possibleAnswers;
            this.Hints = new ObservableCollection<Hint>();
        }

        public string Name { get; set; }

        public string Task { get; set; }

        public string PossibleAnswers { get; set; }

        public ICollection<Hint> Hints { get; set; }


        public void AddHint(Hint hint)
        {
            this.Hints.Add(hint);
        }
    }
}