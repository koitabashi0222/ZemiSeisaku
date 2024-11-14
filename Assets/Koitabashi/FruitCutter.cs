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
                // ・ｽ・ｽ・ｽ・ｽ cuttingBounds ・ｽ・ｽ・ｽX・ｽV
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                Bounds targetBounds = targetRenderer.bounds;

                // ・ｽﾍ囲ゑｿｽ・ｽm・ｽF・ｽ・ｽ・ｽﾄ切断・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽs
                if (cuttingBounds.Intersects(targetBounds))
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // ・ｽﾘ断・ｽﾏみとゑｿｽ・ｽﾄ登・ｽ^
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

        // MeshCutNeo ・ｽ・ｽ CutMesh ・ｽ・ｽ・ｽ\・ｽb・ｽh・ｽ・ｽ・ｽg・ｽp・ｽ・ｽ・ｽﾄオ・ｽu・ｽW・ｽF・ｽN・ｽg・ｽ・ｽﾘ断
        (GameObject pieceA, GameObject pieceB) = MeshCutNeo.CutMesh(target, anchorPoint, normalDirection, true, selectedCapMaterial);

        if (pieceA != null && pieceB != null)
        {
            // ・ｽV・ｽ・ｽ・ｽﾉ撰ｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ黷ｽ・ｽs・ｽ[・ｽX・ｽ・ｽalreadyCutObjects・ｽﾉ追会ｿｽ
            alreadyCutObjects.Add(pieceA);
            alreadyCutObjects.Add(pieceB);

            foreach (GameObject piece in new GameObject[] { pieceA, pieceB })
            {
                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody ・ｽﾌみ追会ｿｽ
                rb.mass = 1;
                Vector3 knockbackDirection = new Vector3(0, 1, 1).normalized;  // 方向を正規化
                rb.AddForce(knockbackDirection * forceAmount, ForceMode.Impulse);
                StartCoroutine(HideAfterDelay(piece, rb, 5f));
                alreadyCutObjects.Add(piece);
            }
        }
    }

    IEnumerator HideAfterDelay(GameObject piece, Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        piece.SetActive(false);
        alreadyCutObjects.Remove(piece); // ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾇ暦ｿｽ・ｽﾌゑｿｽ・ｽﾟ削除
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
//                // ・ｽ・ｽ・ｽ・ｽ cuttingBounds ・ｽ・ｽ・ｽX・ｽV
//                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
//                Bounds targetBounds = targetRenderer.bounds;

//                // ・ｽﾍ囲ゑｿｽ・ｽm・ｽF・ｽ・ｽ・ｽﾄ切断・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽs
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
//                Rigidbody rb = piece.AddComponent<Rigidbody>();  // Rigidbody ・ｽﾌみ追会ｿｽ
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
//        alreadyCutObjects.Remove(piece); // ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾇ暦ｿｽ・ｽﾌゑｿｽ・ｽﾟ削除
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
    public GameObject cuttingPlane;       // ・ｽﾘ断・ｽﾊゑｿｽ・ｽ・ｽ・ｽ・ｽ・ｽv・ｽ・ｽ・ｽ[・ｽ・ｽ
    public Vector3 cuttingBoxSize = new Vector3(2, 0.01f, 2);  // ・ｽﾘ断・ｽﾊの範茨ｿｽ
    public string targetTag = "Cuttable"; // ・ｽﾘ断・ｽﾂ能・ｽﾈオ・ｽu・ｽW・ｽF・ｽN・ｽg・ｽﾌタ・ｽO
    private HashSet<GameObject> alreadyCutObjects = new HashSet<GameObject>(); // ・ｽﾘ断・ｽﾏみのオ・ｽu・ｽW・ｽF・ｽN・ｽg・ｽ・ｽ・ｽﾇ暦ｿｽ

    private void OnTriggerEnter(Collider other)
    {
        // ・ｽI・ｽu・ｽW・ｽF・ｽN・ｽg・ｽ・ｽ "Cuttable" ・ｽ^・ｽO・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾄゑｿｽ・ｽﾄ、・ｽ・ｽ・ｽﾂまゑｿｽ・ｽﾘ断・ｽ・ｽ・ｽ・ｽﾄゑｿｽ・ｽﾈゑｿｽ・ｽ・ｽ・ｽm・ｽF
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
                Bounds targetBounds = targetRenderer.bounds;
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                if (cuttingBounds.Intersects(targetBounds)) // ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾄゑｿｽ・ｽ・ｽﾎ切断
                {
                    PerformCut(other.gameObject);
                    alreadyCutObjects.Add(other.gameObject); // ・ｽﾘ断・ｽﾏみとゑｿｽ・ｽﾄ登・ｽ^
                }
                else
                {
                    Debug.Log("・ｽI・ｽu・ｽW・ｽF・ｽN・ｽg・ｽ・ｽ・ｽﾘ断・ｽﾊに触・ｽ・ｽﾄゑｿｽ・ｽﾜゑｿｽ・ｽ・ｽB");
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
                // ・ｽI・ｽ・ｽ・ｽW・ｽi・ｽ・ｽ・ｽﾌマ・ｽe・ｽ・ｽ・ｽA・ｽ・ｽ・ｽ・ｽV・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽ・ｽﾉ適・ｽp
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
                alreadyCutObjects.Add(piece); // ・ｽV・ｽ・ｽ・ｽﾉ撰ｿｽ・ｽ・ｽ・ｽ・ｽ・ｽ黷ｽ・ｽs・ｽ[・ｽX・ｽ・ｽ・ｽﾘ断・ｽﾏみとゑｿｽ・ｽﾄ登・ｽ^
            }

            // ・ｽI・ｽ・ｽ・ｽW・ｽi・ｽ・ｽ・ｽﾌオ・ｽu・ｽW・ｽF・ｽN・ｽg・ｽ・ｽj・ｽ・ｽ
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
