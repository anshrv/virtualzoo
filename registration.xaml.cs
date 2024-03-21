using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    /// Логика взаимодействия для registration.xaml
    /// </summary>
    public partial class registration : Window
    {
        VirtualZooEntities DB = new VirtualZooEntities();
        public registration()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text != "" && email.Text != "" && login.Text != "" && password.Text != "" )
            {
                if (password.Text == passwordConfirm.Text)
                {
                    string Captcha = Convert.ToString(captchaLabel.Content);

                    if (Captcha == captcha.Text)
                    {
                        User us = new User()
                        {
                            ФИО = name.Text,
                            email = email.Text,
                            Логин = login.Text,
                            Пароль = password.Text
                        };
                        DB.User.Add(us);
                        DB.SaveChanges();


                        user user = new user();
                        this.Close();
                        user.Show();
                    }
                    else
                    {
                        MessageBox.Show("Капча введена неверно");
                        //String allowchar = " ";
                        //allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                        //allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
                        //allowchar += "1,2,3,4,5,6,7,8,9,0";
                        //char[] a = { ',' };
                        //String[] ar = allowchar.Split(a);


                        //String pwd = " ";
                        //string temp = " ";
                        //Random r = new Random();


                        //for (int i = 0; i < 6; i++)
                        //{
                        //    temp = ar[(r.Next(0, ar.Length))];
                        //    pwd += temp;
                        //}

                        //captchaLabel.Content = pwd;
                    }
                }
                else
                {
                    MessageBox.Show("Подтверждение пароля не совпадает с паролем.");
                }
            }
            else
            {
                MessageBox.Show("Заполните все окна и повторите попытку.");
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
            autorization autorization = new autorization();
            this.Close();
            autorization.Show();
        }

 

        private void Window_Activated(object sender, EventArgs e)
        {
            string allowchar;
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            string[] ar = allowchar.Split(a);


            string pwd = "";
            string temp;
            Random r = new Random();


            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }

            captchaLabel.Content = pwd;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
