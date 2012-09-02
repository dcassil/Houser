using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;
using System.Text;

namespace houser.Business
{
    public class Note
    {
        #region Fields
        protected string _accountNumber;
        protected string _notes;
        protected int _userID;
        protected bool _isNew;
        #endregion

        #region Properties
        public string AccountNumber { get { return _accountNumber; } set { _accountNumber = value; } }
        public string Notes { get { return _notes; } set { _notes = value; } }
        public int UserID { get { return _userID; } set { _userID = value; } }
        public bool IsNew { get { return _isNew; } set { _isNew = value; } }
        #endregion

        #region Constructors
        public Note()
        { }
        public Note(string accountNumnber, int userID)
        {
            DataRow notes = NoteDB.GetNotesByAccountNumber(accountNumnber, userID);
            if (notes == null)
            {
                _isNew = true;
                _accountNumber = accountNumnber;
                _notes = string.Empty;
                _userID = userID;
            }
            else
            {
                _notes = notes["Note"].ToString();
                _isNew = false;
                _accountNumber = accountNumnber;
                _userID = Convert.ToInt32(notes["UserID"]);
            }
        }
        #endregion

        #region Persistance
        public void Save()
        {
            if (IsNew)
            {
                NoteDB.InsertNote(_accountNumber, _notes, _userID);
                IsNew = false;
            }
            else
                NoteDB.UpdateNote(_accountNumber, _notes, _userID);
        }
        #endregion
    }
}