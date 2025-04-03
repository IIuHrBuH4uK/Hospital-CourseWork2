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
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Diagnostics.Eventing.Reader;


namespace CourseWork2
{
    public partial class MainMenu : Window
    {
        private ObservableCollection<Region> _allRegions; // Все данные
        private ObservableCollection<Region> _filteredRegions; // Фильтрованный список
        private ObservableCollection<Hospital> _allHospitals;
        private ObservableCollection<Hospital> _filteredHospitals;
        private ObservableCollection<Specialization> _allSpecialization;
        private ObservableCollection<Specialization> _filteredSpecialization;
        private ObservableCollection<Doctor> _allDoctor;
        private ObservableCollection<Doctor> _filteredDoctor;

        private DateTime _currentWeekStart = DateTime.Today;



        private readonly int _userId;
        AppContext db;

        public MainMenu(int userId)
        {
            InitializeComponent();
            _userId = userId;
            PersonAccountPanel.Visibility = Visibility.Hidden;
            SignUpDoctorPanel.Visibility = Visibility.Hidden;
            HospPanel.Visibility = Visibility.Hidden;
            SpecPanel.Visibility = Visibility.Hidden;
            DoctorPanel.Visibility = Visibility.Hidden;
            CheckPanel.Visibility = Visibility.Hidden;
            db = new AppContext();
            LoadUserData();
            UpdateDatesDisplay();

        }

        private void LoadRegionData()
        {
            if (_allRegions == null)
            {
                _allRegions = new ObservableCollection<Region>(db.Regions.ToList());
                _filteredRegions = new ObservableCollection<Region>(_allRegions);
                Region_ListView.ItemsSource = _filteredRegions;
            }
        }

        private void LoadHospitalData()
        {
            if (_allHospitals == null)
            {
                _allHospitals = new ObservableCollection<Hospital>(db.Hospitals.ToList());
                _filteredHospitals = new ObservableCollection<Hospital>(_allHospitals);
                Hosp_ListView.ItemsSource = _filteredHospitals;
            }
        }

        private void LoadSpecializationData()
        {
            if (_allSpecialization == null)
            {
                _allSpecialization = new ObservableCollection<Specialization>(db.Specializations.ToList());
                _filteredSpecialization = new ObservableCollection<Specialization>(_allSpecialization);
                Spec_ListView.ItemsSource = _filteredSpecialization;
            }
        }

        private void LoadDoctorData()
        {
            if (_allDoctor == null)
            {
                _allDoctor = new ObservableCollection<Doctor>(db.Doctors.ToList());
                _filteredDoctor = new ObservableCollection<Doctor>(_allDoctor);
                Doctor_ListView.ItemsSource = _filteredDoctor;
                DoctorTime_ListView.ItemsSource = _filteredDoctor;
            }
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
            SignUpDoctorPanel.Visibility = Visibility.Hidden;
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
            LoadRegionData();
            SignUpDoctorPanel.Visibility = Visibility.Visible;
            PersonAccountPanel.Visibility = Visibility.Hidden;
            MainMenuPanel.Visibility = Visibility.Hidden;
            RegionPanel.Visibility = Visibility.Visible;
            Region_ListView.ItemsSource = db.Regions.ToList();

        }

        private void Step1Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Visible;
            HospPanel.Visibility = Visibility.Hidden;
            SpecPanel.Visibility = Visibility.Hidden;
            DoctorPanel.Visibility = Visibility.Hidden;

            CityTextBlock.Text = "";
            HospitalTextBlock.Text = "";
            SpecTextBlock.Text = "";
            DoctorTextBlock.Text = "";
            TimeTextBlock.Text = "";

            Step1Button.Background = Brushes.SkyBlue;
            Step1Button.Foreground = Brushes.White;
            Step2Button.Background = Brushes.Transparent;
            Step2Button.Foreground = Brushes.SkyBlue;
            Step3Button.Background = Brushes.Transparent;
            Step3Button.Foreground = Brushes.SkyBlue;
            Step4Button.Background = Brushes.Transparent;
            Step4Button.Foreground = Brushes.SkyBlue;
            Step5Button.Background = Brushes.Transparent;
            Step5Button.Foreground = Brushes.SkyBlue;

