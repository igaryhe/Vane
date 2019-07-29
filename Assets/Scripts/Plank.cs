using UnityEngine;

public class Plank : MonoBehaviour, CommandInterface
{
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