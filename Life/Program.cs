using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using life;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.VisualBasic;

namespace life
{
    public class Program
    {
        static Board board;

        static private void Reset() //сюда добавить выгрузку из json
        {
            string fileName = "LifeParameters.json";
            string jsonString = File.ReadAllText(fileName);
            Parameters parametrs = JsonSerializer.Deserialize<Parameters>(jsonString)!;

            board = new Board(
                width: parametrs.width,
                height: parametrs.height,
                cellSize: parametrs.cellSize,
                liveDensity: parametrs.liveDensity);
        }

        static void Render()
        {
            for (int row = 0; row < board.Rows; row++)
            {
                for (int col = 0; col < board.Columns; col++)
                {
                    var cell = board.Cells[col, row];
                    if (cell.IsAlive)
                    {
                        Console.Write('*');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write('\n');
            }
        }

        static private Board PopulationFromFile(string name) 
        {
            int width = 0;
            int height = 0;
            int[,] cellValue;

            using (StreamReader sr = new StreamReader(name))
            {
                long x = sr.BaseStream.Length;
                cellValue = new int[100, 100]; //чтобы хотя бы как-то создать
                int i = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    width = words.Length;
                    for (int j = 0; j < width; j++)
                    {
                        if (words[j] == "1") cellValue[i, j] = 1;
                        else if (words[j] == "0") cellValue[i, j] = 0;
                    }
                    i++;
                }
                height = i;
            }

            board = new Board(width-1, height, 1, cellValue);
            return board;
        }     

        static void SavingToFile(int genCount) 
        {
            string fname = "gen-" + genCount.ToString();
            StreamWriter writer = new StreamWriter(fname + ".txt");

            for (int row = 0; row < board.Rows; row++)
            {
                for (int col = 0; col < board.Columns; col++)
                {
                    var cell = board.Cells[col, row];
                    if (cell.IsAlive)
                    {
                        writer.Write('1');
                    }
                    else
                    {
                        writer.Write('0');
                    }
                    writer.Write(',');
                }
                writer.Write("\n");
            }
            writer.Close();
        }

        static string BuildString(Board tempBoard, int col, int row, int n, int m, int a) //a - сколько столбиков назад
        {
            char[] temp = new char[n * m];

            int index = 0;
            for (int j = 0; j < n; j++) //строчка
            {
                for (int i = 0; i < m; i++) //столбик
                {
                    if (board.Cells[col - a + i, row - 1 + j].IsAlive) temp[index] = '1';
                    else temp[index] = '0';
                    index++;
                }
            }
            string s = new string(temp);

            return s;
        }

        static int[] DetectionFigure(Board tempBoard)
        {
            int[] numberOfFigures = new int[15];

            //4*4
            string block = "0000011001100000"; //-1

            //5*5
            string box = "0000000100010100010000000"; //-2
            string boat = "0000000100010100011000000"; //-2
            string ship = "0000001100010100011000000"; //-1

            //6*5
            string hive = "000000010001010010100010000000";//-2

            //5*6
            string hive2 = "000000001100010010001100000000";//-2

            //6*6
            string loaf = "000000001100010010001010000100000000";//-2
            string pond = "000000001100010010010010001100000000";//-2
            string barge = "000000001000010100001010000100000000";//-2
            string longBoat = "000000001000010100001010000110000000";//-2
            string longShip = "000000011000010100001010000110000000";//-1

            //4*6
            string snake = "000000010110011010000000";//-1

            //7*7
            string longBarge = "0000000001000001010000010100000101000001000000000";//-1

            string s; //строка для проверки

            for (int row = 1; row < tempBoard.Rows - 4; row++)
            {
                for (int col = 2; col < tempBoard.Columns - 4; col++)
                {
                    var cell = tempBoard.Cells[col, row];
                    if (cell.IsAlive)
                    {
                        //4*4
                        s = BuildString(tempBoard, col, row, 4, 4, 1);
                        if (s.Equals(block)) numberOfFigures[0]++; //считаем количество

                        //5*5
                        s = BuildString(tempBoard, col, row, 5, 5, 2);
                        if (s.Equals(box)) numberOfFigures[1]++;
                        if (s.Equals(boat)) numberOfFigures[2]++;
                        s = BuildString(tempBoard, col, row, 5, 5, 1);
                        if (s.Equals(ship)) numberOfFigures[3]++;

                        //6*5
                        s = BuildString(tempBoard, col, row, 6, 5, 2);
                        if (s.Equals(hive)) numberOfFigures[4]++;

                        //5*6
                        s = BuildString(tempBoard, col, row, 5, 6, 2);
                        if (s.Equals(hive2)) numberOfFigures[4]++;

                        //4*6
                        s = BuildString(tempBoard, col, row, 4, 6, 1);
                        if (s.Equals(snake)) numberOfFigures[5]++; //считаем количество
                        
                        //6*6
                        s = BuildString(tempBoard, col, row, 6, 6, 2);
                        if (s.Equals(loaf)) numberOfFigures[6]++;
                        if (s.Equals(pond)) numberOfFigures[7]++;
                        if (s.Equals(barge)) numberOfFigures[8]++;
                        if (s.Equals(longBoat)) numberOfFigures[9]++;
                        s = BuildString(tempBoard, col, row, 6, 6, 1);
                        if (s.Equals(longShip)) numberOfFigures[10]++;
                        /*
                        //7*7 не работает, так как использовано много костылей, чтобы исправить нужно учитывать, что сфера а не прямоугольник
                        s = BuildString(tempBoard, col, row, 7, 7, 2);
                        if (s.Equals(longBarge)) numberOfFigures[11]++; //считаем количество
                        */
                    }
                }
            }
            return numberOfFigures;
        }

        static int CountElements(Board tempBoard)
        {
            int count = 0;

            for (int row = 0; row < tempBoard.Rows; row++)
            {
                for (int col = 0; col < tempBoard.Columns; col++)
                {
                    var cell = tempBoard.Cells[col, row];
                    if (cell.IsAlive)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        static int CountSymmetrical(Board tempBoard)
        {
            int count = 0;

            int[] numberOfFigures = DetectionFigure(tempBoard);

            for(int i=0; i < numberOfFigures.Length; i++)
            {
                //симметричными считатем блок, коробку, улий, пруд
                if (i == 0 || i == 1 || i == 4 || i == 7)
                {
                    count += numberOfFigures[i];
                }
            }

            return count;
        }

        public static int checkinElementCount(string name) //функция для тестов
        {
            Board testBoard = PopulationFromFile(name);
            int count = CountElements(testBoard);
            return count;
        }

        public static int checkinSymmetricFigureCount(string name) //функция для тестов
        {
            Board testBoard = PopulationFromFile(name);
            int count = CountSymmetrical(testBoard);
            return count;
        }

        public static int[] checkinDetectionFigure(string name) //функция для тестов
        {
            Board testBoard = PopulationFromFile(name);
            int[] numberOfFigures = DetectionFigure(testBoard);
            return numberOfFigures;
        }

        static void Main(string[] args)
        {
            Reset();
            int genCount = 0;       

            while (true)
            {
                ++genCount;

                if (Console.KeyAvailable) //поверка на нажатые клавишы
                {
                    ConsoleKeyInfo name = Console.ReadKey();
                    if (name.KeyChar == 'q') //выход
                    {
                        break;
                    }
                    else if (name.KeyChar == 's') //сохранение популяции
                    {
                        SavingToFile(genCount);
                    }
                    else if (name.KeyChar == 'n') //добавление популяции из файла
                    {
                        board = PopulationFromFile("gen-239.txt");
                    }
                    else if (name.KeyChar == 'p') //проверка на фигуры
                    {
                        int[] numberOfFigures = DetectionFigure(board);
                    }
                    else if (name.KeyChar == 'c') //количество симметричных
                    {
                        int countSym = CountSymmetrical(board);
                    }
                    else if (name.KeyChar == 'e') //количество симметричных
                    {
                        int count = CountElements(board);
                    }
                }

                Console.Clear(); //очищение консоли
                Render();
                board.Advance();
                Thread.Sleep(100);            
            }
        }
    }
}
