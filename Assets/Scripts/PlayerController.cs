using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Object = System.Object;

public class PlayerController : MonoBehaviour
{
    public GameObject cameraHolder;

    private float mouseSens = 2,
        sprintSpeed = 5,
        walkSpeed = 2,
        jumpForce,
        smoothTime,
        verticalLookRotation;

    private int health;
    private bool isGrounded;
    private Vector3 smoothMoveVelocity, moveAmount;

    private Rigidbody rb;

    private PhotonView PV;

    private PlayerManager playerManager;

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

        //player health
        health = 10;
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
        //if player is not mine then return
        if (!PV.IsMine)
            return;
        //for smooth movement
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount,
            moveDirection * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity,
            smoothTime);
    }

    void Look()
    {
        //if player is not mine then return
        if (!PV.IsMine)
            return;
        //horizontal rotation
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSens);
        //vertical rotation
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSens;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void ReduceHealth(int damage)
    {
        PV.RPC("RPC_ReduceHealth", RpcTarget.All, damage);
    }

    //RPC call on all clients
    [PunRPC]
    void RPC_ReduceHealth(int damage)
    {
        if(!PV.IsMine)
            return;
        
        
        health -= damage;
        if (health < 0)
        {
            //player dies...
            Debug.Log("Player Dead!");
            Die();
        }
    }
    void Die()
    {
        PhotonNetwork.Destroy(PhotonView.Get(this));
        PhotonNetwork.Disconnect();
    }

    //progress objective
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Objective") && Input.GetMouseButton(0))
        {
            PV.RPC("RPC_IncreaseProgress", RpcTarget.All, collisionInfo.gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    void RPC_IncreaseProgress(int viewId)
    {
        //increase progress on objective for all players
        PhotonNetwork.GetPhotonView(viewId).GetComponent<Objective>().IncreaseProgress();
    }
}