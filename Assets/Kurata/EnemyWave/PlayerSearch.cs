using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerSearch : MonoBehaviour
{
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 targetPosition = other.transform.parent.position;// otherの親オブジェクトの位置の座標を指定

            Transform objectTransform = this.transform.parent;//gameObject.GetComponent<Transform>(); // ゲームオブジェクトのTransformコンポーネントを取得
            objectTransform.position = Vector3.Lerp(objectTransform.position, targetPosition, speed * Time.deltaTime); // 目的の位置に移動

            Debug.Log("当たっている");

            transform.parent.LookAt(other.transform.parent);
            Debug.Log(other.transform.parent); 
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("判定を外れた");
        }
    }
}