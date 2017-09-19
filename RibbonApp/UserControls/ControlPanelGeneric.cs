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
    enum OrderBy { Alphabetical, ReverseAlphabetical } // Řazení

    /// <summary>
    /// Třída umožňující používat řazení a stránkování nad různými datovými kolekcemi. 
    /// </summary>
    /// <typeparam name="T">Typ 'T' specifikuje typ kolekce.</typeparam>
    public class ControlPanelGeneric<T>
    {
        /// <summary>
        /// ControlPanel je parametr, který představuje UserControl pro stránkování a řazení.
        /// </summary>
        /// <param name="controlPanel"></param>
        public ControlPanelGeneric(ControlPanel controlPanel)
        {
            // Události UserControlu jsou řešeny v této třídě, ne ve třídě user controlu (protože user control nemůže být generický)
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

            // Události řazení
            _cp.btnOrderAlphabetical.Click += btnOrderAlphabetical_Click;
            _cp.btnOrderReverseAlphabetical.Click += btnOrderReverseAlphabetical_Click;
            _cp.cbOrderBy.SelectionChanged += cbOrderBy_SelectionChanged;

            _numberOfRecordsPerPage = Convert.ToInt32(((ComboBoxItem)_cp.cbNumberOfRecords.SelectedItem).Content);
        }

        // User control, který vykresluje tlačítka pro řazení a stránkování. Je získán z konstruktoru.
        private ControlPanel _cp;
        
        // Pomocné proměnné
        private int _pageNumber = 1; // aktuální číslo stránky
        private int _totalRecords; // celkový počet záznamů k zobrazení
        private int _numberOfRecordsPerPage; // počet záznamů na stránku
        private OrderBy _orderBy = UserControls.OrderBy.Alphabetical; // jak se bude řadit

        // Podle čeho řadit
        public List<string> OrderByCriteria { get; set; }

        // Data, která budeme řadit, filtrovat, stránkovat ...
        // Změny se promítnou v GUI automaticky, protože se jedná o ObservableCollection
        public ObservableCollection<T> DataToTransform { get; set; }

        // Tyto funkce se plní z vnějšku. Spouští se při řazení.
        public Func<ObservableCollection<T>, string, ObservableCollection<T>> OrderByAlphabetical;
        public Func<ObservableCollection<T>, string, ObservableCollection<T>> OrderByReverseAlphabetical;

        // Události, které se spustí, když je vrácen výsledek/popřípadě pokud nic nevyhovuje hledání.
        public event Action SearchResultIsEmpty;
        public event Action SearchResultIsNotEmpty;

        // Metoda, která získává data z databáze
        public Func<ObservableCollection<T>> GetAllDataMethod;

        // Metoda, která se provede, když je stisknuto tlačítko "Vyhledat"
        public Func<ObservableCollection<T>, string, ObservableCollection<T>> SearchMethod;

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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            CallSearchMethod();
        }        

        public void CallSearchMethod()
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

        // Získat to, dle čeho se bude řadit
        private string GetComboBoxCriterionString()
        {
           return _cp.cbOrderBy.SelectedItem.ToString();
        }

        public void Search(string searchString = null)
        {
            // Počáteční inicializace
            _cp.cbNumberOfRecords.IsEnabled = true;
            _cp.btnPrev.IsEnabled = true;
            _cp.btnNext.IsEnabled = true;
            _cp.btnFirst.IsEnabled = true;
            _cp.btnLast.IsEnabled = true;
          
            // Co se má vyhledávat
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
       
        /// <summary>
        /// Resetuje všechno do výchozího stavu.
        /// </summary>
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

        /// <summary>
        /// Metoda zajišťující stránkování. Jako parametr bere data, která se mají stránkovat (tzn. např. již seřazená/filtrovaná data).
        /// </summary>
        /// <param name="data"></param>
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

        // Vyhledávání se spustí i po stisknutí Enter
        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _pageNumber = 1;
                Search();
            }
        }

        // Vyčeštění vyhledávání
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _cp.tbSearch.Text = string.Empty;
            _pageNumber = 1;        
            ResetAll();
        }

        // Tlačítko <<
        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber = 1;
            Search();
        }

        // Tlačítko <
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber--;
            Search();
        }

        // Tlačítko >
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber++;
            Search();
        }

        // Tlačítko >>
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            double divide = Convert.ToDouble(_totalRecords) / Convert.ToDouble(_numberOfRecordsPerPage);
            double totalPages = Math.Ceiling(divide); // zaokrouhlení nahoru, aby se přidala další stránka, i když nebude celá naplněna
            _pageNumber = Convert.ToInt32(totalPages);
            Search();
        }

        // Změna počtu záznamů na stránku
        private void cbNumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboBoxNumberOfItemsPerPage = ((ComboBoxItem)_cp.cbNumberOfRecords.SelectedItem).Content.ToString();
            if(comboBoxNumberOfItemsPerPage.Trim() == "Vše")
            {
                _numberOfRecordsPerPage = _totalRecords;
            }
            else
            {
                _numberOfRecordsPerPage = Convert.ToInt32(((ComboBoxItem)_cp.cbNumberOfRecords.SelectedItem).Content);
            }
            

            if (GetAllDataMethod != null)
            {
                _pageNumber = 1;
                Search();
            }

        }       

        // Po načtení resetovat na výchozí hodnoty
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ResetAll();            
        }

        /// <summary>
        /// Metoda aktualizující GUI.
        /// </summary>
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
       
    }
   
}
