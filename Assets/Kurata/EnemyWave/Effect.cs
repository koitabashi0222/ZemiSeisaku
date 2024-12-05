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
            // �ڐG�_���擾
            Vector3 spawnPosition = other.ClosestPoint(transform.position);

            // �v���n�u�𐶐�
            GameObject spawnedObject = Instantiate(effectprefab, spawnPosition, Quaternion.identity);

            Destroy(spawnedObject, destroyDelay);
        }
    }
}
