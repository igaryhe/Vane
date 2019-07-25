using System.Collections;
using UnityEngine;
public class RotateCommand : Command
{
    private Transform _transform;
    private readonly Vector3 _direction;
    private readonly Vector3 _prevDirection;
    private readonly float _overTime = 3f;
    private GameManager gm = GameManager.Instance;
    
    public RotateCommand(Transform transform, Vector3 direction)
    {
        _transform = transform;
        _direction = direction;
        _prevDirection = transform.forward;
    }
    
    public override void Execute()
    {
        gm.StartCoroutine(Rotate(_direction));
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