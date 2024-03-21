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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using Microsoft.Win32;

namespace VirtualZoo
{
    /// <summary>
    /// Логика взаимодействия для autorization.xaml
    /// </summary>
    public partial class autorization : Window
    {
        VirtualZooEntities DB = new VirtualZooEntities();
        DispatcherTimer timer1 = new DispatcherTimer();
        private int secSave = 0;
        private int minSave = 0;
        private int hourSave = 0;
        private int secCur = 0;
        private int minCur = 0;
        private int hourCur = 0;
        private int lastSeconds = 0;
        private int attempt = 0;
        public autorization()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Login = login.Text;
            string Password = password.Password;
            //Проверяем соответвуют ли логин и пароль тем, что есть в БД
            var userDB = DB.User.FirstOrDefault(x => x.Логин == Login && x.Пароль == Password);
            attempt++;
            if(attempt < 3)
            {
                if (userDB != null)
                {
                    if (userDB.Блокировка)
                    {
                        MessageBox.Show("В данный момент у вас имеется блокировка. Вход запрещен.");
                    }
                    else
                    {
                        int code = (int)userDB.Код_прав;
                        switch (code)
                        {
                            case 1:
                                guest guest = new guest();
                                this.Close();
                                guest.Show();
                                CreateRegistryTime(-1, -1, -1);
                                break;
                            case 2:
                                user user = new user();
                                this.Close();
                                user.Show();
                                CreateRegistryTime(-1, -1, -1);
                                break;
                            case 3:
                                admin admin = new admin();
                                this.Close();
                                admin.Show();
                                CreateRegistryTime(-1, -1, -1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    login.Text = "";
                    password.Password = "";
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            else
            {
                SetBlock(true);
            }
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            secCur = GetSeconds();
            minCur = GetMinutes();
            hourCur = GetHours();
            lastSeconds = 180 - ((hourCur * 3600 + minCur * 60 + secCur) - (hourSave * 3600 + minSave * 60 +
            secSave));
            if (lastSeconds <= 0)
            {
                SetUnblock();
            }
            else
            {
                labelBlock.Content = "Вход заблокирован. \n Повторите попытку через: " + lastSeconds; 
                timer1.Stop();
                timer1.Interval = TimeSpan.FromSeconds(1);
                timer1.Tick += timer1_Tick;
                timer1.Start();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            guest guest = new guest();
            this.Close();
            guest.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            registration registration = new registration();
            this.Close();
            registration.Show();
        }

        //событие для записи времени блокировки в регистр
        private void SetBlock(bool setTime = false)//по умолчанию параметр в значении ложь
        {
            if (setTime)
            {
                secSave = GetSeconds();//запоминаем текущую секунду
                minSave = GetMinutes();//запоминаем текущую минуту
                hourSave = GetHours();//запоминаем текущий час
                CreateRegistryTime(secSave, minSave, hourSave);//сохраняем значения в регистр
            }
            login.IsEnabled = false;
            password.IsEnabled = false;
            GoBtn.IsEnabled = false;
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += timer1_Tick;
            timer1.Start();
            login.Text = "";
            password.Password = "";
            MessageBox.Show("Неверный логин или пароль!");
        }
        //снятие блокировки с кнопок и текстбоксов
        private void SetUnblock()
        {
            login.IsEnabled = true;
            password.IsEnabled = true;
            GoBtn.IsEnabled = true;
            attempt = 0;
            timer1.Stop();
        }
        private int GetSeconds()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 2), 2)); ;
        }
        private int GetMinutes()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 5), 2));
        }
        private int GetHours()
        {
            return Convert.ToInt32(DateTime.Now.ToString().Substring((DateTime.Now.ToString().Length - 8), 2));
        }
        //запись времени в регистр (записывает время пользователя)
        private void CreateRegistryTime(int sec, int min, int hour)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey GIBDD_KEY = currentUserKey.CreateSubKey("GIBDD_TIME");
            GIBDD_KEY.SetValue("seconds", sec.ToString());
            GIBDD_KEY.SetValue("minutes", min.ToString());
            GIBDD_KEY.SetValue("hours", hour.ToString());
            GIBDD_KEY.Close();
        }
        private void GetRegistryTime()
        {
            if (Registry.CurrentUser.OpenSubKey("GIBDD_TIME") == null)
            {
                //значения -1 для проверки возможности входа в программу.
                CreateRegistryTime(-1, -1, -1);
                return;
            }
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey GIBDD_KEY = currentUserKey.OpenSubKey("GIBDD_TIME");
            secSave = Convert.ToInt32(GIBDD_KEY.GetValue("seconds"));
            minSave = Convert.ToInt32(GIBDD_KEY.GetValue("minutes"));
            hourSave = Convert.ToInt32(GIBDD_KEY.GetValue("hours"));
            GIBDD_KEY.Close();
            secCur = GetSeconds();
            minCur = GetMinutes();
            hourCur = GetMinutes();
            if (secSave == -1 && minSave == -1 && hourSave == -1)
            {
                return;
            }
            lastSeconds = 60 - ((hourCur * 3600 + minCur * 60 + secCur) - (hourCur * 3600 + minSave * 60 +
            secSave));
            if (lastSeconds > 0)
            {
                SetBlock();
            }
        }
    }
}
