using CreatureCreator.SDK;
using UnityEngine;

public class FoodSpawnerProxy : ProxyBehaviour
{
    public FoodProxy[] food;
    public MinMax spawnCooldown = new MinMax(120, 180);

    public override bool IsValid()
    {
        if (food.Length == 0)
        {
            Debug.LogError("Must have at least one food proxy.");
            return false;
        }

        if (spawnCooldown.min < 10)
        {
            Debug.LogError("Spawn cooldown must be in the range [10, Infinity).", gameObject);
            return false;
        }

        return base.IsValid();
    }
}
