using HelperClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kufar
{
    class VM:DependencyObject
    {

        public string WebAddress
        {
            get { return (string)GetValue(WebAddressProperty); }
            set { SetValue(WebAddressProperty, value); }
        }
        public static readonly DependencyProperty WebAddressProperty =
            DependencyProperty.Register("WebAddress", typeof(string), typeof(VM), new PropertyMetadata(""));



        AdvertManager advertManager = new AdvertManager("https://www.kufar.by/listings?size=42&sort=lst.d&cur=BYR&cat=5030&ct=9&rgn=all");

        public int? Page
        {
            get { return (int?)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(int?), typeof(VM), new PropertyMetadata(null));


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
            set { WebAddress = value.ad_link; SetValue(advertSelectProperty, value); }
        }
        public static readonly DependencyProperty advertSelectProperty =
            DependencyProperty.Register("advertSelect", typeof(Advert), typeof(VM), new PropertyMetadata(null , WebAddressChange));
        private static void WebAddressChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as VM;
            current.WebAddress = current.advertSelect.ad_link;
            
        }

        public VM()
        {
            StartScan = new DelegateCommand(_StartScan);
            Save = new DelegateCommand(_Save);
            OpenInWebBrowser = new DelegateCommand(_OpenInWebBrowser);
            advertManager.ChangePage += AdvertManager_ChangePage;
            //System.Diagnostics.Process.Start("http://www.onliner.by");
        }

        private void AdvertManager_ChangePage(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate { Page = advertManager.Page; }));
            
        }

        private async void _StartScan(object obj)
        {
            await Task.Run(() => { advertManager.StartScan(); });
            adverts = new ObservableCollection<Advert>(advertManager.AdvertsChange);
            MessageBox.Show("Cmplete!!!");
        }

        private void _Save(object obj)
        {
            advertManager.SaveJson();
            MessageBox.Show("Save OK!!!");
        }

        private void _OpenInWebBrowser(object obj) { System.Diagnostics.Process.Start(WebAddress); }


    }
}
