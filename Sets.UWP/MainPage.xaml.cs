using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sets.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            var test1Item = new ListViewItem
            {
                Content = "Test 1",
                Tag = new Action(() => MainFrame.Navigate(typeof(TestOneSetupPage)))
            };
            var test2Item = new ListViewItem
            {
                Content = "Test 2",
                Tag = new Action(() => MainFrame.Navigate(typeof(TestTwoSetupPage)))
            };
            MainLv.Items.Add(test1Item);
            MainLv.Items.Add(test2Item);
            MainLv.SelectedIndex = 0;
        }

        private void MainLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListViewItem) MainLv.SelectedItem;
            var action = (Action) item.Tag;
            action();
        }
    }
}
