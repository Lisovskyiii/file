
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lab_11
{
    
    class Program
    {
        static void Main(string[] args) {
            
            
            try
            {
                List<string> list = new List<string>();
                ReadFile(list);
                Check(list);
                Solution(list, out ArrayList log, out ArrayList array);
                byte count = 0;
                string invalid = "";
                InputFile(log, array, list,count,invalid);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static List<string> ReadFile(List<string> list)
        {
            FileStream f = new FileStream("file.txt", FileMode.Open);
            StreamReader reader = new StreamReader(f, Encoding.Default);
            while (!reader.EndOfStream)
            {
                string stroka = reader.ReadLine();
                if (stroka != "")
                {
                    list.Add(stroka);
                }
            }
            list.Remove("");
            f.Close();
            return list;
        }
        public static void Check(List<string> list )
        {
            byte error = 0;
         if(list[0].Trim()!= "Plate Numbers")
            {
                error++;
                list.Clear();
                string invalid = "";
                Solution(list, out ArrayList log, out ArrayList array);
                InputFile(log, array, list, error, invalid);
                throw new Exception("File is incorect");
            }
         list.Remove(list[0]);
         Regex regex = new Regex(@"[A-Z]{3}\s{1,}[0-9]{4}\D{0,}");
         for (int i = 0; i < list.Count; i++)
            {
                if (!regex.IsMatch(list[i]))
                {
                    string invalid = list[i];
                    error++;
                    list.RemoveRange(i,list.Count-i);
                    Solution(list,out ArrayList log, out ArrayList array);
                    InputFile(log, array, list, error, invalid);
                }
            }
            Console.WriteLine("File is correct");
        }
        public static void Solution(List<string> list, out ArrayList log, out ArrayList array)
        {
            log = new ArrayList();
            array = new ArrayList();
            char[] separators = new char[] { ' ' };
            for (int k = 0; k < list.Count; k++)
            {
                string[] ss = list[k].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                char[] symbol = ss[0].ToCharArray();
                char[] digital = ss[1].ToCharArray();
                Mas_array(ref digital, array, out byte pos);
                Mas_digital(log, ref symbol, ref pos);
                string dig = digital[0].ToString() + digital[1].ToString() + digital[2].ToString() + digital[3].ToString();
                string sss = symbol[0].ToString() + symbol[1].ToString() + symbol[2].ToString();
                if (pos == 2)
                {
                    array.Remove("0000");
                    array.Add("");
                }
                if(pos<1)
                {
                    array.Add(dig);
                    
                }
                
                if (pos < 2)
                {
                    log.Add(sss);
                }
            }
        }
        public static void Mas_array(ref char [] digital, ArrayList array, out byte pos)
        {
            pos = 0;
            byte count = 0;
            for (int i = 0; i < digital.Length; i++)
            {
                if (digital[i] == '9')
                {
                    count++;
                    if (count == 4)
                    {
                        pos++;
                        array.Add("0000");
                        break;
                    }
                }
                else
                {
                    int a = Convert.ToInt32(digital[digital.Length-1]) + 1;
                    char s = Convert.ToChar(a);
                    digital[digital.Length - 1] = s;
                    break;
                }
            }
        }
        public static void Mas_digital(ArrayList log, ref char[] symbol, ref byte pos)
        {
            int count = 0;
            for (int i = 0; i < symbol.Length; i++)
            {
                if (symbol[i] == 'Z')
                {
                    count++;
                    if (count == 3 & pos > 0)
                    {
                        pos++;
                        log.Add("The last one");
                        break;
                    }
                }
                else if (symbol[symbol.Length - 1] == 'Z' & pos>0)
                {
                    symbol[symbol.Length-1] = 'A';
                    if (symbol[symbol.Length - 2] == 'Z')
                    {
                        symbol[symbol.Length - 2] = 'A';
                        int x = Convert.ToInt32(symbol[0]) + 1;
                        symbol[0] = Convert.ToChar(x);
                        break;
                    }
                    else
                    {
                        int x = Convert.ToInt32(symbol[symbol.Length - 2]) + 1;
                        symbol[symbol.Length - 2] = Convert.ToChar(x);
                        break;
                    }   
                }
                else if (pos>0)
                {
                    int x = Convert.ToInt32(symbol[symbol.Length-1])+1;
                    symbol[symbol.Length - 1] = Convert.ToChar(x);
                    break; 
                }
            }
        }
        public static void InputFile(ArrayList array, ArrayList log, List<string> list, byte error, string invalid )
        {
            File.Delete("file_out.txt");
            FileStream f = new FileStream("file_out.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(f, Encoding.Default);
            writer.WriteLine("Programmer: Lisovskyi Kirill\n");
            for (int i=0; i<list.Count; i++)
            {
                string[] mas = list[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                writer.WriteLine(mas[0]+" "+ mas[1]+ " ---> " + array[i] + " " + log[i]);
            }
            if (error == 1)
            {
                writer.WriteLine($"invalid => {invalid}");
                writer.WriteLine($"\nNumber of plates processed: {list.Count}");
            }
            else if (error == 0)
            {
                writer.WriteLine($"\nNumber of plates processed: {list.Count}");
            }
            writer.Flush();
            f.Close();
        }
    }
}
