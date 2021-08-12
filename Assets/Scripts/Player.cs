using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    private Animator PlayerAnimator { get { return GetComponent<Animator>(); } }
    private Rigidbody Rb { get { return GetComponent<Rigidbody>(); } }

    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _forwardMoveSpeed = 1;
    [SerializeField] private float _ThrowForce = 10;
    [SerializeField] private CinemachineVirtualCamera _cMachineVertualCamera;
    [SerializeField] private Transform PlaneTransform;


    //Local Variables
    public float XPos;
    private bool FingerOn;
    Vector3 planePos;


    private void Awake()
    {
        planePos.y = PlaneTransform.position.y;
        planePos.x = PlaneTransform.position.x;
    }
    private void OnEnable()
    {

        FingerOn = false;
        StartCoroutine(StopMovingAndCloseWings());
        PlayerAnimator.SetInteger("Wings", 1);
        _cMachineVertualCamera.gameObject.SetActive(true);
        transform.parent = null;
        Rb.useGravity = true;
        Rb.velocity = new Vector3(0, StickController.FingerPos * 0.5f, 1) * StickController.FingerPos * _ThrowForce * Time.deltaTime;
        transform.Rotate(90, 0, 0);
    }

    private void Update()
    {
        planePos.z = transform.position.z;
        
        PlaneTransform.position =planePos ;
        ObstaclesCreator.instance.DistanceCalculator(transform.position.z);
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            StartCoroutine(GameOver(0.2f));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        


        if (other.CompareTag("1") || other.CompareTag("2"))
        {
            int ColForce = Int16.Parse(other.tag);
            Rb.AddForce(Vector3.up * 2000 * ColForce);
        }
    }

    private void GetInputs()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                Rb.useGravity = false;
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
                Rb.useGravity = true;
                StartCoroutine(StopMovingAndCloseWings());
                XPos = 0;
                break;
        }
    }


    private void MoveAndOpenWings()
    {

        PlayerAnimator.SetInteger("Wings", 2);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, XPos * 45, 0), Time.deltaTime);
        
        Rb.AddForce(new Vector3(XPos * _moveSpeed, 0, 0) * Time.deltaTime);
    }

    private IEnumerator StopMovingAndCloseWings()
    {
        PlayerAnimator.SetInteger("Wings", 1);
        transform.rotation = Quaternion.Euler(90, 0, 0);
        float sp = 3;
        while (!FingerOn)
        {
            transform.Rotate(0, sp * 5, 0);
            yield return null;
        }
    }

    private IEnumerator GameOver(float t)
    {
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        GameManager.instance.RestartGame();
    }
}
