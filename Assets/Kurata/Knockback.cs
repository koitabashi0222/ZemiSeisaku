using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 1.0f;  // ノックバックの力

    public void ApplyKnockback()
    {

        Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;  // 既存の速度をリセット
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}