using UnityEngine;

public class Board : MonoBehaviour
{
    private Material _mat;
    private Color _color;
    private Vector3 _pos;
    public bool isPlaced;
    private GameManager _gm;

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;
        _color = _mat.color;
        _pos = transform.position;
        _gm = GameManager.Instance;
    }

    private void OnMouseEnter()
    {
        _mat.color = Color.gray;
    }

    private void OnMouseExit()
    {
        _mat.color = _color;
    }

    private void OnMouseDown()
    {
        if (!isPlaced)
        {
            if (_pos.x < 0f && _pos.z < 0f ||
                _pos.x < 0f && _pos.z > _gm.col ||
                _pos.x > _gm.row && _pos.z < 0f ||
                _pos.x > _gm.row && _pos.z > _gm.col) return;
            if (_pos.x < 0f || _pos.z < 0f || _pos.x > _gm.row || _pos.z > _gm.col)
            {
                var pfc = new PlaceFanCommand((int) (_pos.x - 0.5), (int) (_pos.z - 0.5), this);
                pfc.Execute();
                _gm.commands.Push(pfc);
            }
            else
            {
                var ppc = new PlacePlankCommand((int) (_pos.x), (int) (_pos.z), this);
                ppc.Execute();
                _gm.commands.Push(ppc);
            }
        }
    }
}
