using System.IO;

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
                dataAnalyzer.PrintStats("temperature");
                dataAnalyzer.PrintStats("conductivity");
            }
        }
    }
}