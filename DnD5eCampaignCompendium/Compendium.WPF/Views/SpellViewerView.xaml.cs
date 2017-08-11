using Compendium.WPF.Dialogs;
using Compendium.WPF.ViewModels.SpellViewer;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Compendium.WPF.Views
{
    /// <summary>
    /// Interaction logic for SpellViewerView.xaml
    /// </summary>
    public partial class SpellViewerView : UserControl
    {
        public SpellViewerView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsDrawerOpenProperty =
            DependencyProperty.Register(
                "IsDrawerOpen", 
                typeof(bool), 
                typeof(SpellViewerView), 
                new PropertyMetadata(null));
        public bool IsDrawerOpen
        {
            get { return (bool)this.GetValue(IsDrawerOpenProperty); }
            set { this.SetValue(IsDrawerOpenProperty, value); }
        }

        private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new MessageDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }
    }
}
