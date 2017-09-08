﻿using RibbonApp.Database;
using RibbonApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RibbonApp.UserControls
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();            
        }

        
        public ObservableCollection<Customer> DataToTransform { get; set; }

        public void Transform()
        {
            Search();
        }



        private void Search()
        {
            string search = tbSearch.Text;
            if (!string.IsNullOrWhiteSpace(search))
            {               

                ObservableCollection<Customer> searchResult = SearchMethod(GetAllDataMethod(),search);            
                _totalRecords = searchResult.Count();
                Pagination(searchResult);
            }
            else
            {
                ResetAll();
            }
        }


        private void ResetAll()
        {
            var allData = GetAllDataMethod();
            _totalRecords = allData.Count();
            Pagination(allData);
        }

        public void Pagination(ObservableCollection<Customer> data)
        {
            int howManyItemsToSkip = 0;
            if (_pageNumber > 1)
            {
                 howManyItemsToSkip = (_pageNumber-1) * _numberOfRecordsPerPage;
            }


            ObservableCollection<Customer> singlePage = new ObservableCollection<Customer>(data.Skip(howManyItemsToSkip).Take(_numberOfRecordsPerPage));
            DataToTransform.Clear();
            foreach (var item in singlePage)
            {
                DataToTransform.Add(item);
            }

            UpdateLabel();
        }


        public Func<ObservableCollection<Customer>> GetAllDataMethod;
        public Func<ObservableCollection<Customer>,string,ObservableCollection<Customer>> SearchMethod;

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {              
                _pageNumber = 1;
                Search();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            _pageNumber = 1;
            //  Search();
            ResetAll();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber = 1;
            Search();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber--;
            Search();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber++;
             Search();
          
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            double divide = Convert.ToDouble(_totalRecords) / Convert.ToDouble(_numberOfRecordsPerPage);
            double totalPages = Math.Ceiling(divide);
            _pageNumber =Convert.ToInt32( totalPages);
            Search();
        }

        private void cbNumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             _numberOfRecordsPerPage =Convert.ToInt32(((ComboBoxItem)cbNumberOfRecords.SelectedItem).Content);

            if (GetAllDataMethod != null)
            {
                _pageNumber = 1;
                Search();
            }
           
        }

        private int _pageNumber = 1;
        private int _totalRecords;
        private int _numberOfRecordsPerPage;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _totalRecords = GetAllDataMethod().Count();
            UpdateLabel();
           
        }

        private void  UpdateLabel()
        {
            double divide = Convert.ToDouble(_totalRecords) / Convert.ToDouble(_numberOfRecordsPerPage);
            double totalPages = Math.Ceiling(divide);

            if (totalPages == _pageNumber)
            {
                btnNext.IsEnabled = false;
                btnLast.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
                btnLast.IsEnabled = true;
            }

            if(_pageNumber == 1)
            {
                btnFirst.IsEnabled = false;
                btnPrev.IsEnabled = false;
            }
            else
            {
                btnFirst.IsEnabled = true;
                btnPrev.IsEnabled = true;
            }


            lblpageInformation.Content = _pageNumber.ToString() + " / " + (totalPages).ToString();
        }
    }
}
