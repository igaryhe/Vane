using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoxRot : MonoBehaviour
{
    private Renderer _rend;
    private MaterialPropertyBlock block, hover;
    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
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
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        foreach (Transform item in transform)
        {
            if (item.CompareTag("Plank"))
            {
                item.Rotate(Vector3.up, 45);
            }
        }
    }
}
