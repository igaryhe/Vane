using UnityEngine;

public class RotatePlankCommand : Command
{
    private Transform _transform;
    private GameManager _gm = GameManager.Instance;

    public RotatePlankCommand(Transform tr)
    {
        _transform = tr;
    }
    public override void Execute()
    {
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
        }
    }

    public override void Undo()
    {
        if (_transform == null) return;
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
