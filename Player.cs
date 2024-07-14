namespace Connect4CSharp{
    interface IPlayer{
        public Action OnTurn(Board board);
    }

    class AIPlayer :IPlayer{
        private readonly AI ai;
        public AIPlayer(AI ai){
            this.ai = ai;
        }
        public Action OnTurn(Board board){
            Console.WriteLine("AIの思考中...");
            ai.Move(board);
            return Action.MOVE;
        }
    }
    class HumanPlayer : IPlayer{
        private IEvaluator evaluator;
        public HumanPlayer(){
            evaluator = new PatternEvaluator();
        }
        
        public Action OnTurn(Board board){
            while(true){
                //次の評価値を表示
                var xeList = board.GetMovablePos()
                    .Select(x =>{
                        board.Move(x);
                        int e = -evaluator.Evaluate(board);
                        board.Undo();
                        return (x, e);})
                    .OrderByDescending(xe=>xe.e);
                foreach(var (x, e) in xeList){
                    Console.WriteLine($"{(char)('a'+x-1)} : {e}");
                }
                
                // 入力受け付け
                Console.WriteLine("どこに置く？ ( U:待った  Q:終了 )");
                char input = Console.ReadKey().KeyChar;
                if(input == 'Q'){
                    return Action.QUIT;
                }
                else if(input == 'U'){
                    bool ok = board.Undo();
                    if(ok){
                        return Action.UNDO;
                    }
                    else{
                        Console.WriteLine("   *** 戻せない！");
                        continue;
                    }
                }
                else if(char.IsUpper(input)){
                    Console.WriteLine("   *** 小文字で入力を！");
                    continue;
                }
                else{
                    int x = input - 'a' + 1;
                    bool ok = board.Move(x);
                    if(ok){
                        return Action.MOVE;
                    }
                    if(!ok){
                        Console.WriteLine("   *** そこには置けません");
                        continue;
                    }
                }
            }
        }
    }
}