using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    private Animator PlayerAnimator { get { return GetComponent<Animator>(); } }
    private Rigidbody Rb { get { return GetComponent<Rigidbody>(); } }

    [SerializeField] private float _moveSpeed=1;
    [SerializeField] private float _ThrowForce=10;
    [SerializeField] private CinemachineVirtualCamera _cMachineVertualCamera;



    //Local Variables
    public float XPos;
    private bool IsPlaying;




    private void OnEnable()
    {
        IsPlaying = true;
        _cMachineVertualCamera.gameObject.SetActive(true);
        transform.parent = null;
        Rb.useGravity = true;
        Rb.velocity = new Vector3(0, StickController.FingerPos*0.5f , 1) *StickController.FingerPos* _ThrowForce * Time.deltaTime;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            GetInputs();
        }
    }

    private void FixedUpdate()
    {
        if (IsPlaying)
        {
            Move();
        }
    }

    private void Move()
    {
        Rb.AddForce( Vector3.right * XPos*_moveSpeed * Time.deltaTime);
    }

    private void GetInputs()
    {
        Touch touch= Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                XPos = touch.deltaPosition.normalized.x;
                break;

            case TouchPhase.Stationary:
                XPos = 0;
                break;

            case TouchPhase.Moved:
                XPos = touch.deltaPosition.normalized.x;
                break;

            case TouchPhase.Ended:
                XPos = 0;
                break;
        }
    }
}
