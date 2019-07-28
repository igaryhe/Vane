﻿using UnityEngine;

public class Board : MonoBehaviour
{
    private Material _mat;
    private Renderer _rend;
    private Color _color;
    private Vector3 _pos;
    public bool isPlaced;
    private GameManager _gm;
    private Material grass;
    private MaterialPropertyBlock block, hover;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _mat = GetComponent<Renderer>().material;
        grass = GetComponentsInChildren<Renderer>()[2].material;
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
        _rend.SetPropertyBlock(hover);
    }

    private void OnMouseExit()
    {
        // _mat.color = _color;
        _rend.SetPropertyBlock(block);
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
                if (!_gm.isPcountZero())
                {
                    var ppc = new PlacePlankCommand((int) (_pos.x), (int) (_pos.z), this);
                    ppc.Execute();
                    _gm.commands.Push(ppc);
                }
            }
        }
    }
}
