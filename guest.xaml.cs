using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Логика взаимодействия для guest.xaml
    /// </summary>
    public partial class guest : Window
    {
        public VirtualZooEntities DB = new VirtualZooEntities();
        public guest()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var lists = DB.zoo.ToList();
            for (int i = 0; i < DB.zoo.Count(); i++)
            {
                WrapPanel wp = new WrapPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                Label labelName = new Label();

                wp.Height = 200;
                wp.Width = 200;

                labelName.Content = lists[i].Name_of_the_animal;

                string savePath = System.IO.Path.GetFullPath(@"..\..\res\img");
                savePath = savePath + "\\" + lists[i].Image;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(savePath);
                bitmap.EndInit();
                img.Source = bitmap;

                img.Height = 150;
                img.Width = 200;

                img.Uid = lists[i].Number.ToString();

                wp.Children.Add(img);
                wp.Children.Add(labelName);
                ListView.Items.Add(wp);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            registration registration = new registration();
            this.Close();
            registration.Show();
        }
    }
}
