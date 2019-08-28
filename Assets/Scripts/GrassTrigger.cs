using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrigger : MonoBehaviour
{
    public Vector3 windDir;
    private float i = 0;
    private ParticleSystem leaf;
    private ParticleSystem petal;

    private void Start()
    {
        leaf = GetComponent<ParticleSystem>();
        if(GetComponentsInChildren<ParticleSystem>().Length > 1)
        {
            petal = GetComponentsInChildren<ParticleSystem>()[1];
        }
    }
    
    private void Update()
    {
        if(i < 0f && i > -0.5f)
        {
            windDir = Vector3.zero;
            var main = leaf.main;
            main.loop = false;
            var emit = leaf.emission;
            emit.enabled = false;
            if(petal != null)
            {
                main = petal.main;
                main.loop = false;
                emit = petal.emission;
                emit.enabled = false;
            }
        }
        i -= Time.deltaTime * 1f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            i = 1f;
            windDir = other.transform.right;
            var main = leaf.main;
            main.loop = true;
            var emit = leaf.emission;
            emit.enabled = true;
            var vel = leaf.velocityOverLifetime;
            vel.x = -windDir.z * 1.25f;
            vel.z = windDir.x * 1.25f;
            if (petal != null)
            {
                main = petal.main;
                main.loop = true;
                emit = petal.emission;
                emit.enabled = true;
                vel = petal.velocityOverLifetime;
                vel.x = -windDir.z * 1.25f;
                vel.z = windDir.x * 1.25f;
            }
        }
    }

}
