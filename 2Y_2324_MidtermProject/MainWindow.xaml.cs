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

namespace _2Y_2324_MidtermProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext _dbConn = null;
        bool flag = false;

        public MainWindow()
        {
            InitializeComponent();
            _dbConn = new DataClasses1DataContext(Properties.Settings.Default.MidtermConnectionString);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text.Length > 0 && txtPass.Text.Length > 0)
            {
                flag = false;
                IQueryable<Login> selectResults = from s in _dbConn.Logins
                                                     where s.Login_ID == txtUser.Text
                                                     select s;

                if (selectResults.Count() == 1)
                {
                    //MessageBox.Show("Username exists");
                    foreach (Login s in selectResults)
                    {
                        if (s.Login_Pass == txtPass.Text)
                        {
                            string messageString = $"Login complete.";
                            messageString += $" Welcome {s.Login_Name}!";
                            //if (s.LoginDate == null)

                            //else
                            //    messageString += $" Welcome back {s.LoginName}! Havent seen you since {s.LoginDate}";

                            MessageBox.Show(messageString);
                            //s.LoginDate = DateTime.Now;

                            //tblLog tlog = new tblLog();
                            //tlog.LoginID = s.LoginID;
                            //tlog.LogDate = (DateTime)s.LoginDate;

                            //_dbConn.tblLogs.InsertOnSubmit(tlog);
                            flag = true;
                            break;
                        }
                    }
                    _dbConn.SubmitChanges();
                }
            }

            if (flag)
            {
                pnlLogin.Visibility = Visibility.Collapsed;
                pnlCategory.Visibility = Visibility.Visible;
                txtPass.Text = "";
                txtUser.Text = "";

            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            pnlLogin.Visibility = Visibility.Visible;
            pnlCategory.Visibility = Visibility.Collapsed;
        }


    }
}
