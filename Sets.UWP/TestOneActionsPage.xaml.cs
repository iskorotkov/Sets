using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Sets.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestOneActionsPage : Page
    {
        private Set _set;

        public TestOneActionsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _set = (Set) e.Parameter;
            Refresh();
        }

        private void Refresh()
        {
            SetOutputBlock.Text = _set.ToString();
            ValueBox.Text = string.Empty;
        }

        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private int GetProcessedNumber() => int.Parse(ValueBox.Text.Trim());

        private void AddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                _set.Add(GetProcessedNumber());
                Refresh();
            }
            catch (OutOfSetRangeException exception)
            {
                AddFlyoutText.Text = exception.Message;
                AddButtonFlyout.ShowAt(AddButton);
            }
            catch (Exception)
            {
                AddFlyoutText.Text = "Incorrect format";
                AddButtonFlyout.ShowAt(AddButton);
            }
        }

        private void RemoveButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                _set.Remove(GetProcessedNumber());
                Refresh();
            }
            catch (Exception)
            {
                RemoveFlyoutText.Text = "Incorrect format";
                RemoveButtonFlyout.ShowAt(RemoveButton);
            }
        }

        private void ContainsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                var number = GetProcessedNumber();
                ContainsFlyoutBlock.Text =
                    _set.Contains(number) ? $"Set contains {number}" : $"Set does not contain {number}";
            }
            catch (Exception)
            {
                ContainsFlyoutBlock.Text = "Incorrect format";
            }
        }
    }
}
