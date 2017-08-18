using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Compendium.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for LabeledTextBlock.xaml
    /// </summary>
    public partial class LabeledTextBlock : UserControl
    {
        public LabeledTextBlock()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText", typeof(string), typeof(LabeledTextBlock), new FrameworkPropertyMetadata(null));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public static readonly DependencyProperty TextBlockProperty = DependencyProperty.Register(
            "TextBlock", typeof(string), typeof(LabeledTextBlock), new FrameworkPropertyMetadata(null));

        public string TextBlock
        {
            get { return (string)GetValue(TextBlockProperty); }
            set { SetValue(TextBlockProperty, value); }
        }
    }
}
