using Capital.Core.Modules;
using ModernWpf.Controls;
using System;
using System.Windows;

namespace Capital
{
    public partial class CreateNewDialog
    {
        private ConfigurationHandler configHandler;

        public ConfigurationViewItem currentViewItem = null;

        public CreateNewDialog(ConfigurationHandler configHandler)
        {
            InitializeComponent();

            this.configHandler = configHandler;
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //Check for empty config name
            if (String.IsNullOrEmpty(configNameTxt.Text))
            {
                configLabel.Content = "Config Name (Required): ";
                args.Cancel = true;
                return;
            }

            /*
             * Check for successKeys parsing, if it does not contain a ',' then its a clear indicator
             * that the parsing will fail and throw an exception.
             */

            if (!successKeysTxt.Text.Contains(",") || String.IsNullOrEmpty(successKeysTxt.Text))
            {
                args.Cancel = true;
                Hide();
                _ = new CustomDialog("Error", "Could not parse the success keys.").ShowAsync();
                return;
            }

            string[] successKeys = successKeysTxt.Text.Split(',');

            //Parse the data to JSON Object
            ConfigurationViewItem viewItem = new ConfigurationViewItem()
            {
                author = authorNameTxt.Text,
                configName = configNameTxt.Text, 
                timeCreated = DateTime.Now,
                websiteName = websiteNameTxt.Text
            };

            ConfigurationSaveItem saveItem = new ConfigurationSaveItem()
            {
                viewItem = viewItem,
                successKeys = successKeys
            };

            if (currentViewItem == null) //Adding a new configuration
            {
                configHandler.saveConfig(saveItem, false);
                configHandler.windowRef.configView.Items.Add(viewItem); 
            }
            else //Editing an existng configuration
            {
                configHandler.deleteConfig(currentViewItem, false); //Remove existing
                configHandler.saveConfig(saveItem, false); //Save the new one
                configHandler.windowRef.configView.Items.Add(viewItem); //Add new one to ListView
            }
        }

        private void OnCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private void OnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            currentViewItem = null;
            Title = "Create New";
            configLabel.Content = "Config Name: ";
            authorNameTxt.Text = "";
            websiteNameTxt.Text = "";
            configNameTxt.Text = "";
            successKeysTxt.Text = "";
        }
    }
}
