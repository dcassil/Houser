using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;

namespace houser
{
    public class PropertyComps
    {
        public static bool PropertyCompExists(string subject, string comp)
        {
            return PropertyCompsDB.PropertyCompExists(subject, comp);
        }

        public static bool PropertyHasComps(string subject)
        {
            return PropertyCompsDB.PropertyHasComps(subject);
        }

        public static void InsertPropertyComp(string subject, string comp)
        {
            PropertyCompsDB.InsertPropertyComp(subject, comp);
        }
    }
}