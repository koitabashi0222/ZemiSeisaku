using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab; // �e��Prefab
    public Transform bulletSpawnPoint; // �e�����˂����ʒu
    public Transform playerCamera; // �v���C���[�̃J�������A�^�b�`
    public float shootInterval = 2f; // �e�����Ԋu
    public float bulletSpeed = 10f; // �e�̑��x
    public float predictionTime = 0.5f; // �v���C���[�̈ړ���\�����鎞��

    private bool isPlayerInRange = false;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // �G�̌������v���C���[�J���������ɒ���
            Vector3 direction = playerCamera.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction); // �S�����̉�]

            transform.parent.rotation = Quaternion.Slerp(
                transform.parent.rotation,
                targetRotation,
                Time.deltaTime * 5f // ��]���x
            );

            // �e��������
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                StartCoroutine(ShootAtPlayer(other));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInRange = false; // �͈͊O�ɂȂ�����e�����̂��~
        }
    }

    IEnumerator ShootAtPlayer(Collider player)
    {
        while (isPlayerInRange)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // �v���C���[�̑��x���擾
                PlayerMovementTracker tracker = player.GetComponent<PlayerMovementTracker>();
                Vector3 playerVelocity = tracker != null ? tracker.CurrentVelocity : Vector3.zero;

                // �e�����B����܂ł̎��Ԃ��v�Z
                float travelTime = Vector3.Distance(bulletSpawnPoint.position, playerCamera.position) / bulletSpeed;

                // �v���C���[�̗\���ʒu���v�Z�iZ�������ɕ␳��������j
                Vector3 predictedPosition = playerCamera.position + playerVelocity * travelTime;
                predictedPosition.z += 1.0f; // Z�������ɕ␳�i��F1.0f��K�X�����j

                // �e��\���ʒu�����ɔ�΂�
                Vector3 bulletDirection = (predictedPosition - bulletSpawnPoint.position).normalized;
                rb.velocity = bulletDirection * bulletSpeed;

            }

            yield return new WaitForSeconds(shootInterval);
        }
    }


}
