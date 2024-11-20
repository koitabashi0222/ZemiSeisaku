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

    public float forceAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && !alreadyCutObjects.Contains(other.gameObject))
        {
            MeshRenderer targetRenderer = other.GetComponent<MeshRenderer>();
            if (targetRenderer != null)
            {
               
                Bounds cuttingBounds = new Bounds(cuttingPlane.transform.position, cuttingBoxSize);
                Bounds targetBounds = targetRenderer.bounds;

               
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

       
        (GameObject pieceA, GameObject pieceB) = MeshCutNeo.CutMesh(target, anchorPoint, normalDirection, true, selectedCapMaterial);

        if (pieceA != null && pieceB != null)
        {
            alreadyCutObjects.Add(pieceA);
            alreadyCutObjects.Add(pieceB);

            ApplyForceToPiece(pieceA, new Vector3(1, 1, 1));
            ApplyForceToPiece(pieceB, new Vector3(-1, 1, 1));

            StartCoroutine(HideAfterDelay(pieceA, pieceA.GetComponent<Rigidbody>(), 5f));
            StartCoroutine(HideAfterDelay(pieceB, pieceB.GetComponent<Rigidbody>(), 5f));
        }
    }

    void ApplyForceToPiece(GameObject piece, Vector3 direction)
    {
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = 1;
        Vector3 forceDirection = direction.normalized;
        rb.AddForce(forceDirection * forceAmount, ForceMode.Impulse);
    }

    IEnumerator HideAfterDelay(GameObject piece, Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        piece.SetActive(false);
        alreadyCutObjects.Remove(piece);
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