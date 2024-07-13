using System.Diagnostics;

namespace Connect4CSharp{
    /// <summary>
    /// 盤面を管理するクラス
    /// </summary>
    class Board{
        public const int WIDTH = 7;
        public const int HEIGHT = 6;
        private const int MAX_TURNS = WIDTH * HEIGHT;

        /// <summary>
        /// 現在の手数
        /// </summary>
        public int Turns{get; private set;}
        
        /// <summary>
        /// 現在の手番の色
        /// </summary>
        public Color CurrentColor {get; private set;}
        
        /// <summary>
        /// 盤面の状態を保持する配列
        /// </summary>
        protected Color[,] rawBoard = new Color[WIDTH+2, HEIGHT+2];
        
        /// <summary>
        /// おける場所のx座標の配列。効率化のために手数ごと全てを保持する。
        /// </summary>
        private List<int>[] movablePos = new List<int>[MAX_TURNS+1];
        
        /// <summary>
        /// 石のおいた場所を保持するスタック
        /// </summary>
        private Stack<Point> updateLog = new Stack<Point>();

        public Board(){
            for(int t=0; t<movablePos.Length; t++){
                movablePos[t] = new List<int>();
            }
            Init();
        }

        /// <summary>
        /// 盤面の初期化
        /// </summary>
        public void Init(){
            for(int x=1; x<=WIDTH; x++){
                for(int y=1; y<= HEIGHT; y++){
                    rawBoard[x,y] = Color.Empty;
                }
            }
            //四辺に番兵を配置
            for(int y=0; y<HEIGHT+2; y++){
                rawBoard[0,y] = Color.Wall;
                rawBoard[WIDTH+1,y] = Color.Wall;
            }
            for(int x=0; x<WIDTH+2; x++){
                rawBoard[x,0] = Color.Wall;
                rawBoard[x,HEIGHT+1] = Color.Wall;
            }

            Turns = 0;
            CurrentColor = Color.Blue;
            updateLog.Clear();
            InitMovable();
        }

        /// <summary>
        /// 指定した列に石を置く。手番も交代して、手数も進める。
        /// </summary>
        /// <param name="x">石を置くx座標</param>
        /// <returns>配置に成功したかどうか</returns>
        public bool Move(int x){
            if(x<1 || x>WIDTH) return false;
            if(!movablePos[Turns].Contains(x))return false;
            PlaceDiscs(x);
            Turns++;
            CurrentColor = CurrentColor.Opposite();
            InitMovable();
            return true;
        }
        
        /// <summary>
        /// 待った
        /// </summary>
        /// <returns>待ったができたかどうか</returns>
        public bool Undo(){
            if(Turns == 0) return false;
            Turns--;
            CurrentColor = CurrentColor.Opposite();
            Point lastP = updateLog.Pop();
            rawBoard[lastP.X, lastP.Y] = Color.Empty;
            return true;
        }

        /// <summary>
        /// 勝敗チェック
        /// </summary>
        /// <returns>勝者のColor。ただしColor.Emptyはゲーム継続中、Color.Wallは引き分けを表す</returns>
        public Color CheckWinner(){
            if(updateLog.Count==0) return Color.Empty;//継続中
        
            // ４つ揃っていたら終了(最後に置かれた石の周囲だけ調べれば良い)
            var lastP = updateLog.Peek();
            Color lastC = rawBoard[lastP.X, lastP.Y];
            (int,int)[] diff  = { (0,1), (1,0), (1,1), (1,-1)};
            foreach(var (dx, dy) in diff){
                int renzoku = 1;
                for(int k=1; k<=3; k++){
                    if(rawBoard[lastP.X+dx*k,lastP.Y+dy*k] == lastC){
                        renzoku++;
                    }
                    else break;
                }
                for(int k=1; k<=3; k++){
                    if(rawBoard[lastP.X-dx*k,lastP.Y-dy*k] == lastC){
                        renzoku++;
                    }
                    else break;
                }
                if(renzoku >= 4) return lastC;
            }
            if(Turns == MAX_TURNS) return Color.Wall;//引き分け
            else return Color.Empty; //継続中
        }

        /// <summary>
        /// 指定した座標の色を取得
        /// </summary>
        /// <param name="point">座標</param>
        public Color GetColor(Point point){
            return rawBoard[point.X, point.Y];;
        }

        /// <summary>
        /// おける場所のリスト
        /// </summary>
        public IReadOnlyList<int> GetMovablePos(){
            return movablePos[Turns];
        }
        
        /// <summary>
        /// 最後に置かれた石の座標
        /// </summary>
        public Point GetUpdate(){
            return updateLog.Peek();
        }

        /// <summary>
        /// 指定した列に石を置けるかどうか
        /// </summary>
        /// <param name="x"></param>
        private bool CheckMobility(int x){
            return rawBoard[x,1] == Color.Empty;
        }

        /// <summary>
        /// movablePosの更新
        /// </summary>
        private void InitMovable(){
            movablePos[Turns].Clear();
            for(int x=1; x<=WIDTH; x++){
                if(CheckMobility(x)){
                    movablePos[Turns].Add(x);
                }
            }
        }

        /// <summary>
        /// 指定した列に石を置き、rawBoardを更新
        /// </summary>
        /// <param name="x">石を置くx座標。置けることを事前に確認する必要がある</param>
        private void PlaceDiscs(int x){
            int y = 1;
            Debug.Assert(rawBoard[x,y]==Color.Empty);
            while(rawBoard[x,y+1] == Color.Empty)y++;
            updateLog.Push(new Point(x,y));
            rawBoard[x,y] = CurrentColor;
        }
    }
}