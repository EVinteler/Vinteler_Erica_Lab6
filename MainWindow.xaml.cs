using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace Vinteler_Erica_Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
         New,
         Edit,
         Delete,
         Nothing
    }
public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;

        Binding firstNameBinding = new Binding();
        Binding lastNameBinding = new Binding();
        Binding makeBinding = new Binding();
        Binding colorBinding = new Binding();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            firstNameBinding.Path = new PropertyPath("FirstName");
            lastNameBinding.Path = new PropertyPath("LastName");
            makeBinding.Path = new PropertyPath("Make");
            colorBinding.Path = new PropertyPath("Color");
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameBinding);
            makeTextBox.SetBinding(TextBox.TextProperty, makeBinding);
            colorTextBox.SetBinding(TextBox.TextProperty, colorBinding);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;

            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            //customerOrdersViewSource.Source = ctx.Orders.Local;

            inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;

            ctx.Customers.Load();
            ctx.Orders.Load();
            ctx.Inventories.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            //cmbCustomers.DisplayMemberPath = "FirstName";
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local;
            //cmbInventory.DisplayMemberPath = "Make";
            cmbInventory.SelectedValuePath = "CarId";


            BindDataGrid();
        }

        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.CarId
                 equals inv.CarId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CarId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }

        private void SetValidationBinding()
        {
            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            // string required
            firstNameValidationBinding.ValidationRules.Add(new StringNotEmpty());
            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameValidationBinding);

            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            // string min length validator
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameValidationBinding);
        }

        // BUTOANELE PT CUSTOMERS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void btnNewCus_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewCus.IsEnabled = false;
            btnEditCus.IsEnabled = false;
            btnDeleteCus.IsEnabled = false;

            btnSaveCus.IsEnabled = true;
            btnCancelCus.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevCus.IsEnabled = false;
            btnNextCus.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            Keyboard.Focus(firstNameTextBox);
        }
        private void btnEditCus_Click (object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNewCus.IsEnabled = false;
            btnEditCus.IsEnabled = false;
            btnDeleteCus.IsEnabled = false;

            btnSaveCus.IsEnabled = true;
            btnCancelCus.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevCus.IsEnabled = false;
            btnNextCus.IsEnabled = false;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            Keyboard.Focus(firstNameTextBox);

            SetValidationBinding();
        }
        private void btnDeleteCus_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNewCus.IsEnabled = false;
            btnEditCus.IsEnabled = false;
            btnDeleteCus.IsEnabled = false;

            btnSaveCus.IsEnabled = true;
            btnCancelCus.IsEnabled = true;
            customerDataGrid.IsEnabled = false;
            btnPrevCus.IsEnabled = false;
            btnNextCus.IsEnabled = false;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);

            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
        }
        private void btnCancelCus_Click (object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNewCus.IsEnabled = true;
            btnEditCus.IsEnabled = true;
            btnDeleteCus.IsEnabled = true;

            btnSaveCus.IsEnabled = false;
            btnCancelCus.IsEnabled = false;
            customerDataGrid.IsEnabled = true;
            btnPrevCus.IsEnabled = true;
            btnNextCus.IsEnabled = true;

            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;

            firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameBinding);
            lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameBinding);
        }

        private void btnSaveCus_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New) // saving after we make a new element
            {
                try
                {
                    // instantiem customer entity
                    customer = new Customer()
                    {
                        //CustId = Int32.Parse(custIdTextBox.Text.Trim()),
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    // adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    // salvam modificarile
                    ctx.SaveChanges();
                }
                // using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewCus.IsEnabled = true;
                btnEditCus.IsEnabled = true;
                btnSaveCus.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevCus.IsEnabled = true;
                btnNextCus.IsEnabled = true;
                custIdTextBox.IsEnabled = false;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
            }
            else if (action == ActionState.Edit) // saving after we edit an element
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    //customer.CustId = Int32.Parse(custIdTextBox.Text.Trim());
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    // salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);

                btnNewCus.IsEnabled = true;
                btnEditCus.IsEnabled = true;
                btnDeleteCus.IsEnabled = true;

                btnSaveCus.IsEnabled = false;
                btnCancelCus.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevCus.IsEnabled = true;
                btnNextCus.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;

                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameBinding);
            }
            else if (action == ActionState.Delete) // saving after we delete an element
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();

                btnNewCus.IsEnabled = true;
                btnEditCus.IsEnabled = true;
                btnDeleteCus.IsEnabled = true;

                btnSaveCus.IsEnabled = false;
                btnCancelCus.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                btnPrevCus.IsEnabled = true;
                btnNextCus.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;

                firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameBinding);
                lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameBinding);
            }
        }

        private void btnNextCus_Click (object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevCus_Click (object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }


        // BUTOANELE PENTRU INVENTORY ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void btnNewInv_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNewInv.IsEnabled = false;
            btnEditInv.IsEnabled = false;
            btnDeleteInv.IsEnabled = false;

            btnSaveInv.IsEnabled = true;
            btnCancelInv.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrevInv.IsEnabled = false;
            btnNextInv.IsEnabled = false;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);

            makeTextBox.Text = "";
            colorTextBox.Text = "";
            Keyboard.Focus(firstNameTextBox);
        }
        private void btnEditInv_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();

            btnNewInv.IsEnabled = false;
            btnEditInv.IsEnabled = false;
            btnDeleteInv.IsEnabled = false;

            btnSaveInv.IsEnabled = true;
            btnCancelInv.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrevInv.IsEnabled = false;
            btnNextInv.IsEnabled = false;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);

            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;
            Keyboard.Focus(makeTextBox);

        }
        private void btnDeleteInv_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();

            btnNewInv.IsEnabled = false;
            btnEditInv.IsEnabled = false;
            btnDeleteInv.IsEnabled = false;

            btnSaveInv.IsEnabled = true;
            btnCancelInv.IsEnabled = true;
            inventoryDataGrid.IsEnabled = false;
            btnPrevInv.IsEnabled = false;
            btnNextInv.IsEnabled = false;

            BindingOperations.ClearBinding(makeTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(colorTextBox, TextBox.TextProperty);

            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;
        }
        private void btnCancelInv_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNewInv.IsEnabled = true;
            btnEditInv.IsEnabled = true;
            btnDeleteInv.IsEnabled = true;

            btnSaveInv.IsEnabled = false;
            btnCancelInv.IsEnabled = false;
            inventoryDataGrid.IsEnabled = true;
            btnPrevInv.IsEnabled = true;
            btnNextInv.IsEnabled = true;

            makeTextBox.IsEnabled = false;
            colorTextBox.IsEnabled = false;

            makeTextBox.SetBinding(TextBox.TextProperty, makeBinding);
            colorTextBox.SetBinding(TextBox.TextProperty, colorBinding);
        }

        private void btnSaveInv_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New) // saving after we make a new element
            {
                try
                {
                    // instantiem customer entity
                    inventory = new Inventory()
                    {
                        Make = makeTextBox.Text.Trim(),
                        Color = colorTextBox.Text.Trim()
                    };
                    // adaugam entitatea nou creata in context
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    // salvam modificarile
                    ctx.SaveChanges();
                }
                // using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewInv.IsEnabled = true;
                btnEditInv.IsEnabled = true;
                btnSaveInv.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevInv.IsEnabled = true;
                btnNextInv.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
            }
            else if (action == ActionState.Edit) // saving after we edit an element
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Make = makeTextBox.Text.Trim();
                    inventory.Color = colorTextBox.Text.Trim();
                    // salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                inventoryViewSource.View.MoveCurrentTo(inventory);

                btnNewInv.IsEnabled = true;
                btnEditInv.IsEnabled = true;
                btnDeleteInv.IsEnabled = true;

                btnSaveInv.IsEnabled = false;
                btnCancelInv.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevInv.IsEnabled = true;
                btnNextInv.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;

                makeTextBox.SetBinding(TextBox.TextProperty, makeBinding);
                colorTextBox.SetBinding(TextBox.TextProperty, colorBinding);
            }
            else if (action == ActionState.Delete) // saving after we delete an element
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();

                btnNewInv.IsEnabled = true;
                btnEditInv.IsEnabled = true;
                btnDeleteInv.IsEnabled = true;

                btnSaveInv.IsEnabled = false;
                btnCancelInv.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevInv.IsEnabled = true;
                btnNextInv.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;

                makeTextBox.SetBinding(TextBox.TextProperty, makeBinding);
                colorTextBox.SetBinding(TextBox.TextProperty, colorBinding);
            }
        }

        private void btnNextInv_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }
        private void btnPrevInv_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        // BUTOANELE PT ORDERS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void btnSaveOrd_Click (object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;

                    // instantiem Order entity
                    order = new Order()
                    {
                        CustId = customer.CustId,
                        CarId = inventory.CarId
                    };

                    // adaugam entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    // salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNewOrd.IsEnabled = true;
                btnEditOrd.IsEnabled = true;
                btnSaveOrd.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPrevOrd.IsEnabled = true;
                btnNextOrd.IsEnabled = true;

                cmbCustomers.IsEnabled = false;
                cmbInventory.IsEnabled = false;
            }

            else if (action == ActionState.Edit)
            {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                        // salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                customerViewSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;

                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Order Deleted Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            // code before changes / before step 40
            /*else if (action == ActionState.Edit) // saving after we edit an element
            {
                try
                {
                    // schimbam order-ul de la drop down-uri
                    order = (Order)ordersDataGrid.SelectedItem;
                    order.CustId = cmbCustomers.SelectedIndex;
                    order.CarId = cmbCustomers.SelectedIndex;

                    // salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerOrdersViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                customerOrdersViewSource.View.MoveCurrentTo(order);

                btnNewOrd.IsEnabled = true;
                btnEditOrd.IsEnabled = true;
                btnDeleteOrd.IsEnabled = true;

                btnSaveOrd.IsEnabled = false;
                btnCancelOrd.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPrevOrd.IsEnabled = true;
                btnNextOrd.IsEnabled = true;

                cmbCustomers.IsEnabled = false;
                cmbInventory.IsEnabled = false;
            }
            else if (action == ActionState.Delete) // saving after we delete an element
            {
                try
                {
                    order = (Order)ordersDataGrid.SelectedItem;
                    ctx.Orders.Remove(order);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerOrdersViewSource.View.Refresh();

                btnNewOrd.IsEnabled = true;
                btnEditOrd.IsEnabled = true;
                btnDeleteOrd.IsEnabled = true;

                btnSaveOrd.IsEnabled = false;
                btnCancelOrd.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPrevOrd.IsEnabled = true;
                btnNextOrd.IsEnabled = true;

                cmbCustomers.IsEnabled = false;
                cmbInventory.IsEnabled = false;
            }*/
        }
       
    }
}
