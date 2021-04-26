using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball", menuName = "Balls")]
public class Ball : ScriptableObject
{
    public string ballName;

    public Color ballColor;
    public float size = 1;
    public float weight = 1;
    public PhysicMaterial PhscMaterial;
}
