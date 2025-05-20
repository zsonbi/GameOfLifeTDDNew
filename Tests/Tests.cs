using System.Threading.Tasks;
using GameOfLifeTDDNew;

namespace Tests
{
    //Tests to write
    //TestProgramForInputsTest
    //SingleGenerationTest
    //MultipleGenerationTest
    //TryOutOfBoundsGetCellTest
  

    public class Tests
    {

        [Fact]
        public async Task ParseBlinkerRLETest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "blinker.rle"));
            Assert.NotNull(game);

            Assert.Equal(3, game.BoardWidth);
            Assert.Equal(1, game.BoardHeight);
            Assert.Contains(2, game.SurvivalAmounts);
            Assert.Contains(3, game.SurvivalAmounts);
            Assert.Contains(3, game.BirthAmounts);
            for (int i = 0; i < 3; i++)
            {
                Assert.True(game.GetCell(0, i));
            }

        }

        [Fact]
        public async Task ParseBlockRLETest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "block.rle"));
            Assert.NotNull(game);

            Assert.Equal(2, game.BoardWidth);
            Assert.Equal(2, game.BoardHeight);
            Assert.Contains(2, game.SurvivalAmounts);
            Assert.Contains(3, game.SurvivalAmounts);
            Assert.Contains(3, game.BirthAmounts);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.True(game.GetCell(i, j));
                }
            }
        }

        [Fact]
        public async Task ParseGliderRLETest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "glider.rle"));
            Assert.NotNull(game);

            Assert.Equal(3, game.BoardWidth);
            Assert.Equal(3, game.BoardHeight);
            Assert.Contains(2, game.SurvivalAmounts);
            Assert.Contains(3, game.SurvivalAmounts);
            Assert.Contains(3, game.BirthAmounts);

            //Assert the cell states
            Assert.False(game.GetCell(0, 0));
            Assert.True(game.GetCell(0, 1));
            Assert.False(game.GetCell(0, 2));

            Assert.False(game.GetCell(1, 0));
            Assert.False(game.GetCell(1, 1));
            Assert.True(game.GetCell(1, 2));

            Assert.True(game.GetCell(2, 0));
            Assert.True(game.GetCell(2, 1));
            Assert.True(game.GetCell(2, 2));

        }

        [Fact]
        public async Task ParseGosperGliderGunTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "gosperglidergun.rle"));
            Assert.NotNull(game);

            Assert.Equal(36, game.BoardWidth);
            Assert.Equal(9, game.BoardHeight);
            Assert.Contains(2, game.SurvivalAmounts);
            Assert.Contains(3, game.SurvivalAmounts);
            Assert.Contains(3, game.BirthAmounts);


            //Will only tast the first and last column since the size of the shape
            Assert.False(game.GetCell(0, 0));
            Assert.False(game.GetCell(0, game.BoardWidth - 1));

            Assert.False(game.GetCell(1, 0));
            Assert.False(game.GetCell(1, game.BoardWidth - 1));

            Assert.False(game.GetCell(2, 0));
            Assert.True(game.GetCell(2, game.BoardWidth - 1));

            Assert.False(game.GetCell(3, 0));
            Assert.True(game.GetCell(3, game.BoardWidth - 1));

            Assert.True(game.GetCell(4, 0));
            Assert.False(game.GetCell(4, game.BoardWidth - 1));

            Assert.True(game.GetCell(5, 0));
            Assert.False(game.GetCell(5, game.BoardWidth - 1));

            Assert.False(game.GetCell(6, 0));
            Assert.False(game.GetCell(6, game.BoardWidth - 1));

            Assert.False(game.GetCell(7, 0));
            Assert.False(game.GetCell(7, game.BoardWidth - 1));

            Assert.False(game.GetCell(8, 0));
            Assert.False(game.GetCell(8, game.BoardWidth - 1));

        }


        [Fact]
        public async Task ParseNotExistingFileThrowsErrorTest()
        {
            await Assert.ThrowsAsync<FileNotFoundException>(() => RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "NoSuchFile.rle")));
        }

        [Theory]
        [InlineData("invalid1.rle")]
        [InlineData("invalid2.rle")]
        [InlineData("invalid3.rle")]
        [InlineData("invalid4.rle")]
        public async Task TryParseInvalidRLETest(string filePath)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, filePath)));
        }

        [Fact]
        public async Task TryOutOfBoundsGetCellTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "blinker.rle"));
            Assert.Throws<ArgumentOutOfRangeException>(() => game.GetCell(0,5));
        }

        [Theory]
        [InlineData("blinker.rle")]
        [InlineData("block.rle")]
        [InlineData("glider.rle")]
        [InlineData("gosperglidergun.rle")]
        public async Task WriteOutRLETest(string filePath)
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, filePath));

            //ToMakeItUnique
            string fileId=DateTime.Now.Ticks.ToString();


            await RLEParser.WriteOutRLE(game, fileId + "Result.rle");

            Assert.True(File.Exists(fileId + "Result.rle"));

            Game testBlinkerGame = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, filePath));
            Game writtenOutGame = await RLEParser.ReadInRLE(fileId + "Result.rle");
        }

        [Fact]
        public async Task AloneCellTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "aloneCell.rle"));
            Assert.False(game.GetCell(0, 0));
            Assert.True(game.GetCell(0, 1));
            Assert.False(game.GetCell(0, 2));
            game.Tick();
            for (int i = 0; i < 3; i++)
            {
                Assert.False(game.GetCell(0, i));
            }
        }
        [Fact]
        public async Task TwoCellsNextToEachOtherTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "twoCellsNextToEachOther.rle"));
            Assert.True(game.GetCell(0, 0));
            Assert.True(game.GetCell(0, 1));
            Assert.False(game.GetCell(0, 2));
            game.Tick();
            for (int i = 0; i < 3; i++)
            {
                Assert.False(game.GetCell(0, i));
            }
        }

        [Fact]
        public async Task BirthTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "birthTestFile.rle"));
            Assert.True(game.GetCell(0, 0));
            Assert.True(game.GetCell(0, 1));
            Assert.True(game.GetCell(1, 0));
            Assert.False(game.GetCell(1, 1));
            game.Tick();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.True(game.GetCell(i, j));
                }
            }
        }


        [Fact]
        public async Task SurvivalTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "survivalTestFile.rle"));
            Assert.True(game.GetCell(0, 0));
            Assert.True(game.GetCell(0, 1));
            Assert.True(game.GetCell(0, 2));
            Assert.True(game.GetCell(1, 1));
            Assert.False(game.GetCell(1,0));
            Assert.False(game.GetCell(1,2));
            Assert.False(game.GetCell(2,0));
            Assert.False(game.GetCell(2,1));
            Assert.False(game.GetCell(2, 2));

            game.Tick();
            //After tick
            Assert.True(game.GetCell(0, 0));
            Assert.True(game.GetCell(0, 1));
            Assert.True(game.GetCell(0, 2));
            Assert.True(game.GetCell(1, 1));
            Assert.True(game.GetCell(1, 0));
            Assert.True(game.GetCell(1, 2));
            Assert.False(game.GetCell(2, 0));
            Assert.False(game.GetCell(2, 1));
            Assert.False(game.GetCell(2, 2));
        }

        [Fact]
        public async Task NeighbourCountTest()
        {
            Game game = await RLEParser.ReadInRLE(Path.Combine(Configs.INPUT_FILES, "glider.rle"));

            Assert.Equal(5, game.GetNeighbourCount(1,1));
        }

    }
}