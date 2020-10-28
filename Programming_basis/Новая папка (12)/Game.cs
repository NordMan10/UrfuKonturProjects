using System.Windows.Forms;

namespace Digger
{
    public static class Game
    {
        private const string testMap = @"
TTTTT
TP  T
TTTTT
   M 
TTTTT";

        private const string mapWithPlayerTerrain = @"
TTT T
TTP T
T T T
TT TT";

        private const string mapWithPlayerTerrainSackGold = @"
PTTGTT TSTS
TST  TSTTTS
TTTTTTSTTTG
T TSTS TTTG
T TTTG STTT
TTGTTT TTTT
TTTGTTTTGTT";

        private const string mapWithPlayerTerrainSackGoldMonster = @"
PTTGTT TSTTTT
TST  T TTMTTT
TTT TT TTTTTT
T TST  TTTTTT
T TTTGM TSTTT
T TMT M TSTTT
TSTSTTMTTTTTT
S TTST  TGTTT
 TGST MTTTTTT
 T  TMTTTTTTT
TTTTTTTT    T";

        public static ICreature[,] Map;
        public static int Scores;
        public static bool IsOver;

        public static Keys KeyPressed;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap()
        {
            Map = CreatureMapCreator.CreateMap(mapWithPlayerTerrainSackGoldMonster);
        }
    }
}