
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;

namespace SeaBattle
{
    class Program
    {
        public static string name = "";
        public static string[,] field = new string[10, 10];
        public static string[,] shipField = new string[10, 10];
        static void Main(string[] args)
        {
            FillAndShowPlatvorm();
            GenerateShipField();
            CreateMenu();
        }
        public static void FillAndShowPlatvorm()
        {
            for(int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    field[y, x] = "*";
                }
            }
        }
        public static void GenerateShipField()
        {
            for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        shipField[y, x] = "0";
                    }
                }
            RandomizeShips();
        }

        private static void RandomizeShips()
        {
            List<int> shipList = new List<int>() {4, 3, 3, 2, 2, 2, 1, 1, 1, 1 }; // creating list of ships's dimension
            for(int i = 0; i < shipList.Count; i++)
            {
                int caseShipList = shipList[i];
                switch (caseShipList) // Calculating every ship position from largest to smallest
                {
                    case 4:
                        GenerateLargeShip(caseShipList);
                        break;
                    case 3:
                        GenerateBigShip(caseShipList);
                        break;
                    case 2:
                        GenerateMediumShip(caseShipList);
                        break;
                    case 1:
                        GenerateSmallShip(caseShipList);
                        break;
                }
            }
            
        }

        private static void GenerateSmallShip(int caseShipList) // generatin ship with 1 part 
        {
            bool[,] checkList = new bool[3,3];
            int x = 0;
            int y = 0;
            Random choice = new Random();
            do
            {
                x = choice.Next(0, 9);
                y = choice.Next(0, 9);
                GenerateBlankCasesAroundShips(checkList, 3, 3, shipField, x, y);
            } while (!CalculateAllValues(checkList));
            GenerateNextPoint(y + 1, x + 1, true, caseShipList);
        }

        private static void GenerateMediumShip(int caseShipList) // generating ship with 2 parts
        {
            bool[,] checkList;
            int x = 0;
            int y = 0;
            bool direction = false; // true - direction is vertiacl, false - direction is horizontal
            int coordinateX = 8;
            int coordinateY = 8;
            Random choice = new Random();
            int directionNum = choice.Next(0, 2);
            if(directionNum == 0) // direction is horizontal
            {
                coordinateY = 9;
                checkList = new bool[3, 5];
                do
                {
                    x = choice.Next(0, coordinateX);
                    y = choice.Next(0, coordinateY);
                    GenerateBlankCasesAroundShips(checkList, 5, 3, shipField, x, y);
                } while (!CalculateAllValues(checkList));
                GenerateNextPoint(y + 1, x + 1, direction, caseShipList);
            }
            else // direction is vertical
            {
                direction = true;
                coordinateX = 9;
                checkList = new bool[5, 3];
                do
                {
                    x = choice.Next(0, coordinateX);
                    y = choice.Next(0, coordinateY);
                    GenerateBlankCasesAroundShips(checkList, 3, 5, shipField, x, y);
                } while (!CalculateAllValues(checkList));
                GenerateNextPoint(y + 1, x + 1, direction, caseShipList);
            }
        }

        private static void GenerateBlankCasesAroundShips(bool[,] checkList, int xMax, int yMax, string[,] shipField, int xPoint, int yPoint)
        {
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    try
                    {
                        checkList[y, x] = (shipField[yPoint + y, xPoint + x] != "■"); // define, if case contains ship part   
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
        }

        private static bool CalculateAllValues(bool[,] checkList)
        {
            bool result = true;
            foreach(bool check in checkList)
            {
                result = result & check;
            }
            return result;
        }

        private static void GenerateBigShip(int caseShipList) // generating ship with 3 parts
        {
            bool[,] checkList;
            int x = 0;
            int y = 0;
            bool direction = false; // true - direction is vertical, false - direction is horizontal
            int coordinateX = 7;
            int coordinateY = 7;
            Random choice = new Random();
            int directionNum = choice.Next(0, 2);
            if (directionNum == 0) // direction is horizontal
            {
                checkList = new bool[3, 6];
                coordinateY = 9;
                do // finding suitable case for first point of big ship
                {
                    x = choice.Next(0, coordinateX);
                    y = choice.Next(0, coordinateY);
                    GenerateBlankCasesAroundShips(checkList, 6, 3, shipField, x, y);
                } while (!CalculateAllValues(checkList));
                GenerateNextPoint(y + 1, x + 1, direction, caseShipList);
            }
            else if (directionNum == 1) // direction is vertical
            {
                checkList = new bool[6, 3];
                direction = true; // vertical
                coordinateX = 9;
                do // finding suitable case for first point of big ship
                {
                    x = choice.Next(0, coordinateX);
                    y = choice.Next(0, coordinateY);
                    GenerateBlankCasesAroundShips(checkList, 3, 6, shipField, x, y);
                } while (!CalculateAllValues(checkList));
                GenerateNextPoint(y + 1, x + 1 , direction, caseShipList);
            }
        }

        private static void GenerateLargeShip(int shipNumber) // generating ship with 4 parts
        {
            bool direction = false; // true - direction is vertical, false - direction is horizontal
            int coordinateX = 6;
            int coordinateY = 6;
            Random choice = new Random();
            int directionNum = choice.Next(0, 2); 
            if (directionNum == 0) // 0 is false
            {
                coordinateY = 9;
            }
            else // 1 is true
            {
                direction = true; // direction is vertical
                coordinateX = 9;
            }
            int y = choice.Next(0, coordinateY + 1); // generating first point 
            int x = choice.Next(0, coordinateX + 1);
            shipField[y, x] = "■";
            GenerateNextPoint(y, x, direction, shipNumber);
            
        }
        private static void GenerateNextPoint(int y, int x, bool direction, int shipNumber)
        {
            switch (shipNumber)
            {
                #region if ship consists of 4 parts
                case 4:
                    if (direction) // direction is vertical
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            shipField[y + i, x] = "■";
                        }
                        break;
                    }
                    else // direction is horizontal
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            shipField[y, x + i] = "■";
                        }
                        break;
                    }
                #endregion
                #region if ship consist of 3 parts
                case 3:
                    if (direction) // if direction is vertical
                    {
                        for (int i = 0; i < shipNumber; i++)
                        {
                            shipField[y + i, x] = "■";
                        }
                        break;
                    }
                    else //if direction is horizontal
                    {
                        for (int i = 0; i < shipNumber; i++)
                        {
                            shipField[y, x + i] = "■";
                        }
                        break;
                    }
                #endregion
                #region if ship consists of 2 parts
                case 2:
                    if(direction) // if direction is vertical
                    {
                        for (int i = 0; i < shipNumber; i++)
                        {
                            shipField[y + i, x] = "■";
                        }
                        break;
                    }
                    else // if direction is horizontal
                    {
                        for ( int i = 0; i < shipNumber; i++)
                        {
                            shipField[y, x + i] = "■";
                        }
                        break;
                    }
                #endregion
                #region if ship consists of 1 part
                case 1:
                    shipField[y, x] = "■";
                    break;
                    #endregion
            }
        }
            

        public static void CreateMenu()
        {
            List<string> menuChoice = new List<string>() {
                "╔══╦════╦══╦═══╦════╗\n║╔═╩═╗╔═╣╔╗║╔═╗╠═╗╔═╝\n║╚═╗─║║─║╚╝║╚═╝║─║║\n╚═╗║─║║─║╔╗║╔╗╔╝─║║\n╔═╝║─║║─║║║║║║║──║║\n╚══╝─╚╝─╚╝╚╩╝╚╝──╚╝",
                "╔╗─╔═══╦══╦══╗╔═══╦═══╦══╗╔══╦══╦═══╦══╗╔══╗\n║║─║╔══╣╔╗║╔╗╚╣╔══╣╔═╗║╔╗║║╔╗║╔╗║╔═╗║╔╗╚╣╔═╝\n║║─║╚══╣╚╝║║╚╗║╚══╣╚═╝║╚╝╚╣║║║╚╝║╚═╝║║╚╗║╚═╗\n║║─║╔══╣╔╗║║─║║╔══╣╔╗╔╣╔═╗║║║║╔╗║╔╗╔╣║─║╠═╗║\n║╚═╣╚══╣║║║╚═╝║╚══╣║║║║╚═╝║╚╝║║║║║║║║╚═╝╠═╝║\n╚══╩═══╩╝╚╩═══╩═══╩╝╚╝╚═══╩══╩╝╚╩╝╚╝╚═══╩══╝",
                "╔═══╦══╗╔══╦══╦════╗\n║╔══╩═╗║║╔═╩╗╔╩═╗╔═╝\n║╚══╗─║╚╝║──║║──║║\n║╔══╝─║╔╗║──║║──║║\n║╚══╦═╝║║╚═╦╝╚╗─║║\n╚═══╩══╝╚══╩══╝─╚╝" };

            string userChoice = "";
            int choice = menuChoice.Count - 1;
            int cycles = menuChoice.Count;
            Console.Clear();
            Console.WindowHeight = 40; // creating console properties
            Console.WindowWidth = 90;
            // creating list with menu options
            DisplayOptions(menuChoice);
            userChoice = ChoiceInConsole(menuChoice, userChoice, choice, cycles);
            Console.Clear();
            if (userChoice == menuChoice[0])
            {
                PlayGame();
            }
            else if (userChoice == menuChoice[1])
            {
                ShowLeaderBoard();
            }
            else if (userChoice == menuChoice[2])
            {
                Environment.Exit(0);
            }
        }

        private static void PlayGame()
        {
            Console.WriteLine("Hello");
            do
            {
                Console.Clear();
                Console.WriteLine("╔╗╔╦══╦╗╔╦═══╗╔╗─╔╦══╦╗──╔╦═══╗\n" +
                                  "║║║║╔╗║║║║╔═╗║║╚═╝║╔╗║║──║║╔══╝\n" +
                                  "║╚╝║║║║║║║╚═╝║║╔╗─║╚╝║╚╗╔╝║╚══╗\n" +
                                  "╚═╗║║║║║║║╔╗╔╝║║╚╗║╔╗║╔╗╔╗║╔══╝\n" +
                                  "─╔╝║╚╝║╚╝║║║║─║║─║║║║║║╚╝║║╚══╗\n" +
                                  "─╚═╩══╩══╩╝╚╝─╚╝─╚╩╝╚╩╝──╚╩═══╝\n" +
                                  "Name must have 1-8 characters");
                name = Console.ReadLine();
            } while (name.Length < 1 || name.Length > 8);
            if(name.Length < 8)
            {
                string whiteSpaces = String.Concat(Enumerable.Repeat(" ", (8 - name.Length)));
                name = String.Concat(name, whiteSpaces); 
            }
            Console.Clear();
            ChoiceInGame();
        }

        private static void ChoiceInGame()
        {
            bool isTrue = true;
            int choiceX = 0;
            int choiceY = 0;
            int countPoint = 0;
            int points = 0;
            int notCounted = 0;
            ShowField(points);
            do
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                Console.Clear();
                if (input.Key == ConsoleKey.Enter)
                {
                    bool isExplode = ExplodeShip(choiceX, choiceY); // enter code here!
                    if (isExplode)
                    {
                        countPoint++;
                        points += ( 100 - notCounted) / 20;
                        if (countPoint == 20)
                        {
                            name += points;
                            Congratulations(name);
                        }
                    }
                    else
                        notCounted++;
                    MakeFieldColor(choiceY, choiceX, points);
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    choiceY += 1;
                    if (choiceY == 10)
                        choiceY = 0;
                    MakeFieldColor(choiceY, choiceX, points);
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    choiceY -= 1;
                    if (choiceY == -1)
                        choiceY = 9;
                    MakeFieldColor(choiceY, choiceX, points);
                }
                else if (input.Key == ConsoleKey.RightArrow)
                {
                    choiceX += 1;
                    if (choiceX == 10)
                        choiceX = 0;
                    MakeFieldColor(choiceY, choiceX, points);
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {
                    choiceX -= 1;
                    if (choiceX == -1)
                        choiceX = 9;
                    MakeFieldColor(choiceY, choiceX, points);
                }
                else
                    continue;
            } while (isTrue);
        }

        private static void Congratulations(string name)
        {
            Console.WriteLine("╔══╦══╦╗─╔╦═══╦═══╦══╦════╦╗╔╦╗─╔══╦════╦══╦══╦╗─╔╦══╦╗\n",
                              "║╔═╣╔╗║╚═╝║╔══╣╔═╗║╔╗╠═╗╔═╣║║║║─║╔╗╠═╗╔═╩╗╔╣╔╗║╚═╝║╔═╣║\n",
                              "║║─║║║║╔╗─║║╔═╣╚═╝║╚╝║─║║─║║║║║─║╚╝║─║║──║║║║║║╔╗─║╚═╣║\n",
                              "║║─║║║║║╚╗║║╚╗║╔╗╔╣╔╗║─║║─║║║║║─║╔╗║─║║──║║║║║║║╚╗╠═╗╠╝\n",
                              "║╚═╣╚╝║║─║║╚═╝║║║║║║║║─║║─║╚╝║╚═╣║║║─║║─╔╝╚╣╚╝║║─║╠═╝╠╗\n",
                              "╚══╩══╩╝─╚╩═══╩╝╚╝╚╝╚╝─╚╝─╚══╩══╩╝╚╝─╚╝─╚══╩══╩╝─╚╩══╩╝\n",
                              "╔╗╔╦══╦╗╔╗─╔╗╔╗╔╦══╦╗─╔╦╗\n",
                              "║║║║╔╗║║║║─║║║║║╠╗╔╣╚═╝║║\n",
                              "║╚╝║║║║║║║─║║║║║║║║║╔╗─║║\n",
                              "╚═╗║║║║║║║─║║║║║║║║║║╚╗╠╝\n",
                              "─╔╝║╚╝║╚╝║─║╚╝╚╝╠╝╚╣║─║╠╗\n",
                              "─╚═╩══╩══╝─╚═╝╚═╩══╩╝─╚╩╝\n");
            using (StreamWriter writer = new StreamWriter("leadersBoard.txt", true))
            {
                writer.WriteLine(name);
            }
            System.Threading.Thread.Sleep(4000);
            CreateMenu();
        }

        private static void MakeFieldColor(int choiceY, int choiceX, int points)
        {
            Console.WriteLine($"\t\t\tPoints: {points}");
            for (int y = 0; y < 10; y++)
            {
                for(int x = 0; x < 10; x++)
                {
                    if(y == choiceY && x == choiceX)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{field[choiceY, choiceX]}");
                        Console.ResetColor();
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.Write($"{field[y, x]}   ");
                    }
                }
                Console.WriteLine("\n\n");
            }
        }

        private static bool ExplodeShip(int x, int y)
        {
            if (shipField[y, x] == "■")
            {
                field[y, x] = "■";
                return true;
            }
            else
            {
                field[y, x] = "0";
                return false;
            }
        }

        private static void ShowField(int points)
        {
            Console.WriteLine($"\t\t\tPoints: {points}");
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Console.Write($"{field[y, x]}   ");
                }
                Console.WriteLine($"\n\n");
            }
        }

        private static string ChoiceInConsole(List<string> choiceList, string userChoice, int choice, int cycles)
        {
            bool optionsTrue = true;
            do
            {
                int count = choiceList.Count;
                ConsoleKeyInfo input = Console.ReadKey(true);
                string last = userChoice;
                Console.Clear();
                DisplayOptions(choiceList);
                if (input.Key == ConsoleKey.Enter)
                {
                    optionsTrue = false;
                    userChoice = last;
                }
                else if (input.Key == ConsoleKey.UpArrow)
                {
                    Console.Clear();
                    choice = (choice - 1);
                    if (choice == -1)
                        choice = 2;
                    choice %= cycles;
                    userChoice = choiceList[choice];
                    for (int i = 0; i < count; i++)
                    {
                        if (i != choice)
                            Console.WriteLine(choiceList[i]);
                        else
                            MakeTextColor(choiceList[i]);
                    }
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    Console.Clear();
                    choice = (choice + 1) % cycles;
                    userChoice = choiceList[choice];
                    for (int i = 0; i < count; i++)
                    {
                        if (i == choice)
                            MakeTextColor(choiceList[i]);
                        else
                            Console.WriteLine(choiceList[i]);
                    }
                }
                else
                    continue;
            } while (optionsTrue);
            return userChoice;
        }

        private static void MakeTextColor(string option)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(option);
            Console.ResetColor();
        }

        private static void DisplayOptions(List<string> menuChoice) // method for showing all options
        {
            foreach (string option in menuChoice)
            {
                Console.WriteLine(option);
            }
        }
        private static void ShowLeaderBoard() // Method for showing leaderboard page with light
        {
            List<string> leaderBoardList = new List<string>()
            {
                "╔╗─╔═══╦══╦══╗╔═══╦═══╦══╗╔══╦══╦═══╦══╗╔══╗\n║║─║╔══╣╔╗║╔╗╚╣╔══╣╔═╗║╔╗║║╔╗║╔╗║╔═╗║╔╗╚╣╔═╝\n║║─║╚══╣╚╝║║╚╗║╚══╣╚═╝║╚╝╚╣║║║╚╝║╚═╝║║╚╗║╚═╗\n║║─║╔══╣╔╗║║─║║╔══╣╔╗╔╣╔═╗║║║║╔╗║╔╗╔╣║─║╠═╗║\n║╚═╣╚══╣║║║╚═╝║╚══╣║║║║╚═╝║╚╝║║║║║║║║╚═╝╠═╝║\n╚══╩═══╩╝╚╩═══╩═══╩╝╚╝╚═══╩══╩╝╚╩╝╚╝╚═══╩══╝",
                "╔══╗╔══╦══╦╗╔══╗\n║╔╗║║╔╗║╔═╣║║╔═╝\n║╚╝╚╣╚╝║║─║╚╝║\n║╔═╗║╔╗║║─║╔╗║\n║╚═╝║║║║╚═╣║║╚═╗\n╚═══╩╝╚╩══╩╝╚══╝" };
            int cycles = leaderBoardList.Count;
            string userChoice = "";
            int choice = leaderBoardList.Count - 1;
            Console.Clear();
            DisplayOptions(leaderBoardList);
            userChoice = ChoiceInConsole(leaderBoardList, userChoice, choice, cycles);
            if (userChoice == leaderBoardList[0])
            {
                
                string[] rateList = new string[3]; // List for 3 leaders
                List<int> numList = new List<int>(); // List for sorting rate
                string line = "";
                string fileLocation = @"C:\Users\37255\Desktop\SeaBattle\SeaBattle\bin\Debug\netcoreapp3.1\leadersBoard.txt";
                FileInfo file = new FileInfo(fileLocation);
                if (file.Length != 0)
                {
                    using (StreamReader reader = new StreamReader("leadersBoard.txt", true))
                    {
                        List<string> fullList = new List<string>(); // list with name + points
                        while ((line = reader.ReadLine()) != null)
                        {
                            fullList.Add(line);
                        }
                        for (int i = 0; i < fullList.Count; i++) // take points for rating
                        {
                            int point = int.Parse(fullList[i].Substring(7, (fullList[i].Length - 7))); // get integers
                            numList.Add(point);
                        }
                        SortChangeRateList(numList, fullList, rateList);

                        for(int i = 0; i < rateList.Length; i++)
                        {
                            if (rateList[i] == null)
                                Console.WriteLine("");
                            else
                                Console.WriteLine($"{i}.{rateList[i].Substring(0, 8)} made {rateList[i].Substring(8, rateList[i].Length - 8)} points");
                        }
                        System.Threading.Thread.Sleep(3000);
                        CreateMenu();
                    }
                }
                else
                {
                    Console.WriteLine(leaderBoardList[0]);
                    Console.WriteLine("╔═══╦══╦╗──╔╦═══╗╔╗─╔══╦══╦════╗",
                                      "║╔══╣╔╗║║──║║╔══╝║║─╚╗╔╣╔═╩═╗╔═╝",
                                      "║║╔═╣╚╝║╚╗╔╝║╚══╗║║──║║║╚═╗─║║",
                                      "║║╚╗║╔╗║╔╗╔╗║╔══╝║║──║║╚═╗║─║║",
                                      "║╚═╝║║║║║╚╝║║╚══╗║╚═╦╝╚╦═╝║─║║",
                                      "╚═══╩╝╚╩╝──╚╩═══╝╚══╩══╩══╝─╚╝",
                                      "╔══╦══╗─╔═══╦╗──╔╦═══╦════╦╗╔╗",
                                      "╚╗╔╣╔═╝─║╔══╣║──║║╔═╗╠═╗╔═╣║║║",
                                      "─║║║╚═╗─║╚══╣╚╗╔╝║╚═╝║─║║─║╚╝║",
                                      "─║║╚═╗║─║╔══╣╔╗╔╗║╔══╝─║║─╚═╗║",
                                      "╔╝╚╦═╝║─║╚══╣║╚╝║║║────║║──╔╝║",
                                      "╚══╩══╝─╚═══╩╝──╚╩╝────╚╝──╚═╝");
                    Console.WriteLine(leaderBoardList[1]);
                }
            }
            else
                CreateMenu();
        }

        private static void SortChangeRateList(List<int> numList, List<string> fullList, string[] rateList)
        {
            //Method which takes list with numbers, sort it, 
            for(int i = 0; i < numList.Count - 1; i++) // sorting List by Bubble sorting
            {
                for (int j = 0; j < numList.Count - i - 1; j++)
                {
                    if (numList[j] < numList[j + 1])
                    {
                        numList[j] += numList[j + 1];
                        numList[j + 1] = numList[j] - numList[j + 1];
                        numList[j] -= numList[j + 1];
                    }
                }
            }
            //find rated names with points to add them to rate list by using regular expressions
            if (fullList.Count < 3)
            {
                for(int i = 0; i < fullList.Count; i++)
                {
                    rateList[i] = fullList[i];
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < fullList.Count; j++)
                    {
                        Match match = Regex.Match(fullList[j], ("^[a-zA-Z0-9]{8}" + numList[i])); // finding matches in fullList
                        if (match.Success)
                            rateList[i] = match.ToString();
                    }
                }
            }
        }
    }
}