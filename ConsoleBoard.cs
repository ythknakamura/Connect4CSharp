namespace Connect4CSharp{
    class ConsoleBoard : Board{
        /// <summary>
        /// コンソール上に盤面を表示
        /// </summary>
        public void Print(){
            Console.Write("  ");
            for(int x = 0; x<Board.WIDTH; x++){
                Console.Write($"{(char)('a'+x)} ");
            }
            Console.WriteLine();
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