using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity.Migrations;


namespace CourseWork2
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
         }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass_1 = passBox1.Password.Trim();

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
            if (pass_1.Length < 5)
            {
                passBox1.Background = Brushes.LightSkyBlue;
                passBox1.ToolTip = "Введите пароль больше 5 символов";
            }
            else
            {
                passBox1.Background = Brushes.Transparent;
                passBox1.ToolTip = null;
            }

            User authUser = null;
            using (AppContext db = new AppContext())
            {
                authUser = db.Users.Where(a => a.Login == login && a.Password == pass_1).FirstOrDefault();
            }

            if (authUser == null) 
            {
                MessageBox.Show("Логин или пароль введены неверно");
            }
            else
            {
                MainMenu mainMenu = new MainMenu(authUser.Id);
                mainMenu.Show();
                this.Close();
            }
        }
        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            MainWindow reg = new MainWindow();
            reg.Show();
            this.Close();
        }
    }
}
