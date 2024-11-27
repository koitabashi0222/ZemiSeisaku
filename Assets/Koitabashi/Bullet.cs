using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 弾の速度
    public GameObject shooter; // 発射元のEnemyShooter
    private bool isReflected = false; // 跳ね返しフラグ

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "katana" && !isReflected)
        {
            // 発射元（shooter）が存在する場合、その方向へ跳ね返す
            if (shooter != null)
            {
                Vector3 reflectedDirection = (shooter.transform.position - transform.position).normalized;
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.velocity = reflectedDirection * speed;

                transform.rotation = Quaternion.LookRotation(reflectedDirection);
                isReflected = true;
            }
        }
        else if (other.gameObject.tag == "Enemy" && isReflected)
        {
            GameObject parent = other.transform.parent.gameObject; // 親オブジェクトを取得

            if (parent != null)
            {
                Destroy(parent); // 親オブジェクトを削除
            }
            Destroy(gameObject); // 弾を削除
        }
    }
}
