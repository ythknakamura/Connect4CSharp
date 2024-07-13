using System.Diagnostics;

namespace Connect4CSharp{
    class Program{
        static void Main(){
            Debug.Assert(Board.WIDTH<=26, "盤面の幅が大きすぎる");
            Debug.Assert(Board.HEIGHT<=9, "盤面の高さが大きすぎる");
            //TestPlay();
            Play();
        }

        static void Play(){
            ConsoleBoard board = new ConsoleBoard();
            IPlayer[] players = new IPlayer[2];
            players[0] = new AIPlayer(new RandomAI(Color.Blue));
            players[1] = new AIPlayer(new WeakestAI(Color.Yellow));
            while(true){
                Console.Clear();
                int currentPlayerIdx = board.CurrentColor == Color.Blue ? 0 : 1;

                // 最後の一手を表示
                if(board.Turns!=0){
                    Point lastP = board.GetUpdate();
                    Console.WriteLine($"前回の手 : {lastP}");
                }

                // 盤面を表示して、勝敗判定。
                Console.WriteLine();
                board.Print();
                Console.WriteLine();
                switch(board.CheckWinner()){
                    case Color.Blue:
                        Console.WriteLine("🔵の勝ち！");
                        return;
                    case Color.Yellow:
                        Console.WriteLine("🟡の勝ち！");
                        return;
                    case Color.Wall:
                        Console.WriteLine("引き分け！");
                        return;
                }
                
                // 次の一手を受け付け
                Console.WriteLine($"第{board.Turns+1}手 : {board.CurrentColor.ToConsoleString()}の手番");
                
                Action action = players[currentPlayerIdx].OnTurn(board);
                if(action == Action.QUIT){
                    Console.WriteLine("終了");
                    return;
                }
                else if(action == Action.UNDO){
                    //次の相手が人間ではないなら、待ったは2連続で行う。
                    if(typeof(HumanPlayer).IsInstanceOfType(players[board.CurrentColor == Color.Blue ? 0 : 1])){
                        board.Undo();
                    }
                    continue;
                }
            }
        }

        /// <summary>
        /// 全てが手動のテストプレイ
        /// </summary>
        static void TestPlay(){
            ConsoleBoard board = new ConsoleBoard();
            string message = "";

            while(true){
                // 盤面を表示して、勝敗判定。直前のエラーメッセージも表示
                Console.Clear();
                if(message != ""){
                    Console.WriteLine($"### {message}");
                    message = "";
                }
                Console.WriteLine();
                board.Print();
                Console.WriteLine();
                switch(board.CheckWinner()){
                    case Color.Blue:
                        Console.WriteLine("🔵の勝ち！");
                        return;
                    case Color.Yellow:
                        Console.WriteLine("🟡の勝ち！");
                        return;
                    case Color.Wall:
                        Console.WriteLine("引き分け！");
                        return;
                }
                
                // 次の一手を受け付け
                Console.WriteLine($"第{board.Turns+1}手 : {board.CurrentColor.ToConsoleString()}の手番");
                Console.WriteLine("どこに置く？ ( U:待った  Q:終了 )");
                
                char input = Console.ReadKey().KeyChar;
                if(input == 'Q'){
                    Console.WriteLine("終了");
                    return;
                }
                else if(input == 'U'){
                    bool ok = board.Undo();
                    if(!ok){
                        message = "戻せない！";
                    }
                }
                else if(char.IsUpper(input)){
                    message = "小文字で入力を！";
                }
                else{
                    int x = input - 'a' + 1;
                    bool ok = board.Move(x);
                    if(!ok){
                        message = "そこには置けません";
                    }
                }
            }
        }
    }
}