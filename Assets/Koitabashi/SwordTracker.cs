using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTracker : MonoBehaviour
{
    private Vector3 previousPosition; // �O�t���[���̈ʒu
    public float currentSpeed; // ���̑��x

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // ���݂̑��x���v�Z
        currentSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;
    }
}
