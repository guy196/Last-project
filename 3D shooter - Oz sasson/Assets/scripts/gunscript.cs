using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunscript : MonoBehaviour
{
    public float damage = 10;

    public Camera fpscam;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }

    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
