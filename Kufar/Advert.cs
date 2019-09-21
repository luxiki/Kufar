using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kufar
{

    public class Ads
    {
        public List<Advert> ads { get; set; } 
    }

    public class Advert
    {
        public string subject { get; set; }

        public string newSubject { get; set; }

        public int? price_byn_change { get; set; }

        public int? price_byn { get; set; }

        public string ad_link { get; set; }

        public Advert() { }
        public Advert(Advert Old, Advert New)
        {
            if( Old.subject != New.subject)
            {
                newSubject = New.subject;
            }
            subject = Old.subject;
            ad_link = Old.ad_link;
            price_byn = Old.price_byn/100;
            price_byn_change = New.price_byn/100;

        }

    }
}
