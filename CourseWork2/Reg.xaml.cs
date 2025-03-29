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
                User user = new User(login, email, pass_2);

                db.Users.Add(user);
                db.SaveChanges();
            }

        }

        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }
    }
}
