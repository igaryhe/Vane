using UnityEngine;

public class PlacePlankCommand : Command
{
    private int _x, _y;
    private Board _board;
    private GameObject _plank;
    private GameObject _instance;

    public PlacePlankCommand(int x, int y, Board board)
    {
        _x = x;
        _y = y;
        _board = board;
        _plank = GameManager.Instance.plank;
    }
    public override void Execute()
    {
        _instance = Object.Instantiate(_plank, new Vector3(_x + 0.5f, 0.5f, _y + 0.5f), Quaternion.identity);
        _board.isPlaced = true;
    }

    public override void Undo()
    {
        Object.Destroy(_instance);
        _board.isPlaced = false;
    }
}