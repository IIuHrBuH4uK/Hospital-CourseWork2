﻿using System;
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
            //textBoxLogin.Text = "admin";
            //passBox1.Password = "admin";
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthUser();
        }
        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            MainWindow reg = new MainWindow();
            reg.Show();
            this.Close();
        }

        private void textBoxLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => (char.IsLetterOrDigit(c) && c <= 127) && !char.IsWhiteSpace(c));

        }

        private void passBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => (char.IsLetterOrDigit(c) && c <= 127) && !char.IsWhiteSpace(c));
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

        }

        private void AuthUser()
        {
            string login = textBoxLogin.Text.Trim();
            string pass_1 = passBox1.Password.Trim();

            if (login.Length < 5)
            {
                ErrorLogin_TextBlock.Visibility = Visibility.Visible;
                ErrorLogin_TextBlock.Text = "Введите логин больше 5 символов";
            }
            
            if (pass_1.Length < 5)
            {
                ErrorPassBox1_TextBlock.Visibility = Visibility.Visible;
                ErrorPassBox1_TextBlock.Text = "Введите пароль больше 5 символов";
                return ;
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AuthUser();
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
    }
}
