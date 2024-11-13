using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    private bool isKnockedBack = false;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>(); // Rigidbodyを追加
        rb.isKinematic = true; // 通常時は物理演算を無効
    }

    void Update()
    {
        if (isKnockedBack)
        {
           // Knockback();
            isKnockedBack = false; // ノックバック処理後フラグをリセット
        }
    }

    public void TriggerKnockback(Vector3 direction)
    {
        isKnockedBack = true;
        rb.isKinematic = false; // ノックバックを行うために一時的に無効化
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
    }

    
}