using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Capital.Core.IO;
using Capital.Core.Modules;
using Leaf.xNet;
using MahApps.Metro.Controls;

namespace Capital.Core.Main
{
    public class EngineSettings
    {
        public int interval { get; set; }
        public string productURL { get; set; }
    }

    public class StockEngine
    {
        private MainWindow windowRef;

        public StockEngine(MainWindow windowRef)
        {
            this.windowRef = windowRef;

            LoggerFactory.debug(this, "Object created.");
        }

        public void createInstance(ConfigurationSaveItem saveItem, EngineSettings settings)
        {
            if (saveItem != null)
            {
                Instance newInstance = new Instance(settings, saveItem);

                windowRef.stockListView.Items.Add(new DashboardViewItem { product = settings.productURL, store = "Null", lastChecked = "Null", price = "$0", timesChecked = "0 Times", timeStarted = "Null" });

                newInstance.start(windowRef);
            }
        }
    }

    public class Instance
    {
        private EngineSettings settings;
        private ConfigurationSaveItem saveItem;

        public DateTime timeStarted = DateTime.Now;
        public int timesChecked = 0;
        public bool runInstance = true;

        public Instance(EngineSettings settings, ConfigurationSaveItem saveItem)
        {
            this.settings = settings;
            this.saveItem = saveItem;

            LoggerFactory.debug(this, "Started instance on config: " + saveItem.viewItem.configName);
        }

        public void start(MainWindow windowRef)
        {
            new Thread(() =>
            {
                while (runInstance)
                {
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        httpRequest.ClearAllHeaders();
                        httpRequest.UserAgent = Http.ChromeUserAgent();
                        httpRequest.ConnectTimeout = 10000;

                        string response = httpRequest.Get(settings.productURL, null).ToString();

                        timesChecked++;

                        windowRef.stockListView.Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            DashboardViewItem item = windowRef.stockListView.Items.IndexOf(2)();
                            
                            (new DashboardViewItem { product = "Nvidia RTX 3080 Max Pro", store = "MSY - Melton", lastChecked = "30 seconds ago", price = "$1300 USD", timesChecked = "1313100 Times", timeStarted = "12/2/2021" });
                        }));
                    }

                    Thread.Sleep(settings.interval * 1000);
                }
            }).Start();
        }
    }
}
