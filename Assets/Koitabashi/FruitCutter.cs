using UnityEngine;
using BLINDED_AM_ME;
using System.Collections.Generic;
using System.Collections;
public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;       // 切断面を示すプレーン
    public Material capMaterialCenter;    // 中心に近い部分の切断面に使用するマテリアル
    public Material capMaterialEdge;      // 端に近い部分の切断面に使用するマテリアル
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // 切断面の範囲
    public string targetTag = "Cuttable"; // 切断可能なオブジェクトのタグ
    public float distanceThreshold = 0.5f; // 中心からどれくらいの距離でマテリアルを変えるか
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // 切断済みのオブジェクトを管理
    private void OnTriggerEnter(Collider other)
    {
        // オブジェクトが "Cuttable" タグを持っていて、かつまだ切断されていないか確認
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // 交差していれば切断
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // 切断済みとして登録
                }
                else
                {
                    Debug.Log("オブジェクトが切断面に触れていません。");
                }
            }
        }
    }
    void PerformCut(GameObject target)
    {
        Vector3 anchorPoint = cuttingPlane.transform.position;
        Vector3 normalDirection = cuttingPlane.transform.up;

        Vector3 targetCenter = target.GetComponent<Collider>().bounds.center;
        float distanceFromCenter = Vector3.Distance(targetCenter, anchorPoint);
        Material selectedCapMaterial = (distanceFromCenter < distanceThreshold) ? capMaterialCenter : capMaterialEdge;

        GameObject[] pieces = MeshCut.Cut(target, anchorPoint, normalDirection, selectedCapMaterial);
        if (pieces != null)
        {
            foreach (GameObject piece in pieces)
            {
                Rigidbody rb = piece.AddComponent<Rigidbody>();
                rb.mass = 1;
                rb.AddForce(Vector3.up * Random.Range(1f, 3f), ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);

                StartCoroutine(HideAfterDelay(piece, 5f)); // 5秒後に非表示
                alreadyCutObjects.Add(piece);
            }
        }
    }

    IEnumerator HideAfterDelay(GameObject piece, float delay)
    {
        yield return new WaitForSeconds(delay);
        piece.SetActive(false); // 非表示にする
    }
    void OnDrawGizmos()
    {
        if (cuttingPlane != null)
        {
            Vector3 anchorPoint = cuttingPlane.transform.position;
            Gizmos.color = Color.black;
            Gizmos.matrix = Matrix4x4.TRS(anchorPoint, cuttingPlane.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, cuttingBoxSize);
        }
    }
}



/*using UnityEngine;
using System.Collections.Generic;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;       // 切断面を示すプレーン
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // 切断面の範囲
    public string targetTag = "Cuttable"; // 切断可能なオブジェクトのタグ
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // 切断済みのオブジェクトを管理

    private void OnTriggerEnter(Collider other)
    {
        // オブジェクトが "Cuttable" タグを持っていて、かつまだ切断されていないか確認
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // 交差していれば切断
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // 切断済みとして登録
                }
                else
                {
                    Debug.Log("オブジェクトが切断面に触れていません。");
                }
            }
        }
    }

    void PerformCut(GameObject target)
    {
        Vector3 anchorPoint = cuttingPlane.transform.position;
        Vector3 normalDirection = cuttingPlane.transform.up;

        GameObject[] pieces = SimpleMeshCut.Cut(target, anchorPoint, normalDirection);
        if (pieces != null)
        {
            foreach (GameObject piece in pieces)
            {
                // オリジナルのマテリアルを新しい部分に適用
                MeshRenderer originalRenderer = target.GetComponent<MeshRenderer>();
                MeshRenderer newRenderer = piece.GetComponent<MeshRenderer>();
                if (originalRenderer != null && newRenderer != null)
                {
                    newRenderer.materials = originalRenderer.materials;
                }

                Rigidbody rb = piece.AddComponent<Rigidbody>();
                rb.mass = 1;
                rb.AddForce(Vector3.up * Random.Range(1f, 3f), ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);
                alreadyCutObjects.Add(piece); // 新たに生成されたピースも切断済みとして登録
            }

            // オリジナルのオブジェクトを破棄
            Destroy(target);
        }
    }

    void OnDrawGizmos()
    {
        if (cuttingPlane != null)
        {
            Vector3 anchorPoint = cuttingPlane.transform.position;
            Gizmos.color = Color.black;
            Gizmos.matrix = Matrix4x4.TRS(anchorPoint, cuttingPlane.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, cuttingBoxSize);
        }
    }
}*/
