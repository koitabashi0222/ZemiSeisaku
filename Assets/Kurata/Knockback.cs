/*using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 1.0f;
    private float initialYPosition;
    private Rigidbody rb;
    private bool isKnockbackActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;  // 初期状態では重力を無効にする
        }
        initialYPosition = transform.position.y;
    }

    public void ApplyKnockback()
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;

            rb.velocity = Vector3.zero;  // 既存の速度をリセット
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            rb.useGravity = true;  // ノックバック時に重力を有効にする
            Debug.Log("Gravity Enabled: " + rb.useGravity);
            isKnockbackActive = true;
        }
    }

    void Update()
    {
        float currentYPosition = transform.position.y; // Y座標を最初に取る

        // Y座標が4以上であれば重力を強制的に有効にする
        if (currentYPosition >= 4f && !rb.useGravity)
        {
            rb.useGravity = true;  // 重力を有効にする
            Debug.Log("Gravity Enabled at y = " + currentYPosition);
            isKnockbackActive = true; 
        }
        // ノックバックがアクティブであり、重力が有効になっているか確認
        if (isKnockbackActive && rb != null)
        {
            Debug.Log("Current Y Position: " + currentYPosition);

            

            // ノックバック後に元のY座標に戻ったら重力を無効にする
            if (currentYPosition <= initialYPosition)
            {
                rb.useGravity = false;  // 重力を無効にする
                rb.velocity = Vector3.zero;  // 落下の速度をリセット
                isKnockbackActive = false;

                // 位置を初期位置に戻すが、Y座標だけリセット
                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
                Debug.Log("Gravity Disabled at y = " + currentYPosition);
            }
        }
    }
}
*/

using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 1.0f;
    private float initialYPosition;
    private Rigidbody rb;
    private bool isKnockbackActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;  // 初期状態では重力を無効にする
        }
        initialYPosition = transform.position.y;
    }

    public void ApplyKnockback()
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;

            rb.velocity = Vector3.zero;  // 既存の速度をリセット
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            rb.useGravity = true;  // ノックバック時に重力を有効にする
            Debug.Log("Gravity Enabled: " + rb.useGravity);
            isKnockbackActive = true;

            // ノックバック後に数秒遅れて処理を開始
            StartCoroutine(HandleKnockbackDelay());
        }
    }

    private IEnumerator HandleKnockbackDelay()
    {
        // ノックバック後に数秒待機 (例えば2秒後)
        yield return new WaitForSeconds(5.0f);

        // 遅延後にノックバックが終了しているかを確認
        while (isKnockbackActive)
        {
            float currentYPosition = transform.position.y;

            Debug.Log("Current Y Position: " + currentYPosition);

            // Y座標が初期位置に戻った場合に重力を無効にする
            if (currentYPosition <= initialYPosition)
            {
                rb.useGravity = false;  // 重力を無効にする
                rb.velocity = Vector3.zero;  // 落下の速度をリセット
                isKnockbackActive = false;

                // 位置を初期位置に戻すが、Y座標だけリセット
                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
                Debug.Log("Gravity Disabled at y = " + currentYPosition);

                Destroy(rb,0.5f);
            }
            yield return null;  // 毎フレーム処理を待つ
        }
    }
    void Update()
    {
        float currentYPosition = transform.position.y; // Y座標を最初に取る

        // Y座標が4以上であれば重力を強制的に有効にする
        if (currentYPosition >= 4f && !rb.useGravity)
        {
            rb.useGravity = true;  // 重力を有効にする
            Debug.Log("Gravity Enabled at y = " + currentYPosition);
            isKnockbackActive = true;
        }
        // ノックバックがアクティブであり、重力が有効になっているか確認
        if (isKnockbackActive && rb != null)
        {
            Debug.Log("Current Y Position: " + currentYPosition);



            // ノックバック後に元のY座標に戻ったら重力を無効にする
            if (currentYPosition <= initialYPosition)
            {
                rb.useGravity = false;  // 重力を無効にする
                rb.velocity = Vector3.zero;  // 落下の速度をリセット
                isKnockbackActive = false;

                // 位置を初期位置に戻すが、Y座標だけリセット
                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
                Debug.Log("Gravity Disabled at y = " + currentYPosition);

                Destroy(rb,0.5f);
            }
        }
    }
}