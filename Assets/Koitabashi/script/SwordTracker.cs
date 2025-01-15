using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTracker : MonoBehaviour
{
    public Transform centerEyeAnchor; // OVRCameraRigのCenterEyeAnchorをアサイン
    private Vector3 previousLocalPosition; // 前フレームのローカル座標
    public float currentSpeed; // ローカル座標での刀の速度

    void Start()
    {
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor が未設定です。OVRCameraRig の CenterEye を設定してください。");
            return;
        }

        // 初期ローカル位置を記録
        previousLocalPosition = centerEyeAnchor.InverseTransformPoint(transform.position);
    }

    void Update()
    {
        if (centerEyeAnchor == null) return;

        // 現在のローカル位置を取得
        Vector3 currentLocalPosition = centerEyeAnchor.InverseTransformPoint(transform.position);

        // ローカル座標での速度を計算
        currentSpeed = (currentLocalPosition - previousLocalPosition).magnitude / Time.deltaTime;

        // 現在のローカル位置を次フレームのために記録
        previousLocalPosition = currentLocalPosition;
    }
}
