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
            RegistWindow registWindow = new RegistWindow();
            registWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textbox_pass.Text) || !string.IsNullOrEmpty(textbox_pass.Text))
            {
                authEmployee = classes.EF.Context.UserData.ToList().Where(i => i.Password == textbox_pass.Text && i.UserName == textbox_login.Text).FirstOrDefault();

                if (authEmployee != null)
                {
                    classes.ActiveUser.activeUser = authEmployee;
                    MainWindow mainWindow = new MainWindow();
                    this.Close();
                    mainWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Данные пользователя неверны или не существуют", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }
    }
}
