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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Monitis2GoogleDocs.Monitis;
using Monitis2GoogleDocs.Monitis.MonitisData;

namespace Monitis2GoogleDocs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ExternalMonitors externalMonitorList;
        private ExternalMonitorsInfo externalMonitorInfoList;
        private ApplUtils.Locations locationsUsed;
        private Monitis.MonitisData.ExternalSnapshot.ExternalSnapshots externalSnapshots;


        public bool monitsKeyOK = false;
        public bool googleLogonOK = false;

        public MainWindow()
        {
            InitializeComponent();


            enableButtons(false);
            Properties.Settings.Default.IsLoggedOnToGoogle = false;
            Properties.Settings.Default.Save();
            
            if (!Properties.Settings.Default.googleDocSpreadSheetName.Equals("") )
            {
                spreadSheetName.Text = Properties.Settings.Default.googleDocSpreadSheetName.ToString();
            }
            else
            {
                spreadSheetName.Text = Properties.Settings.Default.googeSpreadsheetNameRequired;
                googleSpreadsheetButton.Visibility = System.Windows.Visibility.Hidden;
                Properties.Settings.Default.IsGoogleDocSpreadSheetCreated = false;
            }

        }

        private void googleSpreadsheetButtonButton_Click(object sender, RoutedEventArgs e)
        {


            if (GoogleDocs.GoogleSpreadSheetService.getSpreadSheet(this.spreadSheetName.Text))
            {

                Properties.Settings.Default.IsGoogleDocSpreadSheetCreated = true;
                Properties.Settings.Default.googleDocSpreadSheetName = this.spreadSheetName.Text;
                Properties.Settings.Default.Save();

                labelSpreadSheetExists.Content = "Spreadsheet OK";
                labelStatus.Content = "Press the Get Monitors button to retrieve the list of External Monitors from your Monitis account.";
                this.googleSpreadsheetButton.IsEnabled = false;
                this.buttonGetMonitors.IsEnabled = true;


            }


        }


        private void externalMonitors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //MessageBox.Show(externalMonitors.SelectedItem.ToString() + " " + externalMonitorList.getExternalMonitorId( externalMonitors.SelectedItem.ToString()));
            
        }

        private void spreadSheetName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkSpreadsheetName();
        }


        //Get list of external monitors from within your Monitis account
        //For each monitor returned create a worksheet within the spreadhseet
        private void buttonGetMonitors_Click(object sender, RoutedEventArgs e)
        {
            //get the list of external monitors from your Monitis account
            externalMonitorList = MonitisService.getListOfExternalMonitors();
            externalMonitorInfoList = MonitisService.getExternalMonitorInfoList(externalMonitorList);
            //get list of locations of all external monitors in the Monitis account
            locationsUsed = new ApplUtils.Locations(externalMonitorInfoList);
            
            
            Properties.Settings.Default.externalMonitors = new System.Collections.Specialized.StringCollection();
            //populate the ComboBox
            foreach (ExternalMonitor ext in externalMonitorList.testList)
            {
                Properties.Settings.Default.externalMonitors.Add(ext.name);
                Properties.Settings.Default.Save();
                GoogleDocs.GoogleSpreadSheetService.addWorkSheet(ext.name, 1, 7);
                dataGridExternalMonitors.Items.Add(new ApplUtils.ExternalMonitor(ext.name, true));
            }
            
            GoogleDocs.GoogleSpreadSheetService.deleteWorkSheet("Sheet 1");
            this.buttonGetMonitors.IsEnabled = false;
            this.buttonPostData.IsEnabled = true;
            
            
        }//buttonGetMonitors_Click

        private void buttonPostData_Click(object sender, RoutedEventArgs e)
        {
            //get data from Monitis account for all the locations from which your services are being monitored 
            externalSnapshots = MonitisService.getExternalSnapshots(locationsUsed.listOfUsedLocationIds);
            //set column headers
            GoogleDocs.GoogleSpreadSheetService.setWorkSheetHeadersByWorksheet();
            if (GoogleDocs.GoogleSpreadSheetService.postDataWorkSheetByMonitor(externalSnapshots))
            {
                labelStatus.Content = "Posting to Google Spreadsheet completed!";
            } else 
            {
                labelStatus.Content = "Error encountered whilst posting data to Google Spreadsheet!";
            }

            buttonPostData.IsEnabled = false;
        }//buttonPostData_Click

        private void buttonLogon_Click(object sender, RoutedEventArgs e)
        {
            Views.Windows.GoogleLogon googleLogonWindow = new Views.Windows.GoogleLogon(this);
            googleLogonWindow.parentWindow = this;
            googleLogonWindow.ShowDialog();
            

            //enableButtons(Properties.Settings.Default.IsLoggedOnToGoogle);
            if (monitsKeyOK && googleLogonOK)
            {
                this.googleSpreadsheetButton.IsEnabled = true;
                this.buttonLogon.IsEnabled = false;
                this.labelStatus.Content = "Press the Spreadsheet Exists button to verify that the spreadsheet exists.";
            }
            else
            {
                this.labelStatus.Content = "Login details to Monitis and Google must be verified. Press the Logon button.";
                this.googleSpreadsheetButton.IsEnabled = false;
            }

            checkSpreadsheetName();
        }//buttonLogon_Click



        private void enableButtons(bool _enable)
        {

            this.buttonGetMonitors.IsEnabled = _enable;
            this.googleSpreadsheetButton.IsEnabled = _enable;
            this.buttonPostData.IsEnabled = _enable;


        }

        private void dataGridExternalMonitors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void spreadSheetName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void checkSpreadsheetName()
        {
            if (spreadSheetName.Text.Equals(""))
            {
                spreadSheetName.Text = Properties.Settings.Default.googeSpreadsheetNameRequired;
                
                googleSpreadsheetButton.IsEnabled = false;
                labelStatus.Content = "Spreadsheet name is required!";

            }
            else if (spreadSheetName.Text.Equals(Properties.Settings.Default.googeSpreadsheetNameRequired))
            {
                googleSpreadsheetButton.IsEnabled = false;
            }


            if ((googleLogonOK && monitsKeyOK) && (!spreadSheetName.Text.Equals("")) && (!spreadSheetName.Text.Equals(Properties.Settings.Default.googeSpreadsheetNameRequired)))
            {
                labelStatus.Content = "Press the Spreadsheet Exists button to verify spreadsheet.";
                googleSpreadsheetButton.IsEnabled = true;
            }

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
