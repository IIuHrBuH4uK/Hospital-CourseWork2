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
                textBoxLogin.Background = Brushes.LightSkyBlue;
                textBoxLogin.ToolTip = "Введите логин";
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                textBoxEmail.Background = Brushes.LightSkyBlue;
                textBoxEmail.ToolTip = "Введите email";
                return;
            }

            if (string.IsNullOrWhiteSpace(pass_1) || string.IsNullOrWhiteSpace(pass_2))
            {
                passBox1.Background = Brushes.LightSkyBlue;
                passBox1.ToolTip = "Введите пароль";
                passBox2.Background = Brushes.LightSkyBlue;
                passBox2.ToolTip = "Введите пароль";
                return;
            }

            // Проверка длины логина
            if (login.Length < 5)
            {
                textBoxLogin.Background = Brushes.LightSkyBlue;
                textBoxLogin.ToolTip = "Введите логин больше 5 символов";
                return;
            }

            // Проверка email (должна быть ДО проверки длины пароля)
            if (!IsValidEmail(email))
            {
                textBoxEmail.Background = Brushes.LightSkyBlue;
                textBoxEmail.ToolTip = "Введите корректный email";
                return;
            }

            // Проверка длины пароля
            if (pass_1.Length < 5)
            {
                passBox1.Background = Brushes.LightSkyBlue;
                passBox1.ToolTip = "Введите пароль больше 5 символов";
                return;
            }

            // Проверка совпадения паролей
            if (pass_1 != pass_2)
            {
                passBox2.Background = Brushes.LightSkyBlue;
                passBox1.Background = Brushes.LightSkyBlue;
                passBox2.ToolTip = "Пароли отличаются";
                passBox1.ToolTip = "Пароли отличаются";
                return;
            }

            // Проверка существующего логина
            if (db.Users.Any(u => u.Login == login))
            {
                textBoxLogin.Background = Brushes.LightSkyBlue;
                textBoxLogin.ToolTip = "Этот логин уже занят";
                return;
            }

            // Проверка существующего email
            if (db.Users.Any(u => u.Email == email))
            {
                textBoxEmail.Background = Brushes.LightSkyBlue;
                textBoxEmail.ToolTip = "Этот email уже зарегистрирован";
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
            textBoxLogin.Background = Brushes.Transparent;
            textBoxLogin.ToolTip = null;
            textBoxEmail.Background = Brushes.Transparent;
            textBoxEmail.ToolTip = null;
            passBox1.Background = Brushes.Transparent;
            passBox1.ToolTip = null;
            passBox2.Background = Brushes.Transparent;
            passBox2.ToolTip = null;
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

            if (e.Key == Key.Enter)
                RegUser();
        }

        private void passBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
                RegUser();
        }

        private void passBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
                RegUser();
        }

        private void textBoxEmail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
                RegUser();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                RegUser();
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
