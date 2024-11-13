using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    private bool isKnockedBack = false;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>(); // Rigidbody��ǉ�
        rb.isKinematic = true; // �ʏ펞�͕������Z�𖳌�
    }

    void Update()
    {
        if (isKnockedBack)
        {
           // Knockback();
            isKnockedBack = false; // �m�b�N�o�b�N������t���O�����Z�b�g
        }
    }

    public void TriggerKnockback(Vector3 direction)
    {
        isKnockedBack = true;
        rb.isKinematic = false; // �m�b�N�o�b�N���s�����߂Ɉꎞ�I�ɖ�����
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
    }

    
}