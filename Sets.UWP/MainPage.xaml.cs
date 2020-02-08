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

            MainLv.Items.Add(new Test("Test 1", () => MainFrame.Navigate(typeof(TestOneSetupPage))));
            MainLv.Items.Add(new Test("Test 2", () => MainFrame.Navigate(typeof(TestTwoSetupPage))));
            MainLv.SelectedIndex = 0;
        }

        private void MainLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (Test) MainLv.SelectedItem;
            item.Execute();
        }

        private class Test
        {
            private readonly string _text;
            public Action Execute { get; }

            public Test(string text, Action execute)
            {
                _text = text;
                Execute = execute;
            }

            public override string ToString() => _text;
        }
    }
}