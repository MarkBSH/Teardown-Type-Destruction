using UnityEngine;

[CreateAssetMenu(fileName = "FullObjectData", menuName = "Scriptable Objects/FullObjectData")]
public class FullObjectData : ScriptableObject
{
    public BoolArray3D[] GridData;

    public Color[] AvailableColors;

    public float CubeStrength;
    public float CubeMass;
    public float CubeDespawnTime;
}

[System.Serializable]
public class BoolArray3D
{
    public BoolArray2D[] layer;
}

[System.Serializable]
public class BoolArray2D
{
    public bool[] row;
}