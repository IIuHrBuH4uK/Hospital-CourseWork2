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
        private ObservableCollection<Hospital> _allHospitals; // Все данные
        private ObservableCollection<Hospital> _filteredHospitals; // Фильтрованный список

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
            var hospitals = db.Hospital.ToList(); // Загружаем все данные из БД

            _allHospitals = new ObservableCollection<Hospital>(hospitals);
            _filteredHospitals = new ObservableCollection<Hospital>(hospitals);

            Hospital_ListView.ItemsSource = _filteredHospitals;
        }


        private void LoadUserData()
        {
            var user = db.Users.FirstOrDefault(a => a.Id == _userId);
            if (user != null)
            {
                textBoxSurname.Text = user.LastName;
                textBoxName.Text = user.FirstName;
                textBoxPatronymic.Text = user.MiddleName;
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
                user.LastName = patronymic;
                user.FirstName = name;
                user.MiddleName = surname;
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
            OblPanel.Visibility = Visibility.Visible;
            Hospital_ListView.ItemsSource = db.Hospital.ToList();
        }

        private void Step1Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Step2Button_Click(object sender, RoutedEventArgs e)
        {

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
            private void SearchObl(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = Search_TextBox.Text.Trim().ToLower();

                _filteredHospitals.Clear();
                foreach (var hospital in _allHospitals)
                {
                    if (hospital.Obl.ToLower().Contains(searchText))
                    {
                        _filteredHospitals.Add(hospital);
                    }
                }

                Hospital_ListView.ItemsSource = null;  // Принудительное обновление ListView
                Hospital_ListView.ItemsSource = _filteredHospitals;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Hospital_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Hospital_ListView.SelectedItem is Hospital selectedHospital)
            {
                CityTextBlock.Text = selectedHospital.Obl;
                Step1Button.Background = Brushes.Transparent;
                Step1Button.Foreground = Brushes.SkyBlue;
                Step2Button.Background = Brushes.SkyBlue;
                Step2Button.Foreground = Brushes.White;
                OblPanel.Visibility = Visibility.Hidden;
                HospPanel.Visibility = Visibility.Visible;
            }
        }

        private void SearchHosp(object sender, TextChangedEventArgs e)
        {

        }

        private void Hosp_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
