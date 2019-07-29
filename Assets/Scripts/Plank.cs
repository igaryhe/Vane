using UnityEngine;

public class Plank : MonoBehaviour, CommandInterface
{
    private Renderer[] _rend;
    private MaterialPropertyBlock block, hover;
    void Start()
    {
        _rend = transform.parent.GetComponentsInChildren<Renderer>();
        hover = new MaterialPropertyBlock();
        block = new MaterialPropertyBlock();
        hover.SetColor("_BaseColor", Color.yellow);
        for(int i = 0; i < _rend.Length; i++)
        {
        _rend[i].GetPropertyBlock(block);
        }
    }
    private void OnMouseEnter()
    {
        for (int i = 0; i < _rend.Length; i++)
        {
            _rend[i].SetPropertyBlock(hover);
        }
        // _mat.color = Color.gray;
    }
    private void OnMouseExit()
    {
        for (int i = 0; i < _rend.Length; i++)
        {
            _rend[i].SetPropertyBlock(block);
        }
        // _mat.color = _color;
    }
    private void OnMouseDown()
    {
        var rpc = new RotatePlankCommand(transform);
        rpc.Execute();
        GameManager.Instance.commands.Push(rpc);
        foreach (Transform item in transform.parent)
        {
            if (item.CompareTag("Fan"))
            {
                item.Rotate(Vector3.forward, -45);
            }
        }
    }

    public Command command { get; set; }
}