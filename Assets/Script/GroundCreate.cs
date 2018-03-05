using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCreate : MonoBehaviour {

    public int width = 61;
    public int height = 61;
    public int level = 3;
    public int floorHeight = 10;

    private int[,] maze;
    private List<Cell> StartCells;
    private Stack<Cell> CurrentWallCells;
    private Random random;
    private GameObject cube;
    private GameObject upDownCube;

    // Use this for initialization
    void Start () {

        if (width % 2 == 0) { ++width; }
        if (height % 2 == 0) { ++width; }

        maze = new int[width, height];
        StartCells = new List<Cell>();
        CurrentWallCells = new Stack<Cell>();
        random = new Random();
        cube = (GameObject)Resources.Load("Cube");
        upDownCube = (GameObject)Resources.Load("UpDownCube");


        for (var k = 0; k < level; k++)
        {
            CreateMaze();
            maze[1, 1] = 1;
            maze[width - 2, height - 2] = 1;
             

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (maze[i, j] == 0)
                    {
                        Instantiate(cube, new Vector3(i, floorHeight * k, j), Quaternion.identity);
                    }
                }
            }
            if(k % 2 == 0)
            {
                GameObject obj = Instantiate(upDownCube, new Vector3(width - 2, floorHeight * k, height - 2), Quaternion.identity) as GameObject;
                CubeControl cubeControl = obj.GetComponent<CubeControl>();
                cubeControl.movement = new Vector3(0, floorHeight, 0);
            }
            else
            {
                GameObject obj = Instantiate(upDownCube, new Vector3(1, floorHeight * k, 1), Quaternion.identity) as GameObject;
                CubeControl cubeControl = obj.GetComponent<CubeControl>();
                cubeControl.movement = new Vector3(0, floorHeight, 0);
            }

        }

    }



    public int[,] CreateMaze()
    {
        // 各マスの初期設定を行う
        for (int y = 0; y < this.height; y++)
        {
            for (int x = 0; x < this.width; x++)
            {
                // 外周のみ壁にしておき、開始候補として保持
                if (x == 0 || y == 0 || x == this.width - 1 || y == this.height - 1)
                {
                    this.maze[x, y] = Wall;
                }
                else
                {
                    this.maze[x, y] = Path;
                    // 外周ではない偶数座標を壁伸ばし開始点にしておく
                    if (x % 2 == 0 && y % 2 == 0)
                    {
                        // 開始候補座標
                        StartCells.Add(new Cell(x, y));
                    }
                }
            }
        }

        // 壁が拡張できなくなるまでループ
        while (StartCells.Count > 0)
        {
            // ランダムに開始セルを取得し、開始候補から削除
            var index = Random.Range(0, StartCells.Count);
            var cell = StartCells[index];
            StartCells.RemoveAt(index);
            var x = cell.X;
            var y = cell.Y;

            // すでに壁の場合は何もしない
            if (this.maze[x, y] == Path)
            {
                // 拡張中の壁情報を初期化
                CurrentWallCells.Clear();
                ExtendWall(x, y);
            }
        }
        return this.maze;
    }

    // 指定座標から壁を生成拡張する
    private void ExtendWall(int x, int y)
    {
        // 伸ばすことができる方向(1マス先が通路で2マス先まで範囲内)
        // 2マス先が壁で自分自身の場合、伸ばせない
        var directions = new List<Direction>();
        if (this.maze[x, y - 1] == Path && !IsCurrentWall(x, y - 2))
            directions.Add(Direction.Up);
        if (this.maze[x + 1, y] == Path && !IsCurrentWall(x + 2, y))
            directions.Add(Direction.Right);
        if (this.maze[x, y + 1] == Path && !IsCurrentWall(x, y + 2))
            directions.Add(Direction.Down);
        if (this.maze[x - 1, y] == Path && !IsCurrentWall(x - 2, y))
            directions.Add(Direction.Left);

        // ランダムに伸ばす(2マス)
        if (directions.Count > 0)
        {
            // 壁を作成(この地点から壁を伸ばす)
            SetWall(x, y);

            // 伸ばす先が通路の場合は拡張を続ける
            var isPath = false;
            var dirIndex = Random.Range(0,directions.Count);
            switch (directions[dirIndex])
            {
                case Direction.Up:
                    isPath = (this.maze[x, y - 2] == Path);
                    SetWall(x, --y);
                    SetWall(x, --y);
                    break;
                case Direction.Right:
                    isPath = (this.maze[x + 2, y] == Path);
                    SetWall(++x, y);
                    SetWall(++x, y);
                    break;
                case Direction.Down:
                    isPath = (this.maze[x, y + 2] == Path);
                    SetWall(x, ++y);
                    SetWall(x, ++y);
                    break;
                case Direction.Left:
                    isPath = (this.maze[x - 2, y] == Path);
                    SetWall(--x, y);
                    SetWall(--x, y);
                    break;
            }
            if (isPath)
            {
                // 既存の壁に接続できていない場合は拡張続行
                ExtendWall(x, y);
            }
        }
        else
        {
            // すべて現在拡張中の壁にぶつかる場合、バックして再開
            var beforeCell = CurrentWallCells.Pop();
            ExtendWall(beforeCell.X, beforeCell.Y);
        }
    }

    // 壁を拡張する
    private void SetWall(int x, int y)
    {
        this.maze[x, y] = Wall;
        if (x % 2 == 0 && y % 2 == 0)
        {
            CurrentWallCells.Push(new Cell(x, y));
        }
    }

    // 拡張中の座標かどうか判定
    private bool IsCurrentWall(int x, int y)
    {
        return CurrentWallCells.Contains(new Cell(x, y));
    }

    // 通路・壁情報
    const int Path = 0;
    const int Wall = 1;

    // セル情報
    private struct Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    // 方向
    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
