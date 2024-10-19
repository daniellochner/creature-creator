using DanielLochner.Assets;
using UnityEngine;

public class FoodProxy : ProxyBehaviour
{
    public GameObject model;
    public Diet diet;
    public MinMax minMaxHunger;
    public MinMax minMaxHeal;

    public override bool IsValid()
    {
        if (model == null)
        {
            Debug.LogError("Model must be provided.");
            return false;
        }

        if (minMaxHunger.min < 0 || minMaxHunger.max > 1)
        {
            Debug.LogError("Hunger value must be in the range [0, 1].");
            return false;
        }

        if (minMaxHeal.min < 0 || minMaxHeal.max > 1)
        {
            Debug.LogError("Heal value must be in the range [0, 1].");
            return false;
        }

        return base.IsValid();
    }

    public enum Diet
    {
        Omnivore,
        Carnivore,
        Herbivore
    }
}
