namespace Connect4CSharp{
    class NegamaxAI : AI{
        public override string Name => "NegamaxAI";
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
                int eval = -Negamax(board, maxDepth, -beta, -alpha);
                board.Undo();
                if(evalMax < eval){
                    evalMax = eval;
                    xBest = x;
                }
            }
            board.Move(xBest);
        }

        private int Negamax(Board board, int limit, int alpha, int beta){
            if(board.CheckWinner() != Color.Empty || limit == 0){
                return Evaluate(board);
            }
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int score = -Negamax(board, limit-1, -beta, -alpha);
                board.Undo();
                if(score > alpha) alpha = score;
                if(alpha >= beta) return alpha;
            }
            return alpha;
        }
    }
}