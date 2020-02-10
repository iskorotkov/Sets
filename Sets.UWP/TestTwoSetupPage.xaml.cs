using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Sets.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestTwoSetupPage : Page
    {
        public TestTwoSetupPage()
        {
            InitializeComponent();
        }

        private void UpdateButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var max = (int) SetMaxSlider.Value;
            try
            {
                ShowResultsForSets(new SimpleSet(max), new SimpleSet(max), LogicalUnion, LogicalIntersection);
                ShowResultsForSets(new BitSet(max), new BitSet(max), BitUnion, BitIntersection);
                ShowResultsForSets(new MultiSet(max), new MultiSet(max), MultisetUnion, MultisetIntersection);
            }
            catch (Exception)
            {
                FlyoutText.Text = "Incorrect format";
                ButtonFlyout.ShowAt(UpdateButton);
            }
        }

        private void ShowResultsForSets(Set set1, Set set2, TextBlock unionTb, TextBlock intersectionTb)
        {
            PopulateSets(set1, set2);
            unionTb.Text = (set1 + set2).ToString();
            intersectionTb.Text = (set1 * set2).ToString();
        }

        private void PopulateSets(Set set1, Set set2)
        {
            set1.Append(SetOneBox.Text);
            set2.Append(SetTwoBox.Text);
        }
    }
}
