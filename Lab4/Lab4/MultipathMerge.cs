using System;
using System.IO;
using System.Text;

namespace Lab4
{
    public enum StreamNumber
    {
        StreamA,
        StreamB,
        StreamC

    }
    public class MultipathMerge
    {
        private string FileInput { get; }
        private long _iterations, _segments;
        private readonly int _columnNumber;


        public MultipathMerge(string input, int columnNumber)
        {
            FileInput = input;
            _columnNumber = columnNumber;
            _iterations = 1;
        }

        public void Sort()
        {
            while (true)
            {
                SplitToFiles();
                if (_segments == 1) break;
                MergePairs();
            }
        }

        private void SplitToFiles()
        {
            _segments = 1;
            Console.WriteLine("\n Разделение на два файла с интервалом в " + _iterations);
            using StreamReader streamReader = new StreamReader(File.OpenRead(FileInput));
            using StreamWriter writerA = new StreamWriter(File.Create("a.txt", 65536));
            using StreamWriter writerB = new StreamWriter(File.Create("b.txt", 65536));
            using StreamWriter writerC = new StreamWriter(File.Create("c.txt", 65536));
            long counter = 0;
            StreamNumber streamNumber = StreamNumber.StreamA;
            while (!streamReader.EndOfStream)
            {
                if (counter == _iterations)
                {
                    if (streamNumber == StreamNumber.StreamA) streamNumber = StreamNumber.StreamB;
                    else if (streamNumber == StreamNumber.StreamB) streamNumber = StreamNumber.StreamC;
                    else if (streamNumber == StreamNumber.StreamC) streamNumber = StreamNumber.StreamA;
                    _segments++;
                    counter = 0;
                }

                string element = streamReader.ReadLine();
                if (element is null) break;
                if (streamNumber == StreamNumber.StreamA)
                {
                    writerA.WriteLine(element);
                }
                else if (streamNumber == StreamNumber.StreamB)
                {
                    writerB.WriteLine(element);
                }
                else if (streamNumber == StreamNumber.StreamC)
                {
                    writerC.WriteLine(element);
                }

                counter++;
            }
            streamReader.Close();
            writerA.Close();
            writerB.Close();
            writerC.Close();
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
            using StreamReader CstreamReader = new StreamReader(File.OpenRead("c.txt"));
            counter = 0;
            Console.Write("Файл C: ");
            while (!CstreamReader.EndOfStream)
            {
                if (counter == _iterations)
                {
                    Console.Write(" | ");
                    counter = 0;
                    _segments++;
                }

                Console.Write(CstreamReader.ReadLine().Split(" ")[_columnNumber] + " ");

                counter++;
            }
            Console.WriteLine();
        }

