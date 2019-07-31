using UnityEngine;
using UnityEngine.EventSystems;

public class Plank : MonoBehaviour
{
    private Renderer[] _rend;
    private MaterialPropertyBlock block, hover;
    public int seq;
    private void Start()
    {
        _rend = transform.GetComponentsInChildren<Renderer>();
        hover = new MaterialPropertyBlock();
        block = new MaterialPropertyBlock();
        hover.SetColor("_BaseColor", Color.yellow);
        foreach (var t in _rend)
        {
            t.GetPropertyBlock(block);
        }
    }
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        foreach (var t in _rend)
        {
            t.SetPropertyBlock(hover);
        }
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        foreach (var t in _rend)
        {
            t.SetPropertyBlock(block);
        }

    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        var rpc = new RotatePlankCommand(seq);
        rpc.Execute();
        GameManager.Instance.commands.Add(rpc);
    }

    public Command command { get; set; }
}