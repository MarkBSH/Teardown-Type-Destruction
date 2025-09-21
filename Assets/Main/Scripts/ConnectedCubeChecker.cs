using UnityEngine;
using System.Collections.Generic;

public class ConnectedCubeChecker : MonoBehaviour
{
    public GameObject m_ConnectedCube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Connected Cube"))
        {
            m_ConnectedCube = other.gameObject;
        }
    }
}
