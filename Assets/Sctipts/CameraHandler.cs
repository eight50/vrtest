using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

// OVRCameraRigにアタッチする
public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Camera _zoomCamera;
    [SerializeField] private GameObject _zoomPanel;
    [SerializeField] private GameObject _outLine;

    OVRManager VRManager;
    OVRDisplay VRDisplay;
    bool isOrigin;
    bool isRotation;
    Vector3 changeRotation;
    Vector2 leftStick;
    Vector2 rightStick;

    // Start is called before the first frame update
    void Start()
    {
        _zoomCamera.enabled = true;
        _zoomPanel.SetActive(false);
        _outLine.SetActive(false);
        // OVRManegerのシングルトンオブジェクトを取得
        VRManager = OVRManager.instance;
        VRDisplay = OVRManager.display;
        isOrigin = true;
        isRotation = false;
        changeRotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        leftStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        rightStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        Debug.Log("x:" + leftStick.x + ", y:" + leftStick.y);

        if(leftStick.x == 0 && leftStick.y == 0) {
            if(!isOrigin) {
                this.transform.position = Vector3.zero;
                _zoomCamera.transform.position = Vector3.zero;
                _zoomPanel.transform.position = new Vector3(0, 0, 3);
                _outLine.transform.position = new Vector3(0, 0, 3.2f);
                isOrigin = true;
                isRotation = false;
                _zoomPanel.SetActive(false);
                _outLine.SetActive(false);
                VRManager.useRotationTracking = true;             
            }
        } else {
            if(!isRotation) {
                Vector3 changePosition = new Vector3(0, 0, leftStick.y);
                // HMDのx,y,z軸の角度取得
                changeRotation = new Vector3(InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.x, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.y, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.z);

                // OVRCameraRigの位置変更、zoomCameraで見る場合は移動しない
                // this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);

                // zoomCameraの位置変更
                _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
                

                // zoomPanelの位置、　角度変更
                _zoomPanel.transform.position = _zoomPanel.transform.rotation * (Quaternion.Euler(changeRotation) * _zoomPanel.transform.position);
                _zoomPanel.transform.rotation = Quaternion.Euler(changeRotation);
                _outLine.transform.position = _outLine.transform.rotation * (Quaternion.Euler(changeRotation) * _outLine.transform.position);
                _outLine.transform.rotation = Quaternion.Euler(changeRotation);
                _zoomPanel.SetActive(true);
                _outLine.SetActive(true);

                isOrigin = false;
                isRotation = true;
                // VRDisplay.RecenterPose();
                // VRManager.useRotationTracking = false;
            } else {
                Vector3 changePosition = new Vector3(0, 0, leftStick.y);

                // this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);

                _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
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
