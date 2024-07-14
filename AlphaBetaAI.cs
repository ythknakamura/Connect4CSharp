namespace Connect4CSharp{
    class AlphaBetaAI : AI{
        public override string Name => "AlphaBetaAI";
        private const int maxDepth = 5;
        private const int infinity = 1<<30;
        public override void Move(Board board){
            int xBest = 0;
            int evalMax = -infinity;
            int alpha = -infinity;
            int beta = infinity;
            ResetEvalCount();
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int eval = MinLevel(board, maxDepth, alpha, beta);
                board.Undo();
                if(evalMax < eval){
                    evalMax = eval;
                    xBest = x;
                }
            }
            board.Move(xBest);
        }

        private int MaxLevel(Board board, int limit, int alpha, int beta){
            if(board.CheckWinner() != Color.Empty || limit == 0){
                return Evaluate(board);
            }
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int score = MinLevel(board, limit-1, alpha, beta);
                board.Undo();
                alpha = Math.Max(alpha, score);
                if(beta<=alpha) return alpha;
            }
            return alpha;
        }
        private int MinLevel(Board board, int limit, int alpha, int beta){
            if(board.CheckWinner() != Color.Empty || limit == 0){
                return -Evaluate(board);
            }
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int score = MaxLevel(board, limit-1, alpha, beta);
                board.Undo();
                beta = Math.Min(beta, score);
                if(beta<=alpha) return beta;
            }
            return beta;
        }

    }
}