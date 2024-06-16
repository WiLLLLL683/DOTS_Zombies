using Unity.Entities;
using UnityEngine;

public class InputAuthoring : MonoBehaviour
{
}

class InputBaker : Baker<InputAuthoring>
{
    public override void Bake(InputAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent<InputState>(entity);
    }
}
