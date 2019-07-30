using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour
{
    public bool isPlaced;

    private Material _mat;
    private Renderer _rend;
    private Color _color;
    private Vector3 _pos;
    private GameManager _gm;
    private Material grass;
    private MaterialPropertyBlock block, hover;
    private Vector3 windDirection;
    private Vector3 lastDirection;
    private float i;
    private GrassTrigger _gt;
    private float lastStr = 0.1f;
    private float windStr = 0.1f;

    private void Start()
    {
        _gt = GetComponentInChildren<GrassTrigger>();
        _rend = GetComponent<Renderer>();
        _mat = GetComponent<Renderer>().material;
        grass = GetComponentsInChildren<Renderer>()[1].material;
        _color = _mat.color;
        _pos = transform.position;
        _gm = GameManager.Instance;
        foreach (Transform item in transform)
        {
            if (item.CompareTag("Grass"))
            {
                item.gameObject.GetComponent<Renderer>().material = grass;
            }
        }
        hover = new MaterialPropertyBlock();
        block = new MaterialPropertyBlock();
        hover.SetColor("_BaseColor", Color.gray);
        _rend.GetPropertyBlock(block);
    }

    private void OnMouseEnter()
    {
        // _mat.color = Color.gray;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        _rend.SetPropertyBlock(hover);
    }

    private void OnMouseExit()
    {
        // _mat.color = _color;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        _rend.SetPropertyBlock(block);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
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
                if (!_gm.isPcountZero())
                {
                    var ppc = new PlacePlankCommand((int) (_pos.x), (int) (_pos.z), this);
                    ppc.Execute();
                    _gm.commands.Push(ppc);
                }
            }
        }
    }

    private void Update()
    {
        if(i > 1)
        {
            i = 0;
            lastDirection = windDirection;
            lastStr = windStr;
            windDirection = Quaternion.Euler(0,0,90) * new Vector3(_gt.windDir.x, _gt.windDir.z, 0);
            windDirection *= 0.2f;
            windStr = 1;
        }
        else
        {
            grass.SetVector("_WindDir", Vector3.Lerp(lastDirection, windDirection, i));
            //grass.SetFloat("_WindSpd", Mathf.Lerp(0.1f, 1f, Vector3.Lerp(lastDirection.normalized, windDirection.normalized, i).magnitude));
            grass.SetFloat("_WindSpd", Mathf.Lerp(0.1f, 1f, windDirection.normalized.magnitude));
        }
        i += Time.deltaTime * 3;
    }
}
