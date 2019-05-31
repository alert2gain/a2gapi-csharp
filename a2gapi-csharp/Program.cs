using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace a2gapi_csharp
{
    /* Here we define the class Sensor and InternalData. */
    class Sensor
    {
        public string Station { get; set; }
        public InternalData Data { get; set; }
    }

    class InternalData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime LastRead { get; set; }
    }
        
    class Program
    {
        /* Here we create a method to create the sensor. */
        static void CreateSensor()
        {
            /* Here we initialize the sensor, use your own logic to do this. */
            Sensor sensor = new Sensor()
            {
                Station = "Station 1",
                Data = new InternalData()
                {
                    Temperature = 15.8,
                    Humidity = 7.8,
                    LastRead = DateTime.UtcNow
                }
            };

            /* This boolean is to confirm if the data was sent to the API. */
            bool successful = false;

            while (successful == false)
            {
                successful = SendData(sensor);
            }
        }

        /* This is the metodh to sent data to A2G InputStream API. */
        static bool SendData(Sensor sensor)
        {
            /* We serialize the sensor */
            string sensorData = JsonConvert.SerializeObject(sensor);

            /* And we put it in the dictionary "AllData", also we put the InputStream Key. */
            Dictionary<string, string> allData = new Dictionary<string, string>();
            allData.Add("IKEY", "[YOUR_INPUTSTREAM_KEY]");
            allData.Add("Data", sensorData);

            /* Here we initialize the RestClient. Remember to replace your API Key. */
            RestClient restClient = new RestClient("https://listen.a2g.io/v1/testing/inputstream");
            RestRequest restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("x-api-key", "[YOUR_API_KEY]");
            restRequest.AddJsonBody(allData);

            IRestResponse response = restClient.Execute(restRequest);

            /* We confirm if the data was sent */
            if (response.IsSuccessful == true)
            {
                Console.WriteLine($"Status: {response.Content}");
                return true;
            }
            
            Console.WriteLine($"Status: {response.Content}");
            return false;
        }

        /* This is the main method to initialize the program */
        static void Main(string[] args)
        {
            CreateSensor();
            Console.ReadLine();
        }
    }
}