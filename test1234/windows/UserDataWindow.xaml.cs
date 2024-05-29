using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
using test1234.classes;
using test1234.db;

namespace test1234.windows
{
    /// <summary>
    /// Логика взаимодействия для UserDataWindow.xaml
    /// </summary>
    public partial class UserDataWindow : Window
    {
        public UserDataWindow()
        {
            InitializeComponent();
            listview_userReview.ItemsSource = EF.Context.Func_GetUserReviews(ActiveUser.activeUser.Id).ToList();
            listview_userView.ItemsSource = EF.Context.Func_GetUserViewHitory(ActiveUser.activeUser.Id).ToList();
            textbox_newpass.Text = ActiveUser.activeUser.Password;
            textbox_newlogin.Text = ActiveUser.activeUser.UserName;
            textbox_newemail.Text = ActiveUser.activeUser.Email;
            if (!string.IsNullOrEmpty(ActiveUser.activeUser.ProfilePhotoLink))
            {
                image_profile.ImageSource = new BitmapImage(new Uri(ActiveUser.activeUser.ProfilePhotoLink));
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = "png";
            if (openFileDialog.ShowDialog() == true)
            {
                image_profile.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (border_shade.Visibility != Visibility.Visible)
            {
                border_shade.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (border_shade.Visibility != Visibility.Collapsed)
            {
                border_shade.Visibility = Visibility.Collapsed;
            }
        }

        private void button_saveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!new EmailAddressAttribute().IsValid(textbox_newemail.Text) && !string.IsNullOrEmpty(ActiveUser.activeUser.Email))
            {
                MessageBox.Show("Электронная почта не подходит", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!string.IsNullOrEmpty(textbox_newlogin.Text) && !string.IsNullOrEmpty(textbox_newlogin.Text) && !string.IsNullOrEmpty(textbox_newlogin.Text) && !string.IsNullOrEmpty(textbox_newlogin.Text))
            {
                MessageBoxResult boxResult = MessageBox.Show("Вы уверены, что хотите сохранить данные?", "Сохранение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (boxResult == MessageBoxResult.Yes)
                {
                    ActiveUser.activeUser.UserName = textbox_newlogin.Text;
                    ActiveUser.activeUser.Password = textbox_newpass.Text;
                    ActiveUser.activeUser.Email = textbox_newemail.Text;
                    ActiveUser.activeUser.ProfilePhotoLink = image_profile.ImageSource.ToString();

                    EF.Context.SaveChanges();

                    MessageBox.Show("Изменения успешно сохранены!", "Сохранение", MessageBoxButton.OK);
                }
                return;
            }
            MessageBox.Show("Сохранение невозможно, ошибка данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void stackpanel_back_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
