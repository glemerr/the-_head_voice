
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PerimeterSpawner : MonoBehaviour
{
    [Header("Dimensiones del bloque")]
    public float thickness = 1f;
    public float height = 12f;

    public void SpawnFaces(ZoneTrigger zoneTrigger, GameObject blockPrefab)
    {
        if (blockPrefab == null || zoneTrigger == null)
        {
            Debug.LogWarning("Asigna blockPrefab y zoneCollider en el Inspector.");
            return;
        }

        var bc = zoneTrigger.GetComponent<BoxCollider>();
        Vector3 c = bc.center;
        Vector3 s = bc.size+ new Vector3(10f, 0f, 10f);
        //StartCoroutine(delay(3.0f));
        SpawnFace("Front", c + new Vector3(0, 0, s.z * 0.5f),
                new Vector3(s.x, height, thickness), blockPrefab);
        SpawnFace("Back", c + new Vector3(0, 0, -s.z * 0.5f),
                new Vector3(s.x, height, thickness), blockPrefab);
        SpawnFace("Left", c + new Vector3(-s.x * 0.5f, 0, 0),
                new Vector3(thickness, height, s.z), blockPrefab);
        SpawnFace("Right", c + new Vector3(s.x * 0.5f, 0, 0),
                new Vector3(thickness, height, s.z), blockPrefab);
    }

    public void ClearFaces()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void SpawnFace(string name, Vector3 localPos, Vector3 localScale, GameObject blockPrefab)
    {
        var go = Instantiate(blockPrefab, transform);
        go.name = name;
        go.transform.localPosition = localPos;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = localScale;
    }

}