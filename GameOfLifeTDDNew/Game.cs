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
            return matrix[row, col];
        }

        public Game(string[] fileContent)
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
                        int cellsToPlace = num =="" ? 1 : Convert.ToInt32(num);
                        for (int j = 0; j < cellsToPlace; j++)
                        {
                            matrix[i, colIndex++] = item == 'o';
                        }
                        num = "";
                    }
                }
            }
        }
    }
}
