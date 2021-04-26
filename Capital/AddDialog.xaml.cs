using Capital.Core.IO;
using Capital.Core.Modules;
using ModernWpf.Controls;
using System;
using System.Windows;

namespace Capital
{
    public partial class AddDialog
    {
        private ConfigurationHandler configHandler;

        public AddDialog(ConfigurationHandler configHandler)
        {
            InitializeComponent();
            this.configHandler = configHandler;

            LoggerFactory.debug(this, "Size: " + configHandler.saveItemList.Count);
            foreach (ConfigurationSaveItem saveItem in configHandler.saveItemList)
            {   
                configBox.Items.Add(saveItem.viewItem.configName);
            }
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void OnClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {

        }

        private void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {

        }
    }
}
