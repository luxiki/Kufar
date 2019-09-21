using HelperClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kufar
{
    class VM:DependencyObject
    {
        private readonly string FILEPATH = "WebAdress.ini";
        public string WebAddress
        {
            get { return (string)GetValue(WebAddressProperty); }
            set { SetValue(WebAddressProperty, value); }
        }

        public static readonly DependencyProperty WebAddressProperty =
            DependencyProperty.Register("WebAddress", typeof(string), typeof(VM), new PropertyMetadata(""));

        public int? Page
        {
            get { return (int?)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(int?), typeof(VM), new PropertyMetadata(null));

        public bool IEnabled
        {
            get { return (bool)GetValue(IEnabledProperty); }
            set { SetValue(IEnabledProperty, value); }
        }
        public static readonly DependencyProperty IEnabledProperty =
            DependencyProperty.Register("IEnabled", typeof(bool), typeof(VM), new PropertyMetadata(true));


        public DelegateCommand StartScan { get; set; }
        public DelegateCommand Save { get; set; }

        public DelegateCommand OpenInWebBrowser { get; set; }

        //private ObservableCollection<Advert> _adverts;
        public ObservableCollection<Advert> adverts
        {
            get { return (ObservableCollection<Advert>)GetValue(advertsProperty); }
            set { SetValue(advertsProperty, value); }
        }

        public static readonly DependencyProperty advertsProperty =
            DependencyProperty.Register("adverts", typeof(ObservableCollection<Advert>), typeof(VM), new PropertyMetadata(null));

        public Advert advertSelect
        {
            get { return (Advert)GetValue(advertSelectProperty); }
            set { SetValue(advertSelectProperty, value); }
        }

        public static readonly DependencyProperty advertSelectProperty =
            DependencyProperty.Register("advertSelect", typeof(Advert), typeof(VM), new PropertyMetadata(null));

        private AdvertManager advertManager;


        public VM()
        {
            Application.Current.MainWindow.Closing += MainWindow_Closing;
            StartScan = new DelegateCommand(_StartScan);
            Save = new DelegateCommand(_Save);
            OpenInWebBrowser = new DelegateCommand(_OpenInWebBrowser);
            try
            {
                WebAddress = File.ReadAllText(FILEPATH);
            }
            catch (Exception)
            {
                WebAddress = "http://www.kufar.by";
                File.WriteAllText(FILEPATH,WebAddress);
            }
            advertManager = new AdvertManager();
            advertManager.ChangePage += AdvertManager_ChangePage;
            //System.Diagnostics.Process.Start("http://www.onliner.by");
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if ((adverts!=null) && (adverts.Count > 0))
            {
                File.WriteAllText(FILEPATH, WebAddress);
            }
        }

        private void AdvertManager_ChangePage(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate { Page = advertManager.Page; }));
        }

        private async void _StartScan(object obj)
        {
            IEnabled = false;
            string url = WebAddress;
            await Task.Run(() => { advertManager.StartScan(url); });
            adverts = new ObservableCollection<Advert>(advertManager.AdvertsChange);
            MessageBox.Show("Cmplete!!!");
        }

        private void _Save(object obj)
        {
            advertManager.SaveJson();
            MessageBox.Show("Save OK!!!");
        }

        private void _OpenInWebBrowser(object obj)
        {
            if ((advertSelect!=null) && (!string.IsNullOrEmpty(advertSelect.ad_link)))
            {
                System.Diagnostics.Process.Start(advertSelect.ad_link);
            }
        }

        private static void WebAddressChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var current = d as VM;
            //if (!string.IsNullOrEmpty(current.WebAddress))
            //{
            //    current.WebAddress = current.advertSelect.ad_link;
            //}
        }


    }
}
