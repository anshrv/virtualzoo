using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VirtualZoo
{
    /// <summary>
    /// Логика взаимодействия для zooEdit.xaml
    /// </summary>
    public partial class zooEdit : Window
    {
        public VirtualZooEntities DB = new VirtualZooEntities();
        public zooEdit()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
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
                wp.Width = 150;

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

                img.Height = 150;
                img.Width = 300;

                img.Uid = lists[i].Number.ToString();

                wp.Children.Add(img);
                wp.Children.Add(Name);
                wp.Children.Add(Description);
                wp.Children.Add(Fact);
                ListView.Items.Add(wp);
            }
        }

        private void ListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mousePosition = Mouse.GetPosition(this);
            IInputElement element = InputHitTest(mousePosition);
            string elementName = (element as FrameworkElement)?.Uid;
            int id = Convert.ToInt32(elementName);
            var zoo = DB.zoo.FirstOrDefault(x => x.Number == id);
            LabelName.Content = zoo.Name_of_the_animal;
            name.Text = zoo.Name_of_the_animal;
            description.Text = zoo.Описание;
            fact.Text = zoo.Interesting_fact;
            img.Text = zoo.Image;
            sound.Text = zoo.sound;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg) | *.jpg";
            openFileDialog.ShowDialog(this);
            img.Text = Path.GetFileName(openFileDialog.FileName);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files (*.mp3) | *.mp3";
            openFileDialog.ShowDialog(this);
            sound.Text = Path.GetFileName(openFileDialog.FileName);

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var zoo = DB.zoo.FirstOrDefault(x => x.Name_of_the_animal == (string)LabelName.Content);
            zoo.Name_of_the_animal = name.Text;
            zoo.Описание = description.Text;
            zoo.Interesting_fact = fact.Text;
            zoo.sound = sound.Text;
            zoo.Image = img.Text;
            DB.SaveChanges();
            ListView.Items.Clear();
            MessageBox.Show("Изменения сохранены.");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            admin admin = new admin();
            this.Close();
            admin.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            zoo zoo = new zoo()
            {
                Name_of_the_animal = name.Text,
                Описание = description.Text,
                Interesting_fact = fact.Text,
                sound = sound.Text,
                Image = img.Text
            };
            DB.zoo.Add(zoo);
            DB.SaveChanges();
            MessageBox.Show("Добавление сохраненено.");
        }
    }
}
