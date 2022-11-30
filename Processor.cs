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
        public readonly Dictionary<string, int> Dictionary = new();
        public readonly List<int> Fails = new();
        public void Run(ICheck check, string[] files)
        {
            foreach (var file in files)
            {
                if (!File.Exists(file))
                    continue;
                var lines = File.ReadAllLines(file).ToList();
                for (int i = 0; i < lines.Count;)
                {
                    var line = lines[i];
                    try
                    {
                        SplitString(line, out int id, out string key, out int value);
                        if (check.Check(line))
                        {
                            AddToDictionary(value, key);
                            lines.Remove(line);
                        }
                        else
                        {
                            Fails.Add(id);
                            throw new Exception($"Невозможно добавить строку: {line}");
                        }
                    }
                    catch(Exception e) 
                    {
                        Console.WriteLine(e.Message);
                        i++;
                        continue;
                    }
                }
                File.WriteAllText(file, string.Join("\r\n", lines), Encoding.Default);
            }
        }

        private static void SplitString(string str, out int id, out string key, out int valye)
        {
            Regex re = new(@"^(\d+)\s*([a-zA-Zа-яА-Я]+)\s*(\-*\d+)$");
            Match match = re.Match(str.Trim());
            if (match.Captures.Count == 0)
                throw new Exception($"Строка имеет неправильный вид: {str}");
            id = int.Parse(match.Groups[1].Value);
            key = match.Groups[2].Value;
            valye = int.Parse(match.Groups[3].Value);
        }

        private void AddToDictionary(int value, string key)
        {
            if (Dictionary.ContainsKey(key))
                Dictionary[key] += value;
            else
                Dictionary[key] = value;
        }
    }
}
