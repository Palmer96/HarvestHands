using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBenchButton : MonoBehaviour
{
    public enum ButtonType
    {
        Next = 0,
        Previous = 1,
    }
    public CraftingBench bench;

    public ButtonType type = ButtonType.Next;

    public void ActivateButton()
    {
        if (bench == null)
            return;
        if (type == ButtonType.Next)
            bench.NextRecipeChoice();
        else
            bench.PreviousRecipeChocie();
    }
}
