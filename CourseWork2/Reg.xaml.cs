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

            string login = textBoxLogin.Text.Trim(); //Метод удаляет лишние пробелы
            string email = textBoxEmail.Text.ToLower().Trim(); //Приводим всё к нижнему регистру и удаляем лишние пробелы
            string pass_1 = passBox1.Password.Trim();
            string pass_2 = passBox2.Password.Trim();

            if (login.Length < 5)
            {
                textBoxLogin.Background = Brushes.LightSkyBlue;
                textBoxLogin.ToolTip = "Введите логин больше 5 символов";
            }
            else
            {
                textBoxLogin.Background = Brushes.Transparent;
                textBoxLogin.ToolTip = null;
            }

            if (!IsValidEmail(email))
            {
                textBoxEmail.Background = Brushes.LightSkyBlue;
                textBoxEmail.ToolTip = "Введите корректный email";
            }
            else
            {
                textBoxEmail.Background = Brushes.Transparent;
                textBoxEmail.ToolTip = null;
            }

            if (pass_1.Length < 5)
            {
                passBox1.Background = Brushes.LightSkyBlue;
                passBox1.ToolTip = "Введите пароль больше 5 символов";

                passBox2.Background = Brushes.Transparent;
                passBox2.ToolTip = null;
            }
            else
            {
                passBox1.Background = Brushes.Transparent;
                passBox1.ToolTip = null;

                if (passBox1.Password != passBox2.Password)
                {
                    passBox2.Background = Brushes.LightSkyBlue;
                    passBox1.Background = Brushes.LightSkyBlue;
                    passBox2.ToolTip = "Пароли отличаются";
                    passBox1.ToolTip = "Пароли отличаются";
                }
                else
                {
                    passBox1.Background = Brushes.Transparent;
                    passBox1.ToolTip = null;
                    passBox2.Background = Brushes.Transparent;
                    passBox2.ToolTip = null;
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

                User user = new User(login, email, pass_2);

                db.Users.Add(user);
                db.SaveChanges();

                Auth auth = new Auth();
                auth.Show();
                this.Close();

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
            }

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
        }

        private void passBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void passBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void textBoxEmail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
