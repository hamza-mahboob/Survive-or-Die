using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cameraHolder;
    private float mouseSens = 2, sprintSpeed = 5, walkSpeed = 2, jumpForce, smoothTime, verticalLookRotation;
    private bool isGrounded;
    private Vector3 smoothMoveVelocity, moveAmount;

    private Rigidbody rb;

    private PhotonView PV;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //destroy other cameras & components (like rigidbody) for local player
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    private void Update()
    {
        Look();
        Movement();
    }


    private void FixedUpdate()
    {
        //if player is not mine then return
        if (!PV.IsMine)
            return;
        //moving rigidbody
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void Movement()
    {
        //for smooth movement
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount,
            moveDirection * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity,
            smoothTime);
    }

    void Look()
    {
        //horizontal rotation
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSens);
        //vertical rotation
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSens;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Objective()
    {
        //player progresses objective
    }
}