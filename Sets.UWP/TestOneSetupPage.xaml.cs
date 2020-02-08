﻿using System;
using System.IO;
using System.Linq;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sets.UWP
{
    public sealed partial class TestOneSetupPage : Page
    {
        private string _selectedFile;

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

            // TODO: Open actions page in App's frame
            Frame.Navigate(typeof(TestOneActionsPage), set);
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

        private void ReadFromBox(Set set)
        {
            try
            {
                set.Append(SetContentTb.Text);
            }
            catch (Exception e)
            {
                // TODO: Handle exception
            }
        }

        private void ReadFromFile(Set set)
        {
            // TODO: Handle file not selected
            using (var reader = new StreamReader(_selectedFile))
            {
                var content = reader.ReadToEnd();
                var numbers = content.Trim()
                    .Split(' ', '\t', '\n', '\r')
                    .Where(s => s.Any())
                    .Select(int.Parse);
                set.Append(numbers.ToArray());
            }
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
                _text = text;
                Execute = execute;
                OnSelected = onSelected;
            }

            private readonly string _text;
            public Action<Set> Execute { get; }
            public Action OnSelected { get; }

            public override string ToString() => _text;
        }

        private class CreationMethod
        {
            public CreationMethod(string text, Func<int, Set> execute)
            {
                _text = text;
                Execute = execute;
            }

            private readonly string _text;
            public Func<int, Set> Execute { get; }

            public override string ToString() => _text;
        }

        private async void FilePickerButton_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".txt");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                _selectedFile = file.Path;
                FilePickerStatus.Text = $"File selected: {file.Name}";
            }
            else
            {
                FilePickerStatus.Text = "File wasn't selected";
            }
        }
    }
}