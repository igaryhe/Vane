using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameObject vane, fan, plank, barrier, stone;
    public GameObject[] board;
    public TextMeshProUGUI remains;
    public Transform level;
    [HideInInspector]
    public int col, row;
    [HideInInspector]
    public Vector3 win;
    [HideInInspector]
    public List<Command> commands = new List<Command>();
    public GameObject ui;
    [HideInInspector]
    public GameObject winds, planks;
    [HideInInspector]
    public int levelNum;
    public CameraRotator cr;
    [HideInInspector]
    public List<Transform> plankList;

    private List<VaneData> _vanes;

    private const float O = 0.5f;
    public int _count;
    public int _pcount, _pmax;
    private int _fcount;
    private bool _isPlayed;
    public AudioManager am;

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
        LoadGame();
        am.Play("wind");
        plankList = new List<Transform>();
    }

    private void Update()
    {
        if (_count == 0)
        {
            if (!_isPlayed)
            {
                am.Play("crow");
                _isPlayed = !_isPlayed;
                foreach (var v in _vanes)
                {
                    v.tr.gameObject.GetComponent<Celebrate>().enabled = true;
                }
            }
            ui.SetActive(true);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Undo();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reset();
                commands.Clear();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToLevelSelector();
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
        remains.text = _pcount + "/" + _pmax;;
    }

    public void PlankInc()
    {
        _pcount++;
        remains.text = _pcount + "/" + _pmax;
    }

    public bool isPcountZero()
    {
        return _pcount == 0;
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
                var boardInstance = Instantiate(board[Mathf.RoundToInt(Random.Range(0,board.Length))],
                    new Vector3((i + 0.5f), 0, (j + 0.5f)), Quaternion.identity);
                boardInstance.transform.parent = boards.transform;
                if (b[i][j] == '.') continue;
                var boardCompoment = boardInstance.GetComponent<Board>();
                boardCompoment.isPlaced = true;
                if (b[i][j] == 's')
                {
                    var instance = Instantiate(stone, new Vector3(i + O, 0, j + O), Quaternion.identity);
                    instance.transform.parent = items.transform;
                    foreach (Transform item in boardInstance.transform)
                    {
                        if (item.CompareTag("Grass")) Destroy(item.gameObject);
                    }
                }
                else if (b[i][j] == 'b')
                {
                    var instance = Instantiate(barrier, new Vector3(i + O, 0.5f, j + O), Quaternion.identity);
                    instance.transform.parent = items.transform;
                    foreach (Transform item in boardInstance.transform)
                    {
                        if (item.CompareTag("Grass")) Destroy(item.gameObject);
                    }
                }
                else
                {
                    _count++;
                    var forward = NumToV3(b[i][j]);
                    var rotation = Quaternion.LookRotation(forward, Vector3.up);
                    var instance = Instantiate(vane, new Vector3(i + O, 0, j + O), rotation);
                    instance.transform.parent = vanes.transform;
                    _vanes.Add(new VaneData(instance.transform, forward));
                }
            }
        }
    }

    private string ReadParam(string file)
    {
        file = file.Replace("\r", string.Empty);
        
        // read plank count
        _pcount = (int)char.GetNumericValue(file[0]);
        _pmax = _pcount;
        remains.text = _pcount + "/" + _pmax;
        file = file.Remove(0, 2);

        win = NumToV3(file[0]);
        file = file.Remove(0, 2);
        
        // read wind count
        _fcount = (int)char.GetNumericValue(file[0]);
        return file.Remove(0, 2);
    }

    private void LoadLevel(int l)
    {
        // plankList = new List<Transform>();
        _isPlayed = false;
        _vanes = new List<VaneData>();
        ui.SetActive(false);
        winds = new GameObject();
        winds.name = "Winds";
        winds.transform.parent = level;
        planks = new GameObject();
        planks.name = "Planks";
        planks.transform.parent = level;
        _count = 0;
        
        var file = Resources.Load<TextAsset>("Levels/level" + l).ToString();
        
        // read _count, _pcount and _fcount
        file = ReadParam(file);
        plankList.Clear();

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
        // level.gameObject.SetActive(true);
        LoadLevel(levelNum);
    }

    public void ReturnToLevelSelector()
    {
        if (levelNum >= LevelData.progress)
            SaveSystem.Save(levelNum);
        SceneManager.LoadScene("Selection");
    }

    public void LoadGame()
    {
        levelNum = LevelData.currentLevel;
        LoadLevel(levelNum);
        cr.transform.position = new Vector3(row / 2f, 0, col / 2f);
    }

    public void Reset()
    {
        DestroyLevel();
        LoadLevel(levelNum);
    }

    public void Undo()
    {
        if (commands.Count > 0)
        {
            commands[commands.Count - 1].Undo();
            commands.RemoveAt(commands.Count - 1);
            // Reset();
            // foreach (var c in commands)
            // {
            //     c.Execute();
            // }
            ResetRotation();
        }
    }

    private void ResetRotation()
    {
        foreach (Transform w in winds.transform)
        {
            Destroy(w.gameObject);
        }
        foreach (var v in _vanes)
        {
            var vane = v.tr.gameObject.GetComponent<Vane>();
            vane.ResetDirection();
            v.Rotate();
        }
    }
}
