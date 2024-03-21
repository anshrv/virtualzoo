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
    /// Логика взаимодействия для user.xaml
    /// </summary>
    public partial class user : Window
    {
        MediaPlayer sound = new MediaPlayer();
        public VirtualZooEntities DB = new VirtualZooEntities();
        public user()
        {
            InitializeComponent();
        }

        private void ListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            sound.Stop();
            System.Windows.Point mousePosition = Mouse.GetPosition(this);
            IInputElement element = InputHitTest(mousePosition);
            string elementName = (element as FrameworkElement)?.Uid;
            int id = Convert.ToInt32(elementName);
            var zoo = DB.zoo.FirstOrDefault(x => x.Number == id);

            string savePath = System.IO.Path.GetFullPath(@"..\..\res\sound");
            savePath = savePath + "\\" + zoo.sound;


            sound.Open(new Uri(savePath));
            sound.Play();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var lists = DB.zoo.ToList();
            for (int i = 0; i < DB.zoo.Count(); i++)
            {
                WrapPanel wp = new WrapPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                Label Name = new Label();
                TextBlock Description = new TextBlock();
                TextBlock Fact = new TextBlock();
                Description.TextWrapping = TextWrapping.Wrap;
                Fact.TextWrapping = TextWrapping.Wrap;

                wp.Height = 700;
                wp.Width = 300;

                Name.Content = lists[i].Name_of_the_animal;
                Description.Text = "Описание: " + "\n" + lists[i].Описание;
                Fact.Text = "Интересный факт: " + "\n" + lists[i].Interesting_fact;


                string savePath = System.IO.Path.GetFullPath(@"..\..\res\img");
                savePath = savePath + "\\" + lists[i].Image;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(savePath);
                bitmap.EndInit();
                img.Source = bitmap;
                img.MouseDown += new MouseButtonEventHandler(ListView_MouseDown);

                img.Height = 275;
                img.Width = 300;

                img.Uid = lists[i].Number.ToString();


                wp.Children.Add(img);
                wp.Children.Add(Name);
                wp.Children.Add(Description);
                wp.Children.Add(Fact);
                ListView.Items.Add(wp);
            }
        }
    }
}
