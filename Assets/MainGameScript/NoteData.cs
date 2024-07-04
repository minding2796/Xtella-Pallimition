using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MainGameScript
{
    public static class NoteData
    {
        public static Tuple<float, float> LoadOffsets()
        {
            var strData = readFile("Results/offsets.r");
            return strData.Equals("") ? new Tuple<float, float>(0f, 0f) : new Tuple<float, float>(float.Parse(strData.Split(",")[0]), float.Parse(strData.Split(",")[1]));
        }
        public static void SaveOffsets(float nfd, float jd)
        {
            writeFile("Results/offsets.r", nfd + "," + jd);
        }
        public static List<double> LoadRanks()
        {
            var strData = readFile("Results/results.r");
            if (strData.Equals("")) return new List<double>();
            var arr = strData.Replace(" ", "").Replace("\n", "").Split(",");
            return arr.Select(double.Parse).ToList();
        }
        public static void SaveRanks(IEnumerable<double> list)
        {
            var res = list.Aggregate("", (current, d) => current + (d + ","));
            writeFile("Results/results.r", res[..^1]);
        }
        public static Queue<Tuple<Tuple<float, float>, int>> LoadData(string title)
        { 
            Queue<Tuple<Tuple<float, float>, int>> list = new();
            
            var strData = readFile("Assets/NoteData/" + title + ".nd");
            var arr = strData.Replace(" ", "").Replace("\n", "").Split(",");
            foreach (var s in arr)
            {
                string[] data = s.Split("|");
                list.Enqueue(data.Length == 2
                    ? new Tuple<Tuple<float, float>, int>(
                        new Tuple<float, float>(float.Parse(data[0]), float.Parse(data[0])), int.Parse(data[1]))
                    : new Tuple<Tuple<float, float>, int>(
                        new Tuple<float, float>(float.Parse(data[0]), float.Parse(data[2])), int.Parse(data[1])));
            }
            return list;
        }

        private static string readFile(string path)
        {
            var result = "";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                var sr = new StreamReader(path);
                //Read the first line of text
                var line = sr.ReadLine();
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
                var sw = new StreamWriter(path);
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