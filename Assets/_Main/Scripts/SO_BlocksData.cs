using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/BlocksData")]
public class SO_BlocksData : ScriptableObject
{
    public List<Block> Blocks = new List<Block>();

    /// <summary>
    /// Returns a random block from the list, or null if the list is empty.
    /// </summary>
    public Block GetRandomBlock()
    {
        if (Blocks == null || Blocks.Count == 0)
        {
            Debug.LogWarning("Blocks list is empty!");
            return null;
        }

        int randomIndex = Random.Range(0, Blocks.Count);
        return Blocks[randomIndex];
    }
}

[System.Serializable]
public class Block
{
    public int R = 0; // Rows
    public int C = 0; // Columns
    public int number = 0; // Pairs

    public Block(int r, int c, int number)
    {
        R = r;
        C = c;
        this.number = number;
    }
}
