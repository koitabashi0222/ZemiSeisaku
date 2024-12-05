using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject effectprefab;
    public float destroyDelay = 3f;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cuttable"))
        {
            // 接触点を取得
            Vector3 spawnPosition = other.ClosestPoint(transform.position);

            // プレハブを生成
            GameObject spawnedObject = Instantiate(effectprefab, spawnPosition, Quaternion.identity);

            Destroy(spawnedObject, destroyDelay);
        }
    }
}
