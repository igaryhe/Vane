using UnityEngine;

public class RotatePlankCommand : Command
{
    private Transform _transform;

    public RotatePlankCommand(Transform transform)
    {
        _transform = transform;
    }
    public override void Execute()
    {
        _transform.Rotate(Vector3.up, 45);
    }

    public override void Undo()
    {
        _transform.Rotate(Vector3.up, -45);
    }
}
