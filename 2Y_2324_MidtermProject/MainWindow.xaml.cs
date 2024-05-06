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
        string _selector;

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

        private void btnBack2Category_Click(object sender, RoutedEventArgs e)
        {
            lvPets.Items.Clear();
            lvSupplies.Items.Clear();
            pnlCategory.Visibility = Visibility.Visible;
            pnlInventory.Visibility = Visibility.Collapsed;
            pnlInformation.Visibility = Visibility.Collapsed;
            pnlPetInfo.Visibility = Visibility.Collapsed;
            pnlSupplyInfo.Visibility = Visibility.Collapsed;
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
            _selector = "pet";
            txtInfoHead.Text = "Pet Information";
            btnBack2Inv.Visibility = Visibility.Visible;
            btnBack2Category3.Visibility = Visibility.Collapsed;
            if (lvPets.SelectedItem != null)
            {
                dynamic selectedItem = lvPets.SelectedItem;
                string name = selectedItem.Column1;

                IQueryable<Pet> selectResults = from s in _dbConn.Pets
                                                where s.Pet_Name == name
                                                select s;

                pnlInventory.Visibility = Visibility.Collapsed;
                lvPets.Visibility = Visibility.Collapsed;
                pnlInformation.Visibility = Visibility.Visible;
                pnlPetInfo.Visibility = Visibility.Visible;

                foreach (Pet p in selectResults)
                {
                    txtPetName.Text = p.Pet_Name;
                    txtPetAge.Text = p.Pet_Age.ToString();
                    txtPetDob.Text = p.Pet_DOB.ToString();

                    switch (p.Pet_Type)
                    {
                        case "Dog":
                            cbPetType.SelectedIndex = 0;
                            break;
                        case "Cat":
                            cbPetType.SelectedIndex = 1;
                            break;
                    }

                    switch (p.Pet_Breed)
                    {
                        case "Golden Retriever":
                            cbPetBreed.SelectedIndex = 0;
                            break;
                        case "Shih Tzu":
                            cbPetBreed.SelectedIndex = 1;
                            break;
                        case "Siamese":
                            cbPetBreed.SelectedIndex = 2;
                            break;
                        case "Persian":
                            cbPetBreed.SelectedIndex = 3;
                            break;
                    }

                    switch (p.Pet_Gender)
                    {
                        case "Male":
                            cbPetSex.SelectedIndex = 0;
                            break;
                        case "Female":
                            cbPetSex.SelectedIndex = 1;
                            break;
                    }
                }
            }
        }

        private void lvSupplies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selector = "supply";
            txtInfoHead.Text = "Supply Information";
            btnBack2Inv.Visibility = Visibility.Visible;
            btnBack2Category3.Visibility = Visibility.Collapsed;
            if (lvSupplies.SelectedItem != null)
            {
                dynamic selectedItem = lvSupplies.SelectedItem;
                string name = selectedItem.Column1;

                IQueryable<Supply> selectResults = from s in _dbConn.Supplies
                                                   where s.Supply_Name == name
                                                   select s;

                pnlInventory.Visibility = Visibility.Collapsed;
                lvSupplies.Visibility = Visibility.Collapsed;
                pnlInformation.Visibility = Visibility.Visible;
                pnlSupplyInfo.Visibility = Visibility.Visible;

                foreach (Supply s in selectResults)
                {
                    txtSupplyName.Text = s.Supply_Name;
                    txtSupplyQty.Text = s.Supply_Quantity.ToString();

                    switch (s.Supply_Type)
                    {
                        case "Dog Supply":
                            cbSupplyType.SelectedIndex = 0;
                            break;
                        case "Cat Supply":
                            cbSupplyType.SelectedIndex = 1;
                            break;
                    }
                }
            }
        }

        private void btnBack2Inv_Click(object sender, RoutedEventArgs e)
        {
            pnlInventory.Visibility = Visibility.Visible;
            pnlInformation.Visibility = Visibility.Collapsed;
            pnlPetInfo.Visibility = Visibility.Collapsed;
            pnlSupplyInfo.Visibility = Visibility.Collapsed;

            switch (_selector)
            {
                case "pet":
                    lvPets.Visibility = Visibility.Visible;
                    break;
                case "supply":
                    lvSupplies.Visibility = Visibility.Visible;
                    break;
            }
        }

        private int GetNum(TextBox txtbox)
        {
            bool isNum = false;
            string uInput = "";
            int num = 0;

            uInput = txtbox.Text;
            isNum = int.TryParse(uInput, out num);

            if (!isNum)
            {
                MessageBox.Show($"{txtbox.Text} is not a number. Please try again.");
                return -1;
            }

            return num;
        }

        private string GetPetID()
        {
            var highestPet = _dbConn.Pets.OrderByDescending(p => p.Pet_ID).FirstOrDefault();

            return highestPet.Pet_ID;
        }

        private string GeneratePetID(string highestID)
        {
            int num = int.Parse(highestID.Substring(3)); 
            num++; 
            return "PET" + num.ToString("D3");
        }

        private string GetSupplyID()
        {
            var highestSupply = _dbConn.Supplies.OrderByDescending(p => p.Supply_ID).FirstOrDefault();

            return highestSupply.Supply_ID;
        }
        private string GenerateSupplyID(string highestID)
        {
            int num = int.Parse(highestID.Substring(3));
            num++;
            return "SUP" + num.ToString("D3");
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ChoiceWindow newWin = new ChoiceWindow();
            bool? dialogResult = newWin.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                txtInfoHead.Text = "Pet Information";
                pnlInformation.Visibility = Visibility.Visible;
                pnlPetInfo.Visibility = Visibility.Visible;
                pnlCategory.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtInfoHead.Text = "Supply Information";
                pnlInformation.Visibility = Visibility.Visible;
                pnlSupplyInfo.Visibility = Visibility.Visible;
                pnlCategory.Visibility = Visibility.Collapsed;
            }
            btnBack2Inv.Visibility = Visibility.Collapsed;
            btnBack2Category3.Visibility = Visibility.Visible;
        }

        private void btnNewPet_Click(object sender, RoutedEventArgs e)
        {
            Pet npet = new Pet();
            npet.Pet_ID = GeneratePetID(GetPetID());
            npet.Avail_ID = "AVL001";
            npet.Pet_Name = txtPetName.Text;
            npet.Pet_DOB = txtPetDob.Text;

            if (!string.IsNullOrWhiteSpace(txtPetAge.Text))
            {
                npet.Pet_Age = GetNum(txtPetAge);
            }

            switch (cbPetType.SelectedIndex)
            {
                case 0:
                    npet.Pet_Type = "Dog";
                    break;
                case 1:
                    npet.Pet_Type = "Cat";
                    break;
                default:
                    npet.Pet_Type = "None";
                    break;
            }

            switch (cbPetBreed.SelectedIndex)
            {
                case 0:
                    npet.Pet_Breed = "Golden Retriever";
                    break;
                case 1:
                    npet.Pet_Breed = "Shih Tzu";
                    break;
                case 2:
                    npet.Pet_Breed = "Siamese";
                    break;
                case 3:
                    npet.Pet_Breed = "Persian";
                    break;
                default:
                    npet.Pet_Breed = "None";
                    break;
            }

            switch (cbPetSex.SelectedIndex)
            {
                case 0:
                    npet.Pet_Gender = "Male";
                    break;
                case 1:
                    npet.Pet_Gender = "Female";
                    break;
                default:
                    npet.Pet_Gender = "None";
                    break;
            }

            _dbConn.Pets.InsertOnSubmit(npet);

            if (npet.Pet_Name != null)
            {
                if(npet.Pet_Age != -1)
                {
                    _dbConn.SubmitChanges();
                    MessageBox.Show("Succesfully added pet!");
                    ClearTBCB();
                }
            }
        }

        private void btnNewSupply_Click(object sender, RoutedEventArgs e)
        {
            Supply nsupply = new Supply();
            nsupply.Supply_ID = GenerateSupplyID(GetSupplyID());
            nsupply.Avail_ID = "AVL001";
            nsupply.Supply_Name = txtSupplyName.Text;

            if (!string.IsNullOrWhiteSpace(txtSupplyQty.Text))
            {
                nsupply.Supply_Quantity = GetNum(txtSupplyQty);
            }

            switch (cbSupplyType.SelectedIndex)
            {
                case 0:
                    nsupply.Supply_Type = "Dog Supply";
                    break;
                case 1:
                    nsupply.Supply_Type = "Cat Supply";
                    break;
                default:
                    nsupply.Supply_Type = "None";
                    break;
            }

            _dbConn.Supplies.InsertOnSubmit(nsupply);

            if (nsupply.Supply_Name != null)
            {
                if (nsupply.Supply_Quantity != -1)
                {
                    _dbConn.SubmitChanges();
                    MessageBox.Show("Succesfully added supply!");
                    ClearTBCB();
                }
            }
        }

        private void ClearTBCB()
        {
            txtPetName.Text = "";
            txtPetAge.Text = "";
            txtPetDob.Text = "";
            cbPetType.SelectedIndex = -1;
            cbPetBreed.SelectedIndex = -1;
            cbPetSex.SelectedIndex = -1;

            txtSupplyName.Text = "";
            txtSupplyQty.Text = "";
            cbSupplyType.SelectedIndex = -1;
        }
    }
}
