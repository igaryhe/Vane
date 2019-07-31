using UnityEngine;

public class VaneData
{
    public Transform tr;
    public Vector3 dir;
    public VaneData(Transform tr, Vector3 dir)
    {
        this.tr = tr;
        this.dir = dir;
    }

    public void Rotate()
    {
        tr.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}