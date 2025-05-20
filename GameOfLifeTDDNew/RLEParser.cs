using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeTDDNew
{
    public static class RLEParser
    {

        public static async Task<Game> ReadInRLE(string filePath)
        {
            return new Game(await File.ReadAllLinesAsync(filePath));
        }

    }
}
