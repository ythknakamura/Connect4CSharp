namespace Connect4CSharp{
    class Board{
        public const int WIDTH = 7;
        public const int HEIGHT = 6;
        public const int MAX_TURNS = WIDTH * HEIGHT;

        public int Turns{get; private set;}
        public Color CurrentColor {get; private set;}
        protected Color[,] rawBoard = new Color[WIDTH+2, HEIGHT+2];
        private List<int>[] movablePos = new List<int>[MAX_TURNS+1];
        private Stack<Point> updateLog = new Stack<Point>();

        public Board(){
            for(int t=0; t<movablePos.Length; t++){
                movablePos[t] = new List<int>();
            }
            Init();
        }
        public void Init(){
            for(int x=1; x<=WIDTH; x++){
                for(int y=1; y<= HEIGHT; y++){
                    rawBoard[x,y] = Color.Empty;
                }
            }
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

        public bool Move(int x){
            if(x<1 || x>WIDTH) return false;
            if(!movablePos[Turns].Contains(x))return false;
            PlaceDiscs(x);
            Turns++;
            CurrentColor = CurrentColor.Opposite();
            InitMovable();
            return true;
        }
        
        public bool Undo(){
            if(Turns == 0) return false;
            Turns--;
            CurrentColor = CurrentColor.Opposite();
            Point lastP = updateLog.Pop();
            rawBoard[lastP.X, lastP.Y] = Color.Empty;
            return true;
        }

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

        public Color GetColor(Point point){
            return rawBoard[point.X, point.Y];;
        }
        public IReadOnlyList<int> GetMovablePos(){
            return movablePos[Turns];
        }
        public Point GetUpdate(){
            return updateLog.Peek();
        }
        private bool CheckMobility(int x){
            return rawBoard[x,1] == Color.Empty;
        }
        private void InitMovable(){
            movablePos[Turns].Clear();
            for(int x=1; x<=WIDTH; x++){
                if(CheckMobility(x)){
                    movablePos[Turns].Add(x);
                }
            }
        }
        private void PlaceDiscs(int x){
            int y = 1;
            while(rawBoard[x,y+1] == Color.Empty)y++;
            updateLog.Push(new Point(x,y));
            rawBoard[x,y] = CurrentColor;
        }
    }
}