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

        public static async Task<string> WriteOutRLE(Game game,string filePath)
        {
            string encoded=game.Encode();

           await File.WriteAllTextAsync(filePath, encoded);

            return encoded;
        }

    }
}
