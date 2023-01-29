using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Lab4
{
    public class DirectMerge
    {
        private string FileInput { get; }
        private long _iterations, _segments;
        private readonly int _columnNumber;
        

        public DirectMerge(string input, int columnNumber)
        {
            FileInput = input;
            _columnNumber = columnNumber;
            _iterations = 1; 
        }

        public void Sort()
        {
            while (true)
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
            _segments = 1;
            Console.WriteLine("\n Разделение на два файла с интервалом в " + _iterations);
            using StreamReader streamReader = new StreamReader(File.OpenRead(FileInput));
            using StreamWriter writerA = new StreamWriter(File.Create("a.txt", 65536));
            using StreamWriter writerB = new StreamWriter(File.Create("b.txt", 65536));
            long counter = 0;
            bool flag = true; 
            while (!streamReader.EndOfStream)
            {
                if (counter == _iterations)
                {
                    flag = !flag;
                    counter = 0;
                    _segments++;
                }

                string element = streamReader.ReadLine();
                if (element is null) break;
                if (flag)
                {
                    writerA.WriteLine(element);
                }
                else
                {
                    writerB.WriteLine(element);
                }

                counter++;
            }
            streamReader.Close();
            writerA.Close();
            writerB.Close();
            ShowFiles();
        }
        public void ShowFiles()
        {
            using StreamReader AstreamReader = new StreamReader(File.OpenRead("a.txt"));
            int counter = 0;
            Console.Write("\nФайл А: ");
            while (!AstreamReader.EndOfStream)
            {
                if (counter == _iterations)
                {
                    Console.Write(" | ");
                    counter = 0;
                    _segments++;
                }

                Console.Write(AstreamReader.ReadLine().Split(" ")[_columnNumber] + " ");

                counter++;
            }
            AstreamReader.Close();
            Console.WriteLine();
            using StreamReader BstreamReader = new StreamReader(File.OpenRead("b.txt"));
            counter = 0;
            Console.Write("Файл B: ");
            while (!BstreamReader.EndOfStream)
            {
                if (counter == _iterations)
                {
                    Console.Write(" | ");
                    counter = 0;
                    _segments++;
                }

                Console.Write(BstreamReader.ReadLine().Split(" ")[_columnNumber] + " ");

                counter++;
            }
            Console.WriteLine();
        }
        private void JoinFiles() 
        {
            StringBuilder sb = new StringBuilder();
            using StreamReader readerA = new StreamReader(File.OpenRead("a.txt"));
            using StreamReader readerB = new StreamReader(File.OpenRead("b.txt"));
            using StreamWriter bw = new StreamWriter(File.Create(FileInput));
            
            long counterA = _iterations, counterB = _iterations;
            string elementA = null, elementB = null, strA = null, strB = null;
            bool pickedA = false, pickedB = false, endA = false, endB = false;
            while (!endA || !endB)
            {
                if (counterA == 0 && counterB == 0)
                {
                    counterA = _iterations;
                    counterB = _iterations;
                }
                
                if (!readerA.EndOfStream)
                {
                    if(counterA>0 && !pickedA)
                    {
                        strA = readerA.ReadLine();
                        elementA = strA?.Split(" ")[_columnNumber];
                        pickedA = true;
                    }
                }
                else
                {
                    endA = true;
                }

                if (!readerB.EndOfStream)
                {
                    if(counterB>0 && !pickedB)
                    {
                        strB = readerB.ReadLine();
                        elementB = strB?.Split(" ")[_columnNumber];
                        pickedB = true;
                    }
                }
                else
                {
                    endB = true;
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
                            counterA--;
                            pickedA = false;
                        }
                        else
                        {
                            bw.WriteLine(strB);
                            Console.WriteLine("Запись " + elementB);
                            sb.Append("\n" + strB);
                            counterB--;
                            pickedB = false;
                        }
                    }
                    else
                    {
                        bw.WriteLine(strA);
                        Console.WriteLine("Запись " + elementA);
                        sb.Append("\n" + strA);
                        counterA--;
                        pickedA = false;
                    }
                }
                else if(pickedB)
                {
                    bw.WriteLine(strB);
                    Console.WriteLine("Запись " + elementB);
                    sb.Append("\n" + strB);
                    counterB--;
                    pickedB = false;
                }
            }
            _iterations *= 2;

            Console.WriteLine("\nИзначальный файл: " + sb.ToString());           
            Console.WriteLine();

        }
    }
}