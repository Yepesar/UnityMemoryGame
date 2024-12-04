using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/BlocksData")]
public class SO_BlocksData : ScriptableObject
{
    #region Variables

    public List<Block> Blocks = new List<Block>(); // List of block data

    #endregion

    #region Public Methods

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

        // Select a random block from the list
        int randomIndex = Random.Range(0, Blocks.Count);
        return Blocks[randomIndex];
    }

    #endregion
}

[System.Serializable]
public class Block
{
    #region Variables

    public int R = 0; // Row index
    public int C = 0; // Column index
    public int number = 0; // Pair number or identifier

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new block with specified row, column, and number.
    /// </summary>
    /// <param name="r">Row index of the block.</param>
    /// <param name="c">Column index of the block.</param>
    /// <param name="number">Number associated with the block.</param>
    public Block(int r, int c, int number)
    {
        R = r;
        C = c;
        this.number = number;
    }

    #endregion
}
