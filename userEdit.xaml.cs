using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
using System.Xml.Linq;

namespace VirtualZoo
{
    /// <summary>
    /// Логика взаимодействия для userEdit.xaml
    /// </summary>
    public partial class userEdit : Window
    {
        VirtualZooEntities DB = new VirtualZooEntities();
        public userEdit()
        {
            InitializeComponent();
        }

        

        private void back_Click(object sender, RoutedEventArgs e)
        {
            admin admin = new admin();
            this.Close();
            admin.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var lists = DB.User.ToList();
            for (int i = 0; i < DB.User.Count(); i++)
            {
                login.Items.Add(lists[i].Логин);
            }
        }

        private void login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nameLabel.Content = "ФИО пользователя:";
            emailLabel.Content = "Почта пользователя:";
            passwordLabel.Content = "Пароль пользователя:";
            statusLabel.Content = "Статус пользователя:";
            var User = DB.User.FirstOrDefault(x => x.Логин == login.SelectedItem.ToString());
            nameLabel.Content += "\t" + User.ФИО;
            emailLabel.Content += "\t" + User.email;
            passwordLabel.Content += "\t" + User.Пароль;
            string Block;
            if(User.Блокировка)
            {
                Block = "Заблокирован.";
                unblock.Visibility = Visibility.Visible;
                block.Visibility = Visibility.Hidden;
            }
            else
            {
                Block = "Блокировка отсутствует.";
                unblock.Visibility = Visibility.Hidden;
                block.Visibility = Visibility.Visible;
            }
            statusLabel.Content += "\t" + Block;
        }

        private void block_Click(object sender, RoutedEventArgs e)
        {
            userEdit userEdit = new userEdit();
            var User = DB.User.FirstOrDefault(x => x.Логин == login.SelectedItem.ToString());
            User.Блокировка = true;
            DB.SaveChanges();
            MessageBox.Show("Пользователь " + User.Логин + " Заблокирован.");
            this.Close();
            userEdit.Show();
            
        }
        private void unblock_Click(object sender, RoutedEventArgs e)
        {
            userEdit userEdit = new userEdit();
            var User = DB.User.FirstOrDefault(x => x.Логин == login.SelectedItem.ToString());
            User.Блокировка= false;
            DB.SaveChanges();
            MessageBox.Show("Пользователь " + User.Логин + " разблокирован.");
            this.Close();
            userEdit.Show();
        }
    }
}
