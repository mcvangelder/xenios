using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Xenios.UI.Controls
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private ObservableCollection<ViewModel.MultiSelectComboBoxItem> _itemList = 
            new ObservableCollection<ViewModel.MultiSelectComboBoxItem>();

        public MultiSelectComboBox()
        {
            InitializeComponent();
        }

        #region Dependency Properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), 
                typeof(MultiSelectComboBox), 
                new FrameworkPropertyMetadata(null, 
                       new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
           DependencyProperty.Register("SelectedItems", 
                typeof(IList), typeof(MultiSelectComboBox),
                 new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), 
                typeof(MultiSelectComboBox), 
                        new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), 
                typeof(MultiSelectComboBox), 
                        new UIPropertyMetadata(string.Empty));

        #endregion

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
                            
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DetermineSelectedItems();
            control.SetText();
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.RenderList();
        }

        private void RenderList()
        {
            _itemList.Clear();

            foreach (var item in ItemsSource)
            {
                _itemList.Add(new ViewModel.MultiSelectComboBoxItem(item.ToString()));
            }

            _multiSelectComboBox.ItemsSource = _itemList;
        }

        private void SetText()
        {
            if (SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (var item in _itemList)
                {
                    if (item.IsSelected)
                    {
                        displayText.Append(item.DisplayName);
                        displayText.Append(", ");
                    }
                }
                Text = displayText.ToString().TrimEnd(new char[] { ',', ' ' });
            }

            if (String.IsNullOrEmpty(Text))
            {
                Text = this.DefaultText;
            }
        }

        private void SetSelectedItemsOnDropDown()
        {
            if (SelectedItems != null)
                SelectedItems.Clear();

            SelectedItems = (IList)Activator.CreateInstance(SelectedItems.GetType());

            if (this.ItemsSource.Count > 0)
            {
                int index = 0;
                foreach (var item in _itemList)
                {
                    if (item.IsSelected)
                    {
                        SelectedItems.Add(ItemsSource[index]);
                    }
                    index++;
                }
            }
        }

        private void DetermineSelectedItems()
        {
            foreach (var selectedItem in SelectedItems)
            {
                var item = _itemList.FirstOrDefault(i => i.DisplayName == selectedItem.ToString());
                if (item == null)
                    continue;

                item.IsSelected = true;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedItemsOnDropDown();
        }
    }
}
