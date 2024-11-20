using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

// OVRCameraRigにアタッチする
public class CameraHandler : MonoBehaviour
{
    OVRManager VRManager;
    bool isOrigin;
    bool isRotation;
    Vector3 changeRotation;
    Vector2 leftStick;

    // Start is called before the first frame update
    void Start()
    {
        VRManager = OVRManager.instance;
        isOrigin = true;
        isRotation = false;
        changeRotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        leftStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        Debug.Log("x:" + leftStick.x + ", y:" + leftStick.y);

        if(leftStick.x == 0 && leftStick.y == 0) {
            if(!isOrigin) {
                this.transform.position = Vector3.zero;
                isOrigin = true;
                isRotation = false;
                VRManager.useRotationTracking = true;             
            }
        } else {
            if(!isRotation) {
                Vector3 changePosition = new Vector3(0, 0, leftStick.y);
                // HMDのx,y,z軸の角度取得
                changeRotation = new Vector3(InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.x, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.y, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.z);
                // OVRCameraRigの位置変更
                this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
                isOrigin = false;
                isRotation = true;
                OVRManager.display.RecenterPose();
                VRManager.useRotationTracking = false;
            } else {
                Vector3 changePosition = new Vector3(0, 0, leftStick.y);
                this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
            }
        }


        // returns a Vector2 of the primary (typically the Left) thumbstick’s current state.
        // (X/Y range of -1.0f to 1.0f)
        // var leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); 

        /*if (leftInput.y >= 0.5f) {
            this.transform.position += new Vector3(0f, 0f, dis);
        } else if (leftInput.y <= -0.5f) {
            this.transform.position -= new Vector3(0f, 0f, dis);
        }

        if (leftInput.x >= 0.5f) {
            this.transform.position += new Vector3(dis, 0f, 0f);
        } else if(leftInput.x <= -0.5f) {
            this.transform.position -= new Vector3(dis, 0f, 0f);
        }
        */

    }
}
