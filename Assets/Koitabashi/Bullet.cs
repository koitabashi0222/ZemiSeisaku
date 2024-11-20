using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �e�̑��x
    private bool isReflected = false; // ���˕Ԃ��t���O

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "katana" && !isReflected)
        {
            // ���̌����ƓG�̕������l�����Ē��˕Ԃ�
            Vector3 reflectedDirection = CalculateReflectedDirection(collision);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = reflectedDirection * speed;

            transform.rotation = Quaternion.LookRotation(reflectedDirection);
            isReflected = true;
        }
        else if (collision.gameObject.tag == "Enemy" && isReflected)
        {
            // �G�Ƀ_���[�W�����i���̗�j
            Destroy(collision.gameObject); // �G������
            Destroy(gameObject); // �e������
        }
    }

    // ���˕Ԃ�������v�Z����
    Vector3 CalculateReflectedDirection(Collision collision)
    {
        // ���̌���
        Vector3 katanaDirection = collision.gameObject.transform.forward;

        // �Փ˂����e�̌��݈ʒu�ƓG�̈ʒu
        Vector3 bulletPosition = transform.position;
        GameObject enemy = FindClosestEnemy(); // �ł��߂��G��T��
        Vector3 enemyDirection = (enemy.transform.position - bulletPosition).normalized;

        // �G�̕������x�[�X�ɁA���̌�����������������
        float influence = 0.3f; // ���̉e���x�i0�`1�Œ����j
        Vector3 reflectedDirection = Vector3.Lerp(enemyDirection, katanaDirection, influence).normalized;

        return reflectedDirection;
    }

    // �ł��߂��G��T��
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

    // �e�𔽎˂�����
    void ReflectBullet(Vector3 collisionNormal)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // ���˕������v�Z
        Vector3 reflectedDirection = Vector3.Reflect(rb.velocity.normalized, collisionNormal);

        // ���x�ƌ������X�V
        rb.velocity = reflectedDirection * speed;

        // �e�̌������X�V
        transform.rotation = Quaternion.LookRotation(reflectedDirection);
    }
}
