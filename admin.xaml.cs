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

namespace VirtualZoo
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        public admin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zooEdit zooEdit = new zooEdit();
            this.Close();
            zooEdit.Show();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            userEdit userEdit = new userEdit();
            this.Close();
            userEdit.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            autorization autorization = new autorization();
            this.Close();
            autorization.Show();
        }
    }
}
