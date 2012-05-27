using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.GData.Client;
using Google.GData.Spreadsheets;
using Google.GData.Extensions;
using Monitis2GoogleDocs.Monitis.MonitisData.ExternalSnapshot;

using System.Windows;


namespace Monitis2GoogleDocs.GoogleDocs
{
    class GoogleSpreadSheetService
    {
        public static string USERNAME;
        public static string PASSWORD;

        public static SpreadsheetEntry spreadSheet = null;
       

        private static SpreadsheetsService spreadSheetsService = null;



        public static void setSpreadSheetService(bool reset)
        {
            if ((spreadSheetsService == null) || reset)
            {
                spreadSheetsService = new SpreadsheetsService("Monitis2GoogleSpreadSheetService");
                spreadSheetsService.setUserCredentials(USERNAME, PASSWORD);
            }
        }

        public static Boolean testLogon()
        {
            

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.      
            SpreadsheetQuery query = new SpreadsheetQuery();

            // Make a request to the API and get all spreadsheets.      
            try
            {
                
                SpreadsheetFeed feed = spreadSheetsService.Query(query);

            }
            catch (Google.GData.Client.InvalidCredentialsException)
            {
                MessageBox.Show("Invalid email/password");
                return false;
            }

            return true;
        }



        public static bool getSpreadSheet(string _spreadSheetName)
        {

            bool spreadSheetFound = false;

            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.      
            SpreadsheetQuery query = new SpreadsheetQuery();      
            
            // Make a request to the API and get all spreadsheets.      
            SpreadsheetFeed feed = spreadSheetsService.Query(query);      
            
            // Iterate through all of the spreadsheets returned          
            foreach (SpreadsheetEntry entry in feed.Entries)      
            {        
                // Print the title of this spreadsheet to the screen        
                Console.WriteLine(entry.Title.Text);

                if (_spreadSheetName.Equals(entry.Title.Text))
                {
                    spreadSheet = entry;
                    spreadSheetFound = true;
                    break;
                }
               
            }

            if (!spreadSheetFound)
            {
                string message = "Spreadsheet - " + _spreadSheetName +" - does not exist. Create spreadhseet?";
                string title = "Google Docs - Spreadsheet Not Found";

                if (MessageBox.Show(message,  title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    DocumentService.createSpreadSheet(_spreadSheetName);
                    //recursive call. This serves to set spreadSheet as a SpreadsheetEntry
                    //have to check whteher Documententry created in DocumentService.createSpreadSheet
                    //can be converted into a SpreadsheetEntry as this would avoid recursive call.
                    spreadSheetFound = getSpreadSheet(_spreadSheetName);

                }

                
            }

            return spreadSheetFound;

        } //getSpreadSheet


        public static void deleteWorkSheet(string _workSheetTitle)
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets;

            // loop on worksheets
            // compare _workSheetName to the Title of the worksheet
            foreach (WorksheetEntry ws in wsFeed.Entries)
            {

                if (ws.Title.Text.Equals(_workSheetTitle))
                {
                    ws.Delete();
                }
            }
            

        }

        public static void addWorkSheet(string _workSheetTitle, int _rows, int _cols)
        {

            if (workSheetExists(_workSheetTitle))
            {
                return;
            }


            // Create a local representation of the new worksheet.      
            WorksheetEntry worksheet = new WorksheetEntry();      
            worksheet.Title.Text = _workSheetTitle;
            worksheet.ColCount = new ColCountElement((uint)_cols);
            worksheet.RowCount = new RowCountElement((uint)_rows);      
            
            // Send the local representation of the worksheet to the API for      
            // creation.  The URL to use here is the worksheet feed URL of our      
            // spreadsheet.      
            WorksheetFeed wsFeed = spreadSheet.Worksheets;
            spreadSheetsService.Insert(wsFeed, worksheet);

        }

        public static void getWorkSheetName()
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets;

            // Iterate through each worksheet in the spreadsheet.      
            foreach (WorksheetEntry entry in wsFeed.Entries)      
            {        
                // Get the worksheet's title, row count, and column count.        
                string title = entry.Title.Text;        
                int rowCount = (int) entry.Rows;        
                int colCount = (int) entry.Cols;        
                // Print the fetched information to the screen for this worksheet.        
                Console.WriteLine(title + "- rows:" + rowCount + " cols: " + colCount);

                // Update the local representation of the worksheet.      
                //entry.Title.Text = "HoHo";      
                entry.Cols = 20;      
                entry.Rows = 20;
                entry.Update();
            }
        }//getWorkSheetName

        public static void addWorkSheet2(string _workSheetTitle, int _rows, int _cols)
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets;

