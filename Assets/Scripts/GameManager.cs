using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameObject vane, fan, plank, board, barrier, stone;
    public TextMeshProUGUI remains;
    public Transform level;
    public int col, row;
    public Vector3 win;
    public Stack<Command> commands = new Stack<Command>();
    public GameObject ui;
    public GameObject winds, planks;
    public int levelNum;
    
    private const float O = 0.5f;
    private int _count;
    private int _pcount;
    private int _fcount;

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
        LoadLevel(levelNum);

    }

    private void Update()
    {
        if (_count == 0)
        {
            ui.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (commands.Count > 0)
            {
                commands.Pop().Undo();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyLevel();
            LoadLevel(levelNum);
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
        remains.text = _pcount.ToString();
    }

    public void PlankInc()
    {
        _pcount++;
        remains.text = _pcount.ToString();
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

    private void PlaceFan(List<string> locations)
    {
        var fans = new GameObject();
        fans.transform.parent = level;
        foreach (var f in locations)
        {
            var n = (float) char.GetNumericValue(f[1]);
            GameObject instance;
            switch (f[0])
            {
                case 'c':
                    switch (f[2])
                    {
                        case 'd':
                            instance = Instantiate(fan, new Vector3(-0.5f ,0, n + 0.5f),
                                Quaternion.LookRotation(Vector3.right, Vector3.up));
                            instance.transform.parent = fans.transform;
                            break;
                        case 'u':
                            instance = Instantiate(fan, new Vector3(row + 0.5f ,0, n + 0.5f),
                                Quaternion.LookRotation(Vector3.left, Vector3.up));
                            instance.transform.parent = fans.transform;
                            break;
                    }
                    break;
                case 'r':
                    switch (f[2])
                    {
                        case 'r':
                            instance = Instantiate(fan, new Vector3(n + 0.5f ,0, -0.5f),
                                Quaternion.LookRotation(Vector3.forward, Vector3.up));
                            instance.transform.parent = fans.transform;
                            break;
                        case 'l':
                            instance = Instantiate(fan, new Vector3(n + 0.5f ,0, col + 0.5f),
                                Quaternion.LookRotation(Vector3.back, Vector3.up));
                            instance.transform.parent = fans.transform;
                            break;
                    }
                    break;
            }
            
        }
    }

    private void PlaceVane(char[][] b)
    {
        var boards = new GameObject();
        boards.name = "Boards";
        boards.transform.parent = level;
        var vanes = new GameObject();
        vanes.name = "Vanes";
        vanes.transform.parent = level;
        var items = new GameObject();
        items.name = "Items";
        items.transform.parent = level;
        for (var i = 0; i != row; i++)
        {
            for (var j = 0; j != col; j++)
            {
                var boardInstance = Instantiate(board, new Vector3((i + 0.5f), 0, (j + 0.5f)),
                    Quaternion.identity);
                boardInstance.transform.parent = boards.transform;
                if (b[i][j] == '.') continue;
                boardInstance.GetComponent<Board>().isPlaced = true;
                if (b[i][j] == 's')
                {
                    var instance = Instantiate(stone, new Vector3(i + O, 0, j + O), Quaternion.identity);
                    instance.transform.parent = items.transform;
                }
                else if (b[i][j] == 'b')
                {
                    var instance = Instantiate(barrier, new Vector3(i + O, 0.5f, j + O), Quaternion.identity);
                    instance.transform.parent = items.transform;
                }
                else
                {
                    _count++;
                    var forward = NumToV3(b[i][j]);
                    var instance = Instantiate(vane, new Vector3(i + O, 0, j + O),
                        Quaternion.LookRotation(forward, Vector3.up));
                    instance.transform.parent = vanes.transform;
                }
            }
        }
    }

    private string ReadParam(string file)
    {
        file = file.Replace("\r", string.Empty);
        
        // read plank count
        _pcount = (int)char.GetNumericValue(file[0]);
        remains.text = _pcount.ToString();
        file = file.Remove(0, 2);

        win = NumToV3(file[0]);
        file = file.Remove(0, 2);
        
        // read wind count
        _fcount = (int)char.GetNumericValue(file[0]);
        return file.Remove(0, 2);
    }

    private void LoadLevel(int l)
    {
        ui.SetActive(false);
        winds = new GameObject();
        winds.name = "Winds";
        winds.transform.parent = level;
        planks = new GameObject();
        planks.name = "Planks";
        planks.transform.parent = level;
        _count = 0;
        
        string filepath = "Assets/Resources/Levels/level" + l + ".txt";
        var file = File.ReadAllText(filepath);
        
        // read _count, _pcount and _fcount
        file = ReadParam(file);

        // read fan location
        var fans = new List<string>();
        for (var i = 0; i != _fcount; i++)
        { 
            fans.Add(file.Substring(0, 4));
            file = file.Remove(0, 4);
        }

        // read board
        var b = file.Split('\n').Select(c => c.ToCharArray()).ToArray();
        row = b.Length;
        col = b[0].Length;
        
        PlaceFan(fans);
        PlaceVane(b);
    }

    private void DestroyLevel()
    {
        foreach (Transform l in level)
        {
            Destroy(l.gameObject);
        }
    }

    public void NextLevel()
    {
        levelNum++;
        DestroyLevel();
        LoadLevel(levelNum);
    }
}
