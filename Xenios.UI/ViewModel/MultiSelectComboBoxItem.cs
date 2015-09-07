using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenios.UI.ViewModel
{
    public class MultiSelectComboBoxItem : ViewModelBase
    {
        /// <summary>
        /// The <see cref="DisplayName" /> property's name.
        /// </summary>
        public const string DisplayNamePropertyName = "DisplayName";

        private String _displayName = null;

        /// <summary>
        /// Sets and gets the DisplayName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String DisplayName
        {
            get
            {
                return _displayName;
            }

            set
            {
                if (_displayName == value)
                {
                    return;
                }

                _displayName = value;
                RaisePropertyChanged(DisplayNamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        private bool _isSelected = false;

        /// <summary>
        /// Sets and gets the IsSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;
                RaisePropertyChanged(IsSelectedPropertyName);
            }
        }


        public MultiSelectComboBoxItem(String displayName, bool isSelected=false)
        {
            DisplayName = displayName;
            IsSelected = isSelected;
        }
    }
}
