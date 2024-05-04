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
using System.Windows.Markup;
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
            flag = false;
            if (txtUser.Text.Length > 0 && txtPass.Text.Length > 0)
            {
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

                            MessageBox.Show(messageString);
                            //Log tlog = new Log();
                            //tlog.
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
                txtPass.Text = null;
                txtUser.Text = null;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            pnlLogin.Visibility = Visibility.Visible;
            pnlCategory.Visibility = Visibility.Collapsed;
        }

        private void GetPetData(int age, string type)
        {
            IQueryable<Pet> selectResults = null;

            if (age == 1)
            {
                selectResults = from s in _dbConn.Pets
                                where s.Pet_Age <= 1 && s.Pet_Type == type
                                select s;
            }
            else
            {
                selectResults = from s in _dbConn.Pets
                                where s.Pet_Age >= age && s.Pet_Type == type
                                select s;
            }

            foreach (Pet p in selectResults)
            {
                lvPets.Items.Add(new { Column1 = p.Pet_Name, Column2 = p.Pet_Age, Column3 = p.Pet_Breed, Column4 = p.Pet_Gender });
            }
        }

        private void GetSupplyData(string type)
        {
            IQueryable<Supply> selectResults = from s in _dbConn.Supplies
                                               where s.Supply_Type == type
                                               select s;

            foreach (Supply s in selectResults)
            {
                lvSupplies.Items.Add(new { Column1 = s.Supply_Name, Column2 = s.Supply_Quantity, Column3 = s.Supply_Type });
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            lvPets.Items.Clear();
            lvSupplies.Items.Clear();
            pnlCategory.Visibility = Visibility.Visible;
            pnlInventory.Visibility = Visibility.Collapsed;
            lvPets.Visibility = Visibility.Collapsed;
            lvSupplies.Visibility = Visibility.Collapsed;
        }

        private void btnPuppy_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(1, "Dog");
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnDog_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(2, "Dog");
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnDogSupply_Click(object sender, RoutedEventArgs e)
        {
            GetSupplyData("Dog Supply");
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvSupplies.Visibility = Visibility.Visible;
        }

        private void btnKitten_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(1, "Cat");
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnCat_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(2, "Cat");
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnCatSupply_Click(object sender, RoutedEventArgs e)
        {
            GetSupplyData("Cat Supply");
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvSupplies.Visibility = Visibility.Visible;
        }

        private void lvPets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvPets.SelectedItem != null)
            {
                dynamic selectedItem = lvPets.SelectedItem;
                string column1Value = selectedItem.Column1;
                // Retrieve other column values similarly

                MessageBox.Show($"You double-clicked on: {column1Value}");
            }
        }
    }
}
