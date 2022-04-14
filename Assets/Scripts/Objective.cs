using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public float objectiveProgress {
        get;
        set;
    }

    private void Start()
    {
        objectiveProgress = 0;
    }

    public void IncreaseProgress()
    {
        objectiveProgress += Time.deltaTime;
        
        if (objectiveProgress >= 100)
            objectiveProgress = 100;
        
        GetComponentInChildren<TextMesh>().text = "Progress: " + (int) objectiveProgress + "%";
    }
    
}
