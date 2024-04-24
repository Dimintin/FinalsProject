using System;
using System.Collections.Generic;
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
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Windows.Media.Animation;
using test1234.classes;

namespace test1234.windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        db.UserData authEmployee = null;

        public AuthWindow()
        {
            InitializeComponent();
        }

        private void textblock_regist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stackpanel_auth.Visibility = Visibility.Collapsed;
            stackpanel_regist.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textbox_pass.Text) || !string.IsNullOrEmpty(textbox_pass.Text))
            {
                authEmployee = classes.EF.Context.UserData.ToList().Where(i => i.Password == textbox_pass.Text && i.UserName == textbox_login.Text).FirstOrDefault();

                if (authEmployee != null)
                {
                    ActiveUser.activeUser = authEmployee;

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Данные пользователя неверны или не существуют", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void button_regist_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(textbox_newemail.Text) && !new EmailAddressAttribute().IsValid(textbox_newemail.Text))
            {
                MessageBox.Show("Введите верную электронную почту", "Ошибка значения", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
            }
            if (!string.IsNullOrEmpty(textbox_newlogin.Text) || !string.IsNullOrEmpty(textbox_newpass.Text))
            {
                db.UserData user = new db.UserData();

                user.Id = EF.Context.UserData.Max(i => i.Id) + 1;
                user.UserName = textbox_newlogin.Text;
                user.Password = textbox_newpass.Text;
                user.RegistDate = DateTime.Now;
                user.Email = textbox_newemail.Text;

                try
                {
                    EF.Context.UserData.Add(user);
                    EF.Context.SaveChanges();

                    ActiveUser.activeUser = user;

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Поля логина и пароля обязательны к запонению", "Ошибка значения", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stackpanel_auth.Visibility = Visibility.Visible;
            stackpanel_regist.Visibility = Visibility.Collapsed;
        }
    }
}
