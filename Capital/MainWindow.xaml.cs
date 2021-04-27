using Capital.Core.IO;
using Capital.Core.Main;
using Capital.Core.Modules;
using ModernWpf.Controls;
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

    public partial class MainWindow : Window
    {
        private ConfigurationHandler configHandler;
        public CreateNewDialog createNewDialog;
        private StockEngine stockEngine;

        public MainWindow()
        {
            InitializeComponent();

            configHandler = new ConfigurationHandler(this);
            createNewDialog = new CreateNewDialog(configHandler);
            stockEngine = new StockEngine(this);

            stockListView.ItemsSource = stockEngine.dashItems;
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddDialog dialog = new AddDialog(configHandler);
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                //Get save item from the selecton | No error catching right now, lets just hope it finds it
                ConfigurationSaveItem saveItem = configHandler.saveItemList.Where(var => var.viewItem.configName == dialog.configBox.Text).First();

                //Define engine settings
                EngineSettings settings = new EngineSettings()
                {
                    interval = (int)dialog.checkingIntervalNumeral.Value,
                    productURL = dialog.productURLText.Text,
                    price = dialog.priceText.Text
                };

                //Create an instance of the checker in the StockEngine
                stockEngine.createInstance(saveItem, settings);
            }
        }

        private void createNewButton_Click(object sender, RoutedEventArgs e)
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

        private void editConfiguration_Click(object sender, RoutedEventArgs e)
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
