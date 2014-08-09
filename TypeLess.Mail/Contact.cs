using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeLess;

namespace TypeLess.Mail
{
    public class Contact
    {
        private ContactType _type;
        private string _name;
        private string _mail;

        public Contact(string mailAddress)
        {
            mailAddress.If("mailAddress").IsNull.IsNotValidEmail.ThenThrow();

            this._name = mailAddress;
            this._mail = mailAddress;
        }

        public Contact(string mailAddress, string name)
        {
            mailAddress.If("mailAddress").IsNull.IsNotValidEmail.ThenThrow();

            this._name = name;
            this._mail = mailAddress;
        }

        public Contact(string mailAddress, ContactType type)
        {
            mailAddress.If("mailAddress").IsNull.IsNotValidEmail.ThenThrow();

            this._type = type;
            this._name = mailAddress;
            this._mail = mailAddress;
        }

        public Contact(string mailAddress, string name, ContactType type)
        {
            mailAddress.If("mailAddress").IsNull.IsNotValidEmail.ThenThrow();

            this._type = type;
            this._name = name;
            this._mail = mailAddress;
        }

        public ObjectAssertion IsInvalid()
        {
            return ObjectAssertion.New(_mail.If("mail address").IsNull.IsNotValidEmail);
        }

        public ContactType Type
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string MailAddress
        {
            get { return _mail; }
        }
    }
}
