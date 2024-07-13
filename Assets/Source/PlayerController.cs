using System.Linq;
using NeonGesture;
using UnityEngine;

namespace Neon
{
    public class PlayerController : MonoBehaviour
    {
        public void Update()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended) Debug.Log("TouchPhase.Began");
            }

            foreach (Gesture gesture in MobileInputProccessor.Gestures)
            {
                if (gesture is TapGesture tapGesture)
                {
                    Debug.Log("TapGesture");
                }
                else if (gesture is HoldGesture holdGesture)
                {
                    Debug.Log("HoldGesture");
                }
                else if (gesture is SwipeGesture swipeGesture)
                {
                    Debug.Log("SwipeGesture " + swipeGesture.Horizontal + " " + swipeGesture.Vertical);
                } 
                else if (gesture is PinchGesture pinchGesture)
                {
                    Debug.Log("PinchGesture");
                }
            }
        }
    }
}