            WorksheetEntry worksheet = new WorksheetEntry();
            worksheet = ((WorksheetEntry)wsFeed.Entries[0]);
            worksheet.Cols = (uint) _cols;
            worksheet.Rows = (uint) _rows;
            worksheet.Title.Text = _workSheetTitle;
            spreadSheetsService.Insert(wsFeed, worksheet);

        }//addWorkSheet2

        public static void getDataCellFeed()
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets;
            WorksheetEntry worksheet = ((WorksheetEntry)wsFeed.Entries[0]);

            // Fetch the cell feed of the worksheet.      
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            CellFeed cellFeed = spreadSheetsService.Query(cellQuery);      
            
            // Iterate through each cell, updating its value if necessary.      
            foreach (CellEntry cell in cellFeed.Entries)      
            {        
                // Print the cell's address in A1 notation        
                Console.WriteLine(cell.Title.Text);        
                // Print the cell's address in R1C1 notation        
                Console.WriteLine(cell.Id.Uri.Content.Substring(cell.Id.Uri.Content.LastIndexOf("/") + 1));        
                // Print the cell's formula or text value        
                Console.WriteLine(cell.InputValue);        
                
                // Print the cell's calculated value if the cell's value is numeric        
                // Prints empty string if cell's value is not numeric        
                Console.WriteLine(cell.NumericValue);        
                
                // Print the cell's displayed value (useful if the cell has a formula)        
                Console.WriteLine(cell.Value);      
            }

            /////////////////////////////////////////////////////////////////////////////////////

            // Fetch the cell feed of the worksheet.      
            CellQuery cellQuery2 = new CellQuery(worksheet.CellFeedLink);      
            cellQuery2.MinimumRow = 2;      
            cellQuery2.MinimumColumn = 4;      
            cellQuery2.MaximumColumn = 4;
            CellFeed cellFeed2 = spreadSheetsService.Query(cellQuery2);

            Console.WriteLine("/////////////////////////////////////////////////////");

            // Iterate through each cell, printing its value.      
            foreach (CellEntry cell in cellFeed2.Entries)      
            {        
                // Print the cell's address in A1 notation        
                Console.WriteLine(cell.Title.Text);        
                
                // Print the cell's address in R1C1 notation        
                Console.WriteLine(cell.Id.Uri.Content.Substring(cell.Id.Uri.Content.LastIndexOf("/") + 1));        
                
                // Print the cell's formula or text value        
                Console.WriteLine(cell.InputValue);        
                
            }

            Console.WriteLine("/////////////////////////////////////////////////////");
        }//getDataCellFeed



        public static void setWorkSheetHeaders()
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets;
            WorksheetEntry worksheet = ((WorksheetEntry)wsFeed.Entries[0]);

            // Fetch the cell feed of the worksheet.      
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            CellFeed cellFeed = spreadSheetsService.Query(cellQuery);

            CellEntry cellEntry1 = new CellEntry(1, 1, "id");
            CellEntry cellEntry2 = new CellEntry(1, 2, "name");
            CellEntry cellEntry3 = new CellEntry(1, 3, "type");
            CellEntry cellEntry4 = new CellEntry(1, 4, "time");
            CellEntry cellEntry5 = new CellEntry(1, 5, "perf");
            CellEntry cellEntry6 = new CellEntry(1, 6, "status");
            CellEntry cellEntry7 = new CellEntry(1, 6, "location");


            cellFeed.Insert(cellEntry1);
            cellFeed.Insert(cellEntry2);
            cellFeed.Insert(cellEntry3);
            cellFeed.Insert(cellEntry4);
            cellFeed.Insert(cellEntry5);
            cellFeed.Insert(cellEntry6);
            cellFeed.Insert(cellEntry7);
        
        }//setWorkSheetHeaders


        public static void setWorkSheetHeadersByWorksheet()
        {

            IDictionary<string, WorksheetEntry> workSheetList = new Dictionary<string, WorksheetEntry>();
            WorksheetFeed wsFeed = spreadSheet.Worksheets;
            //WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];


            CellQuery cellQuery ;
            CellFeed cellFeed ;

            CellEntry cellEntry1 ;
            CellEntry cellEntry2 ;
            CellEntry cellEntry3 ;
            CellEntry cellEntry4 ;
            CellEntry cellEntry5 ;
            CellEntry cellEntry6 ;
            CellEntry cellEntry7 ;

            
            foreach (WorksheetEntry ws in wsFeed.Entries)
            {
                //workSheetList.Add(ws.Title.Text, ws);

                // Fetch the cell feed of the worksheet.      
                cellQuery = new CellQuery(ws.CellFeedLink);
                cellFeed = spreadSheetsService.Query(cellQuery);

                cellEntry1 = new CellEntry(1, 1, "id");
                cellEntry2 = new CellEntry(1, 2, "name");
                cellEntry3 = new CellEntry(1, 3, "type");
                cellEntry4 = new CellEntry(1, 4, "time");
                cellEntry5 = new CellEntry(1, 5, "perf");
                cellEntry6 = new CellEntry(1, 6, "status");
                cellEntry7 = new CellEntry(1, 7, "location");


                cellFeed.Insert(cellEntry1);
                cellFeed.Insert(cellEntry2);
                cellFeed.Insert(cellEntry3);
                cellFeed.Insert(cellEntry4);
                cellFeed.Insert(cellEntry5);
                cellFeed.Insert(cellEntry6);
                cellFeed.Insert(cellEntry7);

            }


        }//setWorkSheetHeadersBy

    
        public static bool workSheetExists(string _workSheetName)
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets;

            // loop on worksheets
            // compare _workSheetName to the Title of the worksheet
            foreach (WorksheetEntry ws in wsFeed.Entries)
            {

                if (ws.Title.Text.Equals(_workSheetName))
                {
                    return true;
                }
            }
            
            return false;
        }

        //Post data to Google Docs
        //Worksheets names within speadsheet have been set to the monitor names
        //Monitor name will be used to identify worksheet within which to post data.
        public static bool postDataWorkSheetByMonitor(ExternalSnapshots _externalSnapshots)
        {
            IDictionary<string, ListFeed> workSheetList = new Dictionary<string, ListFeed>();
            WorksheetFeed wsFeed = spreadSheet.Worksheets;
            AtomLink listFeedLink1;
            ListQuery listQuery1;
            ListFeed listFeed1;


            try
            {

                // loop on worksheets
                // Fetch the list feed of the worksheet.
                // The listfeed is required to post data to the worksheet
                // create collection of key/value pairs where the key is the worksheet name and the value is the listfeed for the worksheet
                foreach (WorksheetEntry ws in wsFeed.Entries)
                {

                    listFeedLink1 = ws.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
                    listQuery1 = new ListQuery(listFeedLink1.HRef.ToString());
                    listFeed1 = spreadSheetsService.Query(listQuery1);

                    workSheetList.Add(ws.Title.Text, listFeed1);
                }


                //row to post
                ListEntry row;

                //loop on the externalSnapshots.
                //each entry within the reultList will be used to compose a row for posting to the Goolge spreadsheet
                foreach (ExternalSnapshot externalSnapshot in _externalSnapshots.resultList)
                {
                    foreach (Data dataToPost in externalSnapshot.data)
                    {
                        row = new ListEntry();
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "id", Value = dataToPost.id.ToString() });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "type", Value = dataToPost.testType });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "name", Value = dataToPost.name });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "perf", Value = dataToPost.perf.ToString() });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "time", Value = dataToPost.time });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "status", Value = dataToPost.time });
                        row.Elements.Add(new ListEntry.Custom() { LocalName = "location", Value = externalSnapshot.locationShortName });


                        // Send the new row to the Google Spreadsheets API for insertion.    
                        row = spreadSheetsService.Insert(workSheetList[dataToPost.name], row); ;

                    }
                }

            }
            catch (Exception _exception)
            {
                MessageBox.Show(_exception.Message);
                return false;
            }

            return true;
        } // postDataWorkSheetByMonitor


        //this methos posts the data to the first workSheet withiin the spreadSheet
        public static void postDataToSameWorkSheet(ExternalSnapshots _externalSnapshots)
        {
            WorksheetFeed wsFeed = spreadSheet.Worksheets; 
            WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];

            // Define the URL to request the list feed of the worksheet.      
            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.      
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = spreadSheetsService.Query(listQuery);

            ListEntry row;

            foreach (ExternalSnapshot externalSnapshot in _externalSnapshots.resultList)
            {
                foreach (Data dataToPost in externalSnapshot.data)
                {
                    row = new ListEntry();
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "id", Value = dataToPost.id.ToString() });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "type", Value = dataToPost.testType });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "name", Value = dataToPost.name });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "perf", Value = dataToPost.perf.ToString() });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "time", Value = dataToPost.time });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "status", Value = dataToPost.time });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "location", Value = externalSnapshot.locationShortName });

                    // Send the new row to the API for insertion.    
                    row = spreadSheetsService.Insert(listFeed, row); ;
                
                }
            }
        } // postData
    
    }
}
