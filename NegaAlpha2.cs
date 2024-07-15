namespace Connect4CSharp{
    class NegaAlphaAI2 : AI{
        public override string Name => "NegaAlphaAI2";
        private const int maxDepth = 7;
        private const int infinity = 1<<30;
        public override void Move(Board board){
            int xBest = 0;
            int alpha = -infinity;
            int beta = infinity;
            ResetEvalCount();
            var pos = board.GetMovablePos()
                .Select(x => {
                    board.Move(x);
                    int eval = -Evaluate(board);
                    board.Undo();
                    return (x,eval);})
                .OrderByDescending(xe => xe.eval)
                .Select(xe => xe.x);
            foreach(int x in pos){
                board.Move(x);
                int eval = -NegaAlpha(board, maxDepth, -beta, -alpha);
                board.Undo();
                if(alpha < eval){
                    alpha = eval;
                    xBest = x;
                }
            }
            board.Move(xBest);
        }

        private int NegaAlpha(Board board, int limit, int alpha, int beta){
            if(board.CheckWinner() != Color.Empty || limit == 0){
                return Evaluate(board);
            }
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int score = -NegaAlpha(board, limit-1, -beta, -alpha);
                board.Undo();
                if(score > alpha) alpha = score;
                if(alpha >= beta) return alpha;
            }
            return alpha;
        }
    }
}