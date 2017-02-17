using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRecipeReward : PrototypeQuestReward
{
    GameObject recipeReward = null;

    public virtual void GenerateReward()
    {
        if (rewardGiven)
            return;

        rewardGiven = true;
        if (recipeReward != null)
            CraftingManager.instance.knownRecipes.Add(recipeReward.GetComponent<CraftingRecipe>());
    }
}
