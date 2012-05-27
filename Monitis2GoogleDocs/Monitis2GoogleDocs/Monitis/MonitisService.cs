using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace Monitis2GoogleDocs.Monitis
{
    class MonitisService
    {

        //You API key and Secret Key
        //private static String apiKey = "29O61R8FIEE4H4JDL2KRNOTAS";
        //private static String apiKey = Properties.Settings.Default.monitisApiKey;
        public static String apiKey { get; set; }
        private static String secretKey = "";
        //private static String authToken = requestAuthToken();


        private static String requestAuthToken()
        {
            apiKey = Properties.Settings.Default.monitisApiKey;
            String url = "http://www.monitis.com/api?action=authToken&apikey=" + apiKey + "&secretkey=" + secretKey;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            String authToken = DissectAuthString("authToken", responseFromServer);
            return authToken;
        }//end requestAuthToken


        private static String DissectAuthString(String keyString, String cString)
        {

            String dissected = "";

            cString = cString.Remove(0, keyString.Length + 5);
            int pos = cString.IndexOf("\"}");
            dissected = cString.Remove(pos, 2);

            return dissected;
        }


        //http://www.monitis.com/api?apikey=6ROJLHN9EI2UFT14AIGH4MUKQU&output=json&version=2&action=tests
        //http://www.monitis.com/api?apikey=6ROJLHN9EI2UFT14AIGH4MUKQU&output=xml&version=2&action=tests

        public static bool isApiKeyOK()
        {
            
            String url = "http://www.monitis.com/api?apikey=" + apiKey + "&output=json&version=2&action=tests";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";


            try
            {

                WebResponse response = request.GetResponse();

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Monitis API Key not verified");
                return false;
            }


            System.Windows.MessageBox.Show("Monitis API Key verified");
            return true;
        }        
        public static MonitisData.ExternalMonitors  getListOfExternalMonitors()
        {
            apiKey = Properties.Settings.Default.monitisApiKey;
            String url = "http://www.monitis.com/api?apikey="  + apiKey + "&output=json&version=2&action=tests";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            //StreamReader reader = new StreamReader(dataStream);
            //string responseFromServer = reader.ReadToEnd();
            //response.Close();

            //Console.WriteLine(responseFromServer);

            //Start deserialize ExternalMonitors
            MemoryStream memoryStreamExternalMonitors = new MemoryStream();
            DataContractJsonSerializer serializerExternalMonitors = new DataContractJsonSerializer(typeof(MonitisData.ExternalMonitors));
            memoryStreamExternalMonitors.Position = 0;
            MonitisData.ExternalMonitors externalMonitors = (MonitisData.ExternalMonitors)serializerExternalMonitors.ReadObject(dataStream);
            //End of deserialize ExternalMonitors

            externalMonitors.setITestList();

            return externalMonitors;
        } // getListOfExternalMonitors


        public static MonitisData.ExternalMonitorInfo getExternalMonitorInfo(int _externalMonitorId)
        {
            apiKey = Properties.Settings.Default.monitisApiKey;
            String url = "http://www.monitis.com/api?apikey=" + apiKey + "&output=json&version=2&action=testinfo&testId=" + _externalMonitorId;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            Console.WriteLine(responseFromServer);

            //replace the field name params with param_s as params is a keyword
            responseFromServer = responseFromServer.Replace("params", "param_s");

            //convert string to stream.
            byte[] byteArray = Encoding.ASCII.GetBytes(responseFromServer); 
            MemoryStream streamFromString = new MemoryStream(byteArray);

            //Start deserialize ExternalMonitors
            MemoryStream memoryStreamExternalMonitorInfo = streamFromString;
            DataContractJsonSerializer serializerExternalMonitorInfo = new DataContractJsonSerializer(typeof(MonitisData.ExternalMonitorInfo));
            memoryStreamExternalMonitorInfo.Position = 0;
            MonitisData.ExternalMonitorInfo externalMonitorInfo = (MonitisData.ExternalMonitorInfo)serializerExternalMonitorInfo.ReadObject(memoryStreamExternalMonitorInfo);
            //End of deserialize ExternalMonitors

            return externalMonitorInfo;

        } // getExternalMonitorInfo


        public static MonitisData.ExternalMonitorsInfo getExternalMonitorInfoList(MonitisData.ExternalMonitors _externalMonitors)
        {
            apiKey = Properties.Settings.Default.monitisApiKey;
            MonitisData.ExternalMonitorsInfo externalMonitorsInfo = new MonitisData.ExternalMonitorsInfo();
            foreach (MonitisData.ExternalMonitor ext in _externalMonitors.getItestList().Values)
            {
                externalMonitorsInfo.add(getExternalMonitorInfo(ext.id));
            }

            return externalMonitorsInfo;
        }



        public static MonitisData.ExternalSnapshot.ExternalSnapshots getExternalSnapshots(string usedLocationIds)
        {
            apiKey = Properties.Settings.Default.monitisApiKey;
            // http://www.monitis.com/api?apikey=6ROJLHN9EI2UFT14AIGH4MUKQU&output=json&version=2&action=testsLastValues&locationIds=1,2,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20

            String url = "http://www.monitis.com/api?apikey=" + apiKey + "&output=json&version=2&action=testsLastValues&locationIds=" + usedLocationIds;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            response.Close();

            responseFromServer = "{\"resultList\":" + responseFromServer;
            responseFromServer = responseFromServer + "}";
            //convert string to stream.
            byte[] byteArray = Encoding.ASCII.GetBytes(responseFromServer);
            MemoryStream streamFromString = new MemoryStream(byteArray);

            //Start deserialize ExternalMonitors
            //MemoryStream memoryStreamExternalSnapshots = new MemoryStream();
            MemoryStream memoryStreamExternalSnapshots = streamFromString;
            DataContractJsonSerializer serializerExternalSnapshots = new DataContractJsonSerializer(typeof(MonitisData.ExternalSnapshot.ExternalSnapshots));
            memoryStreamExternalSnapshots.Position = 0;
            MonitisData.ExternalSnapshot.ExternalSnapshots externalSnapshots = (MonitisData.ExternalSnapshot.ExternalSnapshots)serializerExternalSnapshots.ReadObject(memoryStreamExternalSnapshots);
            //End of deserialize ExternalMonitors

            return externalSnapshots;
        } // getExternalSnapshots

    
    
    }





}
