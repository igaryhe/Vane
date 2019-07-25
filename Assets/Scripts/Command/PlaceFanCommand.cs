using System.Collections.Generic;
using UnityEngine;

public class PlaceFanCommand : Command
{
    private int _x, _y;
    private GameObject fan;
    private GameObject _instance;
    // public Stack<RotateCommand> affected = new Stack<RotateCommand>();
    private Board _board;

    public PlaceFanCommand(int x, int y, Board board)
    {
        _x = x;
        _y = y;
        _board = board;
        fan = GameManager.Instance.fan;
        affected = new Stack<RotateCommand>();
    }
    
    public override void Execute()
    {
        if (_x == -1)
        {
            _instance = Object.Instantiate(fan, new Vector3(_x + 0.5f ,0, _y + 0.5f),
                Quaternion.LookRotation(Vector3.right, Vector3.up));
            _instance.GetComponent<Fan>().command = this;
        }

        if (_y == -1)
        {
            _instance = Object.Instantiate(fan, new Vector3(_x + 0.5f ,0, _y + 0.5f), Quaternion.identity);
            _instance.GetComponent<Fan>().command = this;
        }

        _board.isPlaced = true;
    }

    public override void Undo()
    {
        Object.Destroy(_instance);
        _board.isPlaced = false;
        while (affected.Count > 0)
        {
            affected.Pop().Undo();
        }
    }
}