using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using BLINDED_AM_ME;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;
    public Material capMaterialCenter;
    public Material capMaterialEdge;
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);
    public string targetTag = "Cuttable";
    public float distanceThreshold = 0.5f;
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>();

    public float forceAmount = 10f;  // 吹っ飛ばす力の大きさ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                // 毎回 cuttingBounds を更新
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                Bounds targetBounds = targetRenderer.bounds;

                // 範囲を確認して切断処理を実行
                if (cuttingBounds.Intersects(targetBounds))
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject);
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
                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody のみ追加
                rb.mass = 1;
                Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;  // 方向を正規化
                rb.AddForce(knockbackDirection * forceAmount, ForceMode.Force);
                StartCoroutine(HideAfterDelay(piece, rb, 5f));
                alreadyCutObjects.Add(piece);
            }
        }
    }

    IEnumerator HideAfterDelay(GameObject piece, Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        piece.SetActive(false);
        alreadyCutObjects.Remove(piece); // メモリ管理のため削除
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
