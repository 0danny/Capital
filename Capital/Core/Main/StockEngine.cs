using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Capital.Core.IO;
using System.Drawing;
using System.Runtime.CompilerServices;
using Capital.Core.Modules;
using Leaf.xNet;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Capital.Core.Utilities;

namespace Capital.Core.Main
{
    public class DashboardViewItem : INotifyPropertyChanged
    {
        /*
         * I don't know if this needs to be done but for some reason without it
         * the setter in the main vars makes the program crash
         */

        public string productOBJ { get; set; }

        public string storeOBJ { get; set; }

        public string lastHitOBJ { get; set; }

        public string priceOBJ { get; set; }

        private string timesCheckedOBJ;

        public string timeStartedOBJ { get; set; }


        public string product
        {
            get { return productOBJ; }
            set
            {
                productOBJ = value;
                OnPropertyChanged();
            }
        }

        public string store
        {
            get { return storeOBJ; }
            set
            {
                storeOBJ = value;
                OnPropertyChanged();
            }
        }

        public string lastHit
        {
            get { return lastHitOBJ; }
            set
            {
                lastHitOBJ = value;
                OnPropertyChanged();
            }
        }

        public string price
        {
            get { return priceOBJ; }
            set
            {
                priceOBJ = value;
                OnPropertyChanged();
            }
        }

        public string timesChecked
        {
            get { return timesCheckedOBJ; }
            set
            {
                timesCheckedOBJ = value;
                OnPropertyChanged();
            }
        }

        public string timeStarted
        {
            get { return timeStartedOBJ; }
            set
            {
                timeStartedOBJ = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class EngineSettings
    {
        public int interval { get; set; }
        public string productURL { get; set; }
        public string price { get; set; }
    }

    public class StockEngine
    {
        private MainWindow windowRef;
        public ObservableCollection<DashboardViewItem> dashItems = new ObservableCollection<DashboardViewItem>();
        public List<Instance> currentInstances = new List<Instance>();

        public StockEngine(MainWindow windowRef)
        {
            this.windowRef = windowRef;

            LoggerFactory.debug(this, "Object created.");
        }

        public void createInstance(ConfigurationSaveItem saveItem, EngineSettings settings)
        {
            if (saveItem != null)
            {
                DashboardViewItem dashItem = new DashboardViewItem
                {
                    product = settings.productURL,
                    store = saveItem.viewItem.configName,
                    lastHit = "Never", //Most of this will be changed
                    price = settings.price,
                    timesChecked = "0 Times",
                    timeStarted = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss tt")
                };

                dashItems.Add(dashItem);

                Instance newInstance = new Instance(settings, saveItem, this, dashItem);

                currentInstances.Add(newInstance);

                newInstance.start(windowRef);
            }
        }
    }

    public class Instance
    {
        private EngineSettings settings;
        private ConfigurationSaveItem saveItem;
        private StockEngine engineInstance;
        private DashboardViewItem dashItem;

        public int timesChecked = 0;
        public bool runInstance = true;
        public DateTime lastHit = DateTime.MinValue;

        public Instance(EngineSettings settings, ConfigurationSaveItem saveItem, StockEngine engineInstance, DashboardViewItem dashItem)
        {
            this.settings = settings;
            this.saveItem = saveItem;
            this.engineInstance = engineInstance;
            this.dashItem = dashItem;

            LoggerFactory.debug(this, "Started instance on config: " + saveItem.viewItem.configName);
        }

        public void start(MainWindow windowRef)
        {
            new Thread(() =>
            {
                while (runInstance)
                {

                    try
                    {
                        using (HttpRequest httpRequest = new HttpRequest())
                        {
                            httpRequest.ClearAllHeaders();
                            httpRequest.UserAgent = Http.ChromeUserAgent();
                            httpRequest.ConnectTimeout = 10000;

                            string response = httpRequest.Get(settings.productURL, null).ToString();

                            bool hasHit = false;
                            for (int i = 0; i < saveItem.successKeys.Length; i++)
                            {
                                if (response.Contains(saveItem.successKeys[i]))
                                {
                                    hasHit = true;
                                }
                            }

                            if (hasHit)
                            {
                                //hit logic
                                lastHit = DateTime.Now;
                            }

                            
                            Interlocked.Increment(ref timesChecked);

                            windowRef.stockListView.Dispatcher.BeginInvoke(new Action(delegate ()
                            {
                                LoggerFactory.debug(this, "Updating the listview object.");

                                DashboardViewItem changeItem = engineInstance.dashItems.Where(var => var == dashItem).First();

                                if (lastHit != DateTime.MinValue)
                                {
                                    changeItem.lastHit = Helpers.TimeAgo(lastHit);
                                }

                                changeItem.timesChecked = timesChecked + " Times";
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        //No error handling as of yet
                        LoggerFactory.debug(this, ex.Message);
                    }

                    Thread.Sleep(settings.interval * 1000);
                }
            }).Start();
        }
    }
}
