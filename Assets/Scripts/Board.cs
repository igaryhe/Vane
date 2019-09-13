using System;
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
    private Material grass, flower;
    private MaterialPropertyBlock block, hover;
    private Vector3 windDirection;
    private Vector3 lastDirection;
    private float i;
    private GrassTrigger _gt;
    private float lastStr = 0.1f;
    private float windStr = 0.1f;
    private bool pointerOut = false;
    private bool isDrag = false;
    private Vector2 clickPos;
    private Vector2 mousePos;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
    }
    private void Start()
    {
        _gt = GetComponentInChildren<GrassTrigger>();
        _mat = GetComponent<Renderer>().material;
        grass = GetComponentsInChildren<Renderer>()[1].material;
        flower = GetComponentsInChildren<Renderer>()[2].material;
        _color = _mat.color;
        _pos = transform.position;
        _gm = GameManager.Instance;
        foreach (Transform item in transform)
        {
            if (item.CompareTag("Grass"))
            {
                item.gameObject.GetComponent<Renderer>().material = grass;
            }
            if (item.CompareTag("Flower"))
            {
                item.gameObject.GetComponent<Renderer>().material = flower;
            }
        }
        hover = new MaterialPropertyBlock();
        block = new MaterialPropertyBlock();
        var green = new Color32(120, 157, 130, 255);
        hover.SetColor("_BaseColor", green);
        _rend.GetPropertyBlock(block);
    }


    private void OnMouseOver()
    {
        if (!isPlaced && _gm._pcount > 0 && isDrag == false)
        {
            // _mat.color = Color.gray;
            //if (EventSystem.current.IsPointerOverGameObject()) return;
            _rend.SetPropertyBlock(hover);
            pointerOut = false;
        }
    }




    private void OnMouseExit()
    {
        // _mat.color = _color;
        //if (EventSystem.current.IsPointerOverGameObject()) return;
        _rend.SetPropertyBlock(block);
        pointerOut = true;
    }

    private void OnMouseDown()
    {
        clickPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        clickPos = Vector2.zero;
        if (EventSystem.current.IsPointerOverGameObject() || pointerOut == true) return;
        if (!isPlaced && isDrag == false)
        {
            if (_pos.x < 0f && _pos.z < 0f ||
                _pos.x < 0f && _pos.z > _gm.col ||
                _pos.x > _gm.row && _pos.z < 0f ||
                _pos.x > _gm.row && _pos.z > _gm.col) return;
            if (_pos.x < 0f || _pos.z < 0f || _pos.x > _gm.row || _pos.z > _gm.col)
            {
                var pfc = new PlaceFanCommand((int) (_pos.x - 0.5), (int) (_pos.z - 0.5), this);
                pfc.Execute();
                _gm.commands.Add(pfc);
            }
            else
            {
                if (!_gm.isPcountZero())
                {
                    var ppc = new PlacePlankCommand((int) (_pos.x), (int) (_pos.z), this);
                    ppc.Execute();
                    _gm.commands.Add(ppc);
                }
            }
        }
        //isDrag = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(2)) isDrag = false;
        if (clickPos != Vector2.zero && i > 1f) //per 1/3s cauclate the delta mouse position 
        {
            mousePos = Input.mousePosition;
            if ((mousePos - clickPos).magnitude > 0.5f) isDrag = true;
        }
        //if(isDrag == true) _rend.SetPropertyBlock(block);
        if (i > 1) //per 1/3s update grass floating direction
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
            flower.SetVector("_WindDir", Vector3.Lerp(lastDirection, windDirection, i));
            flower.SetFloat("_WindSpd", Mathf.Lerp(0.1f, 1f, windDirection.normalized.magnitude));

        }
        i += Time.deltaTime * 3;
    }
}
