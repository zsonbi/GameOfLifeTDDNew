using System.Threading.Tasks;

namespace GameOfLifeTDDNew
{
    public class Program
    {
        static async Task Main(string[] args)
        {
           await RunSim(args);
        }

        public static async Task RunSim(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Invalid argument count please give it like this: file.rle generationcount");
            }

            string fileName = args[0];
            int generations;
            if (!int.TryParse(args[1], out generations))
            {
                Console.Error.WriteLine("Wrong generation format give number");
            }

            Game game=await RLEParser.ReadInRLE(fileName);
            game.Run(generations);

            Console.WriteLine(await RLEParser.WriteOutRLE(game,"out.rle"));
        }
    }
}
