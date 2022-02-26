using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceScriptableObject", menuName = "ScriptableObjects/ResourceScriptableObject", order = 1)]
public class ResourceScriptableObject : ScriptableObject
{
    public List<Sprite> sprites;
}
