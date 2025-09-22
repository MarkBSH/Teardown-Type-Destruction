using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DestructableParent : MonoBehaviour
{
    public List<GameObject> m_ConnectedCubes = new();
    public List<string> m_ConnectedCubeNames = new();
    public List<GameObject> m_ConnectedPartsList = new();
    public float m_CubeSize;

    public void ConnectionCheck()
    {
        m_ConnectedPartsList.Clear();

        if (m_ConnectedCubes.Count == 0)
        {
            Destroy(gameObject, 0.1f);
            return;
        }

        AddPartCube(m_ConnectedCubes[0]);

        StartCoroutine(WaitAndEvaluate());
    }

    public void AddPartCube(GameObject cube)
    {
        m_ConnectedPartsList.Add(cube);
        m_ConnectedCubes.RemoveAt(m_ConnectedCubes.IndexOf(cube));
        m_ConnectedCubeNames.RemoveAt(m_ConnectedCubeNames.IndexOf(cube.name));

        CheckSideConnections(cube);
    }

    private void CheckSideConnections(GameObject cube)
    {
        string cubeName = cube.name;
        string[] nameParts = cubeName.Split('_');

        int x = int.Parse(nameParts[1]);
        int y = int.Parse(nameParts[2]);
        int z = int.Parse(nameParts[3]);

        TryExploreNeighbor(nameParts[0], x + 1, y, z);
        TryExploreNeighbor(nameParts[0], x - 1, y, z);
        TryExploreNeighbor(nameParts[0], x, y + 1, z);
        TryExploreNeighbor(nameParts[0], x, y - 1, z);
        TryExploreNeighbor(nameParts[0], x, y, z + 1);
        TryExploreNeighbor(nameParts[0], x, y, z - 1);
    }

    private void TryExploreNeighbor(string prefix, int x, int y, int z)
    {
        string neighborName = $"{prefix}_{x}_{y}_{z}";
        int idx = m_ConnectedCubeNames.IndexOf(neighborName);

        if (idx >= 0)
        {
            GameObject neighbor = m_ConnectedCubes[idx];
            AddPartCube(neighbor);
        }
    }

    private IEnumerator WaitAndEvaluate()
    {
        yield return new WaitForSeconds(1f);

        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;
        int minZ = int.MaxValue;
        int maxZ = int.MinValue;

        foreach (GameObject part in m_ConnectedPartsList)
        {
            Vector3 localPos = part.transform.localPosition;
            int x = Mathf.RoundToInt(localPos.x / m_CubeSize);
            int y = Mathf.RoundToInt(localPos.y / m_CubeSize);
            int z = Mathf.RoundToInt(localPos.z / m_CubeSize);

            if (x < minX) minX = x;
            if (x > maxX) maxX = x;
            if (y < minY) minY = y;
            if (y > maxY) maxY = y;
            if (z < minZ) minZ = z;
            if (z > maxZ) maxZ = z;
        }

        Vector3 gridCenter = new Vector3((maxX + minX) / 2f, (maxY + minY) / 2f, (maxZ + minZ) / 2f);
        Vector3 centerWorldPosition = transform.TransformPoint(gridCenter * m_CubeSize);

        GameObject newParent = new("DisconnectedPartParent");
        newParent.transform.position = centerWorldPosition;
        DestructableParent destructableParent = newParent.AddComponent<DestructableParent>();
        destructableParent.m_CubeSize = m_CubeSize;
        newParent.tag = "Destruction Parent";
        newParent.layer = LayerMask.NameToLayer("Destruction Parent");
        newParent.AddComponent<Rigidbody>();
        newParent.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("Destruction Cube", "Destructing Object", "Destruction Parent");
        newParent.AddComponent<BoxCollider>();
        newParent.GetComponent<BoxCollider>().size = new Vector3(maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1) * m_CubeSize;
        newParent.GetComponent<BoxCollider>().center = Vector3.zero;
        newParent.GetComponent<BoxCollider>().excludeLayers = LayerMask.GetMask("Destruction Cube", "Destructing Object", "Destruction Parent");
        GameObject parentTriggerChild = new("TriggerChild");
        parentTriggerChild.tag = "Destruction Parent";
        parentTriggerChild.layer = LayerMask.NameToLayer("Destruction Parent");
        parentTriggerChild.AddComponent<DestructableParentTrigger>();
        parentTriggerChild.transform.SetParent(newParent.transform);
        parentTriggerChild.transform.localPosition = Vector3.zero;
        BoxCollider trigger = parentTriggerChild.AddComponent<BoxCollider>();
        trigger.size = newParent.GetComponent<BoxCollider>().size;
        trigger.center = newParent.GetComponent<BoxCollider>().center;
        trigger.isTrigger = true;
        trigger.excludeLayers = LayerMask.GetMask("Destruction Cube", "Destruction Parent");
        Rigidbody triggerRb = parentTriggerChild.AddComponent<Rigidbody>();
        triggerRb.isKinematic = true;
        triggerRb.useGravity = false;
        triggerRb.excludeLayers = LayerMask.GetMask("Destruction Cube", "Destruction Parent");

        for (int i = 0; i < m_ConnectedPartsList.Count; i++)
        {
            m_ConnectedPartsList[i].transform.SetParent(newParent.transform);
            destructableParent.m_ConnectedCubes.Add(m_ConnectedPartsList[i]);
        }

        ConnectionCheck();
    }

    public void DetachAllCubes(GameObject destructedBy, float force)
    {
        DestructableCube[] cubes = GetComponentsInChildren<DestructableCube>();
        foreach (DestructableCube cube in cubes)
        {
            cube.DetachAndDespawn(destructedBy, force);
        }
        Destroy(gameObject, 0.1f);
    }
}
