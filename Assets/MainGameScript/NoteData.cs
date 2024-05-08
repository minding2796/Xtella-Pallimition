using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DefaultNamespace
{
    public class NoteData
    {
        public static List<double> LoadRanks()
        {
            string strData = readFile("Results/results.r");
            if (strData.Equals("")) return new List<double>();
            string[] arr = strData.Replace(" ", "").Replace("\n", "").Split(",");
            return arr.Select(t => double.Parse(t)).ToList();
        }
        public static void SaveRanks(List<double> list)
        {
            string res = list.Aggregate("", (current, d) => current + (d + ","));
            writeFile("Results/results.r", res.Substring(0, res.Length-1));
        }
        public static Queue<Tuple<Tuple<float, float>, int>> LoadData(string title)
        { 
            Queue<Tuple<Tuple<float, float>, int>> list = new();
            
            string strData = readFile("Assets/NoteData/" + title + ".nd");
            string[] arr = strData.Replace(" ", "").Replace("\n", "").Split(",");
            for (int i = 0; i < arr.Length; i++)
            {
                string[] data = arr[i].Split("|");
                if (data.Length == 2) list.Enqueue(new Tuple<Tuple<float, float>, int>(new Tuple<float, float>(float.Parse(data[0]), float.Parse(data[0])), int.Parse(data[1])));
                else list.Enqueue(new Tuple<Tuple<float, float>, int>(new Tuple<float, float>(float.Parse(data[0]), float.Parse(data[2])), int.Parse(data[1])));
            }
            return list;
        }

        private static string readFile(string path)
        {
            String line, result = "";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(path);
                //Read the first line of text
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    result += line;
                    //Read the next line
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return result;
        }
        
        private static void writeFile(string path, string content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path);
                sw.Write(content);
                sw.Flush();
                sw.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}