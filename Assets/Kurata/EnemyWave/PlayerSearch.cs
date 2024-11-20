using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSearch : MonoBehaviour
{
    public float speed = 0.5f;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 targetPosition = other.transform.parent.position;// other�̐e�I�u�W�F�N�g�̈ʒu�̍��W���w��

            Transform objectTransform = this.transform.parent;//gameObject.GetComponent<Transform>(); // �Q�[���I�u�W�F�N�g��Transform�R���|�[�l���g���擾
            objectTransform.position = Vector3.Lerp(objectTransform.position, targetPosition, speed * Time.deltaTime); // �ړI�̈ʒu�Ɉړ�

            transform.parent.LookAt(other.transform.parent);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("������O�ꂽ");
        }
    }
}