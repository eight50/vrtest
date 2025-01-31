using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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
    float maxDis;
    Vector2 leftStick;
    Vector2 rightStick;
    bool leftStickButton;
    Quaternion leftControllerRotation;
    Quaternion rightControllerRotation;
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
        maxDis = 23f;
    }

    // Update is called once per frame
    void Update() {
        //leftStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        //rightStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        var devices = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
        foreach (var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStick)) { }
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out leftControllerRotation)) { }
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftStickButton)) { }
        }

        desiredCharacteristics = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
        foreach (var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStick)) { }
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out rightControllerRotation)) { }
        }

        desiredCharacteristics = InputDeviceCharacteristics.HeadMounted;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
        foreach (var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out HeadMountedRotation)) { }
        }

        if (leftStickButton) {
            if (!isOrigin) {
                this.transform.position = Vector3.zero;
                _zoomCamera.transform.position = new Vector3(0, 0, 3f);
                _zoomCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
                _zoomPanel.transform.position = new Vector3(0, 0, 3f);
                _zoomPanel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                _outLine.transform.position = new Vector3(0, 0, 3.2f);
                _outLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
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
                // _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);
                // _zoomCamera.transform.rotation = Quaternion.Euler(changeRotation);

                // Vector3 rightRotation = new Vector3(rightControllerRotation.eulerAngles.x, rightControllerRotation.eulerAngles.y, 0);
                // _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(rightRotation) * changePosition);
                // _zoomCamera.transform.rotation = Quaternion.Euler(rightRotation);

                Vector3 leftRotation = new Vector3(leftControllerRotation.eulerAngles.x, leftControllerRotation.eulerAngles.y, 0);
                _zoomCamera.transform.position += _zoomCamera.transform.rotation * (Quaternion.Euler(leftRotation) * changePosition);
                _zoomCamera.transform.rotation = Quaternion.Euler(leftRotation);


                // zoomPanelの位置、　角度変更
                _zoomPanel.transform.position = _zoomPanel.transform.rotation * (Quaternion.Euler(changeRotation) * _zoomPanel.transform.position);
                _zoomPanel.transform.rotation = _zoomPanel.transform.rotation * Quaternion.Euler(changeRotation);
                _outLine.transform.position = _outLine.transform.rotation * (Quaternion.Euler(changeRotation) * _outLine.transform.position);
                _outLine.transform.rotation = _outLine.transform.rotation * Quaternion.Euler(changeRotation);
                _zoomPanel.SetActive(true);
                _outLine.SetActive(true);

                isOrigin = false;
                isRotation = true;
                // VRDisplay.RecenterPose();
                // VRManager.useRotationTracking = false;
            } else {
                if (leftStick.y != 0f) {
                    Vector3 changePosition = Vector3.zero;
                    if (leftStick.y > 0f) {
                        changePosition = new Vector3(0, 0, 0.3f);
                    } else {
                        changePosition = new Vector3(0, 0, -0.3f);
                    }

                    // this.transform.position += this.transform.rotation * (Quaternion.Euler(changeRotation) * changePosition);

                    // 原点のとの距離がmaxDis以下なら、ズームカメラを動かす
                    float r = Vector3.Distance(_zoomCamera.transform.position, Vector3.zero);
                    if (r < maxDis) {
                        _zoomCamera.transform.position += _zoomCamera.transform.rotation * changePosition;
                    }
                }

                    
                // 右スティックでパネル移動
                if (rightStick.x != 0f || rightStick.y != 0f) {
                    Vector3 changePosition = new Vector3(rightStick.x, -1.0f * rightStick.y, 0f);
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
    }
}
