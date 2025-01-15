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
        // エフェクトやサウンド再生処理を追加するならここかな
        Debug.Log($"{cuttable.name} was destroyed!");
    }
}
