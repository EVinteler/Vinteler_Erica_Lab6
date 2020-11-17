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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;
            ctx.Customers.Load();
            // Load data by setting the CollectionViewSource.Source property:
            // customerViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
        }


        // BUTOANELE PT CUSTOMERS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
            }
            else if (action == ActionState.Edit) // saving after we edit an element
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
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

                //firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
                //lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);
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

                //firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameTextBoxBinding);
                //lastNameTextBox.SetBinding(TextBox.TextProperty, lastNameTextBoxBinding);
            }
        }

        private void btnNextCus_Click (object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }
        private void btnPreviousCus_Click (object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }


        // BUTOANELE PENTRU INVENTORY ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

                //makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                //colorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
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

                //makeTextBox.SetBinding(TextBox.TextProperty, makeTextBoxBinding);
                //colorTextBox.SetBinding(TextBox.TextProperty, colorTextBoxBinding);
            }
        }

        private void btnNextInv_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }
        private void btnPreviousInv_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }
    }
}
