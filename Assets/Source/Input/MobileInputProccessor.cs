using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeonGesture
{
    public class MobileInputProccessor : MonoBehaviour
    {
        private static MobileInputProccessor instance;

        public static IEnumerable<Touch>   Touches  => instance.touches;
        public static IEnumerable<Gesture> Gestures => instance.gestures;

        public List<Touch>   touches ;
        
        public List<Touch>   unproccessedTouches;
        public List<Gesture> gestures;

        public void Start()
        {
            if (instance != null) 
            {
                Destroy(gameObject);
                Debug.LogWarning("Multiple instances of MobileInputProccessor found. Destroying the new one.");
                return;
            }

            instance = this;
        }

        public void Update()
        {
            touches.ForEach(touch => touch.UpdateTouch());
    
            var touchesToRemove = touches.FindAll(touch => touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled);

            touchesToRemove.ForEach(touch => touches            .Remove(touch));
            touchesToRemove.ForEach(touch => unproccessedTouches.Remove(touch));
            gestures.RemoveAll(gesture => gesture.IsComplete);

            for (int i = 0; i < Input.touchCount; i++)
            {
                UnityEngine.Touch touch = Input.GetTouch(i);

                if (touch.phase != TouchPhase.Began) continue;
                
                Touch newTouch = new Touch(i);
                touches.Add(newTouch);
                unproccessedTouches.Add(newTouch);
            }

            List<Touch> processedTouches = new();
            foreach (Touch touch in unproccessedTouches)
            {
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) 
                {
                    if (touch.positionDelta.magnitude > 0) gestures.Add(new SwipeGesture(touch));
                    else if (touch.holdTime < 0.5f) gestures.Add(new TapGesture(touch));
                    else Debug.Log("What the" + touch.holdTime);
                    processedTouches.Add(touch);
                }

                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) 
                {
                    if (touch.holdTime >= 0.5f) 
                    {
                        gestures.Add(new HoldGesture(touch));
                        processedTouches.Add(touch);
                    }
                }
                else
                {
                    Debug.Log("unproccessed");
                }
            }
            processedTouches.ForEach(touch => unproccessedTouches.Remove(touch));

            List<PinchGesture> pinchGestures = new();

            gestures.ForEach(gestureA => {
                if (gestureA is not HoldGesture holdGestureA) return;
                if (gestureA.RelatedGestures.Count > 0) return;
                
                if (pinchGestures.Any(pinchGesture => pinchGesture.HoldGestureB == holdGestureA)) return;

                gestures.ForEach(gestureB => {
                    if (gestureB is not HoldGesture holdGestureB) return;
                    if (gestureA == gestureB) return;
                    if (gestureB.RelatedGestures.Count > 0) return;
    
                    //pinchgesture
                    pinchGestures.Add(new PinchGesture(holdGestureA, holdGestureB));
                    holdGestureA.RelatedGestures.Add(pinchGestures.Last());
                    holdGestureB.RelatedGestures.Add(pinchGestures.Last());
                });
            });

            gestures.AddRange(pinchGestures);
        }
 
        public void LateUpdate()
        {

        }
    }
}