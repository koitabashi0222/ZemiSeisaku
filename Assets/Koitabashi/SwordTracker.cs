using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTracker : MonoBehaviour
{
    private Vector3 previousPosition; // 前フレームの位置
    public float currentSpeed; // 刀の速度

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // 現在の速度を計算
        currentSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;
    }
}
