namespace Connect4CSharp{
    abstract class AI{
        public abstract void Move(Board board);
        protected Color MyColor{get; init;}
        protected AI(Color myColor){
            MyColor = myColor;
        }
    }

    class RandomAI : AI{
        private readonly Random random;
        public RandomAI(Color myColor):base(myColor){
            random = new Random();
        }
        public override void Move(Board board){
            var pos = board.GetMovablePos();
            int x = pos[random.Next(pos.Count)];
            board.Move(x);
        }
    }

    class WeakestAI:AI{
        private readonly Random random;
        public WeakestAI(Color myColor):base(myColor){
            random = new Random();
        }
        public override void Move(Board board){
            var pos = board.GetMovablePos();
            foreach(int x in pos){
                board.Move(x);
                if(board.CheckWinner() == MyColor){
                    return;
                }
                board.Undo();
            }
            board.Move(pos[random.Next(pos.Count)]);
        }
    }
}