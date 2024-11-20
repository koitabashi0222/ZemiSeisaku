using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 弾の速度
    private bool isReflected = false; // 跳ね返しフラグ

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "katana" && !isReflected)
        {
            // 刀の向きと敵の方向を考慮して跳ね返す
            Vector3 reflectedDirection = CalculateReflectedDirection(collision);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = reflectedDirection * speed;

            transform.rotation = Quaternion.LookRotation(reflectedDirection);
            isReflected = true;
        }
        else if (collision.gameObject.tag == "Enemy" && isReflected)
        {
            // 敵にダメージ処理（仮の例）
            Destroy(collision.gameObject); // 敵を消す
            Destroy(gameObject); // 弾も消す
        }
    }

    // 跳ね返る方向を計算する
    Vector3 CalculateReflectedDirection(Collision collision)
    {
        // 刀の向き
        Vector3 katanaDirection = collision.gameObject.transform.forward;

        // 衝突した弾の現在位置と敵の位置
        Vector3 bulletPosition = transform.position;
        GameObject enemy = FindClosestEnemy(); // 最も近い敵を探す
        Vector3 enemyDirection = (enemy.transform.position - bulletPosition).normalized;

        // 敵の方向をベースに、刀の向きを少し加味する
        float influence = 0.3f; // 刀の影響度（0～1で調整）
        Vector3 reflectedDirection = Vector3.Lerp(enemyDirection, katanaDirection, influence).normalized;

        return reflectedDirection;
    }

    // 最も近い敵を探す
    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    // 弾を反射させる
    void ReflectBullet(Vector3 collisionNormal)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // 反射方向を計算
        Vector3 reflectedDirection = Vector3.Reflect(rb.velocity.normalized, collisionNormal);

        // 速度と向きを更新
        rb.velocity = reflectedDirection * speed;

        // 弾の向きを更新
        transform.rotation = Quaternion.LookRotation(reflectedDirection);
    }
}
