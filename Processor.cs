using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testTaskAR
{
    internal interface ICheck
    {
        public bool Check(string str);
    }
    internal class Processor
    {
        public readonly Dictionary<string, int> Dictionary  = new Dictionary<string, int>();
        public readonly List<int> Fails = new List<int>();
        public void Run(ICheck check, string[] files)
        {
            try
            {

                foreach (var file in files)
                {
                    if (!File.Exists(file))
                        throw new FileNotFoundException(file);
                    var lines = File.ReadAllLines(file).ToList();
                    for (int i = 0; i < lines.Count;)
                    {
                        var line = lines[i];
                        SplitString(line, out int id, out string key, out int value);
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
                    File.WriteAllText(file, string.Join("\r\n",lines), Encoding.Default);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SplitString(string str, out int id, out string key, out int valye)
        {
            Regex re = new Regex(@"^(\d+)\s*([a-zA-Z]+)\s*(\-*\d+)$");
            Match match = re.Match(str);
            if (match.Captures.Count == 0)
                throw new Exception($"Строка имеет непарвильный вид:{str}");
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
