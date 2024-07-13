using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeonGesture
{
    [Serializable]
    public class Gesture
    {
        public Vector2 startPosition;
        public Touch touch;

        public bool IsComplete => touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled || RelatedGestures.Any(gesture => gesture.IsComplete);

        public List<Gesture> RelatedGestures = new();

        public Gesture(Touch touch)
        {
            this.touch = touch;
            this.startPosition = touch.startPosition;      
        }

        public void CopyOver(Gesture gesture)
        {
            startPosition = gesture.startPosition;
            touch         = gesture.touch        ;
        }
    }

    public class TapGesture : Gesture
    {
        public TapGesture(Touch touch) : base(touch)
        {
            Debug.Log("TapGesture - Construct");
        }
    }

    public class HoldGesture : Gesture
    {
        public SwipeDirection Horizontal  { get; set; }
        public SwipeDirection Vertical    { get; set; }
        public Vector2 Delta => touch.position - startPosition;
        public float Distance => Vector2.Distance(startPosition, touch.position);

        public HoldGesture(Touch touch) : base(touch)
        {
        }
    }

    public enum SwipeDirection { None, Left, Right, Up, Down }

    public class SwipeGesture : Gesture
    {
        public SwipeDirection Horizontal  { get; set; }
        public SwipeDirection Vertical    { get; set; }
        public Vector2        EndPosition { get; set; }

        public Vector2 Delta => EndPosition - startPosition;
        public float Distance => Vector2.Distance(startPosition, EndPosition);
     
        public SwipeGesture(Touch touch) : base(touch)
        {
            EndPosition = touch.position;

            Horizontal = Delta.x > 0.5f ? SwipeDirection.Right : Delta.x < -0.5f ? SwipeDirection.Left : SwipeDirection.None;
            Vertical   = Delta.y > 0.5f ? SwipeDirection.Up    : Delta.y < -0.5f ? SwipeDirection.Down : SwipeDirection.None;

        }
    }

    public class PinchGesture : Gesture
    {
        public float Delta { get; set; }

        public HoldGesture HoldGestureA { get; set; }
        public HoldGesture HoldGestureB { get; set; }

        public PinchGesture(HoldGesture holdGestureA, HoldGesture holdGestureB) : base(holdGestureA.touch)
        {
        }
    }
}