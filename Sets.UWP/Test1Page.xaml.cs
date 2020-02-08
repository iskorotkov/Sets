using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sets.UWP
{
    public sealed partial class Test1Page : Page
    {
        public Test1Page()
        {
            InitializeComponent();
            CollapseInputMethods();

            FillRepresentationCb();
            FillSourceCb();
        }

        private void FillSourceCb()
        {
            var fromFile = new ComboBoxItem
            {
                Content = "From file",
                Tag = new ReadingMethod {Execute = ReadFromFile, OnSelected = OnReadFromFileSelected}
            };
            var fromBox = new ComboBoxItem
            {
                Content = "From box",
                Tag = new ReadingMethod {Execute = ReadFromBox, OnSelected = OnReadFromConsoleBox}
            };

            SourceCb.Items.Add(fromFile);
            SourceCb.Items.Add(fromBox);
            SourceCb.SelectedIndex = 0;
        }

        private void FillRepresentationCb()
        {
            var logical = new ComboBoxItem
            {
                Content = "Logical",
                Tag = new CreationMethod {Execute = CreateLogicalSet}
            };
            var bit = new ComboBoxItem
            {
                Content = "Bit",
                Tag = new CreationMethod {Execute = CreateBitSet}
            };
            var multiset = new ComboBoxItem
            {
                Content = "Multiset",
                Tag = new CreationMethod {Execute = CreateMultiSet}
            };

            RepresentationCb.Items.Add(logical);
            RepresentationCb.Items.Add(bit);
            RepresentationCb.Items.Add(multiset);
            RepresentationCb.SelectedIndex = 0;
        }

        private void CollapseInputMethods()
        {
            FilePickerPanel.Visibility = Visibility.Collapsed;
            SetContentTb.Visibility = Visibility.Collapsed;
        }

        private void createSetBtn_Click(object sender, RoutedEventArgs e)
        {
            var representationItem = (ComboBoxItem) RepresentationCb.SelectedItem;
            var sourceItem = (ComboBoxItem) SourceCb.SelectedItem;

            var max = (int) MaxElemSlider.Value;
            var setCreator = (CreationMethod) representationItem.Tag;
            var method = (ReadingMethod) sourceItem.Tag;

            var set = setCreator.Execute(max);
            method.Execute(set);

            //Frame.Navigate(typeof(Test1Actions), set);
        }

        private SimpleSet CreateLogicalSet(int max) => new SimpleSet(max);

        private BitSet CreateBitSet(int max) => new BitSet(max);

        private MultiSet CreateMultiSet(int max) => new MultiSet(max);

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

        private void ReadFromBox(Set set)
        {
        }

        private void ReadFromFile(Set set)
        {
        }

        private void SourceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBoxItem) SourceCb.SelectedItem;
            var method = (ReadingMethod) item.Tag;
            method.OnSelected();
        }

        private struct ReadingMethod
        {
            public Action<Set> Execute;
            public Action OnSelected;
        }

        private struct CreationMethod
        {
            public Func<int, Set> Execute;
        }
    }
}