namespace Connect4CSharp{
    /// <summary>
    /// 座標を管理する構造体
    /// </summary>
    readonly struct Point{
        public readonly int X;
        public readonly int Y;
        public Point(int x, int y){
            X = x;
            Y = y;
        }
        public override string ToString(){
            return (char)('a' + X - 1) +Y.ToString();
        }
    }

    /// <summary>
    /// 色を表す列挙型
    /// </summary>
    enum Color{Empty, Blue, Yellow, Wall}
    static partial class ColorEnum{
        public static Color Opposite(this Color color){
            return color switch{
                Color.Blue => Color.Yellow,
                Color.Yellow => Color.Blue,
                _ => Color.Empty
            };
        }
        public static string ToConsoleString(this Color color){
            return color switch{
                Color.Blue => "🔵",
                Color.Yellow => "🟡",
                _ => "⬜️"
            };
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