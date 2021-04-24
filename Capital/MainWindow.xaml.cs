using Capital.Core.IO;
using Capital.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Capital
{

    public class DashboardViewItem
    {
        public string product { get; set; }

        public string store { get; set; }

        public string lastChecked { get; set; }

        public string price { get; set; }

        public string timesChecked { get; set; }

        public string timeStarted { get; set; }
    }

    public partial class MainWindow : Window
    {
        private ConfigurationHandler configHandler;
        public CreateNewDialog createNewDialog;

        public MainWindow()
        {
            InitializeComponent();

            configHandler = new ConfigurationHandler(this);
            createNewDialog = new CreateNewDialog(configHandler);

            stockListView.Items.Add(new DashboardViewItem { product = "Nvidia RTX 3080 Max Pro", store = "MSY - Melton", lastChecked = "30 seconds ago", price = "$1300 USD", timesChecked = "1313100 Times", timeStarted = "12/2/2021" });
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _ = new AddDialog().ShowAsync();
        }

        private void CreateNewButton_Click(object sender, RoutedEventArgs e)
        {
            createNewDialog.ShowAsync();
        }

        private void deleteConfiguration_Click(object sender, RoutedEventArgs e)
        {
            if (configView.SelectedItems.Count > 0)
            {
                configHandler.deleteConfig((ConfigurationViewItem)configView.SelectedItem, true);
            }
            else
            {
                _ = new CustomDialog("Information", "Please select a configuration to delete.").ShowAsync();
            }
        }

        private void EditConfiguration_Click(object sender, RoutedEventArgs e)
        {
            if (configView.SelectedItems.Count > 0)
            {
                ConfigurationSaveItem saveItem = configHandler.getSaveItem((ConfigurationViewItem)configView.SelectedItem);

                createNewDialog.currentViewItem = (ConfigurationViewItem)configView.SelectedItem;
                createNewDialog.Title = "Edit Item";
                createNewDialog.authorNameTxt.Text = saveItem.viewItem.author;
                createNewDialog.websiteNameTxt.Text = saveItem.viewItem.websiteName;
                createNewDialog.configNameTxt.Text = saveItem.viewItem.configName;
                createNewDialog.successKeysTxt.Text = String.Join(",", saveItem.successKeys);
                createNewDialog.ShowAsync();
            }
            else
            {
                _ = new CustomDialog("Information", "Please select a configuration to edit.").ShowAsync();
            }
        }
    }
}
