using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastController : MonoBehaviour
{
    public Transform player;
    public GameObject[] targetObjects; // �Ď�����I�u�W�F�N�g�i2�ݒ肷��j
    public float moveSpeed = 5f;

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

    private bool AreAllTargetsDestroyed()
    {
        foreach (GameObject target in targetObjects)
        {
            if (target != null && target.activeInHierarchy) return false; // �I�u�W�F�N�g���j��܂��͔�A�N�e�B�u�łȂ��ꍇ�͈ړ����Ȃ�
        }
        return true; // ���ׂĔj�󂳂�Ă�����
    }

    private void MoveTowardsPlayer()
    {
        // �v���C���[�ւ̕���
        Vector3 direction = (player.position - transform.position).normalized;

        // �ړ��x�N�g��
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}