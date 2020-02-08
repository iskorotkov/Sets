using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sets.UWP
{
    public sealed partial class TestOneSetupPage : Page
    {
        public TestOneSetupPage()
        {
            InitializeComponent();
            CollapseInputMethods();

            FillRepresentationCb();
            FillSourceCb();
        }

        private void FillSourceCb()
        {
            SourceCb.Items.Add(new ReadingMethod("From file", ReadFromFile, OnReadFromFileSelected));
            SourceCb.Items.Add(new ReadingMethod("From box", ReadFromBox, OnReadFromConsoleBox));
            SourceCb.SelectedIndex = 0;
        }

        private void FillRepresentationCb()
        {
            RepresentationCb.Items.Add(new CreationMethod("Logical", CreateLogicalSet));
            RepresentationCb.Items.Add(new CreationMethod("Bit", CreateBitSet));
            RepresentationCb.Items.Add(new CreationMethod("Multiset", CreateMultiSet));
            RepresentationCb.SelectedIndex = 0;
        }

        private void CollapseInputMethods()
        {
            FilePickerPanel.Visibility = Visibility.Collapsed;
            SetContentTb.Visibility = Visibility.Collapsed;
        }

        private void createSetBtn_Click(object sender, RoutedEventArgs e)
        {
            var max = (int) MaxElemSlider.Value;
            var setCreator = (CreationMethod) RepresentationCb.SelectedItem;
            var method = (ReadingMethod) SourceCb.SelectedItem;

            var set = setCreator.Execute(max);
            method.Execute(set);

            //Frame.Navigate(typeof(Test1Actions), set);
        }

        private static SimpleSet CreateLogicalSet(int max) => new SimpleSet(max);

        private static BitSet CreateBitSet(int max) => new BitSet(max);

        private static MultiSet CreateMultiSet(int max) => new MultiSet(max);

        private void OnReadFromFileSelected()
        {
            CollapseInputMethods();
            FilePickerPanel.Visibility = Visibility.Visible;
        }

        private void OnReadFromConsoleBox()
        {
            CollapseInputMethods();
            SetContentTb.Visibility = Visibility.Visible;
        }

        private static void ReadFromBox(Set set)
        {
        }

        private static void ReadFromFile(Set set)
        {
        }

        private void SourceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var method = (ReadingMethod) SourceCb.SelectedItem;
            method.OnSelected();
        }

        private class ReadingMethod
        {
            public ReadingMethod(string text, Action<Set> execute, Action onSelected)
            {
                Text = text;
                Execute = execute;
                OnSelected = onSelected;
            }

            public string Text { get; }
            public Action<Set> Execute { get; }
            public Action OnSelected { get; }

            public override string ToString() => Text;
        }

        private class CreationMethod
        {
            public CreationMethod(string text, Func<int, Set> execute)
            {
                Text = text;
                Execute = execute;
            }

            public string Text { get; }
            public Func<int, Set> Execute { get; }

            public override string ToString() => Text;
        }
    }
}