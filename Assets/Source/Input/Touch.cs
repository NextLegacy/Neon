using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeonGesture
{
    [Serializable]
    public class Touch
    {
        public int touchId;

        public int        fingerId               ;
        public Vector2    position               ;
        public Vector2    rawPosition            ;
        public Vector2    positionDelta          ;
        public float      timeDelta              ;
        public int        tapCount               ;
        public TouchPhase phase                  ;
        public TouchType  type                   ;
        public float      pressure               ;
        public float      maximumPossiblePressure;
        public float      radius                 ;
        public float      radiusVariance         ;
        public float      altitudeAngle          ;
        public float      azimuthAngle           ;

        public Vector2    startPosition;
        public float      holdTime;

        public Touch(int touchId) 
        {
            this.touchId = touchId;
            this.startPosition = Input.GetTouch(touchId).position;
            UpdateTouch();
        }

        public void UpdateTouch() 
        {
            if (touchId >= Input.touchCount) { phase = TouchPhase.Ended; return; }

            UnityEngine.Touch touch = Input.GetTouch(touchId);

            fingerId                = touch.fingerId               ;
            position                = touch.position               ;
            rawPosition             = touch.rawPosition            ;
            positionDelta           = touch.deltaPosition          ;
            timeDelta               = touch.deltaTime              ;
            tapCount                = touch.tapCount               ;
            phase                   = (TouchPhase)touch.phase      ;
            type                    = (TouchType)touch.type        ;
            pressure                = touch.pressure               ;
            maximumPossiblePressure = touch.maximumPossiblePressure;
            radius                  = touch.radius                 ;
            radiusVariance          = touch.radiusVariance         ;
            altitudeAngle           = touch.altitudeAngle          ;
            azimuthAngle            = touch.azimuthAngle           ;

            if (phase == TouchPhase.Began)
            {
                holdTime = 0;
                return;
            }
            if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
            {
                return;
            }

            holdTime += Time.deltaTime;
        }
    }
}