using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    public float speed = 0.5f;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform targetParent = other.transform.parent;
            Transform thisParent = this.transform.parent;

            if (targetParent != null && thisParent != null)
            {
                Vector3 targetPosition = targetParent.position;
                thisParent.position = Vector3.MoveTowards(thisParent.position, targetPosition, speed * Time.deltaTime);

                thisParent.LookAt(targetParent);

                // デバッグ用（移動方向を赤い線で表示）
                Debug.DrawLine(thisParent.position, targetPosition, Color.red);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが判定範囲を離れた");
        }
    }
}