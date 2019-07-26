using System.Collections;
using UnityEngine;
public class RotateCommand : Command
{
    private Transform _transform;
    public readonly Vector3 direction;
    private readonly Vector3 _prevDirection;
    private readonly float _overTime = 3f;
    private GameManager gm = GameManager.Instance;
    
    public RotateCommand(Transform transform, Vector3 direction)
    {
        _transform = transform;
        this.direction = direction;
        _prevDirection = transform.forward;
    }
    
    public override void Execute()
    {
        gm.StartCoroutine(Rotate(direction));
    }

    public override void Undo()
    {
        gm.StartCoroutine(Rotate(_prevDirection));
    }
    
    private IEnumerator Rotate(Vector3 dir)
    {
        var rotation = Quaternion.LookRotation(dir, Vector3.up);
        var startTime = Time.time;
        while (Time.time < startTime + _overTime)
        {
            _transform.rotation = Quaternion.Slerp(_transform.rotation, rotation, 
                (Time.time - startTime) / _overTime);
            yield return null;
        }

        _transform.rotation = rotation;
    }
}