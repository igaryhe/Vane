using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameObject vane, fan, plank, board, barrier, stone;
    private const string filepath = "Assets/Resources/Levels/level1.txt";
    public int col, row;
    private const float O = 0.5f;
    private int _count;
    private int _pcount;
    public Stack<Command> commands = new Stack<Command>();
    public GameObject ui;

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
        ui.SetActive(false);
        var file = File.ReadAllText(filepath);
        
        // read plank count
        _pcount = (int)char.GetNumericValue(file[0]);
        file = file.Remove(0, 2);

        var win = NumToV3(file[0]);
        file = file.Remove(0, 2);
        
        // read wind count
        var wcount = char.GetNumericValue(file[0]);
        file = file.Remove(0, 2);
        
        // read fan location
        var fans = new List<string>();
        for (var i = 0; i != wcount; i++)
        {
            fans.Add(file.Substring(0, 4));
            file = file.Remove(0, 4);
        }

        // read board
        var b = file.Split('\n').Select(c => c.ToCharArray()).ToArray();
        row = b.Length;
        col = b[0].Length;
        foreach (var f in fans)
        {
            switch (f[0])
            {
                case 'c':
                    switch (f[2])
                    {
                        case 'd':
                            Instantiate(fan, new Vector3(-0.5f ,0, (float)char.GetNumericValue(f[1]) + 0.5f),
                                Quaternion.LookRotation(Vector3.right, Vector3.up));
                            break;
                        case 'u':
                            Instantiate(fan, new Vector3(row + 0.5f ,0, (float)char.GetNumericValue(f[1]) + 0.5f),
                                Quaternion.LookRotation(Vector3.left, Vector3.up));
                            break;
                    }
                    break;
                case 'r':
                    switch (f[2])
                    {
                        case 'r':
                            Instantiate(fan, new Vector3((float)char.GetNumericValue(f[1]) + 0.5f ,0, -0.5f),
                                Quaternion.LookRotation(Vector3.forward, Vector3.up));
                            break;
                        case 'l':
                            Instantiate(fan, new Vector3((float)char.GetNumericValue(f[1]) + 0.5f ,0, col + 0.5f),
                                Quaternion.LookRotation(Vector3.back, Vector3.up));
                            break;
                    }
                    break;
            }
        }

        for (var i = 0; i != row; i++)
        {
            for (var j = 0; j != col; j++)
            {
                var boardInstance = Instantiate(board, new Vector3((i + 0.5f), 0, (j + 0.5f)), Quaternion.identity);
                if (b[i][j] != '.')
                {
                    boardInstance.GetComponent<Board>().isPlaced = true;
                    if (b[i][j] == 's')
                    {
                        Instantiate(stone, new Vector3(i + O, 0, j + O), Quaternion.identity);
                    }
                    else if (b[i][j] == 'b')
                    {
                        Instantiate(barrier, new Vector3(i + O, 0.5f, j + O), Quaternion.identity);
                    }
                    else
                    {
                        _count++;
                        var forward = NumToV3(b[i][j]);
                        var instance = Instantiate(vane, new Vector3(i + O, 0, j + O),
                            Quaternion.LookRotation(forward, Vector3.up));
                        instance.GetComponent<Vane>().win = win;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (_count == 0)
        {
            Debug.Log("You Win!");
            ui.SetActive(true);
            Time.timeScale = 0f;
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

    public void PlankDec()
    {
        _pcount--;
    }

    public void PlankInc()
    {
        _pcount++;
    }

    public bool isPcountZero()
    {
        return _pcount == 0;
    }

    private string Trim(string s)
    {
        return s.Remove(0, 1);
    }

    private Vector3 NumToV3(char n)
    {
        switch (n)
        {
            case '4':
                return Vector3.back;
            case '8':
                return Vector3.left;
            case '6':
                return Vector3.forward;
            case '2':
                return Vector3.right;
            case '7':
                return (Vector3.left + Vector3.back).normalized;
            case '9':
                return (Vector3.left + Vector3.forward).normalized;
            case '3':
                return (Vector3.right + Vector3.forward).normalized;
            case '1':
                return (Vector3.right + Vector3.back).normalized;
            default:
                return Vector3.zero;
        }
    }
}
