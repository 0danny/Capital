using Capital.Core.IO;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Capital.Core.Modules
{
    public class ConfigurationHandler
    {
        public MainWindow windowRef;
        private string folderPath = Directory.GetCurrentDirectory() + @"\Configurations";

        public List<ConfigurationSaveItem> saveItemList = new List<ConfigurationSaveItem>();

        public ConfigurationHandler(MainWindow windowRef)
        {
            this.windowRef = windowRef;

            LoggerFactory.debug(this, "object creation successful.");

            loadConfigs();
        }

        //Very inefficient
        public ConfigurationSaveItem getSaveItem(ConfigurationViewItem viewItem)
        {
            return JsonConvert.DeserializeObject<ConfigurationSaveItem>(File.ReadAllText(folderPath + @"\" + viewItem.configName + ".config"));
        }

        public async void deleteConfig(ConfigurationViewItem listViewItem, bool confirmation)
        {
            try
            {
                if (confirmation == true)
                {
                    ContentDialogResult result = await new CustomDialog("Information", "Are you sure you want to delete this configuration?", true).ShowAsync();

                    if (result != ContentDialogResult.Primary)
                    {
                        return;
                    }
                }

                string fullPath = folderPath + @"\" + listViewItem.configName + ".config";
                LoggerFactory.debug(this, "Deleting the file at: " + fullPath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                saveItemList.Remove(saveItemList.Where(var => var.viewItem.configName == listViewItem.configName).First());
                windowRef.configView.Items.Remove(listViewItem);
            }
            catch (Exception ex)
            {
                LoggerFactory.debug(this, "Exception: " + ex.Message);
            }
        }

        public void loadConfigs()
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    _ = new CustomDialog("Information", "Detected a first boot, creating configs directory.").ShowAsync();
                }

                string[] filePaths = System.IO.Directory.GetFiles(folderPath, "*.config");

                //I should probably be doing this in a new thread, will change later
                foreach (string path in filePaths)
                {
                    ConfigurationSaveItem saveItem = JsonConvert.DeserializeObject<ConfigurationSaveItem>(File.ReadAllText(path));
                    saveItemList.Add(saveItem);
                    windowRef.configView.Items.Add(saveItem.viewItem);
                }
            }
            catch (Exception ex)
            {
                _ = new CustomDialog("Error", "Could not load configurations - " + ex.Message).ShowAsync();
            }
        }

        public void saveConfig(ConfigurationSaveItem saveItem, bool overwrite)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string completePathString = folderPath + @"\" + saveItem.viewItem.configName + ".config";

                if (File.Exists(completePathString) && overwrite == false)
                {
                    windowRef.createNewDialog.Hide();
                    _ = new CustomDialog("Error", "A file with the same name already exists.").ShowAsync();
                    return;
                }

                saveItemList.Add(saveItem);

                File.WriteAllText(completePathString, JsonConvert.SerializeObject(saveItem));

                LoggerFactory.debug(this, "New file saved as: " + saveItem.viewItem.configName);
            }
            catch (Exception ex)
            {
                LoggerFactory.debug(this, "Error writing file - " + ex.Message);
            }
        }
    }

    public class ConfigurationSaveItem
    {
        public ConfigurationViewItem viewItem { get; set; }

        public string[] successKeys { get; set; }
    }

    public class ConfigurationViewItem
    {
        public string configName { get; set; }

        public DateTime timeCreated { get; set; }

        public string websiteName { get; set; }

        public string author { get; set; }
    }
}
