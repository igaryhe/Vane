using UnityEngine;

public class RotatePlankCommand : Command
{
    private int _seq;
    private Transform _transform;
    private GameManager _gm = GameManager.Instance;

    public RotatePlankCommand(int seq)
    {
        _seq = seq;
    }

    public RotatePlankCommand(Transform tr)
    {
        _transform = tr;
    }
    public override void Execute()
    {
        // var _transform = _gm.plankList[_seq];
        _gm.am.Play("j");
        foreach (Transform item in _transform)
        {
            if (item.CompareTag("Fan"))
            {
                item.Rotate(Vector3.forward, -45);
            }

            if (item.CompareTag("Plank"))
            {
                item.Rotate(Vector3.up, 45);
            }

            // _transform.Rotate(Vector3.up, 45);
        }
    }

    public override void Undo()
    {
        // var _transform = _gm.plankList[_seq];
        // _transform.Rotate(Vector3.up, -45);
        foreach (Transform item in _transform)
        {
            if (item.CompareTag("Fan"))
            {
                item.Rotate(Vector3.forward, 45);
            }

            if (item.CompareTag("Plank"))
            {
                item.Rotate(Vector3.up, -45);
            }
        }
        
    }
}
