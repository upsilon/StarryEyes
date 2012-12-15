﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using StarryEyes.Models.Tab;

namespace StarryEyes.ViewModels.WindowParts.Timelines
{
    public class ColumnViewModel : ViewModel
    {
        private readonly ICollection<TabModel> _modelCollection;
        private readonly ReadOnlyDispatcherCollection<TabViewModel> _tabs;
        public ReadOnlyDispatcherCollection<TabViewModel> Tabs
        {
            get { return _tabs; }
        }

        public TabViewModel Selected
        {
            get
            {
                return Tabs.Where(_ => _.IsSelected).FirstOrDefault();
            }
            set
            {
                var selected = Tabs.Where(_ => _ == value).FirstOrDefault();
                if (selected == null) return;
                Tabs.ForEach(_ => _.IsSelected = false);
                selected.IsSelected = true;
                RaisePropertyChanged(() => Selected);
            }
        }

        public void SetSelected(TabModel model)
        {
            this.Selected = this.Tabs.Where(_ => _.Model == model).FirstOrDefault();
        }

        public ColumnViewModel(Livet.ObservableSynchronizedCollection<TabModel> tabs)
        {
            this._modelCollection = tabs;
            this.CompositeDisposable.Add(_tabs = ViewModelHelper.CreateReadOnlyDispatcherCollection(
                tabs, _ => new TabViewModel(this, _), DispatcherHelper.UIDispatcher));
        }

        public void AddTab(TabViewModel tab)
        {
            this._modelCollection.Add(tab.Model);
            SetSelected(tab.Model);
        }
    }
}
