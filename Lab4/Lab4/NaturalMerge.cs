using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class NaturalMerge
    {
        private string FileInput { get; }
        private long _segments;
        private readonly int _columnNumber;
        private bool done = false;
        public NaturalMerge(string input, int columnNumber)
        {
            FileInput = input;
            _columnNumber = columnNumber;
        }

        public void Sort()
        {
            while (done == false)
            {
                DevideToFiles();
                if (_segments == 1)
                {
                    Console.WriteLine("\nСортировка окончена!");
                    break;
                }
                JoinFiles();
            }
        }

        private void DevideToFiles()
        {
            done = true;
            _segments = 1;
            Console.WriteLine("\n Разделение на два файла");
            using StreamReader streamReader = new StreamReader(File.OpenRead(FileInput));
            using StreamWriter writerA = new StreamWriter(File.Create("a.txt"));
            using StreamWriter writerB = new StreamWriter(File.Create("b.txt"));
            bool flag = true;
            string str1 = null, str2, element1 = null, element2;
            StringBuilder sbA = new StringBuilder();
            StringBuilder sbB = new StringBuilder();
            while (!streamReader.EndOfStream)
            {
                if (str1 is null)
                {
                    str1 = streamReader.ReadLine();
                    element1 = str1?.Split(" ")[_columnNumber];
                    sbA.Append(str1.Split(" ")[_columnNumber] + " ");
                    writerA.WriteLine(str1);
                }

                str2 = streamReader.ReadLine();
                if (str2 is null | String.Compare(str2, "", StringComparison.Ordinal) == 0) break;
                element2 = str2.Split(" ")[_columnNumber];

                if (String.CompareOrdinal(element1, element2) > 0)
                {
                    if (flag)
                    {
                        writerA.WriteLine("|");
                        sbA.Append(" | ");
                    }
                    else
                    {
                        writerB.WriteLine("|");
                        sbB.Append(" | ");
                    }
                    flag = !flag;
                    _segments++;
                    done = false;
                }

                if (flag)
                {
                    writerA.WriteLine(str2);
                    sbA.Append(str2.Split(" ")[_columnNumber] + " ");
                }
                else
                {
                    writerB.WriteLine(str2);
                    sbB.Append(str2.Split(" ")[_columnNumber] + " ");
                }

                str1 = str2;
                element1 = element2;
            }
            streamReader.Close();
            writerA.Close();
            writerB.Close();
            if (sbA[1].ToString() == "|")
            {
                sbA.Remove(0, 3);
            }
            sbA.Remove(sbA.Length - 4, 3);

            Console.WriteLine("\nФайл А: " + sbA.ToString());
            Console.WriteLine("Файл B: " + sbB.ToString());
        }
        private void JoinFiles()
        {
            StringBuilder sb = new StringBuilder();    
            Console.WriteLine("\n Запись в исходный файл");
            using StreamReader readerA = new StreamReader(File.OpenRead("a.txt"));
            using StreamReader readerB = new StreamReader(File.OpenRead("b.txt"));
            using StreamWriter bw = new StreamWriter(File.Create(FileInput));
            string elementA = null, elementB = null, strA = null, strB = null;
            bool pickedA = false, pickedB = false, endA = false, endB = false;
            while (!endA || !endB)
            {
                if (!endA & !pickedA)
                {
                    strA = readerA.ReadLine();
                    if (strA == "|")
                    {
                        strA = readerA.ReadLine();
                        if(strA != null)
                        {
                            elementA = strA.Split(" ")[_columnNumber];
                        }

                        while (strB != "|" && !readerB.EndOfStream)
                        {
                            Console.WriteLine("Запись " + strB.Split(" ")[_columnNumber]);
                            bw.WriteLine(strB);
                            sb.Append("\n" + strB);
                            strB = readerB.ReadLine();
                        }
                        strB = readerB.ReadLine();
                        if (strB != null)
                        {
                            elementB = strB.Split(" ")[_columnNumber];
                        }
                    }
                    if (strA is null | String.Compare(strA, "", StringComparison.Ordinal) == 0) endA = true;
                    else
                    {
                        
                        elementA = strA.Split(" ")[_columnNumber];
                        pickedA = true;
                    }


                }

                if (!endB & !pickedB)
                {
                    strB = readerB.ReadLine();
                    
                    if (strB == "|")
                    {
                        strB = readerB.ReadLine();
                        if (strB != null)
                        {
                            elementB = strB.Split(" ")[_columnNumber];
                        }
                        while (strA != "|" && !readerA.EndOfStream)
                        {
                            Console.WriteLine("Запись " + strA.Split(" ")[_columnNumber]);
                            bw.WriteLine(strA);
                            sb.Append("\n" + strA);
                            strA = readerA.ReadLine();
                        }
                        strA = readerA.ReadLine();
                        if (strA != null)
                        {
                            elementA = strA.Split(" ")[_columnNumber];
                        }
                        //  Console.WriteLine("SDFGHJ Запись " + strA.Split(" ")[_columnNumber]);
                    }
                    if (strB is null | String.Compare(strB, "", StringComparison.Ordinal) == 0) endB = true;
                    else
                    {
                        elementB = strB.Split(" ")[_columnNumber];
                        pickedB = true;
                    }
                }

                if (pickedA)
                {
                    if (pickedB)
                    {
                        Console.WriteLine("Сравнение " + elementA + " и " + elementB);
                        if (String.CompareOrdinal(elementA, elementB) < 0)
                        {
                            bw.WriteLine(strA);
                            Console.WriteLine("Запись " + elementA);
                            sb.Append("\n" + strA);
                            pickedA = false;
                        }
                        else
                        {
                            bw.WriteLine(strB);
                            Console.WriteLine("Запись " + elementB);
                            sb.Append("\n" + strB);
                            pickedB = false;
                        }
                    }
                    else
                    {
                        bw.WriteLine(strA);
                        Console.WriteLine("Запись " + elementA);
                        sb.Append("\n" + strA);
                        pickedA = false;
                    }
                }
                else
                {
                    bw.WriteLine(strB);
                   // Console.WriteLine("Запись " + elementB);
                    sb.Append("\n" + strB);
                    pickedB = false;
                }
            }

            Console.WriteLine("\nИзначальный файл: " + sb.ToString());
            
            Console.WriteLine();

        }
    }
}
