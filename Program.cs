// See https://aka.ms/new-console-template for more information
using testTaskAR;

var proc = new Processor();
var check = new Check();
proc.Run(check, new string[] { "C:\\test\\1.txt", "C:\\test\\2.txt" });
foreach(var el in proc.Dictionary)
{
    Console.WriteLine(el);
}
Console.ReadLine();
public class Check : ICheck
{
    bool ICheck.Check(string str)
    {

        return str.Length >= 5;
    }
}


