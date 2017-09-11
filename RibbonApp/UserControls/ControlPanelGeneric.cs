using RibbonApp.Database;
using RibbonApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RibbonApp.UserControls
{
    public class ControlPanelGeneric<T>
    {
        public ControlPanelGeneric(ControlPanel controlPanel)
        {
            this._cp = controlPanel;
            _cp.btnFirst.Click += btnFirst_Click;
            _cp.btnNext.Click += btnNext_Click;
            _cp.btnPrev.Click += btnPrev_Click;
            _cp.btnLast.Click += btnLast_Click;
            _cp.cbNumberOfRecords.SelectionChanged += cbNumberOfRecords_SelectionChanged;
            _cp.btnClear.Click += btnClear_Click;
            _cp.btnSearch.Click += btnSearch_Click;
            _cp.tbSearch.KeyDown += tbSearch_KeyDown;
            _cp.Loaded += UserControl_Loaded;

            //sorting
            //_cp.btnOrderAlphabetical.Checked += btnOrderAlphabetical_Checked;
            //_cp.btnOrderAlphabetical.Unchecked += btnOrderAlphabetical_Unchecked;
            //_cp.btnOrderReverseAlphabetical.Checked += btnOrderReverseAlphabetical_Checked;
            //_cp.btnOrderReverseAlphabetical.Unchecked += btnOrderReverseAlphabetical_Unchecked;

            _cp.btnOrderAlphabetical.Click += btnOrderAlphabetical_Click;
            _cp.btnOrderReverseAlphabetical.Click += btnOrderReverseAlphabetical_Click;

            _cp.cbOrderBy.SelectionChanged += cbOrderBy_SelectionChanged;

            _numberOfRecordsPerPage = Convert.ToInt32(((ComboBoxItem)_cp.cbNumberOfRecords.SelectedItem).Content);

        }

        public List<string> OrderByCriteria { get; set; }


        private void cbOrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetAll();
        }



        private void btnOrderAlphabetical_Click(object sender, RoutedEventArgs e)
        {
            _orderBy = UserControls.OrderBy.Alphabetical;
            _cp.btnOrderReverseAlphabetical.IsChecked = false;
            ResetAll();
        }

        private void btnOrderReverseAlphabetical_Click(object sender, RoutedEventArgs e)
        {
            _orderBy = UserControls.OrderBy.ReverseAlphabetical;
            _cp.btnOrderAlphabetical.IsChecked = false;
            ResetAll();
        }


        //private void btnOrderAlphabetical_Checked(object sender, RoutedEventArgs e)
        //{
        //    _orderBy = UserControls.OrderBy.Alphabetical;
        //    _cp.btnOrderReverseAlphabetical.IsChecked = false;
        //    ResetAll();
        //}

        //private void btnOrderAlphabetical_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    _orderBy = UserControls.OrderBy.ReverseAlphabetical;
        //    _cp.btnOrderReverseAlphabetical.IsChecked = true;
        //    ResetAll();
        //}

        //private void btnOrderReverseAlphabetical_Checked(object sender, RoutedEventArgs e)
        //{
        //    _orderBy = UserControls.OrderBy.ReverseAlphabetical;
        //    _cp.btnOrderAlphabetical.IsChecked = false;
        //    ResetAll();
        //}

        //private void btnOrderReverseAlphabetical_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    _orderBy = UserControls.OrderBy.Alphabetical;
        //    _cp.btnOrderAlphabetical.IsChecked = true;
        //    ResetAll();
        //}

        private ControlPanel _cp;


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }


        public ObservableCollection<T> DataToTransform { get; set; }

        public void Transform()
        {
            Search();
        }

        public ObservableCollection<T> OrderBy(ObservableCollection<T> itemsToOrder)
        {
            if(_orderBy == UserControls.OrderBy.Alphabetical)
            {
                if (OrderByAlphabetical != null)
                   return OrderByAlphabetical(itemsToOrder,GetComboBoxCriterionString());
                else return itemsToOrder;

            }
            else if (_orderBy == UserControls.OrderBy.ReverseAlphabetical)
            {
                if (OrderByReverseAlphabetical != null)
                    return OrderByReverseAlphabetical(itemsToOrder,GetComboBoxCriterionString());
                else return itemsToOrder;
            }
            else
            {
                return itemsToOrder;
            }
            
        }

        private string GetComboBoxCriterionString()
        {
           return _cp.cbOrderBy.SelectedItem.ToString();
        }

        public Func<ObservableCollection<T>,string, ObservableCollection<T>> OrderByAlphabetical;
        public Func<ObservableCollection<T>, string,ObservableCollection<T>> OrderByReverseAlphabetical;

        public void Search(string searchString = null)
        {
           _cp.cbNumberOfRecords.IsEnabled = true;
            _cp.btnPrev.IsEnabled = true;
            _cp.btnNext.IsEnabled = true;
            _cp.btnFirst.IsEnabled = true;
            _cp.btnLast.IsEnabled = true;

         
           string search = _cp.tbSearch.Text;
            

            if (!string.IsNullOrWhiteSpace(search))
            {
                ObservableCollection<T> searchResult = SearchMethod(GetAllDataMethod(), search);
               searchResult =  OrderBy(searchResult);

                if (searchResult.Any())
                {
                    if (SearchResultIsNotEmpty != null)
                    {
                        SearchResultIsNotEmpty();
                    }

                    _totalRecords = searchResult.Count();
                    Pagination(searchResult);
                }
                else
                {
                   _cp.lblpageInformation.Text = "0 výsledků";
                    _cp.btnFirst.IsEnabled = false;
                    _cp.btnNext.IsEnabled = false;
                    _cp.btnLast.IsEnabled = false;
                    _cp.btnPrev.IsEnabled = false;
                    _cp.cbNumberOfRecords.IsEnabled = false;
                    if (SearchResultIsEmpty != null)
                    {
                        SearchResultIsEmpty();
                    }
                }

            }
            else
            {
                ResetAll();
            }
        }

        public event Action SearchResultIsEmpty;
        public event Action SearchResultIsNotEmpty;

        private void ResetAll()
        {
            _cp.cbOrderBy.ItemsSource = OrderByCriteria;

            var allData = GetAllDataMethod();
            allData = OrderBy(allData);
            _totalRecords = allData.Count();

            _cp.cbNumberOfRecords.IsEnabled = true;
            _cp.btnPrev.IsEnabled = true;
            _cp.btnNext.IsEnabled = true;
            _cp.btnFirst.IsEnabled = true;
            _cp.btnLast.IsEnabled = true;

            if (SearchResultIsNotEmpty != null && _totalRecords > 0)
            {
                SearchResultIsNotEmpty();
            }
            _cp.tbSearch.Text = string.Empty;
            Pagination(allData);
        }

        public void Pagination(ObservableCollection<T> data)
        {
            int howManyItemsToSkip = 0;
            if (_pageNumber > 1)
            {
                howManyItemsToSkip = (_pageNumber - 1) * _numberOfRecordsPerPage;
            }


            ObservableCollection<T> singlePage = new ObservableCollection<T>(data.Skip(howManyItemsToSkip).Take(_numberOfRecordsPerPage));
            DataToTransform.Clear();
            foreach (var item in singlePage)
            {
                DataToTransform.Add(item);
            }

            UpdateLabel();
        }


        public Func<ObservableCollection<T>> GetAllDataMethod;
        public Func<ObservableCollection<T>, string, ObservableCollection<T>> SearchMethod;

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
            _cp.tbSearch.Text = string.Empty;
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
            _pageNumber = Convert.ToInt32(totalPages);
            Search();
        }

        private void cbNumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _numberOfRecordsPerPage = Convert.ToInt32(((ComboBoxItem)_cp.cbNumberOfRecords.SelectedItem).Content);

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
            //_totalRecords = GetAllDataMethod().Count();
            //UpdateLabel();
           
            ResetAll();
            
        }

        private void UpdateLabel()
        {
            double divide = Convert.ToDouble(_totalRecords) / Convert.ToDouble(_numberOfRecordsPerPage);
            double totalPages = Math.Ceiling(divide);

            if (totalPages == _pageNumber)
            {
                _cp.btnNext.IsEnabled = false;
                _cp.btnLast.IsEnabled = false;
            }
            else
            {
                _cp.btnNext.IsEnabled = true;
                _cp.btnLast.IsEnabled = true;
            }

            if (_pageNumber == 1)
            {
                _cp.btnFirst.IsEnabled = false;
                _cp.btnPrev.IsEnabled = false;
            }
            else
            {
                _cp.btnFirst.IsEnabled = true;
                _cp.btnPrev.IsEnabled = true;
            }


            _cp.lblpageInformation.Text = _pageNumber.ToString() + " / " + (totalPages).ToString();
        }


        private OrderBy _orderBy = UserControls.OrderBy.Alphabetical;
    }

    enum OrderBy { Alphabetical, ReverseAlphabetical }
}