            Step1Button.IsEnabled = true;
            Step2Button.IsEnabled = false;
            Step3Button.IsEnabled = false;
            Step4Button.IsEnabled = false;
            Step5Button.IsEnabled = false;
        }

        private void Step2Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Hidden;
            HospPanel.Visibility = Visibility.Visible;
            SpecPanel.Visibility = Visibility.Hidden;
            DoctorPanel.Visibility = Visibility.Hidden;

            HospitalTextBlock.Text = "";
            SpecTextBlock.Text = "";
            DoctorTextBlock.Text = "";
            TimeTextBlock.Text = "";

            Step1Button.Background = Brushes.Transparent;
            Step1Button.Foreground = Brushes.SkyBlue;
            Step2Button.Background = Brushes.SkyBlue;
            Step2Button.Foreground = Brushes.White;
            Step3Button.Background = Brushes.Transparent;
            Step3Button.Foreground = Brushes.SkyBlue;
            Step4Button.Background = Brushes.Transparent;
            Step4Button.Foreground = Brushes.SkyBlue;
            Step5Button.Background = Brushes.Transparent;
            Step5Button.Foreground = Brushes.SkyBlue;

            Step1Button.IsEnabled = true;
            Step2Button.IsEnabled = true;
            Step3Button.IsEnabled = false;
            Step4Button.IsEnabled = false;
            Step5Button.IsEnabled = false;
        }

        private void Step3Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Hidden;
            HospPanel.Visibility = Visibility.Hidden;
            SpecPanel.Visibility = Visibility.Visible;
            DoctorPanel.Visibility = Visibility.Hidden;

            SpecTextBlock.Text = "";
            DoctorTextBlock.Text = "";
            TimeTextBlock.Text = "";

            Step1Button.Background = Brushes.Transparent;
            Step1Button.Foreground = Brushes.SkyBlue;
            Step2Button.Background = Brushes.Transparent;
            Step2Button.Foreground = Brushes.SkyBlue;
            Step3Button.Background = Brushes.SkyBlue;
            Step3Button.Foreground = Brushes.White;
            Step4Button.Background = Brushes.Transparent;
            Step4Button.Foreground = Brushes.SkyBlue;
            Step5Button.Background = Brushes.Transparent;
            Step5Button.Foreground = Brushes.SkyBlue;

            Step1Button.IsEnabled = true;
            Step2Button.IsEnabled = true;
            Step3Button.IsEnabled = true;
            Step4Button.IsEnabled = false;
            Step5Button.IsEnabled = false;
        }

        private void Step4Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Hidden;
            HospPanel.Visibility = Visibility.Hidden;
            SpecPanel.Visibility = Visibility.Hidden;
            DoctorPanel.Visibility = Visibility.Visible;
            CheckPanel.Visibility = Visibility.Hidden;

            DoctorTextBlock.Text = "";
            TimeTextBlock.Text = "";


            Step1Button.Background = Brushes.Transparent;
            Step1Button.Foreground = Brushes.SkyBlue;
            Step2Button.Background = Brushes.Transparent;
            Step2Button.Foreground = Brushes.SkyBlue;
            Step3Button.Background = Brushes.Transparent;
            Step3Button.Foreground = Brushes.SkyBlue;
            Step4Button.Background = Brushes.SkyBlue;
            Step4Button.Foreground = Brushes.White;
            Step5Button.Background = Brushes.Transparent;
            Step5Button.Foreground = Brushes.SkyBlue;

            Step1Button.IsEnabled = true;
            Step2Button.IsEnabled = true;
            Step3Button.IsEnabled = true;
            Step4Button.IsEnabled = true;
            Step5Button.IsEnabled = false;
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
                LoadHospitalData();
                CityTextBlock.Text = selectedRegion.Reg;
                Step1Button.Background = Brushes.Transparent;
                Step1Button.Foreground = Brushes.SkyBlue;
                Step2Button.Background = Brushes.SkyBlue;
                Step2Button.Foreground = Brushes.White;
                Step2Button.IsEnabled = true;
                RegionPanel.Visibility = Visibility.Hidden;
                HospPanel.Visibility = Visibility.Visible;

                // Фильтруем больницы по выбранному региону
                _filteredHospitals.Clear();
                foreach (var hospital in _allHospitals)
                {
                    if (hospital.RegionId == selectedRegion.Id)
                    {
                        _filteredHospitals.Add(hospital);
                    }
                }
                Step1Button.IsEnabled = true;
                Step2Button.IsEnabled = true;

                Hosp_ListView.ItemsSource = null; // Принудительное обновление ListView
                Hosp_ListView.ItemsSource = _filteredHospitals;
            }
        }

        private void SearchHosp(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchHosp_TextBox.Text.Trim().ToLower();

                // Получаем выбранный регион
                if (Region_ListView.SelectedItem is Region selectedRegion)
                {
                    _filteredHospitals.Clear();

                    foreach (var hospital in _allHospitals)
                    {
                        // Проверяем соответствие региону и совпадение с текстом поиска
                        if (hospital.RegionId == selectedRegion.Id && hospital.Hosp.ToLower().Contains(searchText))
                        {
                            _filteredHospitals.Add(hospital);
                        }
                    }

                    Hosp_ListView.ItemsSource = null;
                    Hosp_ListView.ItemsSource = _filteredHospitals;
                }
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
                LoadSpecializationData();
                HospitalTextBlock.Text = selectedHospital.Hosp;
                Step2Button.Background = Brushes.Transparent;
                Step2Button.Foreground = Brushes.SkyBlue;
                Step3Button.Background = Brushes.SkyBlue;
                Step3Button.Foreground = Brushes.White;
                HospPanel.Visibility = Visibility.Hidden;
                SpecPanel.Visibility = Visibility.Visible;

                // Фильтруем специальности по выбранной больнице
                _filteredSpecialization.Clear();
                foreach (var spec in _allSpecialization)
                {
                    if (spec.HospitalId == selectedHospital.Id)
                    {
                        _filteredSpecialization.Add(spec);
                    }
                }
                Step1Button.IsEnabled = true;
                Step2Button.IsEnabled = true;
                Step3Button.IsEnabled = true;

                Spec_ListView.ItemsSource = null; // Принудительное обновление ListView
                Spec_ListView.ItemsSource = _filteredSpecialization;
            }
        }

        private void SearchSpec(object sender, TextChangedEventArgs e)
        {
            {
                try
                {
                    string searchText = SearchSpec_TextBox.Text.Trim().ToLower();
                    if (Hosp_ListView.SelectedItem is Hospital selectedHosp)
                    {
                        _filteredSpecialization.Clear();
                        foreach (var spec in _allSpecialization)
                        {
                            if (spec.HospitalId == selectedHosp.Id && spec.Spec.ToLower().Contains(searchText))
                            {
                                _filteredSpecialization.Add(spec);
                            }
                        }
                        Spec_ListView.ItemsSource = null;
                        Spec_ListView.ItemsSource = _filteredSpecialization;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }

            }
        }

        private void Spec_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Spec_ListView.SelectedItem is Specialization selectedSpec)
            {
                LoadDoctorData();
                SpecTextBlock.Text = selectedSpec.Spec;
                Step2Button.Background = Brushes.Transparent;
                Step2Button.Foreground = Brushes.SkyBlue;
                Step3Button.Background = Brushes.Transparent;
                Step3Button.Foreground = Brushes.SkyBlue;
                Step4Button.Background = Brushes.SkyBlue;
                Step4Button.Foreground = Brushes.White;

                Step1Button.IsEnabled = true;
                Step2Button.IsEnabled = true;
                Step3Button.IsEnabled = true;
                Step4Button.IsEnabled = true;

                RegionPanel.Visibility = Visibility.Hidden;
                HospPanel.Visibility = Visibility.Hidden;
                SpecPanel.Visibility = Visibility.Hidden;
                DoctorPanel.Visibility = Visibility.Visible;

                // фильтруем доктора по выбранной специальности
                _filteredDoctor.Clear();
                foreach (var doctor in _allDoctor)
                {
                    if (doctor.SpecId == selectedSpec.Id)
                    {
                        _filteredDoctor.Add(doctor);
                    }
                }

                Doctor_ListView.ItemsSource = null; // Принудительное обновление ListView
                Doctor_ListView.ItemsSource = _filteredDoctor;
            }
        }

        private void SearchDoctor(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchDoctor_TextBox.Text.Trim().ToLower();
                if (Spec_ListView.SelectedItem is Specialization selectedSpec)
                {
                    _filteredDoctor.Clear();
                    foreach (var doc in _allDoctor)
                    {
                        if (doc.SpecId == selectedSpec.Id && doc.Name.ToLower().Contains(searchText))
                        {
                            _filteredDoctor.Add(doc);
                        }
                    }
                    Doctor_ListView.ItemsSource = null;
                    Doctor_ListView.ItemsSource = _filteredDoctor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void TimeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Doctor selectedDoctor)
            {
                int dayIndex = Grid.GetColumn(button);
                var dateTextBlock = FindName($"Date{dayIndex}") as TextBlock;
                if (dateTextBlock != null)
                {
                    string selectedRegion = CityTextBlock.Text;
                    string selectedHospital = HospitalTextBlock.Text;
                    string selectedSpecialization = SpecTextBlock.Text;
                    string selectedDate = dateTextBlock.Text;
                    string selectedTime = button.Content.ToString();

                    // Обновляем интерфейс
                    TimeTextBlock.Text = $"{selectedDate} {selectedTime}ч.";

                    DoctorTextBlock.Text = selectedDoctor.Name;
                    Step2Button.Background = Brushes.Transparent;
                    Step2Button.Foreground = Brushes.SkyBlue;
                    Step3Button.Background = Brushes.Transparent;
                    Step3Button.Foreground = Brushes.SkyBlue;
                    Step4Button.Background = Brushes.Transparent;
                    Step4Button.Foreground = Brushes.SkyBlue;
                    Step5Button.Background = Brushes.SkyBlue;
                    Step5Button.Foreground = Brushes.White;

                    Step1Button.IsEnabled = true;
                    Step2Button.IsEnabled = true;
                    Step3Button.IsEnabled = true;
                    Step4Button.IsEnabled = true;
                    Step5Button.IsEnabled = true;

                    RegionPanel.Visibility = Visibility.Hidden;
                    HospPanel.Visibility = Visibility.Hidden;
                    SpecPanel.Visibility = Visibility.Hidden;
                    DoctorPanel.Visibility = Visibility.Hidden;
                    CheckPanel.Visibility = Visibility.Visible;

                    SelectedRegion.Text = selectedRegion;
                    SelectedHospital.Text = selectedHospital;
                    SelectedSpecialization.Text = selectedSpecialization;
                    SelectedDoctor.Text = selectedDoctor.Name;
                    SelectedDate.Text = selectedDate + " " + selectedTime + " ч.";
                }
            }
        }

        private void UpdateDatesDisplay()
        {
            var culture = new CultureInfo("ru-RU");

            // Используем _currentWeekStart вместо DateTime.Today
            var startDate = _currentWeekStart;

            // Обновляем 7 дней (начиная с _currentWeekStart)
            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);
                var dayTextBlock = FindName($"Day{i}") as TextBlock;
                var dateTextBlock = FindName($"Date{i}") as TextBlock;

                if (dayTextBlock != null)
                {
                    dayTextBlock.Text = date.Date == DateTime.Today ? "Сегодня" :
                                      date.ToString("ddd", culture);
                }

                if (dateTextBlock != null)
                {
                    dateTextBlock.Text = date.ToString("d MMMM", culture);
                }
            }

            // Обновляем интервал недели
            var endDate = startDate.AddDays(6);
            interval_time.Text = $"С {startDate.ToString("d MMMM", culture)} по {endDate.ToString("d MMMM", culture)}";

            if (DateTime.Today != startDate)
            {
                PrevWeek_Button.IsEnabled = true;
                PrevWeek_Button.Background = Brushes.SkyBlue;
                PrevWeek_Button.Foreground =Brushes.White;
            }
            else
            {
                PrevWeek_Button.IsEnabled = false;
                PrevWeek_Button.Background = Brushes.Transparent;
                PrevWeek_Button.Foreground = Brushes.SkyBlue;
                NextWeek_Button.Background = Brushes.SkyBlue;
                NextWeek_Button.Foreground = Brushes.White;
            }
        }


        // Обработчик для кнопки "Следующая неделя"

        private void PrevWeek_Button_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            UpdateDatesDisplay();

        }

        private void NextWeek_Button_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            UpdateDatesDisplay();
        }

        
    }
}