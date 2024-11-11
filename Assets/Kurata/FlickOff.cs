using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickOff : MonoBehaviour
{
    private Animator anim;
    public float forceAmount = 10f;
    public float rotationSpeed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.enabled == false)
        {
            // X�������ɉ�]������
            float rotationAmount = -rotationSpeed * Time.deltaTime;  // 1�t���[���ŉ�]����p�x
            transform.Rotate(rotationAmount, 0f, 0f);  // X���ɉ�]��������

            // ��]�p�x��0�`360�x�ɐ�������
            float currentRotationX = transform.eulerAngles.x;
            if (currentRotationX > 360f)
            {
                currentRotationX -= 360f;
            }

            // ��]�p�x��0����360�x�ɕۂ�
            transform.eulerAngles = new Vector3(Mathf.Repeat(currentRotationX, 360f), transform.eulerAngles.y, transform.eulerAngles.z);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("AnimOn", true);
        }
        
        if (other.tag == "katana")
        {
            anim.enabled = false;

            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.mass = 1;
            rb.useGravity = true;

            Vector3 knockbackDirection = new Vector3(0, 1, 1); // �����𐳋K��
            rb.AddForce(knockbackDirection * forceAmount, ForceMode.Impulse);

        }
    }
}
