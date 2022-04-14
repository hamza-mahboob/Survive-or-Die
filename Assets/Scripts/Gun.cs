using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lr;

    private int gunDamage = 2;
    //public GameObject gunPrefab;

    private void Start()
    {
        lr.enabled = false;
    }

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
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hit.point);
                hit.collider.gameObject.GetComponent<PlayerController>().ReduceHealth(gunDamage);
            }
            //play sound
            SoundManager.instance.ShootSound();
        }
        else
            lr.enabled = false;

    }
}