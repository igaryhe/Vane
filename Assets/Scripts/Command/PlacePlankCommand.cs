using UnityEngine;

public class PlacePlankCommand : Command
{
    private int _x, _y;
    private Board _board;
    private GameObject _plank;
    private GameObject _instance;
    private GameManager _gm = GameManager.Instance;

    public PlacePlankCommand(int x, int y, Board board)
    {
        _x = x;
        _y = y;
        _board = board;
        _plank = GameManager.Instance.plank;
    }
    public override void Execute()
    {
        _gm.am.Play("j");
        _instance = Object.Instantiate(_plank, new Vector3(_x + 0.5f, 0.5f, _y + 0.5f), Quaternion.identity);
        _instance.transform.parent = _gm.planks.transform;
        _board.isPlaced = true;
        var p = _instance.GetComponentInChildren<Plank>();
        p.seq = _gm._pmax - _gm._pcount;
        _gm.PlankDec();
        _gm.plankList.Add(_instance.transform);
    }

    public override void Undo()
    {
        Object.Destroy(_instance);
        _board.isPlaced = false;
        _gm.PlankInc();
    }
}