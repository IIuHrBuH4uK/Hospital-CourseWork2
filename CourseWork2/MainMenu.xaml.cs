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
    using System.IO;
    using System.Windows.Markup;
    using Microsoft.Win32;
    using static System.Net.Mime.MediaTypeNames;
    using System.Xml.Linq;
    using System.Diagnostics;
using System.Data.Entity.Core.Mapping;


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
        private ObservableCollection<Ticket> _allTicket;
        private ObservableCollection<Call> _allCall;

        private DateTime _currentWeekStart = DateTime.Today;


        private Ticket _currentTicket;
        private bool _safeTicket;
        private readonly int _userId;
        AppContext db;

        public MainMenu(int userId)
        {
            InitializeComponent();
            _userId = userId;


            HidePersonAccountPanel();
            HideSignUpDoctorPanel();
            HideCallDoctorPanel();
            HideRecordsAndAppealsPanel();




            db = new AppContext();
            LoadUserData();
            UpdateDatesDisplay();

        }

        // Метод загрузки областей
        private void LoadRegionData()
        {
            if (_allRegions == null)
            {
                _allRegions = new ObservableCollection<Region>(db.Regions.ToList());
                _filteredRegions = new ObservableCollection<Region>(_allRegions);
                Region_ListView.ItemsSource = _filteredRegions;
            }
        }

        // Метод загрузки больниц
        private void LoadHospitalData()
        {
            if (_allHospitals == null)
            {
                _allHospitals = new ObservableCollection<Hospital>(db.Hospitals.ToList());
                _filteredHospitals = new ObservableCollection<Hospital>(_allHospitals);
                Hosp_ListView.ItemsSource = _filteredHospitals;
            }
        }

        // Мето загрузки специальностей
        private void LoadSpecializationData()
        {
            if (_allSpecialization == null)
            {
                _allSpecialization = new ObservableCollection<Specialization>(db.Specializations.ToList());
                _filteredSpecialization = new ObservableCollection<Specialization>(_allSpecialization);
                Spec_ListView.ItemsSource = _filteredSpecialization;
            }
        }

        // Метод загрузки врачей
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

        private void LoadTicketData()
        {
            var userTickets = db.Tickets.Where(t => t.UserId == _userId).ToList();

            if (userTickets.Count == 0)
            {
                // Если у пользователя нет билетов, можно загрузить все (если нужно)
                if (_allTicket == null)
                {
                    _allTicket = new ObservableCollection<Ticket>(db.Tickets.ToList());
                    Tickets_ListView.ItemsSource = null;
                }
            }
            else
            {
                // Если билеты есть, обновляем коллекцию
                _allTicket = new ObservableCollection<Ticket>(userTickets);
                Tickets_ListView.ItemsSource = _allTicket;
            }

        }

        private void LoadCallData()
        {
            var userTickets = db.Calls.Where(t => t.UserId == _userId).ToList();

            if (userTickets.Count == 0)
            {
                // Если у пользователя нет билетов, можно загрузить все (если нужно)
                if (_allCall == null)
                {
                    _allCall = new ObservableCollection<Call>(db.Calls.ToList());
                    Calls_ListView.ItemsSource = null;
                }
            }
            else
            {
                // Если билеты есть, обновляем коллекцию
                _allCall = new ObservableCollection<Call>(userTickets);
                Calls_ListView.ItemsSource = _allCall;
            }
        }

        //Метод загрузки данных из базы данных для конкретного пользователя
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
                PersonBirthDay_DatePicker.Text = user.Birthday;
                GenderCombobox.SelectedItem = GenderCombobox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == user.Gender);
            }
        }

        private bool NotNullUser()
        {
            if (textBoxSurname.Text == null || textBoxName.Text == null || textBoxPatronymic.Text == null || textBoxSnils.Text == null || textBoxLocation.Text == null || PersonBirthDay_DatePicker.Text == null || GenderCombobox.SelectedItem == null)
            {
                MessageBox.Show("Проверьте заполненность личного кабинета", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        // Кнопка выхода
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            DeletTicket();
            Close();
        }

        // Кнопка "Личный кабинет"
        private void Person_Account_Button_Click(object sender, RoutedEventArgs e)
        {
            PersonAccountPanel.Visibility = Visibility.Visible;
            TitlePersonAccount_TextBlock.Visibility = Visibility.Visible;
            PersonAccount_Grid.Visibility = Visibility.Visible;
            SafePersonButton.Visibility = Visibility.Visible;
            HideMainMenuPanel();
            HideSignUpDoctorPanel();
            HideCallDoctorPanel();
            HideRecordsAndAppealsPanel();
            DeletTicket();

        }

        // Кнопка "Сохранить" в личном кабинете
        private void SafePersonButton_Click(object sender, RoutedEventArgs e)
        {
            string surname = textBoxSurname.Text.Trim();
            string name = textBoxName.Text.Trim();
            string patronymic = textBoxPatronymic.Text.Trim();
            string snils = textBoxSnils.Text.Trim();
            string location = textBoxLocation.Text.Trim();
            string birthday = PersonBirthDay_DatePicker.Text.Trim();
            string gender = (GenderCombobox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // Запись данных, введеных пользователем в базу данных
            var user = db.Users.FirstOrDefault(u => u.Id == _userId);
            if (user != null)
            {
                user.MiddleName = surname;
                user.FirstName = name;
                user.LastName = patronymic;
                user.SNILS = snils;
                user.Address = location;
                user.Gender = gender;
                user.Birthday = birthday;

                db.SaveChanges();
                System.Windows.MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        // Кнопка "Записаться на приём к врачу"
        private void SignUpDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            if(!NotNullUser())
            return;

            LoadRegionData();
            Region_ListView.ItemsSource = db.Regions.ToList();

            HideMainMenuPanel();
            HidePersonAccountPanel();
            HideCallDoctorPanel();
            HideRecordsAndAppealsPanel();

            ResetSignUpDoctor();

            SignUpDoctorPanel.Visibility = Visibility.Visible;

            TitleSignUpDoctor_TextBlock.Visibility = Visibility.Visible;
            Steps.Visibility = Visibility.Visible;
            Steps_Grid.Visibility = Visibility.Visible;
            PrintButton.IsEnabled = false;
            SafeButton.IsEnabled = false;

        }

        // Кнопка "Шаг 1" в записи к врачу
        private void Step1Button_Click(object sender, RoutedEventArgs e)
        {
            ResetSignUpDoctor();
        }

        // Метод сброса всего выбранного пользователем, возвращение к 1 шагу и удаление талона, если он записался в панели записи приёма к врачу
        private void ResetSignUpDoctor()
        {
            RegionPanel.Visibility = Visibility.Visible;
            HospPanel.Visibility = Visibility.Collapsed;
            SpecPanel.Visibility = Visibility.Collapsed;
            DoctorPanel.Visibility = Visibility.Collapsed;
            CheckPanel.Visibility = Visibility.Collapsed;

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

            Search_TextBox.Text = null;
            SearchHosp_TextBox.Text = null;
            SearchSpec_TextBox.Text = null;
            SearchDoctor_TextBox.Text = null;

            Region_ListView.SelectedItem = null;
            Hosp_ListView.SelectedItem = null;
            Spec_ListView.SelectedItem = null;
            Doctor_ListView.SelectedItem = null;
            DoctorTime_ListView.SelectedItem = null;

            PrintButton.IsEnabled = false;
            SafeButton.IsEnabled = false;

            DeletTicket();
        }

        // Кнопка "Шаг 2" в записи к врачу
        private void Step2Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Collapsed;
            HospPanel.Visibility = Visibility.Visible;
            SpecPanel.Visibility = Visibility.Collapsed;
            DoctorPanel.Visibility = Visibility.Collapsed;
            CheckPanel.Visibility = Visibility.Collapsed;

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

            SearchHosp_TextBox.Text = null;
            SearchSpec_TextBox.Text = null;
            SearchDoctor_TextBox.Text = null;

            Hosp_ListView.SelectedItem = null;
            Spec_ListView.SelectedItem = null;
            Doctor_ListView.SelectedItem = null;
            DoctorTime_ListView.SelectedItem = null;

            DeletTicket();
        }

        // Кнопка "Шаг 3" в записи к врачу
        private void Step3Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Collapsed;
            HospPanel.Visibility = Visibility.Collapsed;
            SpecPanel.Visibility = Visibility.Visible;
            DoctorPanel.Visibility = Visibility.Collapsed;
            CheckPanel.Visibility = Visibility.Collapsed;

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

            SearchSpec_TextBox.Text = null;
            SearchDoctor_TextBox.Text = null;

            Spec_ListView.SelectedItem = null;
            Doctor_ListView.SelectedItem = null;
            DoctorTime_ListView.SelectedItem = null;

            DeletTicket();
        }

        // Кнопка "Шаг 4" в записи к врачу
        private void Step4Button_Click(object sender, RoutedEventArgs e)
        {
            RegionPanel.Visibility = Visibility.Collapsed;
            HospPanel.Visibility = Visibility.Collapsed;
            SpecPanel.Visibility = Visibility.Collapsed;
            DoctorPanel.Visibility = Visibility.Visible;
            CheckPanel.Visibility = Visibility.Collapsed;

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
            ;
            SearchDoctor_TextBox.Text = null;
            Doctor_ListView.SelectedItem = null;
            DoctorTime_ListView.SelectedItem = null;

            DeletTicket();
        }

        // Кнопка "Шаг 5" в записи к врачу. Проверка информации, пока бничего не реализовал
        private void Step5Button_Click(object sender, RoutedEventArgs e)
        {

        }

        // Метод поиска области в строке поиска
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
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Метод выбора пользователем области
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

                RegionPanel.Visibility = Visibility.Collapsed;
                HospPanel.Visibility = Visibility.Visible;
                SpecPanel.Visibility = Visibility.Collapsed;
                DoctorPanel.Visibility = Visibility.Collapsed;
                CheckPanel.Visibility = Visibility.Collapsed;


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

        // Метод поиска больниц в строке поиска
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
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Метод выбора пользователем больницы
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
                RegionPanel.Visibility = Visibility.Collapsed;
                HospPanel.Visibility = Visibility.Collapsed;
                SpecPanel.Visibility = Visibility.Visible;
                DoctorPanel.Visibility = Visibility.Collapsed;
                CheckPanel.Visibility = Visibility.Collapsed;

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

        // метод поиска специальности в строке поиска
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
                    System.Windows.MessageBox.Show($"Ошибка: {ex.Message}");
                }

            }
        }

        // Метод выбора пользователем специальности врача 
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

                RegionPanel.Visibility = Visibility.Collapsed;
                HospPanel.Visibility = Visibility.Collapsed;
                SpecPanel.Visibility = Visibility.Collapsed;
                DoctorPanel.Visibility = Visibility.Visible;
                CheckPanel.Visibility = Visibility.Collapsed;

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

        // Поиск врача в строке поиска
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
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Выбор времени пользователем у врача
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

                    RegionPanel.Visibility = Visibility.Collapsed;
                    HospPanel.Visibility = Visibility.Collapsed;
                    SpecPanel.Visibility = Visibility.Collapsed;
                    DoctorPanel.Visibility = Visibility.Collapsed;
                    CheckPanel.Visibility = Visibility.Visible;

                    SelectedRegion.Text = selectedRegion;
                    SelectedHospital.Text = selectedHospital;
                    SelectedSpecialization.Text = selectedSpecialization;
                    SelectedDoctor.Text = selectedDoctor.Name;
                    SelectedDate.Text = selectedDate + " " + selectedTime + " ч.";

                    SafeInfo();
                }
            }
        }

        // Метод обновления текущей даты у врачей.
        private void UpdateDatesDisplay()
        {
            var culture = new CultureInfo("ru-RU");

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
                PrevWeek_Button.Foreground = Brushes.White;
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


        // Метод кнопки "Следующая неделя"
        private void PrevWeek_Button_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            UpdateDatesDisplay();

        }

        // Метод кнопки "Предыдущая неделя"
        private void NextWeek_Button_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            UpdateDatesDisplay();
        }

        //Метод сохранения данных для талончика и запись его в базу данных
        private void SafeInfo()
        {
            _safeTicket = false;
            string region = SelectedRegion.Text.Trim();
            string hospital = SelectedHospital.Text.Trim();
            string specialization = SelectedSpecialization.Text.Trim();
            string doctor = SelectedDoctor.Text.Trim();
            string date = SelectedDate.Text.Trim();
            string numbertalon = GenerateNextTicketNumber();

            Ticket ticket = new Ticket(_userId, region, hospital, specialization, doctor, date, numbertalon);
            _currentTicket = ticket;

            db.Tickets.Add(ticket);
            db.SaveChanges();

        }

        //Метод генерации талона из базы данных для печати. Помогла нейросеть.
        private void GenerateAndPrintTicket(string region, string hospital, string specialization,
                                  string doctor, string date, string numbertalon)
        {
            try
            {
                // Создаем FlowDocument
                FlowDocument doc = new FlowDocument();
                doc.PagePadding = new Thickness(100);
                doc.Blocks.Add(new Paragraph(new Run("Талон на прием к врачу\n\n"))
                {
                    FontSize = 20,
                    TextAlignment = TextAlignment.Center
                });

                // Добавляем информацию
                doc.Blocks.Add(new Paragraph(new Run($"Регион: {region}")));
                doc.Blocks.Add(new Paragraph(new Run($"Больница: {hospital}")));
                doc.Blocks.Add(new Paragraph(new Run($"Специализация: {specialization}")));
                doc.Blocks.Add(new Paragraph(new Run($"Врач: {doctor}")));
                doc.Blocks.Add(new Paragraph(new Run($"Дата: {date}")));
                doc.Blocks.Add(new Paragraph(new Run($"\n ID пользователя: {_userId}")));
                doc.Blocks.Add(new Paragraph(new Run($"Номер талона: {numbertalon}")));
                doc.Blocks.Add(new Paragraph(new Run("\n\nСпасибо за использование нашей системы!")));


                // Создаем диалог печати
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Настраиваем документ для печати
                    IDocumentPaginatorSource paginatorSource = doc;
                    printDialog.PrintDocument(paginatorSource.DocumentPaginator, "Талон на прием");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при печати: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка печати талончика
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли данные для сохранения
            if (_currentTicket == null)
            {
                System.Windows.MessageBox.Show("Нет данных для сохранения", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            GenerateAndPrintTicket(_currentTicket.Region, _currentTicket.Hospital, _currentTicket.Specialization, _currentTicket.Doctor, _currentTicket.Date, _currentTicket.Numbertalon);
        }

        // Кнопка сохранения талончика на компьютер
        private void SafeButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли данные для сохранения
            if (_currentTicket == null)
            {
                System.Windows.MessageBox.Show("Нет данных для сохранения", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Настраиваем диалог сохранения файла
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveDialog.FileName = $"Талон_{DateTime.Now:yyyyMMddHHmmss}.txt";

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    string fileContent = $"Талон на прием к врачу\n\n" +
                                       $"Регион: {_currentTicket.Region}\n" +
                                       $"Больница: {_currentTicket.Hospital}\n" +
                                       $"Специализация: {_currentTicket.Specialization}\n" +
                                       $"Врач: {_currentTicket.Doctor}\n" +
                                       $"Дата: {_currentTicket.Date}\n" +
                                       $"\nID пользователя: {_currentTicket.UserId}\n" +
                                       $"Номер талона: {_currentTicket.Numbertalon}" +
                                       $"\n\nСпасибо за использование нашей системы!";

                    File.WriteAllText(saveDialog.FileName, fileContent, Encoding.UTF8);

                    System.Windows.MessageBox.Show($"Данные успешно сохранены в файл:\n{saveDialog.FileName}",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Генерация номеров талончика. Помогла нейросеть
        private string GenerateNextTicketNumber()
        {

            // Проверяем, что все необходимые данные выбраны
            if (string.IsNullOrEmpty(SelectedRegion.Text) ||
                string.IsNullOrEmpty(SelectedHospital.Text) ||
                string.IsNullOrEmpty(SelectedDoctor.Text) ||
                string.IsNullOrEmpty(SelectedDate.Text))
            {
                return "1"; // Если не все данные выбраны, начинаем с 1
            }

            string region = SelectedRegion.Text.Trim();
            string hospital = SelectedHospital.Text.Trim();
            string doctor = SelectedDoctor.Text.Trim();
            string date = SelectedDate.Text.Split(' ')[0]; // Берем только дату без времени

            // Ищем записи на эту дату к этому врачу в этой больнице и регионе
            var existingTickets = db.Tickets
                .Where(t => t.Region == region &&
                           t.Hospital == hospital &&
                           t.Doctor == doctor &&
                           t.Date.StartsWith(date)) // Проверяем только начало строки с датой
                .OrderByDescending(t => t.Id)
                .ToList();

            if (existingTickets.Any())
            {
                // Если есть записи, берем последний номер и увеличиваем на 1
                var lastTicket = existingTickets.First();
                if (int.TryParse(lastTicket.Numbertalon, out int lastNumber))
                {
                    return (lastNumber + 1).ToString();
                }
            }

            // Если записей нет или не удалось распарсить номер, начинаем с 1
            return "1";
        }

        //Удаление записи в базе данных текущего талона
        private void DeletTicket()
        {
            if (_currentTicket == null || _safeTicket) return;

            try
            {
                // Находим запись в базе данных по ID
                var ticketToDelete = db.Tickets.FirstOrDefault(t => t.Id == _currentTicket.Id);
                if (ticketToDelete != null)
                {
                    db.Tickets.Remove(ticketToDelete);
                    db.SaveChanges();
                    _currentTicket = null; // Сбрасываем текущий талон
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при удалении записи: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SafeTicketButton_Click(object sender, RoutedEventArgs e)
        {
            PrintButton.IsEnabled = true;
            SafeButton.IsEnabled = true;
            _safeTicket = true;
            System.Windows.MessageBox.Show("Талон будет сохранен в базе данных. Можете его рапечатать или сохранить на компьютер", "Информация",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Кнопка в левом меню на вызов врача
        private void CallDoctor_Button_Click(object sender, RoutedEventArgs e)
        {
            CallDoctorPanel.Visibility = Visibility.Visible;
            TitleCallDoctor_TextBlock.Visibility = Visibility.Visible;
            CallDoctor_Grid.Visibility = Visibility.Visible;
            PatientChoicePanel.Visibility = Visibility.Visible;
            MeCall_Button.Visibility = Visibility.Visible;
            OtherCall_Button.Visibility = Visibility.Visible;
            

            MeCallPanel.Visibility = Visibility.Collapsed;
            MeCallBack_Button.Visibility = Visibility.Collapsed;
            OtherCallPanel.Visibility = Visibility.Collapsed;
            OtherSymptomsPanel.Visibility = Visibility.Collapsed;
            OtherSymptomsBack_Button.Visibility = Visibility.Collapsed;
            // добавить элементы в OtherCallPanel

            MePolisPanel.Visibility = Visibility.Collapsed;
            MePolisBack_Button.Visibility = Visibility.Collapsed;
            MeSymptomsPanel.Visibility = Visibility.Collapsed;
            MeSymptomsBack_Button.Visibility = Visibility.Collapsed;


            HideMainMenuPanel();
            HidePersonAccountPanel();
            HideSignUpDoctorPanel();
            HideRecordsAndAppealsPanel();


        }

        // Кнопка нажатия "Мне" на панели выбора кому вызывать врача
        private void MeCall_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!NotNullUser())
                return;

            PatientChoicePanel.Visibility = Visibility.Collapsed;
            MeCallPanel.Visibility = Visibility.Visible;
            MeCallBack_Button.Visibility = Visibility.Visible;

            var user = db.Users.FirstOrDefault(a => a.Id == _userId);
            if (user != null)
            {
                FullName_TextBox.Text = (user.MiddleName + " " + user.FirstName + " " + user.LastName);
                BirthDate_TextBox.Text = (user.Birthday);
                MeGenderComboBox.SelectedItem = MeGenderComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == user.Gender);
            }
        }

        // Кнопка нажатия "Другому человеку" на панели выбора кому вызывать врача
        private void OtherCall_Button_Click(object sender, RoutedEventArgs e)
        {
            PatientChoicePanel.Visibility = Visibility.Collapsed;
            OtherCallPanel.Visibility = Visibility.Visible;
            OtherCall_Button.Visibility = Visibility.Visible;
            OtherCallBack_Button.Visibility = Visibility.Visible;
        }

        // Кнопка редактирования личной информации на панели MeCall
        private void EditInfo_Button_Click(object sender, RoutedEventArgs e)
        {
            PersonAccountPanel.Visibility = Visibility.Visible;
            TitlePersonAccount_TextBlock.Visibility = Visibility.Visible;
            PersonAccount_Grid.Visibility = Visibility.Visible;
            SafePersonButton.Visibility = Visibility.Visible;

            HideMainMenuPanel();
            HideSignUpDoctorPanel();
            HideCallDoctorPanel();
            HideRecordsAndAppealsPanel();

        }

        // Кнопка подтверждения личной информации и переход на следующую панель с полисом
        private void ConfirmMeButton_Click(object sender, RoutedEventArgs e)
        {
            MeCallBack_Button.Visibility = Visibility.Collapsed;
            MeCallPanel.Visibility = Visibility.Collapsed;
            MePolisPanel.Visibility = Visibility.Visible;
            MePolisBack_Button.Visibility = Visibility.Visible;
            NumberPolis_TextBox.Text = db.Users.FirstOrDefault(u => u.Id == _userId).Polis;
            NumberPhone_TextBox.Text = db.Users.FirstOrDefault(_ => _.Id == _userId).Phone;
            Address_TextBox.Text = db.Users.FirstOrDefault(u => u.Id == _userId).Address;

        }

        // Кнопка получения информации о полисе ОМС
        private void PolisWebButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.gosuslugi.ru/help/faq/oms/4863");
        }

        // Кнопка возвращения к меню выбора кому вызвать врача
        private void MeCallBack_Button_Click(object sender, RoutedEventArgs e)
        {
            MeCallPanel.Visibility = Visibility.Collapsed;
            MeCallBack_Button.Visibility = Visibility.Collapsed;
            PatientChoicePanel.Visibility = Visibility.Visible;
        }

        // Кнопка возвращения к проверке информации
        private void MePolisBack_Button_Click(object sender, RoutedEventArgs e)
        {
            MePolisBack_Button.Visibility = Visibility.Collapsed;
            MePolisPanel.Visibility = Visibility.Collapsed;
            MeCallPanel.Visibility = Visibility.Visible;
            MeCallBack_Button.Visibility = Visibility.Visible;
        }

        // Кнопка возвращения к проверке полиса, телефона и адреса
        private void MeSymptomsBack_Button_Click(object sender, RoutedEventArgs e)
        {
            MeSymptomsBack_Button.Visibility = Visibility.Collapsed;
            MeSymptomsPanel.Visibility = Visibility.Collapsed;
            MePolisPanel.Visibility = Visibility.Visible;
            MePolisBack_Button.Visibility = Visibility.Visible;

        }

        // Кнопка проверки полиса ОМС
        private void NumberPolisPhone_Button_Click(object sender, RoutedEventArgs e)
        {

            string phone = NumberPhone_TextBox.Text.Trim();
            string polis = NumberPolis_TextBox.Text.Trim();

            var user = db.Users.FirstOrDefault(u => u.Id == _userId);

            if (NumberPolis_TextBox.Text.Length < 16)
            {
                NumberPolis_TextBox.Background = Brushes.LightSkyBlue;
                NumberPolis_TextBox.ToolTip = "Введите номер из 16 символов";

            }
            else
            {
                NumberPolis_TextBox.Background = Brushes.Transparent;
                NumberPolis_TextBox.ToolTip = null;


                if (user != null)
                {
                    user.Polis = polis;
                    user.Phone = phone;


                    db.SaveChanges();
                }
            }

            MePolisBack_Button.Visibility = Visibility.Collapsed;
            MePolisPanel.Visibility = Visibility.Collapsed;
            MeSymptomsPanel.Visibility = Visibility.Visible;
            MeSymptomsBack_Button.Visibility = Visibility.Visible;
        }


        // Запрещаем ввод текста в текстбокс с номером полиса
        private void NumberPolis_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        // Запрещаем ввод пробела в текст бокс с номером полиса
        private void NumberPolis_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Блокируем пробел (Key.Space)
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        // Разрешаем только цифры, '+', '-', '(', ')', пробел в текст бокс с номером телефона
        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var allowedChars = new[] { '+', '-', '(', ')', ' ' };
            if (!char.IsDigit(e.Text, 0) && !allowedChars.Contains(e.Text[0])) e.Handled = true;
        }

        // Запрещаем ввод пробела в текст бокс с номером телефона
        private void NumberPhone_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        // Кнопка сохранения симптомов и добавления их в базу данных
        private void SafeSymptoms_Button_Click(object sender, RoutedEventArgs e)
        {
            
            string symptoms = MeSymtopms_TextBox.Text.Trim();
            var user = db.Users.FirstOrDefault(u => u.Id == _userId);

            if (user != null)
            {
                // Создаём новую запись в таблице Calls
                var newCall = new Call
                {
                    UserId = _userId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    Address = user.Address,
                    Phone = user.Phone,
                    Birthdate = user.Birthday,
                    Gender = (MeGenderComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString(),
                    Symptoms = symptoms,
                };

                // Добавляем запись в таблицу Calls
                db.Calls.Add(newCall);
                db.SaveChanges();

                System.Windows.MessageBox.Show("Данные о вызове врача успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            MeSymptomsBack_Button.Visibility = Visibility.Collapsed;
        }

        // Оптимизация закрытия панелей
        // Скрываем панель главного экрана с информацией
        private void HideMainMenuPanel()
        {
            MainMenuPanel.Visibility = Visibility.Collapsed;

            //Сбрасываем все вложенные текстбоксы
            TitleMainMenu_TextBlock.Visibility = Visibility.Collapsed;
            DescriptionMainMenu_TextBlock.Visibility = Visibility.Collapsed;
        }

        // Скрываем панель личного кабинета
        private void HidePersonAccountPanel()
        {
            PersonAccountPanel.Visibility = Visibility.Collapsed;

            // Сбрасываем все вложнные панели и текста
            TitlePersonAccount_TextBlock.Visibility = Visibility.Collapsed;
            PersonAccount_Grid.Visibility = Visibility.Collapsed;
            SafePersonButton.Visibility = Visibility.Collapsed;

        }

        // Скрываем панель записи к врачу
        private void HideSignUpDoctorPanel()
        {
            SignUpDoctorPanel.Visibility = Visibility.Collapsed;

            // Сбрасываем все вложенные панели
            TitleSignUpDoctor_TextBlock.Visibility = Visibility.Collapsed;
            Steps.Visibility = Visibility.Collapsed;
            Steps_Grid.Visibility = Visibility.Collapsed;
            RegionPanel.Visibility = Visibility.Collapsed;
            HospPanel.Visibility = Visibility.Collapsed;
            SpecPanel.Visibility = Visibility.Collapsed;
            DoctorPanel.Visibility = Visibility.Collapsed;
            CheckPanel.Visibility = Visibility.Collapsed;
            CityTextBlock.Text = "";
            HospitalTextBlock.Text = "";
            SpecTextBlock.Text = "";
            DoctorTextBlock.Text = "";
            TimeTextBlock.Text = "";
        }

        //Скрываем панель вызова врача на дом
        private void HideCallDoctorPanel()
        {
            CallDoctorPanel.Visibility = Visibility.Collapsed;

            //Сбрасываем все вложенные панели и текста
            TitleCallDoctor_TextBlock.Visibility = Visibility.Collapsed;
            CallDoctor_Grid.Visibility = Visibility.Collapsed;
            PatientChoicePanel.Visibility = Visibility.Collapsed;
            MeCallPanel.Visibility = Visibility.Collapsed;
            MeCallBack_Button.Visibility = Visibility.Collapsed;
            MeCall_Button.Visibility = Visibility.Collapsed;
            OtherCallPanel.Visibility = Visibility.Collapsed;
            // добавить элементы в OtherCallPanel

            MePolisPanel.Visibility = Visibility.Collapsed;
            MePolisBack_Button.Visibility = Visibility.Collapsed;
            MeSymptomsPanel.Visibility = Visibility.Collapsed;
            MeSymptomsBack_Button.Visibility = Visibility.Collapsed;
            OtherSymptomsPanel.Visibility = Visibility.Collapsed;
            OtherCall_Button.Visibility = Visibility.Collapsed;
            OtherCallBack_Button.Visibility = Visibility.Collapsed;
            OtherSymptomsBack_Button.Visibility = Visibility.Collapsed;

            MeSymtopms_TextBox.Text = null;
            FullName_TextBox.Text = null;
            BirthDate_TextBox.Text = null;
            MeGenderComboBox.SelectedIndex = -1;
            NumberPhone_TextBox.Text = null;
            NumberPolis_TextBox.Text = null;
            Address_TextBox.Text = null;
            OtherFullName_TextBox.Text = null;
            OtherBirthDate_TextBox.Text = null;
            OtherGenderComboBox.SelectedIndex = -1;
            OtherNumberPolis_TextBox.Text = null;
            OtherNumberPhone_TextBox.Text = null;
            OtherAddress_TextBox.Text = null;
            OtherSymtopms_TextBox.Text = null;
        }

        private void HideRecordsAndAppealsPanel()
        {
            RecordsAndAppealsPanel.Visibility = Visibility.Collapsed;
            RecordsAndAppeals_Grid.Visibility = Visibility.Collapsed;
            Tickets_ListView.Visibility = Visibility.Collapsed;
            Calls_ListView.Visibility = Visibility.Collapsed;
            TicketButton.Visibility = Visibility.Collapsed;
            CallButton.Visibility = Visibility.Collapsed;
        }

        private void ConfirmOtcherButton_Click(object sender, RoutedEventArgs e)
        {
            OtherCallBack_Button.Visibility = Visibility.Collapsed;
            OtherCallPanel.Visibility = Visibility.Collapsed;
            OtherSymptomsPanel.Visibility = Visibility.Visible;
            OtherSymptomsBack_Button.Visibility = Visibility.Visible;
        }
        private void OtherSafeSymptoms_Button_Click(object sender, RoutedEventArgs e)
        {

            string birthdate = OtherBirthDate_TextBox.Text.Trim();
            string gender = (OtherGenderComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string number = OtherNumberPolis_TextBox.Text.Trim();
            string phone = OtherNumberPhone_TextBox.Text.Trim();
            string address = OtherAddress_TextBox.Text.Trim();
            string symptoms = OtherSymtopms_TextBox.Text;

            string name = OtherFullName_TextBox.Text.Trim();
            string[] nameparts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string middlename = nameparts.Length > 0 ? nameparts[0] : string.Empty;
            string firstname = nameparts.Length > 1 ? nameparts[1] : string.Empty;
            string lastname = nameparts.Length > 2 ? nameparts[2] : string.Empty;


            var user = db.Users.FirstOrDefault(u => u.Id == _userId);

            if (user != null)
            {
                // Создаём новую запись в таблице Calls
                var newCall = new Call
                {
                    UserId = _userId,
                    FirstName = firstname,
                    LastName = lastname,
                    MiddleName = middlename,
                    Address = address,
                    Phone = phone,
                    Birthdate = birthdate,
                    Gender = gender,
                    Symptoms = symptoms,
                }
            ;

                // Добавляем запись в таблицу Calls
                db.Calls.Add(newCall);
                db.SaveChanges();
                System.Windows.MessageBox.Show("Данные о вызове врача успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void OtherCallBack_Button_Click(object sender, RoutedEventArgs e)
        {
            OtherCallPanel.Visibility = Visibility.Collapsed;
            OtherCallBack_Button.Visibility = Visibility.Collapsed;
            PatientChoicePanel.Visibility = Visibility.Visible;
        }

        private void OtherSymptomsBack_Button_Click(object sender, RoutedEventArgs e)
        {
            OtherSymptomsPanel.Visibility = Visibility.Collapsed;
            OtherSymptomsBack_Button.Visibility = Visibility.Collapsed;
            OtherCallPanel.Visibility = Visibility.Visible;
            OtherCallBack_Button.Visibility = Visibility.Visible;
        }


        // Кнопка запись и обращения
        private void RecordsAndAppeals_Button_Click(object sender, RoutedEventArgs e)
        {

            LoadTicketData();
            HideMainMenuPanel();
            HidePersonAccountPanel();
            HideSignUpDoctorPanel();
            HideCallDoctorPanel();

            RecordsAndAppealsPanel.Visibility = Visibility.Visible;
            RecordsAndAppeals_Grid.Visibility = Visibility.Visible;
            Tickets_ListView.Visibility = Visibility.Visible;
            CallButton.Visibility = Visibility.Visible;
            TicketButton.Visibility = Visibility.Visible;
            Calls_ListView.Visibility = Visibility.Collapsed;

        }

        private void TicketButton_Click(object sender, RoutedEventArgs e)
        {
            TicketButton.Background = Brushes.SkyBlue;
            TicketButton.Foreground = Brushes.White;
            CallButton.Background = Brushes.White;
            CallButton.Foreground = Brushes.SkyBlue;
            
            Calls_ListView.Visibility = Visibility.Collapsed;
            Tickets_ListView.Visibility = Visibility.Visible;
            LoadTicketData();
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Tickets_ListView.Visibility = Visibility.Collapsed;
            Calls_ListView.Visibility = Visibility.Visible;

            TicketButton.Background = Brushes.White;
            TicketButton.Foreground= Brushes.SkyBlue;
            CallButton.Background = Brushes.SkyBlue;
            CallButton.Foreground= Brushes.White;
            LoadCallData();
        }
    }
}