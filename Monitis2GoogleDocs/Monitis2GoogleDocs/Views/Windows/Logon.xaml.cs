using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Monitis2GoogleDocs.Monitis;


namespace Monitis2GoogleDocs.Views.Windows
{
    /// <summary>
    /// Interaction logic for GoogleLogon.xaml
    /// </summary>
    public partial class GoogleLogon : Window
    {
     
        public Monitis2GoogleDocs.MainWindow parentWindow;
        
        
        public GoogleLogon(Monitis2GoogleDocs.MainWindow _parentWindow)
        {

         
            InitializeComponent();
            parentWindow = _parentWindow;
            userName.Text = Properties.Settings.Default.googleUser;
            password.Password = Properties.Settings.Default.googlePassword;
            monitisApiKey.Text = Properties.Settings.Default.monitisApiKey;
            labelGoogleTitle.Content = "google - not logged on";
            labelMonitisTitle.Content = "monitis - key not verified";
        }

     
        
        private void monitsButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.monitisApiKey = monitisApiKey.Text;
            Properties.Settings.Default.Save();
            MonitisService.apiKey = monitisApiKey.Text;
            parentWindow.monitsKeyOK = MonitisService.isApiKeyOK();

            if (parentWindow.monitsKeyOK)
            {
                labelMonitisTitle.Content = "monitis - key verified";
            }            
            else
            {
                labelMonitisTitle.Content = "monitis - key not verified";

            }


        }

        private void googleButton_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text.Equals("") || password.Password.Equals(""))
            {
                MessageBox.Show("Google logon details required.");
                userName.Focus();
                return;

            }

            GoogleDocs.DocumentService.USERNAME = userName.Text;
            GoogleDocs.DocumentService.PASSWORD = this.password.Password;


            GoogleDocs.GoogleSpreadSheetService.USERNAME = userName.Text;
            GoogleDocs.GoogleSpreadSheetService.PASSWORD = this.password.Password;

            GoogleDocs.GoogleSpreadSheetService.setSpreadSheetService(true);

            if (!GoogleDocs.GoogleSpreadSheetService.testLogon())
            {
                this.userName.Focus();
                Properties.Settings.Default.IsLoggedOnToGoogle = false;
                Properties.Settings.Default.Save();
                System.Windows.MessageBox.Show("Correct Google credentials required");
                return;
            }

            Properties.Settings.Default.IsLoggedOnToGoogle = true;
            Properties.Settings.Default.Save();

            parentWindow.googleLogonOK = true;

            labelGoogleTitle.Content = "google - logged on";
            System.Windows.MessageBox.Show("Google credentials verified");
        }

        private void monitisApiKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.monitsKeyOK = false;
            labelMonitisTitle.Content = "monitis - key not verified";

        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.googleLogonOK = false;
            labelGoogleTitle.Content = "google - not logged on";
            
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            parentWindow.googleLogonOK = false;
            labelGoogleTitle.Content = "google - not logged on";

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
