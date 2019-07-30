using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    private float speed = -0.02f;

    private Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _mat.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}