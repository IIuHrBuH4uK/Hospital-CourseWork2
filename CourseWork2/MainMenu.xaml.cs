    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
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
    using System.Windows.Shapes;    
    using System.Data.Entity.Migrations;
    using MaterialDesignThemes.Wpf;
    using System.Collections.ObjectModel;


namespace CourseWork2
    {
    public partial class MainMenu : Window
    {
        private ObservableCollection<Region> _allRegions; // Все данные
        private ObservableCollection<Region> _filteredRegions; // Фильтрованный список
        private ObservableCollection<Hospital> _allHospitals;
        private ObservableCollection<Hospital> _filtredHospitals;

        private readonly int _userId;
        AppContext db;

        public MainMenu(int userId)
        {
            InitializeComponent();
            _userId = userId;
            PersonAccountPanel.Visibility = Visibility.Hidden;
            SignUpDoctorPanel.Visibility = Visibility.Hidden;
            HospPanel.Visibility = Visibility.Hidden;
            db = new AppContext();
            LoadUserData();
            LoadData();
        }

        private void LoadData()
        {
            var regions = db.Regions.ToList(); // Загружаем все данные из БД
            var hospitals = db.Hospitals.ToList();

            _allRegions = new ObservableCollection<Region>(regions);
            _filteredRegions = new ObservableCollection<Region>(regions);
            _allHospitals = new ObservableCollection<Hospital>(hospitals);
            _filtredHospitals = new ObservableCollection<Hospital>(hospitals);

            Region_ListView.ItemsSource = _filteredRegions;
            Hosp_ListView.ItemsSource = _filtredHospitals ;
        }


        private void LoadUserData()
        {
            var user = db.Users.FirstOrDefault(a => a.Id == _userId);
            if (user != null)
            {
                textBoxSurname.Text = user.MiddleName;
                textBoxName.Text = user.FirstName;
                textBoxPatronymic.Text = user.LastName;
                textBoxSnils.Text = user.SNILS;
                textBoxLocation.Text = user.Address;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Person_Account_Button_Click(object sender, RoutedEventArgs e)
        {

            MainMenuPanel.Visibility = Visibility.Hidden;
            PersonAccountPanel.Visibility = Visibility.Visible;
            SignUpDoctorPanel.Visibility= Visibility.Hidden;
        }

        private void SafePersonButton_Click(object sender, RoutedEventArgs e)
        {
            string surname = textBoxSurname.Text.Trim();
            string name = textBoxName.Text.Trim();
            string patronymic = textBoxPatronymic.Text.Trim();
            string snils = textBoxSnils.Text.Trim();
            string location = textBoxLocation.Text.Trim();

            var user = db.Users.FirstOrDefault(u => u.Id == _userId);
            if (user != null)
            {
                user.MiddleName = surname;
                user.FirstName = name;
                user.LastName = patronymic;
                user.SNILS = snils;
                user.Address = location;

                db.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SignUpDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpDoctorPanel.Visibility = Visibility.Visible;
            PersonAccountPanel.Visibility = Visibility.Hidden;
            MainMenuPanel.Visibility = Visibility.Hidden;
            RegionPanel.Visibility = Visibility.Visible;
            Region_ListView.ItemsSource = db.Regions.ToList();
        }

        private void Step1Button_Click(object sender, RoutedEventArgs e)
        {
            HospPanel.Visibility = Visibility.Hidden;
            RegionPanel.Visibility= Visibility.Visible;
            CityTextBlock.Text = "";
            Step1Button.Background = Brushes.SkyBlue;
            Step1Button.Foreground = Brushes.White;
            Step2Button.Background = Brushes.Transparent;
            Step2Button.Foreground = Brushes.SkyBlue;
        }

        private void Step2Button_Click(object sender, RoutedEventArgs e)
        {
            HospPanel.Visibility = Visibility.Visible;
            RegionPanel.Visibility = Visibility.Hidden;
            HospitalTextBlock.Text = "";
            Step1Button.Background = Brushes.Transparent;
            Step1Button.Foreground = Brushes.SkyBlue;
            Step2Button.Background = Brushes.SkyBlue;
            Step2Button.Foreground = Brushes.White;
            Step3Button.Background = Brushes.Transparent;
            Step3Button.Foreground = Brushes.SkyBlue;
        }

        private void Step3Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Step4Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Step5Button_Click(object sender, RoutedEventArgs e)
        {

        }
            private void SearchRegion(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = Search_TextBox.Text.Trim().ToLower();

                _filteredRegions.Clear();
                foreach (var region in _allRegions)
                {
                    if (region.Reg.ToLower().Contains(searchText))
                    {
                        _filteredRegions.Add(region);
                    }
                }

                Region_ListView.ItemsSource = null;  // Принудительное обновление ListView
                Region_ListView.ItemsSource = _filteredRegions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Region_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Region_ListView.SelectedItem is Region selectedRegion)
            {
                CityTextBlock.Text = selectedRegion.Reg;
                Step1Button.Background = Brushes.Transparent;
                Step1Button.Foreground = Brushes.SkyBlue;
                Step2Button.Background = Brushes.SkyBlue;
                Step2Button.Foreground = Brushes.White;
                RegionPanel.Visibility = Visibility.Hidden;
                HospPanel.Visibility = Visibility.Visible;

                // Фильтруем больницы по выбранному региону
                _filtredHospitals.Clear();
                foreach (var hospital in _allHospitals)
                {
                    if (hospital.RegionId == selectedRegion.Id)
                    {
                        _filtredHospitals.Add(hospital);
                    }
                }

                Hosp_ListView.ItemsSource = null; // Принудительное обновление ListView
                Hosp_ListView.ItemsSource = _filtredHospitals;
            }
        }

        private void SearchHosp(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchHosp_TextBox.Text.Trim().ToLower();
                _filtredHospitals.Clear();
                foreach (var hospital in _allHospitals)
                {
                    if (hospital.Hosp.ToLower().Contains(searchText))
                    {
                        _filtredHospitals.Add(hospital);
                    }
                }
                Hosp_ListView.ItemsSource = null;
                Hosp_ListView.ItemsSource = _filtredHospitals;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

        }

        private void Hosp_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Hosp_ListView.SelectedItem is Hospital selectedHospital)
            {
                HospitalTextBlock.Text = selectedHospital.Hosp;
                Step2Button.Background = Brushes.Transparent;
                Step2Button.Foreground = Brushes.SkyBlue;
                Step3Button.Background = Brushes.SkyBlue;
                Step3Button.Foreground = Brushes.White;
                //HospPanel.Visibility = Visibility.Hidden;
                //SpecPanel.Visibility = Visibility.Visible;

                // Фильтруем больницы по выбранному региону
                //_filtredHospitals.Clear();
                //foreach (var hospital in _allHospitals)
                //{
                //    if (hospital.RegionId == selectedRegion.Id)
                //    {
                //        _filtredHospitals.Add(hospital);
                //    }
                //}

                //Hosp_ListView.ItemsSource = null; // Принудительное обновление ListView
                //Hosp_ListView.ItemsSource = _filtredHospitals;
            }
        }
    }
}
