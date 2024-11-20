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
            rb.useGravity = false;  // ������Ԃł͏d�͂𖳌��ɂ���
        }
        initialYPosition = transform.position.y;
    }

    public void ApplyKnockback()
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;

            rb.velocity = Vector3.zero;  // �����̑��x�����Z�b�g
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            rb.useGravity = true;  // �m�b�N�o�b�N���ɏd�͂�L���ɂ���
            Debug.Log("Gravity Enabled: " + rb.useGravity);
            isKnockbackActive = true;
        }
    }

    void Update()
    {
        float currentYPosition = transform.position.y; // Y���W���ŏ��Ɏ��

        // Y���W��4�ȏ�ł���Ώd�͂������I�ɗL���ɂ���
        if (currentYPosition >= 4f && !rb.useGravity)
        {
            rb.useGravity = true;  // �d�͂�L���ɂ���
            Debug.Log("Gravity Enabled at y = " + currentYPosition);
            isKnockbackActive = true; 
        }
        // �m�b�N�o�b�N���A�N�e�B�u�ł���A�d�͂��L���ɂȂ��Ă��邩�m�F
        if (isKnockbackActive && rb != null)
        {
            Debug.Log("Current Y Position: " + currentYPosition);

            

            // �m�b�N�o�b�N��Ɍ���Y���W�ɖ߂�����d�͂𖳌��ɂ���
            if (currentYPosition <= initialYPosition)
            {
                rb.useGravity = false;  // �d�͂𖳌��ɂ���
                rb.velocity = Vector3.zero;  // �����̑��x�����Z�b�g
                isKnockbackActive = false;

                // �ʒu�������ʒu�ɖ߂����AY���W�������Z�b�g
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
            rb.useGravity = false;  // ������Ԃł͏d�͂𖳌��ɂ���
        }
        initialYPosition = transform.position.y;
    }

    public void ApplyKnockback()
    {
        if (rb != null)
        {
            Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;

            rb.velocity = Vector3.zero;  // �����̑��x�����Z�b�g
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            rb.useGravity = true;  // �m�b�N�o�b�N���ɏd�͂�L���ɂ���
            Debug.Log("Gravity Enabled: " + rb.useGravity);
            isKnockbackActive = true;

            // �m�b�N�o�b�N��ɐ��b�x��ď������J�n
            StartCoroutine(HandleKnockbackDelay());
        }
    }

    private IEnumerator HandleKnockbackDelay()
    {
        // �m�b�N�o�b�N��ɐ��b�ҋ@ (�Ⴆ��2�b��)
        yield return new WaitForSeconds(5.0f);

        // �x����Ƀm�b�N�o�b�N���I�����Ă��邩���m�F
        while (isKnockbackActive)
        {
            float currentYPosition = transform.position.y;

            Debug.Log("Current Y Position: " + currentYPosition);

            // Y���W�������ʒu�ɖ߂����ꍇ�ɏd�͂𖳌��ɂ���
            if (currentYPosition <= initialYPosition)
            {
                rb.useGravity = false;  // �d�͂𖳌��ɂ���
                rb.velocity = Vector3.zero;  // �����̑��x�����Z�b�g
                isKnockbackActive = false;

                // �ʒu�������ʒu�ɖ߂����AY���W�������Z�b�g
                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
                Debug.Log("Gravity Disabled at y = " + currentYPosition);

                Destroy(rb,0.5f);
            }
            yield return null;  // ���t���[��������҂�
        }
    }
    void Update()
    {
        float currentYPosition = transform.position.y; // Y���W���ŏ��Ɏ��

        // Y���W��4�ȏ�ł���Ώd�͂������I�ɗL���ɂ���
        if (currentYPosition >= 4f && !rb.useGravity)
        {
            rb.useGravity = true;  // �d�͂�L���ɂ���
            Debug.Log("Gravity Enabled at y = " + currentYPosition);
            isKnockbackActive = true;
        }
        // �m�b�N�o�b�N���A�N�e�B�u�ł���A�d�͂��L���ɂȂ��Ă��邩�m�F
        if (isKnockbackActive && rb != null)
        {
            Debug.Log("Current Y Position: " + currentYPosition);



            // �m�b�N�o�b�N��Ɍ���Y���W�ɖ߂�����d�͂𖳌��ɂ���
            if (currentYPosition <= initialYPosition)
            {
                rb.useGravity = false;  // �d�͂𖳌��ɂ���
                rb.velocity = Vector3.zero;  // �����̑��x�����Z�b�g
                isKnockbackActive = false;

                // �ʒu�������ʒu�ɖ߂����AY���W�������Z�b�g
                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
                Debug.Log("Gravity Disabled at y = " + currentYPosition);

                Destroy(rb,0.5f);
            }
        }
    }
}