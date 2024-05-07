using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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
        string _loggedIn;
        string _mode;
        string _buyPetID;
        string _buySupplyID;
        string _buying;

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
                    foreach (Login s in selectResults)
                    {
                        if (s.Login_Pass == txtPass.Text)
                        {
                            string messageString = $"Login complete.";
                            messageString += $" Welcome {s.Login_Name}!";
                            Login(s.Login_ID);
                            MessageBox.Show(messageString);
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

        private void Login(string LoginID)
        {
            IQueryable<Staff> selectResults = from s in _dbConn.Staffs
                                              where s.Login_ID == LoginID
                                              select s;


            Staff staff = selectResults.FirstOrDefault();
            if (staff != null)
            {
                _loggedIn = staff.Staff_ID;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            pnlLogin.Visibility = Visibility.Visible;
            pnlCategory.Visibility = Visibility.Collapsed;
        }

        private string GetAvail(string ID)
        {
            IQueryable<Avail> selectResults = from s in _dbConn.Avails
                                              where s.Avail_ID == ID
                                              select s;

            Avail temp = selectResults.FirstOrDefault();
            if (temp != null)
            {
                return temp.Avail_Desc;
            }
            return null;
        }

        private void GetPetData(int age, string type)
        {
            lvPets.Items.Clear();
            IQueryable<Pet> selectResults = null;
            if (_mode == "buy")
            {
                if (age == 1)
                {
                    selectResults = from s in _dbConn.Pets
                                    where s.Pet_Age <= 1 && s.Pet_Type == type && s.Avail_ID.Trim() == "AVL001"
                                    select s;
                }
                else
                {
                    selectResults = from s in _dbConn.Pets
                                    where s.Pet_Age >= age && s.Pet_Type == type && s.Avail_ID.Trim() == "AVL001"
                                    select s;
                }
            }
            else
            {
                if (age == 1)
                {
                    selectResults = from s in _dbConn.Pets
                                    where s.Pet_Age <= 1 && s.Pet_Type == type /*&& s.Avail_ID == "AVL001"*/
                                    select s;
                }
                else
                {
                    selectResults = from s in _dbConn.Pets
                                    where s.Pet_Age >= age && s.Pet_Type == type /*&& s.Avail_ID == "AVL001"*/
                                    select s;
                }
            }
            if (selectResults.Count() >= 1)
            {
                foreach (Pet p in selectResults)
                {
                    lvPets.Items.Add(new { Column1 = p.Pet_ID, Column2 = p.Pet_Name, Column3 = p.Pet_Breed, Column4 = p.Pet_Gender, Column5 = GetAvail(p.Avail_ID) });
                }
            }
        }

        private void GetSupplyData(string type)
        {
            lvSupplies.Items.Clear();
            IQueryable<Supply> selectResults = null;
            if (_mode == "buy")
            {
                if (type == "Dog Food" || type == "Dog Toy")
                {
                    selectResults = from s in _dbConn.Supplies
                                    where s.Supply_Type == "Dog Food" || s.Supply_Type == "Dog Toy" && s.Avail_ID == "AVL001"
                                    select s;
                }
                else if (type == "Cat Food" || type == "Cat Toy")
                {
                    selectResults = from s in _dbConn.Supplies
                                    where s.Supply_Type == "Cat Food" || s.Supply_Type == "Cat Toy" && s.Avail_ID == "AVL001"
                                    select s;
                }
            }
            else
            {
                if (type == "Dog Food" || type == "Dog Toy")
                {
                    selectResults = from s in _dbConn.Supplies
                                    where s.Supply_Type == "Dog Food" || s.Supply_Type == "Dog Toy" /*&& s.Avail_ID == "AVL001"*/
                                    select s;
                }
                else if (type == "Cat Food" || type == "Cat Toy")
                {
                    selectResults = from s in _dbConn.Supplies
                                    where s.Supply_Type == "Cat Food" || s.Supply_Type == "Cat Toy" /*&& s.Avail_ID == "AVL001"*/
                                    select s;
                }
            }

            if (selectResults.Count() >= 1)
            {
                foreach (Supply s in selectResults)
                {
                    lvSupplies.Items.Add(new { Column1 = s.Supply_ID, Column2 = s.Supply_Name, Column3 = s.Supply_Quantity, Column4 = s.Supply_Type, Column5 = GetAvail(s.Avail_ID) });
                }
            }
        }

        private void btnBack2Category_Click(object sender, RoutedEventArgs e)
        {
            lvPets.Items.Clear();
            lvSupplies.Items.Clear();
            lvCustomer.Items.Clear();
            pnlCategory.Visibility = Visibility.Visible;
            pnlCustomerList.Visibility = Visibility.Collapsed;
            pnlCustomerInfo.Visibility = Visibility.Collapsed;
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
            _selector = "pet";
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnDog_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(2, "Dog");
            _selector = "pet";
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnDogSupply_Click(object sender, RoutedEventArgs e)
        {
            GetSupplyData("Dog Food");
            _selector = "supply";
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvSupplies.Visibility = Visibility.Visible;
        }

        private void btnKitten_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(1, "Cat");
            _selector = "pet";
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnCat_Click(object sender, RoutedEventArgs e)
        {
            GetPetData(2, "Cat");
            _selector = "pet";
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvPets.Visibility = Visibility.Visible;
        }

        private void btnCatSupply_Click(object sender, RoutedEventArgs e)
        {
            GetSupplyData("Cat Food");
            _selector = "supply";
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Visible;
            lvSupplies.Visibility = Visibility.Visible;
        }

        private void lvPets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvPets.SelectedItem != null)
            {
                txtInfoHead.Text = "Pet Information";
                btnBack2Inv.Visibility = Visibility.Visible;
                btnBack2Category3.Visibility = Visibility.Collapsed;
                dynamic selectedItem = lvPets.SelectedItem;
                string id = selectedItem.Column1;
                _buyPetID = id;

                IQueryable<Pet> selectResults = from s in _dbConn.Pets
                                                where s.Pet_ID == id
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

                    switch (p.Avail_ID.Trim())
                    {
                        case "AVL001":
                            txtAvail.Content = "Available";
                            txtAvail.Background = new SolidColorBrush(Colors.LightGreen);
                            break;
                        case "AVL002":
                            txtAvail.Content = "Adopted";
                            txtAvail.Background = new SolidColorBrush(Colors.IndianRed);
                            break;
                    }

                }
                ChangePetImages();
            }
        }

        private void lvSupplies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvSupplies.SelectedItem != null)
            {
                txtInfoHead.Text = "Supply Information";
                btnBack2Inv.Visibility = Visibility.Visible;
                btnBack2Category3.Visibility = Visibility.Collapsed;
                dynamic selectedItem = lvSupplies.SelectedItem;
                string id = selectedItem.Column1;
                _buySupplyID = id;

                IQueryable<Supply> selectResults = from s in _dbConn.Supplies
                                                   where s.Supply_ID == id
                                                   select s;

                pnlInventory.Visibility = Visibility.Collapsed;
                lvSupplies.Visibility = Visibility.Collapsed;

                foreach (Supply s in selectResults)
                {
                    txtSupplyName.Text = s.Supply_Name;
                    txtSupplyQty.Text = s.Supply_Quantity.ToString();

                    switch (s.Avail_ID.Trim())
                    {
                        case "AVL001":
                            txtAvailSupply.Content = "Available";
                            txtAvail.Background = new SolidColorBrush(Colors.LightGreen);
                            break;
                        case "AVL003":
                            txtAvailSupply.Content = "Unavailable";
                            txtAvail.Background = new SolidColorBrush(Colors.IndianRed);
                            break;
                    }

                    switch (s.Supply_Type)
                    {
                        case "Dog Food":
                            cbSupplyType.SelectedIndex = 0;
                            break;
                        case "Dog Toy":
                            cbSupplyType.SelectedIndex = 1;
                            break;
                        case "Cat Food":
                            cbSupplyType.SelectedIndex = 2;
                            break;
                        case "Cat Toy":
                            cbSupplyType.SelectedIndex = 3;
                            break;
                    }


                }
                ChangeSupplyImages();
                pnlInformation.Visibility = Visibility.Visible;
                pnlSupplyInfo.Visibility = Visibility.Visible;
            }
        }

        private void btnBack2Inv_Click(object sender, RoutedEventArgs e)
        {
            pnlInventory.Visibility = Visibility.Visible;
            pnlInformation.Visibility = Visibility.Collapsed;
            pnlPetInfo.Visibility = Visibility.Collapsed;
            pnlSupplyInfo.Visibility = Visibility.Collapsed;
            btnNewPet.Visibility = Visibility.Collapsed;
            btnNewSupply.Visibility = Visibility.Collapsed;

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
            if (highestPet != null)
            {
                return highestPet.Pet_ID;
            }

            else
            {
                return "PET000";
            }
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

            if (highestSupply != null)
            {
                return highestSupply.Supply_ID;
            }

            else
            {
                return "SUP000";
            }
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
                btnNewPet.Visibility = Visibility.Visible;
                pnlCategory.Visibility = Visibility.Collapsed;
                ClearTBCB();
            }
            else
            {
                txtInfoHead.Text = "Supply Information";
                pnlInformation.Visibility = Visibility.Visible;
                pnlSupplyInfo.Visibility = Visibility.Visible;
                btnNewSupply.Visibility = Visibility.Visible;
                pnlCategory.Visibility = Visibility.Collapsed;
                ClearTBCB();
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
                if (npet.Pet_Age != -1)
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
                    nsupply.Supply_Type = "Dog Food";
                    break;
                case 1:
                    nsupply.Supply_Type = "Dog Toy";
                    break;
                case 2:
                    nsupply.Supply_Type = "Cat Food";
                    break;
                case 3:
                    nsupply.Supply_Type = "Cat Toy";
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

            txtCustName.Text = "";
            txtCustAge.Text = "";
            txtCustNum.Text = "";
            txtCustEmail.Text = "";
            cbCustSex.SelectedIndex = -1;
        }

        private void cbPetBreed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePetImages();
        }

        private void ChangePetImages()
        {
            string path = "/Images/BlankImage.png";

            if (CheckNum() == 1)
            {
                switch (cbPetBreed.SelectedIndex)
                {
                    case 0:
                        path = "/Images/PuppyGoldenRetriever.png";
                        break;
                    case 1:
                        path = "/Images/PuppyShitzu.png";
                        break;
                    case 2:
                        path = "/Images/KittenSiamese.png";
                        break;
                    case 3:
                        path = "/Images/KittenPersian.png";
                        break;
                }
            }
            else if (CheckNum() >= 2)
            {

                switch (cbPetBreed.SelectedIndex)
                {
                    case 0:
                        path = "/Images/AdultGoldenRetriever.png";
                        break;
                    case 1:
                        path = "/Images/AdultShitzu.png";
                        break;
                    case 2:
                        path = "/Images/AdultSiamese.png";
                        break;
                    case 3:
                        path = "/Images/AdultPersian.png";
                        break;
                }
            }

            imgPet.Source = new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void ChangeSupplyImages()
        {
            string path = "/Images/BlankImage.png";
            switch (cbSupplyType.SelectedIndex)
            {
                case 0:
                    path = "/Images/DogFood.png";
                    break;
                case 1:
                    path = "/Images/DogToy.png";
                    break;
                case 2:
                    path = "/Images/CatFood.png";
                    break;
                case 3:
                    path = "/Images/CatToy.png";
                    break;
            }
            imgSupply.Source = new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private int CheckNum()
        {
            if (int.TryParse(txtPetAge.Text, out int result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        private void cbSupplyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeSupplyImages();
        }

        private string GetStaffName(string StaffID)
        {
            IQueryable<Staff> selectResults = selectResults = from s in _dbConn.Staffs
                                                              where s.Staff_ID == StaffID
                                                              select s;
            Staff staff = selectResults.FirstOrDefault();
            if (staff != null)
            {
                return staff.StaffName;
            }
            return null;
        }

        private void GetCustData()
        {
            IQueryable<Customer> selectResults = selectResults = from s in _dbConn.Customers
                                                                 select s;
            foreach (Customer c in selectResults)
            {
                lvCustomer.Items.Add(new { Column1 = c.Customer_Name, Column2 = c.Customer_Number, Column3 = c.Customer_Email, Column4 = GetStaffName(c.Staff_ID) });
            }
        }

        private void btnCustomer_Click(object sender, RoutedEventArgs e)
        {
            GetCustData();
            pnlCategory.Visibility = Visibility.Collapsed;
            pnlCustomerList.Visibility = Visibility.Visible;
            lvCustomer.Visibility = Visibility.Visible;
        }

        private void lvCustomer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvCustomer.SelectedItem != null)
            {
                dynamic selectedItem = lvCustomer.SelectedItem;
                string name = selectedItem.Column1;

                IQueryable<Customer> selectResults = from s in _dbConn.Customers
                                                     where s.Customer_Name == name
                                                     select s;

                pnlInventory.Visibility = Visibility.Collapsed;
                pnlCustomerList.Visibility = Visibility.Collapsed;
                lvCustomer.Visibility = Visibility.Collapsed;
                pnlCustomerInfo.Visibility = Visibility.Visible;

                foreach (Customer s in selectResults)
                {
                    txtCustName.Text = s.Customer_Name;
                    txtCustAge.Text = s.Customer_Age.ToString();
                    txtCustNum.Text = s.Customer_Number.ToString();
                    txtCustEmail.Text = s.Customer_Email;

                    switch (s.Customer_Sex)
                    {
                        case "Male":
                            cbCustSex.SelectedIndex = 0;
                            break;
                        case "Female":
                            cbCustSex.SelectedIndex = 1;
                            break;
                        default:
                            cbCustSex.SelectedIndex = -1;
                            break;
                    }
                    _buying = s.Customer_ID;
                    GetOrderData(s.Customer_ID);
                }
            }
        }

        private string GetCustID()
        {
            var highestCust = _dbConn.Customers.OrderByDescending(p => p.Customer_ID).FirstOrDefault();
            if (highestCust != null)
            {
                return highestCust.Customer_ID;
            }

            else
            {
                return "CUST000";
            }
        }

        private string GenerateCustID(string highestID)
        {
            int num = int.Parse(highestID.Substring(4));
            num++;
            return "CUST" + num.ToString("D3");
        }

        private void btnAddCust_Click(object sender, RoutedEventArgs e)
        {
            ClearTBCB();
            pnlCustomerInfo.Visibility= Visibility.Collapsed;
            pnlCustomerList.Visibility= Visibility.Collapsed;
            lvCustomer.Visibility= Visibility.Collapsed;
            pnlCustomerInfo.Visibility=Visibility.Visible;
            btnNewCust.Visibility=Visibility.Visible;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow newWin = new DeleteWindow();
            bool? dialogResult = newWin.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                if (lvPets.SelectedItem != null || lvSupplies.SelectedItem != null)
                {
                    int age = 0;
                    string ptype = "";
                    string stype = "";

                    if (_selector == "pet")
                    {
                        dynamic selectedItem = lvPets.SelectedItem;
                        string name = selectedItem.Column1;
                        IQueryable<Pet> selectResults = from s in _dbConn.Pets
                                                        where s.Pet_Name == name
                                                        select s;
                        Pet temp = selectResults.FirstOrDefault();
                        age = temp.Pet_Age;
                        ptype = temp.Pet_Type;
                        temp.Avail_ID = "AVL002";
                    }
                    else if (_selector == "supply")
                    {
                        dynamic selectedItem = lvSupplies.SelectedItem;
                        string name = selectedItem.Column1;
                        IQueryable<Supply> selectResults = from s in _dbConn.Supplies
                                                           where s.Supply_Name == name
                                                           select s;
                        Supply temp = selectResults.FirstOrDefault();
                        stype = temp.Supply_Type;
                        temp.Avail_ID = "AVL003";
                    }

                    _dbConn.SubmitChanges();
                    switch (_selector)
                    {
                        case "pet":
                            GetPetData(age, ptype);
                            break;
                        case "supply":
                            GetSupplyData(stype);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Nothing selected. Please try again.");
                }


            }
        }

        private void btnNewCust_Click(object sender, RoutedEventArgs e)
        {
            Customer ncust = new Customer();
            ncust.Customer_ID = GenerateCustID(GetCustID());
            ncust.Staff_ID = _loggedIn;
            ncust.Customer_Name = txtCustName.Text;
            ncust.Customer_Email = txtCustEmail.Text;

            if (!string.IsNullOrWhiteSpace(txtCustAge.Text))
            {
                ncust.Customer_Age = GetNum(txtCustAge);
            }
            if (!string.IsNullOrWhiteSpace(txtCustAge.Text))
            {
                ncust.Customer_Number = GetNum(txtCustNum);
            }

            switch (cbCustSex.SelectedIndex)
            {
                case 0:
                    ncust.Customer_Sex = "Male";
                    break;
                case 1:
                    ncust.Customer_Sex = "Female";
                    break;
                default:
                    ncust.Customer_Sex = "Undefined";
                    break;
            }

            if (ncust.Customer_Name != null)
            {
                if (ncust.Customer_Age != -1 && ncust.Customer_Number != -1)
                {
                    _dbConn.Customers.InsertOnSubmit(ncust);
                    _dbConn.SubmitChanges();
                    MessageBox.Show("Succesfully added customer!");
                    ClearTBCB();
                }
            }
            else
            {
                MessageBox.Show("Must input all details.");
            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            lvPets.Items.Clear();
            lvSupplies.Items.Clear();
            lvCustomer.Items.Clear();
            pnlCategory.Visibility = Visibility.Visible;
            pnlCustomerList.Visibility = Visibility.Collapsed;
            pnlCustomerInfo.Visibility = Visibility.Collapsed;
            pnlInventory.Visibility = Visibility.Collapsed;
            pnlInformation.Visibility = Visibility.Collapsed;
            pnlPetInfo.Visibility = Visibility.Collapsed;
            pnlSupplyInfo.Visibility = Visibility.Collapsed;
            lvPets.Visibility = Visibility.Collapsed;
            lvSupplies.Visibility = Visibility.Collapsed;

            btnAdd.Visibility = Visibility.Collapsed;
            btnCustomer.Visibility = Visibility.Collapsed;
            btnLogout.Visibility = Visibility.Collapsed;
            btnAdoptorBuy.Visibility = Visibility.Collapsed;
            btnPetPurchase.Visibility = Visibility.Visible;

            _mode = "buy";
        }

        private void btnPetPurchase_Click(object sender, RoutedEventArgs e)
        {
            IQueryable<Pet> selectResults = from s in _dbConn.Pets
                                            where s.Pet_Name == _buyPetID
                                            select s;
            Order norder = new Order();
            norder.Order_ID = GenerateOrderID(GetOrderID());
            norder.Staff_ID = _loggedIn;
            norder.Customer_ID = _buying;
            norder.Pet_ID = _buyPetID;
            norder.Order_Date = DateTime.Now.ToString("yyyy-MM-dd");
            norder.Quantity = 1;
            _dbConn.Orders.InsertOnSubmit(norder);
            _dbConn.SubmitChanges();
        }


        private string GetOrderID()
        {
            var highestID = _dbConn.Orders.OrderByDescending(p => p.Order_ID).FirstOrDefault();

            if (highestID != null)
            {
                return highestID.Order_ID;
            }
            else
            {
                return "ORD000";
            }
        }

        private string GenerateOrderID(string highestID)
        {
            int num = int.Parse(highestID.Substring(3));
            num++;
            return "ORD" + num.ToString("D3");
        }

        public void GetOrderData(string CustID)
        {
            IQueryable<Order> selectResults = selectResults = from s in _dbConn.Orders
                                                              where s.Customer_ID == CustID
                                                                 select s;
            foreach (Order o in selectResults)
            {
                lvPurchases.Items.Add(new { Column1 = o.Pet_ID, Column2 = o.Quantity, Column3 = GetStaffName(o.Staff_ID) });
            }
        }
    }
}
