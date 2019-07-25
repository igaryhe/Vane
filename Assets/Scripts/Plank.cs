using UnityEngine;

public class Plank : MonoBehaviour, CommandInterface
{
    private void OnMouseDown()
    {
        var rpc = new RotatePlankCommand(transform);
        rpc.Execute();
        GameManager.Instance.commands.Push(rpc);
    }

    public Command command { get; set; }
}