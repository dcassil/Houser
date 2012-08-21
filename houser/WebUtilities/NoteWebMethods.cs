using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using houser.Business;

namespace houser.WebUtilities
{
    [System.Web.Script.Services.ScriptService]
    public class NoteWebMethods
    {
        [WebMethod]
        public void SaveAccountNote(string accountNumber, string note)
        {
            Note notes = new Note(accountNumber);
            notes.Notes = note;

        }
    }
}