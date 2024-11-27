using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �e�̑��x
    public GameObject shooter; // ���ˌ���EnemyShooter
    private bool isReflected = false; // ���˕Ԃ��t���O

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "katana" && !isReflected)
        {
            // ���ˌ��ishooter�j�����݂���ꍇ�A���̕����֒��˕Ԃ�
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
            GameObject parent = other.transform.parent.gameObject; // �e�I�u�W�F�N�g���擾

            if (parent != null)
            {
                Destroy(parent); // �e�I�u�W�F�N�g���폜
            }
            Destroy(gameObject); // �e���폜
        }
    }
}
