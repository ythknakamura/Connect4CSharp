using System.Diagnostics;

namespace Connect4CSharp{
    class TestAI{

        public void Test(){
            var ais = new AI[] {
                new MinmaxAI(),
                new AlphaBetaAI(),
                new NegaAlphaAI(),
                new NegaAlphaAI2(),
            };
            var xAndMessages = ais.Select(TestPlay).ToList();
            for(int i=1; i<ais.Length; i++){
                Debug.Assert(xAndMessages[0].Count == xAndMessages[i].Count, i.ToString());
            }
            Console.Write("#  ");
            foreach(var ai in ais) Console.Write($" {ai.Name,-16}");
            Console.WriteLine();
            for(int t=0; t<xAndMessages[0].Count; t++){
                for(int i=1; i<ais.Length; i++){
                    Debug.Assert(xAndMessages[0][t].Item1 == xAndMessages[i][t].Item1, i.ToString());
                }
                Console.Write($"{t, 2}:");
                for(int i=0; i<ais.Length; i++){
                    Console.Write($" {xAndMessages[i][t].Item2,-12}");
                }
                Console.WriteLine();
            }
        }
        private List<(int,string)> TestPlay(AI ai){
            ConsoleBoard board = new ConsoleBoard();
            IPlayer[] players = new IPlayer[2];
            players[0] = new AIPlayer(new NextMoveAI());
            players[1] = new AIPlayer(ai);
            var xAndMessages = new List<(int,string)>();
            int currentPlayerIdx = 1;
            while(board.CheckWinner() == Color.Empty){
                players[currentPlayerIdx].OnTurn(board);
                if(currentPlayerIdx==1)xAndMessages.Add((board.GetUpdate().X, players[currentPlayerIdx].Message));
                currentPlayerIdx = 1-currentPlayerIdx;
            }
            return xAndMessages;
        }

    }
}