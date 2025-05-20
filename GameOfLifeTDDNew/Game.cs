using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeTDDNew
{
    public class Game
    {
        private bool[,] matrix;

        public int BoardWidth { get; private set; }
        public int BoardHeight { get; private set; }

        public List<int> SurvivalAmounts { get; private set; } = new List<int>();
        public List<int> BirthAmounts { get; private set; } = new List<int>();


        public bool GetCell(int row, int col)
        {
            if (row < 0 || row > BoardHeight || col < 0 || col > BoardWidth)
            {
                throw new ArgumentOutOfRangeException("Out of the board with this index combination");
            }

            return matrix[row, col];
        }

        public Game(string[] fileContent)
        {
            try
            {


                int index = 0;
                while (index < fileContent.Length && fileContent[index].StartsWith("#"))
                {
                    index++;
                }
                string[] firstLine = fileContent[index++].Split("=");
                BoardWidth = int.Parse(firstLine[1].Split(',')[0]);
                BoardHeight = int.Parse(firstLine[2].Split(',')[0]);

                foreach (char number in firstLine[3].Split('/')[0].Trim('B'))
                {
                    BirthAmounts.Add(number - '0');
                }

                foreach (char number in firstLine[3].Split('/')[1].Trim('S'))
                {
                    SurvivalAmounts.Add(number - '0');
                }

                matrix = new bool[BoardHeight, BoardWidth];

                string board = "";
                for (int i = index; i < fileContent.Length; i++)
                {
                    board += fileContent[i];
                }
                if (!board.EndsWith('!'))
                {
                    throw new ArgumentException("Didn't close with '!'");
                }
                string[] rows = board.Trim('!').Split('$');
                string num = "";
                for (int i = 0; i < rows.Length; i++)
                {
                    int colIndex = 0;

                    foreach (char item in rows[i])
                    {
                        if (item >= '0' && item <= '9')
                        {
                            num += item;
                        }
                        else
                        {
                            int cellsToPlace = num == "" ? 1 : Convert.ToInt32(num);
                            for (int j = 0; j < cellsToPlace; j++)
                            {
                                matrix[i, colIndex++] = item == 'o';
                            }
                            num = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("RLE is invalid format");
            }
        }

        public string Encode()
        {
            string encoded = $"x = {BoardWidth}, y = {BoardHeight}, rule = B{String.Concat(BirthAmounts)}/S{String.Concat(SurvivalAmounts)}\n";

            for (int i = 0; i < BoardHeight; i++)
            {
                int num = 1;
                bool type = matrix[i, 0];
                for (int j = 1; j < BoardWidth; j++)
                {
                    if (type != matrix[i, j])
                    {
                        encoded += (num == 1 ? "" : num) + (type ? "o" : "b");
                        num = 1;
                        type = matrix[i, j];
                    }
                }
                encoded += (num == 1 ? "" : num) + (type ? "o" : "b");

                encoded += "$";
            }

            encoded += "!";
            return encoded;
        }

        public void Run(int generationCount)
        {
            for (int i = 0; i < generationCount; i++)
            {
                Tick();
            }
        }

        public void Tick()
        {
            bool[,] newBoardState = new bool[BoardHeight, BoardWidth];

            for (int i = 0; i < BoardHeight; i++)
            {
                for (int j = 0; j < BoardWidth; j++)
                {
                    int neighbourCount = GetNeighbourCount(i, j);

                    if (matrix[i, j])
                    {
                        newBoardState[i, j] = SurvivalAmounts.Contains(neighbourCount);
                    }
                    else
                    {
                        newBoardState[i, j] = BirthAmounts.Contains(neighbourCount);
                    }
                }
            }

            matrix = newBoardState;
        }

        public int GetNeighbourCount(int row, int col)
        {
            int count = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                if (i < 0 || i >= BoardHeight)
                {
                    continue;
                }
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (j < 0 || j >= BoardWidth || (row == i && col == j))
                    {
                        continue;
                    }
                    if (matrix[i, j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

    }

}
