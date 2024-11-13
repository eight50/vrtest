using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

// OVRCameraRigにアタッチする
public class CameraHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // returns a Vector2 of the primary (typically the Left) thumbstick’s current state.
        // (X/Y range of -1.0f to 1.0f)
        var leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        var dis = 1f;


        Vector2 leftStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        Vector3 changePosition = new Vector3((leftStick.x), 0, (leftStick.y));
        //HMDのY軸の角度取得
        Vector3 changeRotation = new Vector3(0, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.y, 0);
        //OVRCameraRigの位置変更
        this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);


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
