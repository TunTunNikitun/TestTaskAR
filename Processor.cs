using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testTaskAR
{
    internal class Processor
    {
        public static Dictionary<string, int> Dictionary { get; } = new Dictionary<string, int>();
        public static List<int> Fails { get; } = new List<int>();
        public void Run(ICheck check, List<string> files)
        {
            var data = new List<string>();
            foreach (var file in files)
            {
                if(!File.Exists(file))
                    throw new FileNotFoundException(file);
                var lines = File.ReadAllLines(file).ToList();
                for(int i =0; i < lines.Count; )
                {
                    var line = lines[i];
                    int id =0;
                    string key = " ";
                    int value = 0;
                    SplitString(line,ref id, ref key,ref value);
                    if (check.Check(line))
                    {
                        AddToDictionary(value, key);
                        lines.Remove(line);
                    }
                    else
                    {
                        Fails.Add(id);
                        i++;
                    }
                }
                File.WriteAllLines(file, lines);
            }
        }

        private void SplitString(string str,ref int id, ref string key, ref int valye)
        {
            Regex re = new Regex(@"(\d+)\s*([a-zA-Z]+)\s*(\-*\d+)");
            Match match = re.Match(str);           
            id = int.Parse(match.Groups[1].Value);
            key = match.Groups[2].Value;
            valye = int.Parse(match.Groups[3].Value);
        }

        private void AddToDictionary( int value, string key)
        {
            if (Dictionary.Keys.Contains(key))
                Dictionary[key] += value;
            else
                Dictionary[key] = value;
        }
    }
}
