using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace JsonDataAnalysis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("../../data.json"))
            {
                string json = r.ReadToEnd();
                
                DataAnalyzer dataAnalyzer = new DataAnalyzer(json);

                Dictionary<string, Dictionary<string, object>> result =
                    new Dictionary<string, Dictionary<string, object>>
                    {
                        {"current_speed", dataAnalyzer.GetStats("current_speed")},
                        {"temperature", dataAnalyzer.GetStats("temperature")},
                        {"salinity", dataAnalyzer.GetStats("salinity")}
                    };

                string output = JsonConvert.SerializeObject(result, Formatting.Indented);
                Console.WriteLine(output);
            }
        }
    }
}