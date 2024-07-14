namespace Connect4CSharp{
    class MinmaxAI : AI{
        public override string Name => "MinmaxAI";
        private const int maxDepth = 5;
        private const int infinity = 1<<30;
        public override void Move(Board board){
            int xBest = 0;
            int evalMax = -infinity;
            ResetEvalCount();
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int eval = MinLevel(board, maxDepth);
                board.Undo();
                if(evalMax < eval){
                    evalMax = eval;
                    xBest = x;
                }
            }
            board.Move(xBest);
        }

        private int MaxLevel(Board board, int limit){
            if(board.CheckWinner() != Color.Empty || limit == 0){
                return Evaluate(board);
            }
            int scoreMax = -infinity;
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int score = MinLevel(board, limit-1);
                board.Undo();
                if(scoreMax < score) scoreMax = score;
            }
            return scoreMax;
        }
        private int MinLevel(Board board, int limit){
            if(board.CheckWinner() != Color.Empty || limit == 0){
                return -Evaluate(board);
            }
            int scoreMin = infinity;
            foreach(int x in board.GetMovablePos()){
                board.Move(x);
                int score = MaxLevel(board, limit-1);
                board.Undo();
                if(scoreMin > score) scoreMin = score;
            }
            return scoreMin;
        }
    }
}