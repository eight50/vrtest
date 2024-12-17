using System.Collections.Generic;
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
    Vector3 currentRotation;
    int MaxDis = 10;
    Vector2 leftStick;
    Vector2 rightStick;  
    Quaternion HeadMountedRotation;

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
        currentRotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        //leftStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        //rightStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        var devices = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
        foreach(var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick)) { }
        }
        
      
        desiredCharacteristics = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
        foreach (var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick)) { }
        }

        desiredCharacteristics = InputDeviceCharacteristics.HeadMounted;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
        foreach (var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out HeadMountedRotation)) { }
        }




        Debug.Log("x:" + leftStick.x + ", y:" + leftStick.y);

        if (leftStick.x == 0 && leftStick.y == 0) {
            if (!isOrigin) {
                this.transform.position = Vector3.zero;
                _zoomCamera.transform.position = Vector3.zero;
                _zoomCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
                _zoomPanel.transform.position = new Vector3(0, 0, 3f);
                _zoomPanel.transform.rotation = Quaternion.Euler(Vector3.zero);
                _outLine.transform.position = new Vector3(0, 0, 3.2f);
                _outLine.transform.rotation = Quaternion.Euler(Vector3.zero);
                isOrigin = true;
                isRotation = false;
                _zoomPanel.SetActive(false);
                _outLine.SetActive(false);
                VRManager.useRotationTracking = true;
            }
        } else {
            if (!isRotation) {
                Vector3 changePosition = new Vector3(0, 0, leftStick.y);
                // HMDのx,y,z軸の角度取得
                changeRotation = new Vector3(HeadMountedRotation.eulerAngles.x, HeadMountedRotation.eulerAngles.y, HeadMountedRotation.eulerAngles.z);

                currentRotation = changeRotation;

                // OVRCameraRigの位置変更、zoomCameraで見る場合は移動しない
                // this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);

                // zoomCameraの位置変更
                _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
                _zoomCamera.transform.rotation = Quaternion.Euler(changeRotation);


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

                if (_zoomCamera.transform.position.x < MaxDis
                    &&_zoomCamera.transform.position.y < MaxDis
                    &&_zoomCamera.transform.position.z < MaxDis) {
                    _zoomCamera.transform.position += _zoomCamera.transform.rotation * changePosition;
                }
                    
                // 右スティックでパネル移動
                if (rightStick.x != 0f || rightStick.y != 0f) {
                    changePosition = new Vector3(rightStick.x, rightStick.y, 0);
                    // _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
                    _zoomCamera.transform.RotateAround(Vector3.zero, Vector3.up, changePosition.x);
                    _zoomCamera.transform.RotateAround(Vector3.zero, Vector3.right, changePosition.y);
                    // _zoomPanel.transform.position += _zoomPanel.transform.rotation * (Quaternion.Euler(changeRotation) * _zoomPanel.transform.position);
                    _zoomPanel.transform.RotateAround(Vector3.zero, Vector3.up, changePosition.x);
                    _zoomPanel.transform.RotateAround(Vector3.zero, Vector3.right, changePosition.y);
                    // _outLine.transform.position += _outLine.transform.rotation * (Quaternion.Euler(changeRotation) * _outLine.transform.position);
                    _outLine.transform.RotateAround(Vector3.zero, Vector3.up, changePosition.x);
                    _outLine.transform.RotateAround(Vector3.zero, Vector3.right, changePosition.y);
                }
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
