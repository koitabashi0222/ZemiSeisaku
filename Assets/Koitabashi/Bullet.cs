using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // 弾の速度
    public GameObject shooter; // 発射元のEnemyShooter
    private bool isReflected = false; // 跳ね返しフラグ
    [Header("刀で斬る時の速度閾値")]
    [SerializeField] float katanaSpeed = 3.0f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "katana" && !isReflected)
        {
            SwordTracker swordTracker = other.GetComponent<SwordTracker>();
            if (swordTracker != null && swordTracker.currentSpeed > katanaSpeed) // 条件: 刀の速度が5以上
            {
                // 跳ね返し処理
                if (shooter != null)
                {
                    Vector3 reflectedDirection = (shooter.transform.position - transform.position).normalized;
                    Rigidbody rb = GetComponent<Rigidbody>();

                    // 刀の速度を基に弾の反射速度を調整
                    float newSpeed = Mathf.Clamp(swordTracker.currentSpeed * 2f, 10f, 50f);
                    rb.velocity = reflectedDirection * newSpeed;

                    transform.rotation = Quaternion.LookRotation(reflectedDirection);
                    isReflected = true;
                }
            }
            else
            {
                Debug.Log("Swing too slow to reflect!");
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
