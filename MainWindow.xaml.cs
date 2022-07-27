using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace WPF_Contacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static private string sqlConnectionString = @"Data Source=MYZENBOOK\SQLEXPRESS;Initial Catalog = Contacts_Backup_DB; Integrated Security = True; Pooling=False";

        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper(sqlConnectionString);                      
            DisplayContacts();            
        }//end main window

        #region Display Functions

        /*Use This Chart as Reference to Index Postions for the Results Array used inside of all display functions
         *  int id = 0;
            int firstName = 1;
            int lastName = 2;
            int nickName = 3;
            int photo = 4;
            int company = 5;
            int street = 6;
            int city = 7;
            int state = 8;
            int zip = 9;
            int country = 10;
            int notes = 11;
            int favorite = 12;
            int active = 13; 
        * 
        */


        public void DisplayContacts()
        {
            //Query to get data from database table  
            object[][] results = dbHelper.ExecuteReader("SELECT *  FROM Contacts_Backup");
            int firstName = 1;
            int lastName = 2;
            int nickName = 3;
            int id = 0;
           
         
            for (int index = 0; index < results.Length; index++)
            {
                int contactId = (int)results[index][0];
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];
                object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
                lstContacts.Items.Add($"{results[index][id]} : {results[index][firstName]} {results[index][lastName]} : {mainPhoneNumberResults[0][0]}");
                
            }//end for
        }//end Display Contacts method

        public void DisplayContactsToEdit()
        {
            //Query to get data from database table  
            object[][] results = dbHelper.ExecuteReader("SELECT *  FROM Contacts_Backup");        
            int firstName = 1;
            int lastName = 2;
            int nickName = 3;
            int id = 0;
            bool isActive;

            for (int index = 0; index < results.Length; index++)
            {
                int contactId = (int)results[index][0];
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];
                object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");               
                lstEditContact.Items.Add($"{results[index][id]} : {results[index][firstName]} {results[index][lastName]} : {mainPhoneNumberResults[0][0]}");
               
            }//end for
        }//end Display Contacts method

        public void DisplayContactsToDelete()
        {
            //Query to get data from database table  
            object[][] results = dbHelper.ExecuteReader("SELECT *  FROM Contacts_Backup");
            int firstName = 1;
            int lastName = 2;
            int nickName = 3;
            int active = 13;
            int id = 0;


            for (int index = 0; index < results.Length; index++)
            {
                int contactId = (int)results[index][0];
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];
                object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
                if ((bool)results[index][active] == false)
                {
                    lstDeleteContact.Items.Add($"{results[index][id]} : {results[index][firstName]} {results[index][lastName]} : {mainPhoneNumberResults[0][0]}");
                }//end if
            }//end for
        }//end Display Contacts method

        public void DisplayActiveContacts()
        {
            //Query to get data from database table  
            object[][] results = dbHelper.ExecuteReader("SELECT *  FROM Contacts_Backup");
            int firstName = 1;
            int lastName = 2;
            int nickName = 3;
            int active = 13;
            int id = 0;


            for (int index = 0; index < results.Length; index++)
            {
                int contactId = (int)results[index][0];
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];
                object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
                if ((bool)results[index][active] == true)
                {
                    lstContacts.Items.Add($"{results[index][id]} : {results[index][firstName]} {results[index][lastName]} : {mainPhoneNumberResults[0][0]}");
                }//end if
            }//end for
        }//end display active contacts
                                                                    
        public void DisplayFavoriteContacts()
        {
            //Query to get data from database table  
            object[][] results = dbHelper.ExecuteReader("SELECT *  FROM Contacts_Backup");
            int id = 0;
            int firstName = 1;          
            int lastName = 2;
            int nickName = 3;
            int favorite = 12;

            for (int index = 0; index < results.Length; index++)
            {
                int contactId = (int)results[index][0];
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];
                object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
                if ((bool)results[index][favorite] == true)
                {
                    lstContacts.Items.Add($"{results[index][id]} : {results[index][firstName]} {results[index][lastName]} : {mainPhoneNumberResults[0][0]}");
                }//end if
            }//end for
        }

        private void DisplayContactInfoToEdit(int contactId)
        {
             
             object[][] results = dbHelper.ExecuteReader($"SELECT * FROM Contacts_Backup WHERE Id = {contactId}");
             object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
             object[][] emailIdResults = dbHelper.ExecuteReader($"SELECT emailId FROM ContactEmails_Backup WHERE contactsId = {contactId}");
             int phoneNumberId = (int)phoneNumberIdResults[0][0];
             int emailId = (int)emailIdResults[0][0];
             object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
             object[][] secondaryPhoneNumberResults = dbHelper.ExecuteReader($"SELECT SecondaryPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
             object[][] mainEmailResults = dbHelper.ExecuteReader($"SELECT MainEmail FROM Emails_Backup WHERE Id = {emailId} ");
             object[][] secondaryEmailResults = dbHelper.ExecuteReader($"SELECT SecondaryEmail FROM Emails_Backup WHERE Id = {emailId} ");
             int id = 0;
             int firstName = 1;
             int lastName = 2;
             int nickName = 3;
             int photo = 4;
             int company = 5;
             int street = 6;
             int city = 7;
             int state = 8;
             int zip = 9;
             int country = 10;
             int notes = 11;
             int favorite = 12;
             int active = 13;


            for (int index = 0; index < results.Length; index++)
            {
                txtEditFirstName.Text = ($"{results[index][firstName]}"); 
                txtEditLastName.Text = ($"{results[index][lastName]}");
                txtEditNickName.Text = ($"{results[index][nickName]}");
                txtEditPhoto.Text = ($"{results[index][photo]}");
                txtEditPhoneNumber.Text = ($"{mainPhoneNumberResults[index][index]}");
                txtEditSecondaryPhoneNumber.Text = ($"{secondaryPhoneNumberResults[index][index]}");
                txtEditEmail.Text = ($"{mainEmailResults[index][index]}");
                txtEditSecondaryEmail.Text = ($"{secondaryEmailResults[index][index]}");
                txtEditCompany.Text = ($"{results[index][company]}");
                txtEditStreet.Text = ($"{results[index][street]}");
                txtEditCity.Text = ($"{results[index][city]}");
                txtEditState.Text = ($"{results[index][state]}");
                txtEditZip.Text = ($"{results[index][zip]}");
                txtEditCountry.Text = ($"{results[index][country]}");
                txtEditNotes.Text = ($"{results[index][notes]}");
            }//end for
           
        }//end display edit fields

        private void ClearEditFields()
        {
            txtContactToEdit.Text = string.Empty;
            txtEditFirstName.Text = string.Empty;
            txtEditLastName.Text = string.Empty;
            txtEditNickName.Text = string.Empty;
            txtEditCompany.Text = string.Empty;
            txtEditStreet.Text = string.Empty;
            txtEditCity.Text = string.Empty;
            txtEditState.Text = string.Empty;
            txtEditZip.Text = string.Empty;
            txtEditCountry.Text = string.Empty;
            txtEditNotes.Text = string.Empty;
            txtEditPhoneNumber.Text = string.Empty;
            txtEditSecondaryEmail.Text = string.Empty;
            txtEditSecondaryPhoneNumber.Text = string.Empty;
            txtEditEmail.Text = string.Empty;
            txtEditPhoto.Text = string.Empty;
            chkEditFavorite.IsChecked = false;
            chkEditActive.IsChecked = false;

        }//end clear edit fields method

        private void ClearAddContactFields()
        {
            
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtNickName.Text = string.Empty;
            txtCompany.Text = string.Empty;
            txtStreet.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtNotes.Text = string.Empty;
            txtMainPhoneNumber.Text = string.Empty;
            txtSecondaryEmail.Text = string.Empty;
            txtSecondaryPhoneNumber.Text = string.Empty;
            txtMainEmail.Text = string.Empty;
            txtPhoto.Text = string.Empty;

        }//end clear edit fields method
        #endregion

        #region Database Functions

        public void AddContact()
        {           

            //STORE ALL FORM VALUES TO VARIABLES
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string nickName = txtNickName.Text;
            string mainPhoneNumber = txtMainPhoneNumber.Text;
            string secondaryPhoneNumber = txtSecondaryPhoneNumber.Text;
            string mainEmail = txtMainEmail.Text;
            string secondaryEmail = txtSecondaryEmail.Text;
            string photo = txtPhoto.Text;
            string city = txtCity.Text;
            string company = txtCompany.Text;
            string street = txtStreet.Text;
            string state = txtState.Text;
            string zip = txtZip.Text;
            string country = txtCountry.Text;
            string notes = txtNotes.Text;
            bool active = chkActive.IsChecked == true;
            bool favorite = chkFavorite.IsChecked == true;  

            Contacts newContact = new Contacts(firstName,lastName,nickName,photo,company,street,city,state,zip,country,notes);

            if(firstName != "")
            {

                //CREATE QUERY STRING TO ENTER DATA INTO MAIN CONTACTS TABLE
                string addContactquery = $"INSERT INTO Contacts_Backup (FirstName, LastName, NickName, Photo, Company, Street ,City, State, Zip, Country, Notes)" +
                               $"VALUES('{firstName}','{lastName}','{nickName}','{photo}','{company}','{street}','{city}','{state}','{zip}','{country}','{notes}')";


                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(addContactquery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(addContactquery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection

                //CREATE QUERY TO ENTER PHONE NUMBER INTO PHONE NUMBER TABLE
                string phoneNumberquery = $"INSERT INTO PhoneNumbers_Backup (MainPhoneNumber , SecondaryPhoneNumber)" +
                               $"VALUES('{mainPhoneNumber}' , '{secondaryPhoneNumber}')";
                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(phoneNumberquery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(phoneNumberquery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection

                //CREATE QUERY TO ENTER EMAIL INTO EMAIL TABLE
                string emailquery = $"INSERT INTO Emails_Backup (MainEmail , SecondaryEmail)" +
                               $"VALUES('{mainEmail}' , '{secondaryEmail}')";
                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(emailquery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(emailquery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection

           
           
                //Get contact Id that matches the first name entered
                object[][] contactIdResults = dbHelper.ExecuteReader($"SELECT ID FROM Contacts_Backup WHERE FirstName = '{firstName}'");
                int contactId = (int)contactIdResults[0][0];
                
                

           
                //Get Id number that matches the phone number entered
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT ID FROM PhoneNumbers_Backup WHERE MainPhoneNumber = '{mainPhoneNumber}'");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];        
                
              


                //Get Id number that matches the email entered
                object[][] emailIdResults = dbHelper.ExecuteReader($"SELECT ID FROM Emails_Backup WHERE MainEmail = '{mainEmail}'");
                int emailId = (int)emailIdResults[0][0];       
                

                //Populates ContactPhoneNumber table with ids
                string linkPhoneNumberQuery = $"INSERT INTO ContactPhoneNumbers_Backup (contactsId , phoneNumberId)" + $"VALUES ({contactId} , {phoneNumberId})";

                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(linkPhoneNumberQuery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(linkPhoneNumberQuery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection

                //Populates ContactEmails table with ids
                string linkEmailQuery = $"INSERT INTO ContactEmails_Backup (contactsId , emailId)" + $"VALUES ({contactId} , {emailId})";

                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(linkEmailQuery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(linkEmailQuery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection

                
                //CREATE QUERY STRING TO ENTER  FAVORITES INTO MAIN CONTACTS 
               
                    string addFavoritesquery = $"INSERT INTO Contacts_Backup (Favorite)" +
                               $"VALUES('{favorite}')";

                    using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(addFavoritesquery, thisConnection))
                        {
                            thisConnection.Open();
                            dbHelper.ExecuteNonQuery(addFavoritesquery);
                            thisConnection.Close();
                        }//end sql command
                    }//end sql connection
                

                //CREATE QUERY STRING TO ENTER  ACTIVES INTO MAIN CONTACTS 
                
                    string addActivequery = $"INSERT INTO Contacts_Backup (Active)" +
                               $"VALUES('{active}')";


                    using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(addActivequery, thisConnection))
                        {
                            thisConnection.Open();
                            dbHelper.ExecuteNonQuery(addActivequery);
                            thisConnection.Close();
                        }//end sql command
                    }//end sql connection
                

            }//end empty firstname if statement
            ClearAddContactFields();
        }//end add contact function

        private void ViewContactInfo()
        {  
            int contactId = int.Parse(txtContactToView.Text);          
            object[][] results = dbHelper.ExecuteReader($"SELECT * FROM Contacts_Backup WHERE Id = {contactId}");
            object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
            object[][] emailIdResults = dbHelper.ExecuteReader($"SELECT emailId FROM ContactEmails_Backup WHERE contactsId = {contactId}");
            int phoneNumberId = (int)phoneNumberIdResults[0][0];
            int emailId = (int)emailIdResults[0][0];
            object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
            object[][] secondaryPhoneNumberResults = dbHelper.ExecuteReader($"SELECT SecondaryPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
            object[][] mainEmailResults = dbHelper.ExecuteReader($"SELECT MainEmail FROM Emails_Backup WHERE Id = {emailId} ");
            object[][] secondaryEmailResults = dbHelper.ExecuteReader($"SELECT SecondaryEmail FROM Emails_Backup WHERE Id = {emailId} ");

            //Assigning Variable Names To Colums in The Table & The Number assigned is it's index in the Results Array
            int id = 0;
            int firstName = 1;
            int lastName = 2;
            int nickName = 3;
            int photo = 4;
            int company = 5;
            int street = 6;
            int city = 7;
            int state = 8;
            int zip = 9;
            int country = 10;
            int notes = 11;
            int favorite = 12;
            int active = 13;
            string photoString = (string)results[0][photo];
            
            if(photoString != "")
            {
                Uri photoUri = new Uri(photoString);
                imgPhoto.Source = new BitmapImage(photoUri);
                imgPhoto.Visibility = Visibility.Visible;
            }//end if 

            lstContactInfo.Items.Add($"{results[0][firstName]} {results[0][lastName]} : {results[0][nickName]}");
            lstContactInfo.Items.Add($"PhoneNumber: {mainPhoneNumberResults[0][0]}");
            lstContactInfo.Items.Add($"SecondaryPhone: {secondaryPhoneNumberResults[0][0]}");
            lstContactInfo.Items.Add($"Email: {mainEmailResults[0][0]}");
            lstContactInfo.Items.Add($"SecondaryEmail: {secondaryEmailResults[0][0]}");
            lstContactInfo.Items.Add($"Employer: {results[0][company]}");
            lstContactInfo.Items.Add($"Street: {results[0][street]}");
            lstContactInfo.Items.Add($"City: {results[0][city]}");
            lstContactInfo.Items.Add($"State: {results[0][state]}");
            lstContactInfo.Items.Add($"Zip: {results[0][zip]}");
            lstContactInfo.Items.Add($"Country: {results[0][country]}");
            lstContactInfo.Items.Add($"Notes: {results[0][notes]}");
            lstContactInfo.Items.Add($"Favorite: {results[0][favorite]}");
            lstContactInfo.Items.Add($"Active: {results[0][active]}");

        }//end view contact info function

        private void SearchContact()
        {
           // int contactId = int.Parse(txtContactToView.Text);
            string contactToSearch = txtSearchContacts.Text;                              
            object[][] results = dbHelper.ExecuteReader($"SELECT * FROM Contacts_Backup WHERE FirstName LIKE '{contactToSearch}%'");
            for (int index = 0; index < results.Length; index++)
            {
                int contactId = (int)results[index][0];
                object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
                object[][] emailIdResults = dbHelper.ExecuteReader($"SELECT emailId FROM ContactEmails_Backup WHERE contactsId = {contactId}");
                int phoneNumberId = (int)phoneNumberIdResults[0][0];
                int emailId = (int)emailIdResults[0][0];
                object[][] mainPhoneNumberResults = dbHelper.ExecuteReader($"SELECT MainPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
                object[][] secondaryPhoneNumberResults = dbHelper.ExecuteReader($"SELECT SecondaryPhoneNumber FROM PhoneNumbers_Backup WHERE Id = {phoneNumberId} ");
                object[][] mainEmailResults = dbHelper.ExecuteReader($"SELECT MainEmail FROM Emails_Backup WHERE Id = {emailId} ");
                object[][] secondaryEmailResults = dbHelper.ExecuteReader($"SELECT SecondaryEmail FROM Emails_Backup WHERE Id = {emailId} ");
           
            
                int id = 0;
                int firstName = 1;
                int lastName = 2;
                int nickName = 3;
                int photo = 4;
                int company = 5;
                int street = 6;
                int city = 7;
                int state = 8;
                int zip = 9;
                int country = 10;
                int notes = 11;
                string photoString = (string)results[0][photo];

                if (photoString != "")
                {
                    Uri photoUri = new Uri(photoString);
                    imgPhoto.Source = new BitmapImage(photoUri);
                    imgPhoto.Visibility = Visibility.Visible;

                }//end if 

                lstContactInfo.Items.Add($"{results[index][firstName]} {results[index][lastName]} : {results[index][nickName]}");
                lstContactInfo.Items.Add($"PhoneNumber: {mainPhoneNumberResults[0][0]}");
                lstContactInfo.Items.Add($"SecondaryPhone: {secondaryPhoneNumberResults[0][0]}");
                lstContactInfo.Items.Add($"Email: {mainEmailResults[0][0]}");
                lstContactInfo.Items.Add($"SecondaryEmail: {secondaryEmailResults[0][0]}");
                lstContactInfo.Items.Add($"Employer: {results[index][company]}");
                lstContactInfo.Items.Add($"Street: {results[index][street]}");
                lstContactInfo.Items.Add($"City: {results[index][city]}");
                lstContactInfo.Items.Add($"State: {results[index][state]}");
                lstContactInfo.Items.Add($"Zip: {results[index][zip]}");
                lstContactInfo.Items.Add($"Country: {results[index][country]}");
                lstContactInfo.Items.Add($"Notes: {results[index][notes]}");
               
            }//end for loop
        }//end search contact function
    
        private void EditContactInfo(int contactId)
        {
                   
            //STORE ALL FORM VALUES TO VARIABLES
            string firstName = txtEditFirstName.Text;
            string lastName = txtEditLastName.Text;
            string nickName = txtEditNickName.Text;
            string photo = txtEditPhoto.Text;
            string phoneNumber = txtEditPhoneNumber.Text;
            string secondaryPhoneNumber = txtEditSecondaryPhoneNumber.Text;
            string email = txtEditEmail.Text;
            string secondaryEmail = txtEditSecondaryEmail.Text;
            string city = txtEditCity.Text;
            string company = txtEditCompany.Text;
            string street = txtEditStreet.Text;
            string state = txtEditState.Text;
            string zip = txtEditZip.Text;
            string country = txtEditCountry.Text;
            string notes = txtEditNotes.Text;
            bool active = chkEditActive.IsChecked == true;
            bool favorite = chkEditFavorite.IsChecked == true;


            //CREATE QUERY STRING TO ENTER DATA INTO MAIN CONTACTS TABLE
                            
            string editContactquery = $"UPDATE Contacts_Backup  SET FirstName = '{firstName}', LastName = '{lastName}', NickName = '{nickName}', Photo = '{photo}', Company = '{company}', Street = '{street}', City = '{city}', State = '{state}', Zip = '{zip}', Country = '{country}', Notes = '{notes}' WHERE Id = {contactId}";



            using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(editContactquery, thisConnection))
                {
                    thisConnection.Open();
                    dbHelper.ExecuteNonQuery(editContactquery);
                    thisConnection.Close();
                }//end sql command
            }//end sql connection

            //Get PhoneNumber Id From ContactPhoneNumbers Table
            object[][] phoneNumberIdResults = dbHelper.ExecuteReader($"SELECT phoneNumberId FROM ContactPhoneNumbers_Backup WHERE contactsId = {contactId}");
            int phoneNumberId = (int)phoneNumberIdResults[0][0];
           
            //CREATE QUERY TO ENTER PHONE NUMBER INTO PHONE NUMBER TABLE
            string editPhoneNumberquery = $"UPDATE PhoneNumbers_Backup SET MainPhoneNumber = '{phoneNumber}' , SecondaryPhoneNumber = '{secondaryPhoneNumber}' WHERE Id = {phoneNumberId}";
            
            using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(editPhoneNumberquery, thisConnection))
                {
                    thisConnection.Open();
                    dbHelper.ExecuteNonQuery(editPhoneNumberquery);
                    thisConnection.Close();
                }//end sql command
            }//end sql connection

            //Get Email Id From ContactEmails Table
            object[][] emailIdResults = dbHelper.ExecuteReader($"SELECT emailId FROM ContactEmails_Backup WHERE contactsId = {contactId}");
            int emailId = (int)emailIdResults[0][0];

            //CREATE QUERY TO ENTER EMAIL INTO EMAIL TABLE
            string editEmailquery = $"UPDATE Emails_Backup SET MainEmail = '{email}' , SecondaryEmail = '{secondaryEmail}' WHERE Id = { emailId }"; 
           using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
           {
               using (SqlCommand cmd = new SqlCommand(editEmailquery, thisConnection))
               {
                   thisConnection.Open();
                    dbHelper.ExecuteNonQuery(editEmailquery);
                   thisConnection.Close();
               }//end sql command
           }//end sql connection

            //CREATE QUERY STRING TO UPDATE FAVORITES INTO MAIN CONTACTS 

                   
                string addFavoritesquery = $"UPDATE Contacts_Backup SET Favorite = '{favorite}' WHERE Id = {contactId}";
                          
                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(addFavoritesquery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(addFavoritesquery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection
           

            //CREATE QUERY STRING TO UPDATE ACTIVES INTO MAIN CONTACTS 
           
                string addActivequery = $"UPDATE Contacts_Backup SET Active = '{active}' WHERE Id = {contactId}";


                using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(addActivequery, thisConnection))
                    {
                        thisConnection.Open();
                        dbHelper.ExecuteNonQuery(addActivequery);
                        thisConnection.Close();
                    }//end sql command
                }//end sql connection
           

            ClearEditFields();           
        }//end edit contact method

        private void DeleteContact()
        {
           
             int contactToDelete = int.Parse(txtContactToDelete.Text);
                    
            string deletequery = $"DELETE FROM Contacts_Backup WHERE Id = {contactToDelete}";


            using (SqlConnection thisConnection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(deletequery, thisConnection))
                {
                    thisConnection.Open();
                    dbHelper.ExecuteNonQuery(deletequery);
                    thisConnection.Close();
                }//end sql command
            }//end sql connection

            lstDeleteContact.Items.Clear();
        }//end delete contact function
        #endregion

        #region Button Click Events
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            AddContact();
        }//end submit add contact click event

        private void btnEditContact_Click(object sender, RoutedEventArgs e)
        {   string contactID = (txtContactToEdit.Text);
            int contactId; 
            bool success  = int.TryParse(contactID, out contactId);  
            if (success)
            {
                 EditContactInfo(contactId);
            }
        }//end edit contact click event

        private void btnSeeAllContacts_Click(object sender, RoutedEventArgs e)
        {
            DisplayContacts();
        }//end see all contacts click

        private void btnListFavoriteContacts_Click(object sender, RoutedEventArgs e)
        {
            lstContacts.Items.Clear();
            DisplayFavoriteContacts();
        }//end list favorite click event

        private void btnSeeAllContactsToEdit_Click(object sender, RoutedEventArgs e)
        {
            DisplayContactsToEdit();

        }//end see all editable contacts click

        private void btnSeeAllContactsToDelete_Click(object sender, RoutedEventArgs e)
        {
            DisplayContactsToDelete();
        }//end deletebale contacts display

        private void btnDisplayContactToEditInfo_Click(object sender, RoutedEventArgs e)
        {
                      
            string contactID = (txtContactToEdit.Text);
            int contactId;
            bool success = int.TryParse(contactID, out contactId);
            if (success)
            {
                DisplayContactInfoToEdit(contactId);
            }

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //Clear list items 
            lstContacts.Items.Clear();  
            DisplayContacts();
        }//end contacts refresh event

        private void btnEditRefresh_Click(object sender, RoutedEventArgs e)
        {
            //Clear list items 
            lstEditContact.Items.Clear();         
        }// end refresh delete list event

        private void btnDeleteRefresh_Click(object sender, RoutedEventArgs e)
        {
            //Clear list items 
            lstDeleteContact.Items.Clear();         
        }// end refresh delete list event

        private void btnDeleteContact_Click(object sender, RoutedEventArgs e)
        {
            DeleteContact();
        }//end delete contact click

        private void btnViewContactInfo_Click(object sender, RoutedEventArgs e)
        {
            ViewContactInfo();
        }//end view contact click

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchContact();
        }//end search click

        private void btnClearList_Click(object sender, RoutedEventArgs e)
        {
            lstContactInfo.Items.Clear();
            imgPhoto.Visibility = Visibility.Collapsed;
        }//end clear search list click

        private void btnListOnlyActiveContacts_Click(object sender, RoutedEventArgs e)
        {
            lstContacts.Items.Clear();
            DisplayActiveContacts();
        }//end list active contacts click event

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
           // lstContacts.VerticalAlignment = VerticalAlignment.Top;
           // lstContacts.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            lstContacts.Height = 0;
            lstContacts.Width = 0;
        }//end minimize click

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {

           // lstContacts.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
           // VerticalAlignment = VerticalAlignment.Top;
            lstContacts.Height = 400;
            lstContacts.Width = 400;
        }//end maximize click

        #endregion

    }//end class
}//end namespace
