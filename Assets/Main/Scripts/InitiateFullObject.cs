using UnityEngine;
using System.Collections;

public class InitiateFullObject : MonoBehaviour
{
    [SerializeField] private GameObject m_CubePrefab;
    public FullObjectData m_TempData;
    public GameObject m_TempWreckingBall;

    private void Start()
    {
        StartCoroutine(SpawnFullObject(Vector3.zero, m_TempData));
    }

    public IEnumerator SpawnFullObject(Vector3 position, FullObjectData data)
    {
        GameObject parentObject = new("FullObjectParent");
        parentObject.transform.position = position;
        DestructableParent destructableParent = parentObject.AddComponent<DestructableParent>();
        destructableParent.m_CubeSize = m_CubePrefab.transform.localScale.x;

        for (int x = 0; x < data.GridData.Length; x++)
        {
            for (int y = 0; y < data.GridData[x].layer.Length; y++)
            {
                for (int z = 0; z < data.GridData[x].layer[y].row.Length; z++)
                {
                    if (data.GridData[x].layer[y].row[z])
                    {
                        Vector3 spawnPos = new Vector3(x, y, z) * m_CubePrefab.transform.localScale.x + position;
                        GameObject cube = Instantiate(m_CubePrefab, spawnPos, Quaternion.identity);
                        cube.name = $"Cube_{x}_{y}_{z}";
                        cube.transform.SetParent(parentObject.transform);
                        destructableParent.m_ConnectedCubes.Add(cube);
                        cube.GetComponent<Renderer>().material.color = data.AvailableColors[Random.Range(0, data.AvailableColors.Length)];
                        DestructableCube destructable = cube.GetComponent<DestructableCube>();
                        destructable.m_Strength = data.CubeStrength;
                        destructable.m_Mass = data.CubeMass;
                        destructable.m_DespawnTime = data.CubeDespawnTime;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

        parentObject.tag = "Destruction Parent";
        parentObject.layer = LayerMask.NameToLayer("Destruction Parent");
        parentObject.AddComponent<Rigidbody>();
        parentObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("Destruction Cube", "Destructing Object", "Destruction Parent");
        parentObject.AddComponent<BoxCollider>();
        parentObject.GetComponent<BoxCollider>().size = new Vector3(data.GridData.Length, data.GridData[0].layer.Length, data.GridData[0].layer[0].row.Length) * m_CubePrefab.transform.localScale.x;
        parentObject.GetComponent<BoxCollider>().center = new Vector3((data.GridData.Length - 1) / 2f, (data.GridData[0].layer.Length - 1) / 2f, (data.GridData[0].layer[0].row.Length - 1) / 2f) * m_CubePrefab.transform.localScale.x;
        parentObject.GetComponent<BoxCollider>().excludeLayers = LayerMask.GetMask("Destruction Cube", "Destructing Object", "Destruction Parent");
        GameObject parentTriggerChild = new("TriggerChild");
        parentTriggerChild.tag = "Destruction Parent";
        parentTriggerChild.layer = LayerMask.NameToLayer("Destruction Parent");
        parentTriggerChild.AddComponent<DestructableParentTrigger>();
        parentTriggerChild.transform.SetParent(parentObject.transform);
        parentTriggerChild.transform.localPosition = position;
        BoxCollider trigger = parentTriggerChild.AddComponent<BoxCollider>();
        trigger.size = parentObject.GetComponent<BoxCollider>().size;
        trigger.center = parentObject.GetComponent<BoxCollider>().center;
        trigger.isTrigger = true;
        trigger.excludeLayers = LayerMask.GetMask("Destruction Cube", "Destruction Parent");
        Rigidbody triggerRb = parentTriggerChild.AddComponent<Rigidbody>();
        triggerRb.isKinematic = true;
        triggerRb.useGravity = false;
        triggerRb.excludeLayers = LayerMask.GetMask("Destruction Cube", "Destruction Parent");

        SpawnWreckingBall(position + new Vector3(0, 10, 0));
    }

    private void SpawnWreckingBall(Vector3 position)
    {
        Instantiate(m_TempWreckingBall, position, Quaternion.identity);
    }
}
