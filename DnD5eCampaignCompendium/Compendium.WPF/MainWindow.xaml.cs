using Assisticant;
using Compendium.WPF.Dialogs;
using Compendium.WPF.ViewModels;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Compendium.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void Characters_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            if(Characters.SelectedItem != null)
            {
                var vm = ForView.Unwrap<CharacterHeaderViewModel>(Characters.SelectedItem);
                ContentContainer.Content = vm;
            }
        }

        private void Compendium_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            if(CompendiumPages.SelectedItem != null)
            {

                var vm = ForView.Unwrap<BasePageViewModel>(CompendiumPages.SelectedItem);
                if(vm is SpellPageViewModel)
                    ContentContainer.Content = (SpellPageViewModel)vm;
                else if (vm is ClassPageViewModel)
                    ContentContainer.Content = (ClassPageViewModel)vm;
                else if (vm is ContentPageViewModel)
                    ContentContainer.Content = (ContentPageViewModel)vm;
            }
        }
    }
}
