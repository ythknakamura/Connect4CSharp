namespace Connect4CSharp{

    /// <summary>
    /// 座標を管理する構造体
    /// </summary>
    struct Point{
        public int X{get; private set;}
        public int Y{get; private set;}
        public Point(int x, int y){
            X = x;
            Y = y;
        }

        public override string ToString(){
            string str = "";
            str += (char)('a' + X - 1);
            str += (char)('1' + Y - 1);
            return str;
        }
    }

    /// <summary>
    /// 色を表す列挙型
    /// </summary>
    enum Color{Empty, Blue, Yellow, Wall}
    static partial class ColorEnum{
        public static Color Opposite(this Color color){
            switch(color){
                case Color.Blue:   return Color.Yellow;
                case Color.Yellow: return Color.Blue;
                default:           return color;
            }
        }
        public static string ToConsoleString(this Color color){
            switch(color){
                case Color.Blue:   return "🔵";
                case Color.Yellow: return "🟡";
                default:           return "⬜️";
            }
        }
    }

    /// <summary>
    /// Playerの行動を表す列挙型
    /// </summary>
    enum Action{MOVE, UNDO, QUIT}

    [Flags]
    enum Direction {
        NONE = 0,
        UPPER = 1 << 0,
        UPPER_LEFT = 1 << 1,
        LEFT = 1 << 2,
        LOWER_LEFT = 1 << 3,
        LOWER = 1 << 4,
        LOWER_RIGHT = 1 << 5,
        RIGHT = 1 << 6,
        UPPER_RIGHT = 1 << 7
    }
}