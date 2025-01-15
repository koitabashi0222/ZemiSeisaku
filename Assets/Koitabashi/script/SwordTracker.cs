using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTracker : MonoBehaviour
{
    public Transform centerEyeAnchor; // OVRCameraRig��CenterEyeAnchor���A�T�C��
    private Vector3 previousLocalPosition; // �O�t���[���̃��[�J�����W
    public float currentSpeed; // ���[�J�����W�ł̓��̑��x

    void Start()
    {
        if (centerEyeAnchor == null)
        {
            Debug.LogError("CenterEyeAnchor �����ݒ�ł��BOVRCameraRig �� CenterEye ��ݒ肵�Ă��������B");
            return;
        }

        // �������[�J���ʒu���L�^
        previousLocalPosition = centerEyeAnchor.InverseTransformPoint(transform.position);
    }

    void Update()
    {
        if (centerEyeAnchor == null) return;

        // ���݂̃��[�J���ʒu���擾
        Vector3 currentLocalPosition = centerEyeAnchor.InverseTransformPoint(transform.position);

        // ���[�J�����W�ł̑��x���v�Z
        currentSpeed = (currentLocalPosition - previousLocalPosition).magnitude / Time.deltaTime;

        // ���݂̃��[�J���ʒu�����t���[���̂��߂ɋL�^
        previousLocalPosition = currentLocalPosition;
    }
}
