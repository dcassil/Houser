using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser.Data
{
    public class PropertyCompsDB
    {
        public static bool PropertyCompExists(string subject, string comp)
        {
            PropertyData db = new PropertyData();
            var propComp = from p in db.PropComps
                           where p.PropertyAccount == subject && p.CompAccount == comp
                           select p;
            if (propComp.Any())
                return true;
            else
                return false;
        }

        public static bool PropertyHasComps(string subject)
        {
            PropertyData db = new PropertyData();
            var propComp = from p in db.PropComps
                           where p.PropertyAccount == subject
                           select p;
            if (propComp.Any())
                return true;
            else
                return false;
        }

        public static void InsertPropertyComp(string subject, string comp)
        {
            PropertyData db = new PropertyData();
            PropComp propComp = new PropComp();
            propComp.PropertyAccount = subject;
            propComp.CompAccount = comp;

            db.PropComps.InsertOnSubmit(propComp);
            db.SubmitChanges();
        }
    }
}