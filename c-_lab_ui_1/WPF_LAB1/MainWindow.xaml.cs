﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DataLibrary;
using WPF_LAB1;

namespace WPF_LAB1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        V3MainCollection v3mainCollection = new V3MainCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Update()
        {

            DataContext = v3mainCollection;
            this.listBox_Main.ItemsSource = (IEnumerable<V3Data>)DataContext;
            this.listBox_DataOnGrid.ItemsSource = v3mainCollection.getOnlyDataOnGridElems();
            this.listBox_DataCollection.ItemsSource = v3mainCollection.getOnlyDataCollectionElems();
            this.MainCollectionProperties.Text = "Collection Properties\n"
                    + $"ChangeAfterSave: {v3mainCollection.ChangedAfterSave}\n"
                    + $"Count: {v3mainCollection.Count}\n";
            
            

        }

        private bool SaveCollection(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Microsoft.Win32.SaveFileDialog saveFileDialog =
                        new Microsoft.Win32.SaveFileDialog();
                    var save = (bool)saveFileDialog.ShowDialog();
                    if (save)
                    {
                        v3mainCollection.Save(saveFileDialog.FileName);
                        return true;
                    }
                    else
                        return false;
                case MessageBoxResult.No:
                    return true;
                default:
                    return false;
            }
        }

        private bool SaveChangeOrNot(string caption)
        {
            bool deleteCurrentCollection = false;
            if (v3mainCollection.ChangedAfterSave)
            {
                var result = MessageBox.Show(" Do you want to save ?",
                    caption, MessageBoxButton.YesNoCancel);

                deleteCurrentCollection = SaveCollection(result);
                if (deleteCurrentCollection)
                {
                    v3mainCollection = new V3MainCollection();
                }
            }
            else
                v3mainCollection = new V3MainCollection();
            return deleteCurrentCollection;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            SaveChangeOrNot("Save");
            Update();
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            SaveChangeOrNot("Save");

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            bool open = (bool)fileDialog.ShowDialog();
            if (open)
            {
                v3mainCollection.Load(fileDialog.FileName);
            }
            Update();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveCollection(MessageBoxResult.Yes);
        }

        private void AddDefaults_Click(object sender, RoutedEventArgs e)
        {
            v3mainCollection.AddDefaults();
            Update();
        }

        private void AddDefDataCollection_Click(object sender, RoutedEventArgs e)
        {
            v3mainCollection.AddDefaultDataCollection();
            Update();
        }

        private void AddDefDataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            v3mainCollection.AddDefaultDataOnGrid();
            Update();
        }

        private void AddElemFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            var result = (bool)fileDialog.ShowDialog();
            if (result)
                v3mainCollection.AddFromFile(fileDialog.FileName);
            Update();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedMainCollectionBox = this.listBox_Main.SelectedItems;
            var selectedDataOnGridBox = this.listBox_DataOnGrid.SelectedItems;
            var selectedCollectionBox = this.listBox_DataCollection.SelectedItems;
            List<V3Data> selectedItems = new List<V3Data>();
            selectedItems.AddRange(selectedMainCollectionBox.Cast<V3Data>());
            selectedItems.AddRange(selectedDataOnGridBox.Cast<V3Data>());
            selectedItems.AddRange(selectedCollectionBox.Cast<V3Data>());
            foreach (var item in selectedItems)
            {
                v3mainCollection.ChangedAfterSave =
                    v3mainCollection.Remove(item.info, item.t0);
            }
            Update();
        }
        private void ClosingButton_Click(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (v3mainCollection.ChangedAfterSave)
            {
                bool close = SaveChangeOrNot("Close");
                if (!close)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DataCollection(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V3DataCollection)) args.Accepted = true;
                else args.Accepted = false;
                
            }
            Update();


        }

        private void DataOnGrid(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V3DataOnGrid)) args.Accepted = true;
                else args.Accepted = false;
            }
            Update();


        }

        private void listbox_details_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