        private void MergePairs()
        {
            StringBuilder sb = new StringBuilder();
            using StreamReader readerA = new StreamReader(File.OpenRead("a.txt"));
            using StreamReader readerB = new StreamReader(File.OpenRead("b.txt"));
            using StreamReader readerC = new StreamReader(File.OpenRead("c.txt"));
            using StreamWriter bw = new StreamWriter(File.Create(FileInput));
            long counterA = _iterations, counterB = _iterations, counterC = _iterations;
            string elementA = null, elementB = null, strA = null, strB = null, strC = null, elementC = null;
            bool pickedA = false, pickedB = false, pickedC = false, endA = false, endB = false, endC = false;
            while (!endA || !endB || !endC)
            {
                if (counterA == 0 && counterB == 0 && counterC == 0)
                {
                    counterA = _iterations;
                    counterB = _iterations;
                    counterC = _iterations;
                }



                if (!readerA.EndOfStream)
                {
                    if (counterA > 0 && !pickedA)
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
                    if (counterB > 0 && !pickedB)
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

                if (!readerC.EndOfStream)
                {
                    if (counterC > 0 && !pickedC)
                    {
                        strC = readerC.ReadLine();
                        elementC = strC?.Split(" ")[_columnNumber];
                        pickedC = true;
                    }
                }
                else
                {
                    endC = true;
                }
                Console.WriteLine("\n" + elementA + " " + elementB + " " + elementC);
                if (pickedA)
                {
                    if (pickedB)
                    {
                        if (pickedC)
                        {
                            Console.WriteLine("Сравнение " + elementA + " и " + elementB);
                            if (String.CompareOrdinal(elementA, elementB) < 0)
                            {
                                Console.WriteLine("Сравнение " + elementA + " и " + elementC);
                                if (String.CompareOrdinal(elementA, elementC) < 0)
                                {
                                    Console.WriteLine("Запись " + elementA);
                                    bw.WriteLine(strA);
                                    counterA--;
                                    pickedA = false;
                                    sb.Append("\n" + strA);
                                }
                                else
                                {
                                    Console.WriteLine("Запись " + elementC);
                                    bw.WriteLine(strC);
                                    counterC--;
                                    pickedC = false;
                                    sb.Append("\n" + strC);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Сравнение " + elementB + " и " + elementC);
                                if (String.CompareOrdinal(elementB, elementC) < 0)
                                {
                                    Console.WriteLine("Запись " + elementB);
                                    bw.WriteLine(strB);
                                    counterB--;
                                    pickedB = false;
                                    sb.Append("\n" + strB);
                                }
                                else
                                {
                                    Console.WriteLine("Запись " + elementC);
                                    bw.WriteLine(strC);
                                    counterC--;
                                    pickedC = false;
                                    sb.Append("\n" + strC);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Сравнение " + elementA + " и " + elementB);
                            if (String.CompareOrdinal(elementA, elementB) < 0)
                            {
                                bw.WriteLine(strA);
                                Console.WriteLine("Запись " + elementA);
                                counterA--;
                                pickedA = false;
                                sb.Append("\n" + strA);
                            }
                            else
                            {
                                bw.WriteLine(strB);
                                Console.WriteLine("Запись " + elementB);
                                counterB--;
                                pickedB = false;
                                sb.Append("\n" + strB);
                            }
                        }
                    }
                    else if (pickedC)
                    {
                        Console.WriteLine("Сравнение " + elementA + " и " + elementC);
                        if (String.CompareOrdinal(elementA, elementC) < 0)
                        {
                            bw.WriteLine(strA);
                            Console.WriteLine("Запись " + elementA);
                            counterA--;
                            pickedA = false;
                            sb.Append("\n" + strA);
                        }
                        else
                        {
                            Console.WriteLine("Запись " + elementC);
                            bw.WriteLine(strC);
                            counterC--;
                            pickedC = false;
                            sb.Append("\n" + strC);
                        }
                    }
                    else
                    {
                        bw.WriteLine(strA);
                        Console.WriteLine("Запись " + elementA);
                        counterA--;
                        pickedA = false;
                        sb.Append("\n" + strA);
                    }
                }
                else if (pickedB && pickedC)
                {
                    Console.WriteLine("Сравнение " + elementB + " и " + elementC);
                    if (String.CompareOrdinal(elementB, elementC) < 0)
                    {
                        Console.WriteLine("Запись " + elementB);
                        bw.WriteLine(strB);
                        counterB--;
                        pickedB = false;
                        sb.Append("\n" + strB);
                    }
                    else
                    {
                        Console.WriteLine("Запись " + elementC);
                        bw.WriteLine(strC);
                        counterC--;
                        pickedC = false;
                        sb.Append("\n" + strC);
                    }
                }
                else if (pickedC)
                {
                    Console.WriteLine("Запись " + elementC);
                    bw.WriteLine(strC);
                    counterC--;
                    pickedC = false;
                    sb.Append("\n" + strC);
                }
                else if (pickedB)
                {
                    Console.WriteLine("Запись " + elementB);
                    bw.WriteLine(strB);
                    counterB--;
                    pickedB = false;
                    sb.Append("\n" + strB);
                }
            }
            _iterations *= 3;
            bw.Close();
            Console.WriteLine("\nИзначальный файл: " + sb.ToString());
            Console.WriteLine();
        }
    }
}