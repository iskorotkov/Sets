using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sets.UWP
{
    public sealed partial class Test1Page : Page
    {
        public Test1Page()
        {
            this.InitializeComponent();
        }

        private void createSetBtn_Click(object sender, RoutedEventArgs e)
        {
            var repr = (ComboBoxItem)RepresentationCb.SelectedItem;
            var source = (ComboBoxItem)SourceCb.SelectedItem;
            //Frame.Navigate(typeof(Test1Actions), Tuple.Create());
        }
    }
}
