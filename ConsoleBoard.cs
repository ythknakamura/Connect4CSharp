namespace Connect4CSharp{
    class ConsoleBoard:Board{
        public void Print(){
            Console.WriteLine("  a b c d e f g ");
            for(int y=1; y<=HEIGHT; y++){
                Console.Write($"{y} ");
                for(int x=1; x<=WIDTH; x++){
                    Console.Write(rawBoard[x,y].ToConsoleString());
                }
                Console.WriteLine();
            }
        }
    }
}