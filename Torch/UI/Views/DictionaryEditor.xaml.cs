﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Torch.UI.ViewModels;

namespace Torch.UI.Views
{
    /// <summary>
    ///     Interaction logic for DictionaryEditorDialog.xaml
    /// </summary>
    public partial class DictionaryEditorDialog : Window
    {
        private Action _commitChanges;
        private Type _itemType;

        public DictionaryEditorDialog()
        {
            InitializeComponent();
            DataContext = Items;
        }

        public ObservableCollection<IDictionaryItem> Items { get; } = new ObservableCollection<IDictionaryItem>();

        public void Edit(IDictionary dict)
        {
            Items.Clear();
            var dictType = dict.GetType();
            _itemType = typeof(DictionaryItem<,>).MakeGenericType(dictType.GenericTypeArguments[0], dictType.GenericTypeArguments[1]);

            foreach (var key in dict.Keys)
            {
                Items.Add((IDictionaryItem)Activator.CreateInstance(_itemType, key, dict[key]));
            }

            ItemGrid.ItemsSource = Items;

            _commitChanges = () =>
            {
                dict.Clear();
                foreach (var item in Items)
                {
                    dict[item.Key] = item.Value;
                }
            };

            Show();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            _commitChanges?.Invoke();
            Close();
        }

        private void AddNew_OnClick(object sender, RoutedEventArgs e)
        {
            Items.Add((IDictionaryItem)Activator.CreateInstance(_itemType));
        }

        public interface IDictionaryItem
        {
            object Key { get; set; }
            object Value { get; set; }
        }

        public class DictionaryItem<TKey, TValue> : ViewModel, IDictionaryItem
        {
            private TKey _key;
            private TValue _value;

            public DictionaryItem()
            {
                _key = default(TKey);
                _value = default(TValue);
            }

            public DictionaryItem(TKey key, TValue value)
            {
                _key = key;
                _value = value;
            }

            public TKey Key { get => _key; set => SetValue(ref _key, value); }
            public TValue Value { get => _value; set => SetValue(ref _value, value); }

            object IDictionaryItem.Key { get => _key; set => SetValue(ref _key, (TKey)value); }
            object IDictionaryItem.Value { get => _value; set => SetValue(ref _value, (TValue)value); }
        }
    }
}