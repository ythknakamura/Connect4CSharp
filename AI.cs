namespace Connect4CSharp{

    /// <summary>
    /// AIの基底クラス
    /// </summary>
    abstract class AI{
        /// <summary>
        /// AIが次の手を決定する
        /// </summary>
        /// <param name="board">現在の盤面</param>
        public abstract void Move(Board board);

        /// <summary>
        /// AIの名前
        /// </summary>
        public abstract string Name {get;} 

        /// <summary>
        /// AIからのメッセージ
        /// </summary>
        public string Message => $"評価回数:{evalCount}";

        private IEvaluator evaluator = new PatternEvaluator();

        private int evalCount = 0;
        protected void ResetEvalCount(){
            evalCount = 0;
        }
        protected int Evaluate(Board board){
            evalCount++;
            return evaluator.Evaluate(board);
        }
    }


    /// <summary>
    /// ランダムに手を選ぶAI
    /// </summary>
    class RandomAI : AI{
        private readonly Random random = new(192);
        public override string Name => "RandomAI";
        public override void Move(Board board){
            ResetEvalCount();
            var pos = board.GetMovablePos();
            int x = pos[random.Next(pos.Count)];
            board.Move(x);
        }
    }

    /// <summary>
    /// 基本ランダムだが、次で勝てる時は見逃さないAI
    /// </summary>
    class WeakestAI:AI{
        private readonly Random random = new();
        public override string Name => "WeakestAI";
        public override void Move(Board board){
            ResetEvalCount();
            Color myColor = board.CurrentColor;
            var pos = board.GetMovablePos();
            foreach(int x in pos){
                board.Move(x);
                if(board.CheckWinner() == myColor){
                    return;
                }
                board.Undo();
            }
            board.Move(pos[random.Next(pos.Count)]);
        }
    }

    /// <summary>
    /// 次の手で最も評価値が高くなる手を選ぶAI
    /// </summary>
    class NextMoveAI:AI{
        public override string Name => "NextMoveAI";
        public override void Move(Board board){
            ResetEvalCount();
            var xeList = board.GetMovablePos()
                .Select(x =>{
                    board.Move(x);
                    int e = -Evaluate(board);
                    board.Undo();
                    return (x, e);});
            int x = xeList.MaxBy(xe=>xe.e).x;
            board.Move(x);
        }
    }
}