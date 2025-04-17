using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Data.Entity.Migrations;
using MaterialDesignColors;


namespace CourseWork2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppContext db;

        public MainWindow()
        {
            InitializeComponent();
            db = new AppContext();
        }


        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"); //Для проверки почты (помогла нейросеть)
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            RegUser();
        }

        private async void RegUser()
        {
            string login = textBoxLogin.Text.Trim();
            string email = textBoxEmail.Text.ToLower().Trim();
            string pass_1 = passBox1.Password.Trim();
            string pass_2 = passBox2.Password.Trim();

            // Сброс всех подсказок перед проверками
            ResetValidation();

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(login))
            {
                ErrorLogin_TextBlock.Visibility = Visibility.Visible;
                ErrorLogin_TextBlock.Text = "Введите логин";
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ErrorEmail_TextBlock.Visibility = Visibility.Visible;
                ErrorEmail_TextBlock.Text = "Введите email";
            }

            if (string.IsNullOrWhiteSpace(pass_1) || string.IsNullOrWhiteSpace(pass_2))
            {
                ErrorPassBox1_TextBlock.Visibility = Visibility.Visible;
                ErrorPassBox2_TextBlock.Visibility = Visibility.Visible;
                ErrorPassBox1_TextBlock.Text = "Введите пароль";
                ErrorPassBox2_TextBlock.Text = "Введите пароль";
                return;
            }

            // Проверка длины логина
            if (login.Length < 5)
            {
                ErrorLogin_TextBlock.Visibility = Visibility.Visible;
                ErrorLogin_TextBlock.Text = "Введите логин больше 5 символов";
            }

            // Проверка email (должна быть ДО проверки длины пароля)
            if (!IsValidEmail(email))
            {
                ErrorEmail_TextBlock.Visibility = Visibility.Visible;
                ErrorEmail_TextBlock.Text = "Введите корректный email";
            }

            // Проверка длины пароля
            if (pass_1.Length < 5)
            {
                ErrorPassBox1_TextBlock.Visibility = Visibility.Visible;
                ErrorPassBox1_TextBlock.Text = "Введите пароль больше 5 символов";
                return;
            }

            // Проверка совпадения паролей
            if (pass_1 != pass_2)
            {
                ErrorPassBox1_TextBlock.Visibility = Visibility.Visible;
                ErrorPassBox2_TextBlock.Visibility = Visibility.Visible;
                ErrorPassBox1_TextBlock.Text = "Пароли отличаются";
                ErrorPassBox2_TextBlock.Text = "Пароли отличаются";
                
                return;
            }

            // Проверка существующего логина
            if (db.Users.Any(u => u.Login == login))
            {
                ErrorLogin_TextBlock.Visibility = Visibility.Visible;
                ErrorLogin_TextBlock.Text = "Этот логин уже занят";
            }

            // Проверка существующего email
            if (db.Users.Any(u => u.Email == email))
            {
                ErrorEmail_TextBlock.Visibility = Visibility.Visible;
                ErrorEmail_TextBlock.Text = "Этот email уже зарегистрирован";
                return;
            }

            // Если все проверки пройдены
            User user = new User(login, email, pass_2);
            db.Users.Add(user);
            db.SaveChanges();

            this.IsEnabled = false; // Блокируем окно
            await Task.Delay(300);  // Задержка 300 мс

            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }

        // Метод для сброса валидации
        private void ResetValidation()
        {
            ErrorLogin_TextBlock.Visibility= Visibility.Collapsed;
            ErrorEmail_TextBlock.Visibility= Visibility.Collapsed;
            ErrorPassBox1_TextBlock.Visibility= Visibility.Collapsed;
            ErrorPassBox2_TextBlock.Visibility= Visibility.Collapsed;
        }
        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }

        private void textBoxLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => (char.IsLetterOrDigit(c) && c <= 127) && !char.IsWhiteSpace(c));
        }

        private void passBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => char.IsLetterOrDigit(c) && (c <= 127) && !char.IsWhiteSpace(c));
        }

        private void passBox2_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => char.IsLetterOrDigit(c) && (c <= 127) && !char.IsWhiteSpace(c));
        }

        private void textBoxLogin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            ErrorLogin_TextBlock.Visibility = Visibility.Collapsed;
        }

        private void passBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            ErrorPassBox1_TextBlock.Visibility = Visibility.Collapsed;
            ErrorPassBox2_TextBlock.Visibility = Visibility.Collapsed;

        }

        private void passBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            ErrorPassBox1_TextBlock.Visibility = Visibility.Collapsed;
            ErrorPassBox2_TextBlock.Visibility = Visibility.Collapsed;

        }

        private void textBoxEmail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;

            ErrorEmail_TextBlock.Visibility = Visibility.Collapsed;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegUser();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passBox1.Visibility == Visibility.Visible)
            {
                passBox1.Visibility = Visibility.Collapsed;
                VisiblePassBox1_TextBox.Visibility = Visibility.Visible;
                VisiblePassBox1_TextBox.Text = passBox1.Password;
            }
            else
            {
                VisiblePassBox1_TextBox.Visibility = Visibility.Collapsed;
                passBox1.Visibility = Visibility.Visible;
                passBox1.Password = VisiblePassBox1_TextBox.Text;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (passBox2.Visibility == Visibility.Visible)
            {
                passBox2.Visibility = Visibility.Collapsed;
                VisiblePassBox2_TextBox.Visibility = Visibility.Visible;
                VisiblePassBox2_TextBox.Text = passBox2.Password;
            }
            else
            {
                VisiblePassBox2_TextBox.Visibility = Visibility.Collapsed;
                passBox2.Visibility = Visibility.Visible;
                passBox2.Password = VisiblePassBox2_TextBox.Text;
            }
        }
    }
}
