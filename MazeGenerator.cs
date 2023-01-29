using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell{
    public float X;
    public float Y;
    public bool hasRightWall = false;
    public bool hasBottomWall = false;
    public int union;

}
public class MazeGenerator : MonoBehaviour
{
    public MazeCell [,] maze;
    [SerializeField]
    private GameObject _wall;
    [SerializeField]
    private GameObject _walls;
    [SerializeField]
    private GameObject _finish;
    [SerializeField]
    private GameObject _ball;
    private  int SIZE_X = 9;
    private  int SIZE_Y = 9;
    private  float START_X = -4.5f;
    private  float START_Y = 4.5f;
    private  int STEP = 1;
    private  float RightWallChance = 0.5f;
    private  float BottomWallChance = 0.5f;
    private int _counter = 0;

    private MazeCell[,] FillMaze(){                         // initialize and fill every's cell's coordinates
        MazeCell [,] maze = new MazeCell[SIZE_X,SIZE_Y];
        for (int i = 0; i<maze.GetLength(1);i++){
            for (int j = 0;j<maze.GetLength(0);j++){
                maze[j,i] = new MazeCell();
                maze[j,i].X = START_X + STEP*j;
                maze[j,i].Y = START_Y - STEP*i;
            }
        }
        return maze;
    }
    void Start()
    {
        maze = FillMaze();      
        GenerateMaze(maze);
        BuildLevel(maze);
    }
    private void BuildLevel(MazeCell[,] maze){
        for (int i = 0; i<9;i++){
            for (int j=0;j<9;j++){
                if (maze[j,i].hasRightWall){
                    Vector3 pos = new Vector3(maze[j,i].X+0.5f, 1f, maze[j,i].Y);
                    //var wall = Instantiate(_wall, new Vector3(maze[j,i].X+0.5f, 1f, maze[j,i].Y),Quaternion.AngleAxis(90,Vector3.up), _walls.transform);
                    var wall = Instantiate(_wall);
                    wall.transform.parent = _walls.transform;
                    wall.transform.localPosition = new Vector3(maze[j,i].X+0.5f, 1f, maze[j,i].Y);
                    wall.transform.localRotation = Quaternion.AngleAxis(90,Vector3.up);
                    }
                if (maze[j,i].hasBottomWall){
                    //Instantiate(_wall, new Vector3(maze[j,i].X, 1f, maze[j,i].Y-0.5f),transform.rotation, _walls.transform);
                    var wall = Instantiate(_wall);
                    wall.transform.parent = _walls.transform;
                    wall.transform.localPosition = new Vector3(maze[j,i].X, 1f, maze[j,i].Y-0.5f);
                    wall.transform.localRotation = _wall.transform.rotation;
                }
            }
        }
        var ball = Instantiate(_ball);
        ball.transform.parent = transform;
        ball.transform.localPosition = new Vector3(maze[0,0].X, 1f, maze[0,0].Y);
        var finish = Instantiate(_finish);
        finish.transform.parent = transform;
        finish.transform.localPosition = new Vector3(maze[SIZE_X-1,SIZE_Y-1].X, 1f, maze[SIZE_X-1,SIZE_Y-1].Y);
    }
    private void GenerateMaze(MazeCell[,] maze){
        for (int i = 0; i < SIZE_X;i++){
            if (i == 0){                                 //if 1st 
                SetFirstLine(maze, i);
            }
            else{
                SetPreviousLine(maze, i);
            }
            SetRightWalls(maze, i);
            SetBottomWalls(maze, i);
        }
        }

    private void SetBottomWalls(MazeCell[,] maze,int i)
    {
        List<int> unionCount = new List<int>(); // list contains count of cell in each union
        unionCount.Add(0);
        int currentUnion = 0;
        int counter = 0;
        for (int j = 0; j < SIZE_X; j++){       //count cells in union
            int tempUnion = maze[j,i].union;
            if (tempUnion == currentUnion){
                unionCount[counter] = unionCount[counter] + 1;
            }
            else{
                unionCount.Add(1);
                currentUnion = maze[j,i].union;
                counter++;
            }
        }
        currentUnion = maze[0,i].union;
        counter = 0;
        bool hasSpace = true;
        for (int j = 0; j < SIZE_X; j++){       //make bottomwall 
            int tempUnion = maze[j,i].union;
            if(tempUnion!=currentUnion){
                counter++;
                currentUnion = maze[j,i].union;
                hasSpace = true;
            }
            var value = Random.Range(0f,1f);
            if (value >BottomWallChance && hasSpace){
                maze[j,i].hasBottomWall = true;
                unionCount[counter]--;
                if(unionCount[counter] == 1){
                    hasSpace = false;
            }
            }
        }
    }

    private void SetRightWalls(MazeCell[,] maze, int i)
    {
        for (int j = 0; j < SIZE_X; j++){       //randomly set rightwall, otherwise make union as prev
            var value = Random.Range(0f,1f);
            if (value >RightWallChance){
                maze[j,i].hasRightWall =true;
                continue;
            }
            if (j!= 0)
                maze[j,i].union = maze[j-1,i].union;
        }
    }

    private void SetPreviousLine(MazeCell[,] maze, int i)
    {
        MazeCell[] lastline = new MazeCell[SIZE_X]; //previous line
                for (int j = 0; j < SIZE_X; j++){           //copy prev line and delete walls
                    lastline[j] = maze[j,i];
                    lastline[j].hasBottomWall = maze[j,i-1].hasBottomWall;
                    lastline[j].hasRightWall = false;
                    if (lastline[j].hasBottomWall){
                        lastline[j].hasBottomWall =false;
                        lastline[j].union = _counter;
                        _counter++;  
                    }
                    maze[j,i] = lastline[j];
                }
    }

    private void SetFirstLine(MazeCell[,] maze, int i)
    {
        for (int j = 0; j < SIZE_Y; j++){       //set unique union to every cell
            maze[j,i].union = _counter;
            _counter++;
        }
    }
}
