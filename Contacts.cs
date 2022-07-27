using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Contacts
{
    internal class Contacts
    {
        public int id { get; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nickName { get; set; }
        public string photo { get; set; }
        public string company { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }  
        public string zip { get; set; } 
        public string country { get; set; }
        public string notes { get; set; }


        public Contacts(string firstName, string lastName, string nickName, string photo, string company, string street, string city, string state, string zip, string country, string notes)
        {
            
            this.firstName = firstName;
            this.lastName = lastName;
            this.nickName = nickName;
            this.photo = photo;
            this.company = company;
            this.street = street;
            this.city = city;
            this.state = state;
            this.zip = zip;
            this.country = country;
            this.notes = notes;
        }//end constructor
    }//end classs
}//end namespace
