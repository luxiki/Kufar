using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace Kufar
{
    class AdvertManager
    {
        private readonly string URLNEXT = "\"kf-a-c-luaw kf-a-e-luaw\"";

        private readonly string URLNEXTSTART = "\"kf-a-b-uZXI\"";

        private readonly string JSONSTART = "\"listingsData\"";

        private readonly string JSON_OLD_PATH = $"{Environment.CurrentDirectory}\\AdvertsOld.json";

        private string url = null;

        private string html = null;

        private string json = null;

        private string jsonOld;

        public int Page { get; private set; }

        public event EventHandler ChangePage;

        public LinkedList<Advert> AdvertsOld = new LinkedList<Advert>();

        private Ads Adverts = new Ads();

        public LinkedList<Advert> AdvertsChange = new LinkedList<Advert>();

        public AdvertManager()
        {
            Adverts.ads = new List<Advert>();
        }

        public void StartScan(string Url)
        {
            url = Url;
            int w = 53;
            do
            {
                GetHtml();

                UrlNext();

                GetJson();

                AddAdvert();
                ChangePage(null,null);
                Page++;
            }
            while ((!string.IsNullOrEmpty(url)) && (!string.IsNullOrEmpty(html)) );
            while(w-->0) ;
            GetChangeAdvert();

        }

        public void SaveJson()
        {
            File.WriteAllText( JSON_OLD_PATH, JsonConvert.SerializeObject(Adverts));
        }


        private void GetChangeAdvert()
        {
            //try
            //{
                jsonOld = File.ReadAllText(JSON_OLD_PATH);

                var listingsDatas = JsonConvert.DeserializeObject<Ads>(jsonOld);
                if (listingsDatas != null)
                {
                    foreach (var advert in listingsDatas.ads)
                    {
                        AdvertsOld.AddLast(advert);
                    }
                }

                if ((Adverts.ads.Count > 0) && (AdvertsOld.Count > 0))
                {

                    foreach (var advert in Adverts.ads)
                    {
                        var adv = AdvertsOld.FirstOrDefault(z => z.ad_link == advert.ad_link);
                        if (adv != null)
                        {
                            if (adv.price_byn != advert.price_byn)
                            {
                                AdvertsChange.AddLast(new Advert(adv,advert));
                            }
                            AdvertsOld.DefaultIfEmpty(adv);
                        }

                    }

                }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return;
            //}

            

        }
        private void GetJson() { 
        
            if (!string.IsNullOrEmpty(html))
            {
            int index = html.IndexOf(JSONSTART);

            string result = html.Substring(index + 15);

            int s = 1;
            int i = 20;
            while ((s > 0) && (i < result.Length-1))
            {
                i++;
                if (result[i] == '[') { s++; }
                if (result[i] == ']') { s--; }
            }

            json = result.Substring(0, i+1) + '}';

            //File.WriteAllText("ss.txt", json);
            //File.WriteAllText("sss.txt", html);
            }
        }
        private void AddAdvert()
        {
            try
            {
                var listingsDatas = JsonConvert.DeserializeObject<Ads>(json);

                foreach (var advert in listingsDatas.ads)
                {
                    Adverts.ads.Add(advert);
                }
            }
            catch (Exception) { }

        }
        private void UrlNext()
        {
            if (!string.IsNullOrEmpty(html))
            {
                int index = html.IndexOf(URLNEXTSTART) + 250;

                string result = html.Substring(index, 1600);

                index = result.IndexOf(URLNEXT);

                if (index > 0)
                {
                    result = result.Substring(index + URLNEXT.Length + 5);

                    index = result.IndexOf('"');

                    result = result.Substring(index + 1);

                    index = result.IndexOf('"');

                    result = result.Substring(0, index);

                    result = "https://www.kufar.by" + result;

                    result = result.Replace("amp;", "");

                    url = result;
                }
                else { url = null; }
            }
            

        }
        private void GetHtml()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                HttpWebResponse responce = (HttpWebResponse)request.GetResponse();

                using (StreamReader stream = new StreamReader(
                    responce.GetResponseStream(), Encoding.UTF8))
                {
                    html = stream.ReadToEnd();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                html = null;
            }
            
        }


    }
}
