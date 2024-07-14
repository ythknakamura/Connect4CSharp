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

    }


    /// <summary>
    /// ランダムに手を選ぶAI
    /// </summary>
    class RandomAI : AI{
        private readonly Random random = new();
        public override string Name => "RandomAI";
        public override void Move(Board board){
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
        private readonly IEvaluator evaluator = new PatternEvaluator();
        public override string Name => "NextMoveAI";
        public override void Move(Board board){
            var xeList = board.GetMovablePos()
                .Select(x =>{
                    board.Move(x);
                    int e = -evaluator.Evaluate(board);
                    board.Undo();
                    return (x, e);});
            int x = xeList.MaxBy(xe=>xe.e).x;
            board.Move(x);
        }
    }
}