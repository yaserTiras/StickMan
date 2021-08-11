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
    private bool FingerOn;




    private void OnEnable()
    {
        PlayerAnimator.SetInteger("Wings", 1);
        _cMachineVertualCamera.gameObject.SetActive(true);
        transform.parent = null;
        Rb.useGravity = true;
        Rb.velocity = new Vector3(0, StickController.FingerPos*0.5f , 1) *StickController.FingerPos* _ThrowForce * Time.deltaTime;
    }

    private void Update()
    {
        if (transform.position.z % 10 == 0)
        {
            ObstaclesCreator.instance.MoveObsatclesNext();
        }
        if (Input.touchCount == 1)
        {
            GetInputs();
        }
    }

    private void FixedUpdate()
    {
        if (FingerOn)
        {
            MoveAndOpenWings();
        }
        else
        {
            StopMovingAndCloseWings();
        }
    }

    private void StopMovingAndCloseWings()
    {
        transform.Rotate(0, transform.rotation.y * 2,0);
        PlayerAnimator.SetInteger("Wings", 1);
    }

    private void MoveAndOpenWings()
    {
        PlayerAnimator.SetInteger("Wings", 2);
        transform.Rotate(0, 90, 0);
        Rb.AddForce( Vector3.right * XPos*_moveSpeed * Time.deltaTime);
    }

    private void GetInputs()
    {
        Touch touch= Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                FingerOn = true;
                XPos = touch.deltaPosition.normalized.x;
                break;

            case TouchPhase.Stationary:
                XPos = 0;
                break;

            case TouchPhase.Moved:
                XPos = touch.deltaPosition.normalized.x;
                break;

            case TouchPhase.Ended:
                FingerOn = false;
                XPos = 0;
                break;
        }
    }


}
