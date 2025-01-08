using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastController : MonoBehaviour
{
    public Transform player; // �v���C���[��Transform
    public GameObject[] targetObjects; // �Ď�����I�u�W�F�N�g�i2�ݒ肷��j
    public float moveSpeed = 5f; // �ړ����x

    private bool shouldMoveToPlayer = false; // �v���C���[�Ɍ������Ĉړ����邩�ǂ���

    void Update()
    {
        // �Ď�����I�u�W�F�N�g�����ׂĔj�󂳂�Ă��邩�`�F�b�N
        if (!shouldMoveToPlayer && AreAllTargetsDestroyed())
        {
            shouldMoveToPlayer = true; // �v���C���[�Ɍ������Ĉړ��J�n
        }

        // �v���C���[�Ɍ������Ĉړ�
        if (shouldMoveToPlayer && player != null)
        {
            MoveTowardsPlayer();
        }
    }

    // �Ď�����I�u�W�F�N�g�����ׂĔj�󂳂�Ă��邩����
    private bool AreAllTargetsDestroyed()
    {
        foreach (GameObject target in targetObjects)
        {
            if (target != null) return false; // �I�u�W�F�N�g�����݂���ꍇ�͈ړ����Ȃ�
        }
        return true; // ���ׂĔj�󂳂�Ă�����true
    }

    // �v���C���[�Ɍ������Ĉړ�����
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized; // �v���C���[�ւ̕���
        transform.position += direction * moveSpeed * Time.deltaTime; // �v���C���[�ֈړ�
    }
}
