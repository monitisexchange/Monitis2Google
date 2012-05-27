using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.GData.Client;
using Google.GData.Documents;


namespace Monitis2GoogleDocs.GoogleDocs
{
    class DocumentService
    {

        public static string USERNAME;
        public static string PASSWORD;
        private static DocumentEntry documentEntryHandle = null;


        private static DocumentsService getDocumentsService()
        {
            DocumentsService documentsService = new DocumentsService("Monitis2GoogleDocumentrService");
            documentsService.setUserCredentials(USERNAME, PASSWORD);

            return documentsService;
        }


        public static DocumentEntry createSpreadSheet(string _spreadSheetName)
        {

            DocumentsService documentsService = getDocumentsService();

            DocumentEntry documentEntryDefinition = new DocumentEntry();
            documentEntryDefinition.Title.Text = _spreadSheetName;

            documentEntryDefinition.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

            documentEntryHandle = documentsService.Insert(DocumentsListQuery.documentsBaseUri, documentEntryDefinition);

         
  
            
            return documentEntryHandle;
            
        }


        public static void getSpreadSheet(string _spreadSheetName)
        {

            DocumentsService documentsService = getDocumentsService();

            // Instantiate a DocumentsListQuery object to retrieve documents.      
            DocumentsListQuery query = new DocumentsListQuery();

            // Make a request to the API and get all documents.      

            DocumentsFeed feed = documentsService.Query(query);
            DocumentEntry documentEntryDefinition = new DocumentEntry();


            // Iterate through all of the documents returned      
            foreach (DocumentEntry entry in feed.Entries)      
            {        
                // Print the title of this document to the screen        
                Console.WriteLine(" ---- Found - " + entry.Title.Text + "" + entry.IsSpreadsheet);
                
            }

             
//            documentEntryDefinition.Title.Text = "MyMonitisTestSpreadsheet";

//            documentEntryDefinition.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

//            documentEntryHandle = documentsService.Insert(DocumentsListQuery.documentsBaseUri, documentEntryDefinition);

        }



    }
}
