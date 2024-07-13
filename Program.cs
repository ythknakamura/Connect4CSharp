using System.Collections.Concurrent;

namespace Connect4CSharp{
    class Program{
        static void Main(){
            TestPlay();
        }

        static void TestPlay(){
            ConsoleBoard board = new ConsoleBoard();
            string message = "";
            while(true){
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