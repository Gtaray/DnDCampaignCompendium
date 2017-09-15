using Assisticant;
using Compendium.WPF.Dialogs;
using Compendium.WPF.ViewModels;
using Compendium.WPF.Views;
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

        private void Compendium_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            if (CompendiumPages.SelectedItem != null)
            {

                ContentContainer.Content = new SpellViewerView();
                //var vm = ForView.Unwrap<PageHeaderViewModel>(CompendiumPages.SelectedItem);
                //if (vm.Model is SpellPageViewModel)
                //    ContentContainer.Content = (SpellPageViewModel)vm.Model;
                //else if (vm.Model is ClassPageViewModel)
                //    ContentContainer.Content = (ClassPageViewModel)vm.Model;
                //else if (vm.Model is ContentPageViewModel)
                //    ContentContainer.Content = (ContentPageViewModel)vm.Model;
            }
        }

        private void Characters_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            if(Characters.SelectedItem != null)
            {
                var vm = ForView.Unwrap<CharacterHeaderViewModel>(Characters.SelectedItem);
                ContentContainer.Content = vm;
                ContentContainer.DataContext = ForView.Wrap(vm);
            }
        }
    }
}
