using System;
using System.Collections.Generic;

namespace VierGewinnt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Start();
        }
        public static void Start()
        {
            Initialize();
            Helper.DrawField();
            StartGame();
            Console.ReadKey();
        }
        public static void StartGame()
        {
            for (int i = 1; i < 43; i++)
            {
                Console.Title = "Runde " + i;
                if (i % 2 == 1) MakeTurn(Essentials.Players[0]);
                else MakeTurn(Essentials.Players[1]);

                Helper.DrawField();

                if (CheckWin()) break;
            }
            Helper.DrawField();
            Console.WriteLine($"!!Game Over!!");
            if (Essentials.Winner.PlayerName != "") Console.WriteLine($"Winner: {Essentials.Winner.PlayerName}.");
            else Console.WriteLine("Niemand Hat Gewonnen.");
        }

        private static bool CheckWin()
        {
            Boolean foundWin = false;
            foreach(Player currentPlayer in Essentials.Players)
            {
                // oben nach unten
                for (int i = 0; i < 7; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (Essentials.Fields[i][x] == currentPlayer.PlayerColor && Essentials.Fields[i][(x + 1)] == currentPlayer.PlayerColor && Essentials.Fields[i][(x + 2)] == currentPlayer.PlayerColor && Essentials.Fields[i][(x + 3)] == currentPlayer.PlayerColor) foundWin = true;
                        if (foundWin) break;
                    }
                    if (foundWin) break;
                }

                // left to right
                for (int x = 0; x < 6; x++)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        if (Essentials.Fields[i][x] == currentPlayer.PlayerColor && Essentials.Fields[(i + 1)][x] == currentPlayer.PlayerColor && Essentials.Fields[(i + 2)][x] == currentPlayer.PlayerColor && Essentials.Fields[(i + 3)][x] == currentPlayer.PlayerColor) foundWin = true;
                        if (foundWin) break;
                    }
                    if (foundWin) break;
                }
                // Top left to Bottom right
                for (int i = 0; i < 4; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (Essentials.Fields[i][x] == currentPlayer.PlayerColor && Essentials.Fields[i + 1][x + 1] == currentPlayer.PlayerColor && Essentials.Fields[i + 2][x + 2] == currentPlayer.PlayerColor && Essentials.Fields[i + 3][x + 3] == currentPlayer.PlayerColor) foundWin = true;
                        if (foundWin) break;
                    }
                    if (foundWin) break;
                }
                // Bottom left to top right
                for (int i = 0; i < 4; i++)
                {
                    for (int x = 5; x > 3; x--)
                    {
                        if (Essentials.Fields[i][x] == currentPlayer.PlayerColor && Essentials.Fields[i + 1][x - 1] == currentPlayer.PlayerColor && Essentials.Fields[i + 2][x - 2] == currentPlayer.PlayerColor && Essentials.Fields[i + 3][x - 3] == currentPlayer.PlayerColor) foundWin = true;
                        if (foundWin) break;
                    }
                    if (foundWin) break;
                }
                if (foundWin)
                {
                    Essentials.Winner = currentPlayer;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Makes the Current Player in the Parameter do his Next Move
        /// </summary>
        /// <param name="player"></param>
        public static void MakeTurn(Player player)
        {
            Console.WriteLine($"{player.PlayerName}: Bitte Schreibe eine Buchstaben Reihe.");
            string Input = Console.ReadLine();
            while (Input == null || Input == "" || !Essentials.Letters.Contains(Input.ToCharArray()[0]) || !Helper.HasRowSpace(Input.ToCharArray()[0]))
            {
                Helper.DrawField();
                Console.WriteLine($"{player.PlayerName}: Bitte Schreibe eine Richtige Buchstaben Reihe");
                Input = Console.ReadLine();
            }
            char Row = Input[0];
            AddField(Row, player);

            // Wenn man das Spielfeld mit den Pfeiltasten steuern will, dann benutzt man das folgende, benötigte sachen sind über die datei verteilt.
            //do
            //{
            //    if (Console.ReadKey().Key == ConsoleKey.LeftArrow)
            //    {
            //        if (Essentials.CursorPosition[0] != ' ') continue;
            //        for (int i = 1; i < Essentials.CursorPosition.Count; i++)
            //        {
            //            if (Essentials.CursorPosition[i] == 'X')
            //            {
            //                Essentials.CursorPosition[i] = ' ';
            //                Essentials.CursorPosition[i -  1] = 'X';
            //                break;
            //            }
            //        }
            //    }
            //    if (Console.ReadKey().Key == ConsoleKey.RightArrow)
            //    {
            //        if (Essentials.CursorPosition[6] != ' ') continue;
            //        for (int i = 0; i < Essentials.CursorPosition.Count - 1; i++)
            //        {
            //            if (Essentials.CursorPosition[i] == 'X')
            //            {
            //                Essentials.CursorPosition[i] = ' ';
            //                Essentials.CursorPosition[i + 1] = 'X';
            //                break;
            //            }
            //        }
            //    }

            //    Helper.DrawField();
            //} while (Console.ReadKey().Key != ConsoleKey.Enter);
        }
        
        public static void AddField(char Letter, Player player)
        {
            int Row = Helper.GetRow(Letter);

            for (int i = Essentials.AvailableFields[Row].Count - 1; i >= 0; i--)
            {
                if (Essentials.AvailableFields[Row][i] != "")
                {
                    Essentials.AvailableFields[Row][i] = "";
                    Essentials.Fields[Row][i] = player.PlayerColor;
                    break;
                }
            }
        }
        public static void Initialize()
        {
            Essentials.Players.Add(new Player { PlayerName = "Spieler 1", PlayerColor = Field.Red });
            Essentials.Players.Add(new Player { PlayerName = "Spieler 2", PlayerColor = Field.Green });
            for(int i = 0; i < 7; i++)
            {
                Essentials.Fields.Add(new List<Field> { Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty, Field.Empty });
            }
            foreach(char letter in Essentials.Letters)
            {
                Essentials.AvailableFields.Add(new List<string> { letter + "1", letter + "2", letter + "3", letter + "4", letter + "5", letter + "6", });
            }
            Essentials.Winner = new Player { PlayerName = "", PlayerColor = Field.Empty };
        }
    }
    public static class Helper
    {
        public static int GetRow(char Letter)
        {
            if (Letter == 'A') return 0;
            else if (Letter == 'B') return 1;
            else if (Letter == 'C') return 2;
            else if (Letter == 'D') return 3;
            else if (Letter == 'E') return 4;
            else if (Letter == 'F') return 5;
            else if (Letter == 'G') return 6;
            else return 0;
        }
        public static void DrawField()
        {
            Console.Clear();
            // Preiltasten Draw
            //Console.ForegroundColor = ConsoleColor.Magenta;
            //Console.Write("   ");
            //foreach(char Cursor in Essentials.CursorPosition)
            //{
            //    Console.Write($"  {Cursor} ");
            //}
            //Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("   ");
            foreach (char letter in Essentials.Letters)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($" {letter} ");
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n-------------------------------");
            for (int x = 0; x < Essentials.Fields[x].Count; x++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($" {x + 1} ");
                for (int i = 0; i < Essentials.Fields.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("|");
                    if (Essentials.Fields[i][x] == Field.Empty)
                    {
                        Console.Write("   ");
                        continue;
                    }
                    else if (Essentials.Fields[i][x] == Field.Green) Console.ForegroundColor = ConsoleColor.Green;
                    else if (Essentials.Fields[i][x] == Field.Red) Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" X ");
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n-------------------------------");
            }
        }
        public static Boolean HasRowSpace(char Letter)
        {
            int Row = Helper.GetRow(Letter);
            foreach (string row in Essentials.AvailableFields[Row])
            {
                if (row == "") continue;
                if (row[0] == Letter) return true;
            }
            return false;
        }
    }
    public static class Essentials
    {
        public static List<Player> Players = new List<Player>();
        public static List<List<Field>> Fields = new List<List<Field>>();
        public static Player Winner;
        public static List<char> Letters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
        public static List<List<string>> AvailableFields = new List<List<string>>();

        // Pfeiltasten Cursor speicher
        //public static List<char> CursorPosition = new List<char> { 'X', ' ', ' ', ' ', ' ', ' ', ' ' };
    }
    public enum Field
    {
        Empty,
        Red,
        Green
    }
    public class Player
    {
        public string PlayerName { get; set; }
        public Field PlayerColor { get; set; }
    }
}
