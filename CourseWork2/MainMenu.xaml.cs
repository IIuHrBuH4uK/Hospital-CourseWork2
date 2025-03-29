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


namespace CourseWork2
    {
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private readonly int _userId;
        AppContext db;
        public MainMenu(int userId)
        {
            InitializeComponent();
            _userId = userId;
            PersonAccountPanel.Visibility = Visibility.Hidden;
            db = new AppContext();
            LoadUserData();

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

        }
    }
}
