using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameObject vane, fan, plank, board, placeable;
    private const string filepath = "Assets/Resources/Levels/level1.txt";
    private int _col, _row;
    private const float O = 0.5f;
    public Vector3[,] dirs;
    private int _count;
    public Stack<Command> commands = new Stack<Command>();

    public static GameManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                var obj = new GameObject();
                obj.AddComponent<GameManager>();
                _instance = obj.GetComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        var b = File.ReadAllText(filepath).Split('\n').Select(c => c.ToCharArray()).ToArray();
        _row = b.Length;
        _col = b[0].Length;
        dirs = new Vector3[_row,_col];
        for (var i = 0; i != _row; i++)
        {
            for (var j = 0; j != _col; j++)
            {
                dirs[i, j] = Vector3.zero;
            }
        }
        for (var i = 0; i != _row + 1; i++)
        {
            for (var j = 0; j != _col + 1; j++)
            {
                if (i == 0 || j == 0)
                {
                    Instantiate(placeable, new Vector3((i - 0.5f), 0, (j - 0.5f)), Quaternion.identity);
                }
                else
                {
                    Instantiate(board, new Vector3((i - 0.5f), 0, (j - 0.5f)), Quaternion.identity);
                }
            }
        }
        for (var i = 0; i != _row; i++)
        {
            for (var j = 0; j != _col; j++)
            {
                if (b[i][j] != '.')
                {
                    _count++;
                    var forward = new Vector3();
                    switch (b[i][j])
                    {
                        case 'l':
                            forward = Vector3.back;
                            break;
                        case 'u':
                            forward = Vector3.left;
                            break;
                        case 'r':
                            forward = Vector3.forward;
                            break;
                        case 'd':
                            forward = Vector3.right;
                            break;
                    }
                    dirs[i, j] = Vector3.zero;
                    Instantiate(vane, new Vector3(i + O, 0, j + O),
                        Quaternion.LookRotation(forward, Vector3.up));
                }
            }
        }
    }

    private void Update()
    {
        if (_count == 0)
        {
            Debug.Log("You Win!");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (commands.Count > 0)
            {
                commands.Pop().Undo();
            }
        }
    }

    public void Decrease()
    {
        _count--;
    }

    public void Increase()
    {
        _count++;
    }
}
