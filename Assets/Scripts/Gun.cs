using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;

    private int gunDamage = 2;
    //public GameObject gunPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //gun shoots using raycast
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //reduce player health
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<PlayerController>().ReduceHealth(gunDamage);
            }
        }
        //play sound
        ////////////
    }
}