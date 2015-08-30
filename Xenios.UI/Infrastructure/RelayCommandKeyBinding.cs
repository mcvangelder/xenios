using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Xenios.UI.Infrastructure
{
    public class RelayCommandKeyBinding : KeyBinding
    {
        public ICommand RelayCommand
        {
            get { return (ICommand)GetValue(RelayCommandProperty); }
            set { SetValue(RelayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelayCommandProperty =
            DependencyProperty.Register("RelayCommand", typeof(ICommand), typeof(RelayCommandKeyBinding), new FrameworkPropertyMetadata(OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var keyBinding = d as InputBinding;
            if(d != null)
            {
                keyBinding.Command = (ICommand)e.NewValue;
            }
        }

        
    }
}
