using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
                // �ｿｽ�ｿｽ�ｿｽ�ｿｽ cuttingBounds �ｿｽ�ｿｽ�ｿｽX�ｿｽV
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                Bounds targetBounds = targetRenderer.bounds;

                // �ｿｽﾍ囲ゑｿｽ�ｿｽm�ｿｽF�ｿｽ�ｿｽ�ｿｽﾄ切断�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽs
                if (cuttingBounds.Intersects(targetBounds))
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // �ｿｽﾘ断�ｿｽﾏみとゑｿｽ�ｿｽﾄ登�ｿｽ^
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

        // MeshCutNeo �ｿｽ�ｿｽ CutMesh �ｿｽ�ｿｽ�ｿｽ\�ｿｽb�ｿｽh�ｿｽ�ｿｽ�ｿｽg�ｿｽp�ｿｽ�ｿｽ�ｿｽﾄオ�ｿｽu�ｿｽW�ｿｽF�ｿｽN�ｿｽg�ｿｽ�ｿｽﾘ断
        (GameObject pieceA, GameObject pieceB) = MeshCutNeo.CutMesh(target, anchorPoint, normalDirection, true, selectedCapMaterial);

        if (pieceA != null && pieceB != null)
        {
            // �ｿｽV�ｿｽ�ｿｽ�ｿｽﾉ撰ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ黷ｽ�ｿｽs�ｿｽ[�ｿｽX�ｿｽ�ｿｽalreadyCutObjects�ｿｽﾉ追会ｿｽ
            alreadyCutObjects.Add(pieceA);
            alreadyCutObjects.Add(pieceB);

            foreach (GameObject piece in new GameObject[] { pieceA, pieceB })
            {
                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody �ｿｽﾌみ追会ｿｽ
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
        alreadyCutObjects.Remove(piece); // �ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾇ暦ｿｽ�ｿｽﾌゑｿｽ�ｿｽﾟ削除
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









//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using BLINDED_AM_ME;

//public class FruitCutter : MonoBehaviour
//{
//    public GameObject cuttingPlane;
//    public Material capMaterialCenter;
//    public Material capMaterialEdge;
//    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);
//    public string targetTag = "Cuttable";
//    public float distanceThreshold = 0.5f;
//    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>();

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
//        {
//            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
//            if (targetRenderer != null)
//            {
//                // �ｿｽ�ｿｽ�ｿｽ�ｿｽ cuttingBounds �ｿｽ�ｿｽ�ｿｽX�ｿｽV
//                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
//                Bounds targetBounds = targetRenderer.bounds;

//                // �ｿｽﾍ囲ゑｿｽ�ｿｽm�ｿｽF�ｿｽ�ｿｽ�ｿｽﾄ切断�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽs
//                if (cuttingBounds.Intersects(targetBounds))
//                {
//                    PerformCut(other.gameObject);
//                    alreadyCutObjects.Add(other.gameObject);
//                }
//            }
//        }
//    }

//    void PerformCut(GameObject target)
//    {
//        Vector3 anchorPoint = cuttingPlane.transform.position;
//        Vector3 normalDirection = cuttingPlane.transform.up;

//        Vector3 targetCenter = target.GetComponent<Collider>().bounds.center;
//        float distanceFromCenter = Vector3.Distance(targetCenter, anchorPoint);
//        Material selectedCapMaterial = (distanceFromCenter < distanceThreshold) ? capMaterialCenter : capMaterialEdge;

//        GameObject[] pieces = MeshCut.Cut(target, anchorPoint, normalDirection, selectedCapMaterial);
//        if (pieces != null)
//        {
//            foreach (GameObject piece in pieces)
//            {
//                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody �ｿｽﾌみ追会ｿｽ
//                rb.mass = 1;

//                StartCoroutine(HideAfterDelay(piece, 5f));
//                alreadyCutObjects.Add(piece);
//            }
//        }
//    }

//    IEnumerator HideAfterDelay(GameObject piece, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        piece.SetActive(false);
//        alreadyCutObjects.Remove(piece); // �ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾇ暦ｿｽ�ｿｽﾌゑｿｽ�ｿｽﾟ削除
//    }

//    void OnDrawGizmos()
//    {
//        if (cuttingPlane != null)
//        {
//            Vector3 anchorPoint = cuttingPlane.transform.position;
//            Gizmos.color = Color.black;
//            Gizmos.matrix = Matrix4x4.TRS(anchorPoint, cuttingPlane.transform.rotation, Vector3.one);
//            Gizmos.DrawWireCube(Vector3.zero, cuttingBoxSize);
//        }
//    }
//}






/*using UnityEngine;
using System.Collections.Generic;

public class FruitCutter : MonoBehaviour
{
    public GameObject cuttingPlane;       // �ｿｽﾘ断�ｿｽﾊゑｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽv�ｿｽ�ｿｽ�ｿｽ[�ｿｽ�ｿｽ
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // �ｿｽﾘ断�ｿｽﾊの範茨ｿｽ
    public string targetTag = "Cuttable"; // �ｿｽﾘ断�ｿｽﾂ能�ｿｽﾈオ�ｿｽu�ｿｽW�ｿｽF�ｿｽN�ｿｽg�ｿｽﾌタ�ｿｽO
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // �ｿｽﾘ断�ｿｽﾏみのオ�ｿｽu�ｿｽW�ｿｽF�ｿｽN�ｿｽg�ｿｽ�ｿｽ�ｿｽﾇ暦ｿｽ

    private void OnTriggerEnter(Collider other)
    {
        // �ｿｽI�ｿｽu�ｿｽW�ｿｽF�ｿｽN�ｿｽg�ｿｽ�ｿｽ "Cuttable" �ｿｽ^�ｿｽO�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾄゑｿｽ�ｿｽﾄ、�ｿｽ�ｿｽ�ｿｽﾂまゑｿｽ�ｿｽﾘ断�ｿｽ�ｿｽ�ｿｽ�ｿｽﾄゑｿｽ�ｿｽﾈゑｿｽ�ｿｽ�ｿｽ�ｿｽm�ｿｽF
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // �ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾄゑｿｽ�ｿｽ�ｿｽﾎ切断
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // �ｿｽﾘ断�ｿｽﾏみとゑｿｽ�ｿｽﾄ登�ｿｽ^
                }
                else
                {
                    Debug.Log("�ｿｽI�ｿｽu�ｿｽW�ｿｽF�ｿｽN�ｿｽg�ｿｽ�ｿｽ�ｿｽﾘ断�ｿｽﾊに触�ｿｽ�ｿｽﾄゑｿｽ�ｿｽﾜゑｿｽ�ｿｽ�ｿｽB");
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
                // �ｿｽI�ｿｽ�ｿｽ�ｿｽW�ｿｽi�ｿｽ�ｿｽ�ｿｽﾌマ�ｿｽe�ｿｽ�ｿｽ�ｿｽA�ｿｽ�ｿｽ�ｿｽ�ｿｽV�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾉ適�ｿｽp
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
                alreadyCutObjects.Add(piece); // �ｿｽV�ｿｽ�ｿｽ�ｿｽﾉ撰ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽ黷ｽ�ｿｽs�ｿｽ[�ｿｽX�ｿｽ�ｿｽ�ｿｽﾘ断�ｿｽﾏみとゑｿｽ�ｿｽﾄ登�ｿｽ^
            }

            // �ｿｽI�ｿｽ�ｿｽ�ｿｽW�ｿｽi�ｿｽ�ｿｽ�ｿｽﾌオ�ｿｽu�ｿｽW�ｿｽF�ｿｽN�ｿｽg�ｿｽ�ｿｽj�ｿｽ�ｿｽ
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
