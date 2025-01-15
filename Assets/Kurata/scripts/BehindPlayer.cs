using UnityEngine;
using UnityEngine.SceneManagement;

public class BehindPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Cuttable"))
        {
            Destroy(other.gameObject);


            OnCuttableDestroyed(other);
        }
    }

    private void OnCuttableDestroyed(Collider cuttable)
    {
        // �G�t�F�N�g��T�E���h�Đ�������ǉ�����Ȃ炱������
        Debug.Log($"{cuttable.name} was destroyed!");
    }
}
