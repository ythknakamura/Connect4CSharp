namespace Connect4CSharp{
    /// <summary>
    /// 盤面の評価を行うインターフェース
    /// </summary>
    interface IEvaluator{
        /// <summary>
        /// 盤面を評価
        /// </summary>
        /// <param name="board">評価対象の盤面</param>
        /// <returns>評価値</returns>
        int Evaluate(Board board);
    }

    /// <summary>
    /// 勝敗のみを評価
    /// </summary>
    class WLDEvaluator: IEvaluator{
        public int Evaluate(Board board){
            Color cc = board.CurrentColor;
            Color winner = board.CheckWinner();
            if(winner==cc){
                return 10000000;
            }
            else if(winner==cc.Opposite()){
                return -10000000;
            }
            else{
                return 0;
            }
        }
    }

    /// <summary>
    /// ４つ並びのパターンをもとに盤面を評価
    /// </summary>
    class PatternEvaluator:IEvaluator{
        private readonly int[] tatePatterns;
        private readonly int[] yokoPatterns;
        private readonly int[] nnmePatterns;
        public PatternEvaluator(){
            //それぞれのパターンの配点
            // 相手の石が入らない並びに点数をつける
            // 1は自分の石、0は空マスを表す
            tatePatterns = new int[16];
            tatePatterns[0b1111] = 10000000;
            tatePatterns[0b0111] = 2000;
            tatePatterns[0b0011] = 50;
            tatePatterns[0b001] =  1;

            yokoPatterns = new int[16];
            yokoPatterns[0b1111] = 10000000;
            yokoPatterns[0b0111] = 1000;
            yokoPatterns[0b1110] = 1000;
            yokoPatterns[0b1011] = 1000;
            yokoPatterns[0b1011] = 1000;
            yokoPatterns[0b0110] = 500;
            yokoPatterns[0b1100] = 100;
            yokoPatterns[0b0011] = 100;
            yokoPatterns[0b1001] = 10;
            yokoPatterns[0b1010] = 10;
            yokoPatterns[0b0101] = 10;
            yokoPatterns[0b0100] = 1;
            yokoPatterns[0b0010] = 1;

            nnmePatterns = new int[16];
            nnmePatterns[0b1111] = 10000000;
            nnmePatterns[0b0111] = 1000;
            nnmePatterns[0b1110] = 1000;
            nnmePatterns[0b1011] = 1000;
            nnmePatterns[0b1011] = 1000;
            nnmePatterns[0b0110] = 500;
            nnmePatterns[0b1100] = 100;
            nnmePatterns[0b0011] = 100;
            nnmePatterns[0b1001] = 10;
            nnmePatterns[0b1010] = 10;
            nnmePatterns[0b0101] = 10;
            nnmePatterns[0b0100] = 1;
            nnmePatterns[0b0010] = 1;
        }

        public int Evaluate(Board board){
            // 色をビットの変換するスイッチ式
            // 相手の石を大きな値にすることで、あとで彈く
            static int b2bit(Color c) => c switch { Color.Blue => 1, Color.Empty => 0, _ => 100 };
            static int y2bit(Color c) => c switch { Color.Yellow => 1, Color.Empty => 0, _ => 100 };

            int b, bEval = 0, yEval = 0;

            //縦のパターン
            for(int x=1; x<=Board.WIDTH; x++){
                for(int y=1; y<=Board.HEIGHT-3; y++){
                    b = 0;
                    b += b2bit(board.GetColor(x,y+0)); b<<=1;
                    b += b2bit(board.GetColor(x,y+1)); b<<=1;
                    b += b2bit(board.GetColor(x,y+2)); b<<=1;
                    b += b2bit(board.GetColor(x,y+3));
                    if(b<16) bEval += tatePatterns[b];

                    b = 0;
                    b += y2bit(board.GetColor(x,y+0)); b<<=1;
                    b += y2bit(board.GetColor(x,y+1)); b<<=1;
                    b += y2bit(board.GetColor(x,y+2)); b<<=1;
                    b += y2bit(board.GetColor(x,y+3));
                    if(b<16) yEval += tatePatterns[b];
                }
            }

            //横のパターン
            for(int x=1; x<=Board.WIDTH-3; x++){
                for(int y=1; y<=Board.HEIGHT; y++){
                    b = 0;
                    b += b2bit(board.GetColor(x+0,y)); b<<=1;
                    b += b2bit(board.GetColor(x+1,y)); b<<=1;
                    b += b2bit(board.GetColor(x+2,y)); b<<=1;
                    b += b2bit(board.GetColor(x+3,y));
                    if(b<16) bEval += yokoPatterns[b];

                    b = 0;
                    b += y2bit(board.GetColor(x+0,y)); b<<=1;
                    b += y2bit(board.GetColor(x+1,y)); b<<=1;
                    b += y2bit(board.GetColor(x+2,y)); b<<=1;
                    b += y2bit(board.GetColor(x+3,y));
                    if(b<16) yEval += yokoPatterns[b];
                }
            }

            //斜めのパターン
            for(int x=1; x<=Board.WIDTH-3; x++){
                for(int y=1; y<=Board.HEIGHT-3; y++){
                    b = 0;
                    b += b2bit(board.GetColor(x+0,y+0)); b<<=1;
                    b += b2bit(board.GetColor(x+1,y+1)); b<<=1;
                    b += b2bit(board.GetColor(x+2,y+2)); b<<=1;
                    b += b2bit(board.GetColor(x+3,y+3));
                    if(b<16) bEval += yokoPatterns[b];

                    b = 0;
                    b += y2bit(board.GetColor(x+0,y+0)); b<<=1;
                    b += y2bit(board.GetColor(x+1,y+1)); b<<=1;
                    b += y2bit(board.GetColor(x+2,y+2)); b<<=1;
                    b += y2bit(board.GetColor(x+3,y+3));
                    if(b<16) yEval += yokoPatterns[b];

                    b = 0;
                    b += b2bit(board.GetColor(x+0,y+3)); b<<=1;
                    b += b2bit(board.GetColor(x+1,y+2)); b<<=1;
                    b += b2bit(board.GetColor(x+2,y+1)); b<<=1;
                    b += b2bit(board.GetColor(x+3,y+0));
                    if(b<16) bEval += yokoPatterns[b];

                    b = 0;
                    b += y2bit(board.GetColor(x+0,y+3)); b<<=1;
                    b += y2bit(board.GetColor(x+1,y+2)); b<<=1;
                    b += y2bit(board.GetColor(x+2,y+1)); b<<=1;
                    b += y2bit(board.GetColor(x+3,y+0));
                    if(b<16) yEval += yokoPatterns[b];
                }
            }
            // 片方を2倍することで、相手の邪魔を優先するようにしている
            if(board.CurrentColor==Color.Blue){
                return 2*bEval-yEval;
            }
            else{
                return 2*yEval-bEval;
            }
        }
    }
}