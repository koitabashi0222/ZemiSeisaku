using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab; // 弾のPrefab
    public Transform bulletSpawnPoint; // 弾が発射される位置
    public Transform playerCamera; // プレイヤーのカメラをアタッチ
    public float shootInterval = 2f; // 弾を撃つ間隔
    public float bulletSpeed = 10f; // 弾の速度
    public float predictionTime = 0.5f; // プレイヤーの移動を予測する時間

    private bool isPlayerInRange = false;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 敵の向きをプレイヤーカメラ方向に調整
            Vector3 direction = playerCamera.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction); // 全方向の回転

            transform.parent.rotation = Quaternion.Slerp(
                transform.parent.rotation,
                targetRotation,
                Time.deltaTime * 5f // 回転速度
            );

            // 弾を撃つ準備
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
            isPlayerInRange = false; // 範囲外になったら弾を撃つのを停止
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
                // プレイヤーの速度を取得
                PlayerMovementTracker tracker = player.GetComponent<PlayerMovementTracker>();
                Vector3 playerVelocity = tracker != null ? tracker.CurrentVelocity : Vector3.zero;

                // 弾が到達するまでの時間を計算
                float travelTime = Vector3.Distance(bulletSpawnPoint.position, playerCamera.position) / bulletSpeed;

                // プレイヤーの予測位置を計算（Z軸方向に補正を加える）
                Vector3 predictedPosition = playerCamera.position + playerVelocity * travelTime;
                predictedPosition.z += 1.0f; // Z軸方向に補正（例：1.0fを適宜調整）

                // 弾を予測位置方向に飛ばす
                Vector3 bulletDirection = (predictedPosition - bulletSpawnPoint.position).normalized;
                rb.velocity = bulletDirection * bulletSpeed;

            }

            yield return new WaitForSeconds(shootInterval);
        }
    }


}
