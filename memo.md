# 第2章
## 2.1

#### Point 《盤面の座標》
* 盤面は横7マス、縦6マスとし、左上を原点とする。
* Pointは構造体とする。
* 座標x,yを外部から書き換えられないように、フィールドではなく読み取り専用のプロパティとして持つ。
* 引数なしコンストラクタは定義しない。

#### Color 《石の色》
* 定数値ではなく、列挙型とする。
* 色の入れ替えは符号反転ではなく、拡張メソッドで行う。
* 画面への表示もここで定義。
  
#### Disc　《石？》
* いらないと思う。
* 石の配置は後述のBoardで管理する。

## 2.3 

Direction列挙型はBoard外にあるべきなので、Utils.csに移動。使わないけど。

## 2.4
Connect4では石を置けるかどうかの判断は簡単。オセロならば例えば以下のようにする。
```csharp
readonly int[] xk = { 0, 0, -1, 1, 1, -1, -1, 1 };
readonly int[] yk = { -1, 1, 0, 0, -1, -1, 1, 1 };
readonly Direction[] dk = {
    Direction.UPPER, Direction.LOWER,
    Direction.LEFT, Direction.RIGHT,
    Direction.UPPER_RIGHT, Direction.UPPER_LEFT,
    Direction.LOWER_LEFT, Direction.LOWER_RIGHT
};
private Direction checkMobility(int px, int py, Stone stone) {
    if (rawBoard[px, py] != Stone.EMPTY) return Direction.NONE;
    int x, y;
    Direction dir = Direction.NONE;

    for (int k = 0; k < 8; k++) {
        if (rawBoard[px + xk[k], py + yk[k]] == stone.Reverse()) {
            x = px + 2 * xk[k];
            y = py + 2 * yk[k];
            while (rawBoard[x, y] == stone.Reverse()) {
                x += xk[k];
                y += yk[k];
            }
            if (rawBoard[x, y] == stone) dir |= dk[k];
        }
    }
    return dir;
}
```

## 2.5
* C#のサイズ可変配列は`List<T>`である。なおC#ではテンプレートとは言わず、ジェネリックという。
* connect4では打てる場所を調べるのが低コストなので、`movablePos`は不要かもしれないが、オセロでは重要なので利用しておく。
* ColorStorageも不要。オセロでもわざわざclassを作らず、Dictionary<Stone,int>に保存すれば良い。
* UpdateLogも`List<Point>`で管理できるが、最後尾への追加と削除しかしないので、より低機能の`Stack<Point>`で管理する。Stackによるデータ管理は、*皿を積み重ねるイメージ*。一番最後に置いたデータしか操作できないし、データ追加も一番上に積むだけである。

## 2.6
* flipDiscsという名前はConnet4には不適切なので、PlaceDiscsに変更。
* 最上段（Y=1）が空なのは確認済み。あとは直下がEmptyじゃなくなるまで、y座標をインクリメントしている。

## 2.7
* 最大ターン以外で、石が置けないってことはない。
* 石が4つ揃う時、必ず最後に置いた石が関与しているので、その周囲だけ調べれば十分。
* ゲームの終了を調べた瞬間に勝敗もわかるので`IsGameOver`ではなく、`checkWinner`という関数にして、勝敗を返すことにする。
  
## 2.8 
* connect4にパスはない

## 2.9 
* connect4にパスはないので、簡単

## 2.11
* CountDiscは不要。
* C#のGetMovablePosではそもそもコピーは発生しない。変更を阻止するためにListではなくIReadOnlyListを返すようにする。
* updateLogが空なのに、GetUpdateが呼ばれることはないはずなので、if文で分岐せず、エラーで落とす。

## 2.12
* Connect4ではPointを文字から作る機会はない。
* 動作確認用のコードは`Program.cs`に書いてみた。


# 第6章?
とりあえず、動かないとプログラミングしにくいので、本の順序を変更し、Playerから実装する。

## 6.1
人間とAIのプレイヤーを抽象化して、インタフェース`IPlayer`を定義する。`IPlayer`は盤面をもとに、次の一手を決定する`OnTurn`メソッドを持つ。
```csharp
interface IPlayer{
    public Action OnTurn(Board board);
}
```
本のC++では`IPlayer`の代わりに`Player`クラスを使っているが、インタフェースの方が適切である。C#の命名規則に則って`I`を先頭につけている。

本ではPlayerが`void OnTurn(Board board)`メソッドにて、ゲーム終了などの分岐処理を例外を使っているが、これは不適切である。例外は予期せぬエラーを扱うものであり、プログラムの制御を変えるものではない。そこで、動作を報告する`Action`列挙型を`Utils.cs`に作り、`OnTurn`の戻り値を`Action`に変更する。

IPlayer実装するクラスは`HumanPlayer`と`AIPlayer`である。`HumanPlayer`はコンソールから入力を受け取る。`AIPlayer`は`AI`クラスをのインスタンを持ち、`AI`の指示に従って手を打つ。

`AI`は抽象クラスとして定義する。本とは異なり、自分の色`MyColor`も保持するようにした。動作確認用のAIとして、打てる場所へランダムに打つAI`RandomAI`と、ほとんどランダムだけど、次手で勝てる時だけ勝ちに行くAI`WeakestAI`をまず作成する。次手の勝利を確認する方法は単純である。とりあえず打ち、勝敗を確認し、勝っていればその手を打つ。勝っていなければ、待ったをして、違う手を打ってみる、というのを全ての手で試す方法である。

```csharp
public override void Move(Board board){
    var pos = board.GetMovablePos();
        foreach(int x in pos){
            board.Move(x);
            if(board.CheckWinner() == MyColor){
                return;
            }
            board.Undo();
        }
    board.Move(pos[random.Next(pos.Count)]);
}
```