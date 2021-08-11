using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    private Animator StickAnimController { get { return GetComponent<Animator>(); } }

    [Range(0, 1)]
    [SerializeField] private float TouchPos;
    [Range(2,4)] [Tooltip("Higher value means lower swipe distance for player")]
    [SerializeField] private float _swipeDistanceDevider;
    [SerializeField] private float _maxSwipeDistance;
    [SerializeField] private float _swipeSpeed;
    [SerializeField] private float _difference;
    [SerializeField] private float _resetStickDistance;

    public static float FingerPos;


    //temporary Values
    float StartPos = 0;



    private void Awake()
    {
        _maxSwipeDistance = Screen.height / _swipeDistanceDevider;
    }

    private void Update()
    {

        if (Input.touchCount == 1)
            GetInputs();
    }

    private void GetInputs()
    {
        Touch touch=Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                StartPos = touch.position.x;
                _difference = touch.position.x - StartPos;
                TouchPos = _difference / _maxSwipeDistance;
                break;

            case TouchPhase.Stationary:

                break;

            case TouchPhase.Moved:
                _difference = touch.position.x - StartPos;
                TouchPos = _difference / _maxSwipeDistance; 
                StickAnimController.SetFloat("TouchPos", TouchPos);
                break;

            case TouchPhase.Ended:
                _difference = 0;
                if (TouchPos > _resetStickDistance)
                {
                    StickAnimController.SetTrigger("Throw");
                    StickAnimController.speed = TouchPos*1.5f;
                    FingerPos = TouchPos;
                    this.enabled = false;
                }
                else
                {
                    StartCoroutine(ReturnStickToStandardPos(TouchPos));
                }
                break;
        }
    }

    private IEnumerator ReturnStickToStandardPos(float t)
    {
        while (t > 0)
        {
            TouchPos -=Time.deltaTime;
            StickAnimController.SetFloat("TouchPos", TouchPos);
            t -= Time.deltaTime;
            yield return null;
        }
        
    }

    
}
