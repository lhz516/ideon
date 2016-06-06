using UnityEngine;
using System;

public class IdeonGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    [Tooltip("GUI-Text to display gesture-listener messages and gesture information.")]
    public GUIText gestureInfo;

    // singleton instance of the class
    private static IdeonGestureListener instance = null;

    // internal variables to track if progress message has been displayed
    private bool progressDisplayed;
    private float progressGestureTime;

    // whether the needed gesture has been detected or not
    private bool leanLeft;
    private bool leanRight;
    private bool swipeLeft;
    private bool leanLeftWithHip;
    private bool leanRightWithHip;
    private bool ShoulderLeftFront;
    private bool ShoulderRightFront;
    private bool zoomIn;
    private bool zoomOut;

    // if current user exist
    private bool user;

    // Gets the singleton CubeGestureListener instance.
    public static IdeonGestureListener Instance
    {
        get
        {
            return instance;
        }
    }

    // Determines whether lean left is detected.
    public bool IsLeanLeft()
    {
        if (leanLeft)
        {
            leanLeft = false;
            return true;
        }

        return false;
    }

    // Determines whether lean right is detected.
    public bool IsLeanRight()
    {
        if (leanRight)
        {
            leanRight = false;
            return true;
        }

        return false;
    }

    public bool IsSwipeLeft()
    {
        if (swipeLeft)
        {
            swipeLeft = false;
            return true;
        }

        return false;
    }

    public bool IsShoulderLeftFront()
    {
        if (ShoulderLeftFront)
        {
            ShoulderLeftFront = false;
            return true;
        }

        return false;
    }

    public bool IsShoulderRightFront()
    {
        if (ShoulderRightFront)
        {
            ShoulderRightFront = false;
            return true;
        }

        return false;
    }

    public bool IsZoomIn()
    {
        if (zoomIn)
        {
            zoomIn = false;
            return true;
        }

        return false;
    }

    public bool IsZoomOut()
    {
        if (zoomOut)
        {
            zoomOut = false;
            return true;
        }

        return false;
    }
    // Invoked when a new user is detected. start gesture tracking by KinectManager.DetectGesture()
    public void UserDetected(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        // detect these user specific gestures
        manager.DetectGesture(userId, KinectGestures.Gestures.LeanLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.LeanRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.ShoulderLeftFront);
        manager.DetectGesture(userId, KinectGestures.Gestures.ShoulderRightFront);
        manager.DetectGesture(userId, KinectGestures.Gestures.ZoomIn);
        manager.DetectGesture(userId, KinectGestures.Gestures.ZoomOut);

        if (gestureInfo != null)
        {
            gestureInfo.GetComponent<GUIText>().text = "Lean left, right to change the video.";
        }
        user = true;
    }

    // Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
    public void UserLost(long userId, int userIndex)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;
        if (gestureInfo != null)
        {
            gestureInfo.GetComponent<GUIText>().text = string.Empty;
        }
        user = false;
    }

    public bool currentUser()
    {
        return user;
    }

    // Invoked when a gesture is in progress.
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return;

        // this function is currently needed only to display gesture progress, skip it otherwise
        if (gestureInfo == null)
            return;

        if ((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                if (gesture == KinectGestures.Gestures.ZoomIn)
                {
                    string GestureText = "Zoom in";
                    gestureInfo.GetComponent<GUIText>().text = GestureText;
                    zoomIn = true;
                }
                else if (gesture == KinectGestures.Gestures.ZoomOut)
                {
                    string GestureText = "Zoom out";
                    gestureInfo.GetComponent<GUIText>().text = GestureText;
                    zoomOut = true;
                }
                //string sGestureText = string.Format("{0} - {1:F0}%", gesture, screenPos.z * 100f);
                //gestureInfo.GetComponent<GUIText>().text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        else if ((gesture == KinectGestures.Gestures.Wheel || gesture == KinectGestures.Gestures.LeanLeft ||
                 gesture == KinectGestures.Gestures.LeanRight) && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = string.Format("{0} - {1:F0} degrees", gesture, screenPos.z);
                gestureInfo.GetComponent<GUIText>().text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;
            }
        }
        if (gesture == KinectGestures.Gestures.LeanLeft)
        {
            leanLeft = true;
            leanRight = false;
        }
            
        else if (gesture == KinectGestures.Gestures.LeanRight)
        {
            leanRight = true;
            leanLeft = false;
            
        }

        if(gesture == KinectGestures.Gestures.ShoulderLeftFront)
        {
            //string sGestureText = "Shoulder Left Front";
            //gestureInfo.GetComponent<GUIText>().text = sGestureText;
        }
        else if (gesture == KinectGestures.Gestures.ShoulderRightFront)
        {
            //string sGestureText = "Shoulder Right Front";
            //gestureInfo.GetComponent<GUIText>().text = sGestureText;
        }


    }

    // Invoked if a gesture is completed. NOT work for lean left or lean right
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        //Debug.Log("completed");
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false; 

        if (gestureInfo != null)
        {
            string sGestureText = gesture + " detected";
            gestureInfo.GetComponent<GUIText>().text = sGestureText;
        }

        if (gesture == KinectGestures.Gestures.SwipeLeft)
            swipeLeft = true;
        else
            swipeLeft = false;
        return true;
    }

    // Invoked if a gesture is cancelled.
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        // the gestures are allowed for the primary user only
        KinectManager manager = KinectManager.Instance;
        if (!manager || (userId != manager.GetPrimaryUserID()))
            return false;

        if (progressDisplayed)
        {
            progressDisplayed = false;

            if (gestureInfo != null)
            {
                gestureInfo.GetComponent<GUIText>().text = String.Empty;
            }
        }

        return true;
    }

    public void ResetGesture()
    {
        swipeLeft = false;
    }

     void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (progressDisplayed && ((Time.realtimeSinceStartup - progressGestureTime) > 2f))
        {
            progressDisplayed = false;
            gestureInfo.GetComponent<GUIText>().text = String.Empty;

            Debug.Log("Progress ends .");
        }
    }

}
