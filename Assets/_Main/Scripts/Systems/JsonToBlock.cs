using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonToBlock : MonoBehaviour
{
    #region Variables

    [SerializeField] private string jsonFolderPath = "Blocks"; // Subfolder inside StreamingAssets for JSON files
    [SerializeField] private SO_BlocksData blocksDataAsset; // Reference to the ScriptableObject to update

    private string fullPath; // Full path to the JSON folder

    #endregion

    #region Initialization

    /// <summary>
    /// Processes a random JSON file on Awake and updates the ScriptableObject.
    /// </summary>
    private void Awake()
    {
        // Ensure the folder path is relative to StreamingAssets
        fullPath = Path.Combine(Application.streamingAssetsPath, jsonFolderPath);     
        Debug.Log($"Looking for JSON files in: {fullPath}");

        ProcessRandomJson();
    }

    #endregion

    #region JSON Processing

    /// <summary>
    /// Reads a random JSON file from the specified folder, parses it, and updates the ScriptableObject.
    /// </summary>
    public void ProcessRandomJson()
    {
        // Ensure the folder exists
        if (!Directory.Exists(fullPath))
        {
            Debug.LogError($"Folder not found: {fullPath}");
            return;
        }

        // Get all JSON files in the folder
        string[] jsonFiles = Directory.GetFiles(fullPath, "*.json");
        if (jsonFiles.Length == 0)
        {
            Debug.LogWarning("No JSON files found in the folder.");
            return;
        }

        // Select a random JSON file
        string randomJsonFile = jsonFiles[Random.Range(0, jsonFiles.Length)];
        Debug.Log($"Processing JSON file: {randomJsonFile}");

        // Read and parse the JSON file
        string jsonContent = File.ReadAllText(randomJsonFile);
        BlocksContainer blocksContainer = JsonUtility.FromJson<BlocksContainer>(jsonContent);

        // Validate the parsed data
        if (blocksContainer == null || blocksContainer.blocks == null)
        {
            Debug.LogError("Failed to parse JSON or JSON structure is invalid.");
            return;
        }

        // Update the ScriptableObject with the parsed data
        UpdateScriptableObject(blocksContainer.blocks);
    }

    #endregion

    #region ScriptableObject Update

    /// <summary>
    /// Updates the referenced ScriptableObject with the parsed block data.
    /// </summary>
    /// <param name="blocks">List of blocks parsed from the JSON file.</param>
    private void UpdateScriptableObject(List<Block> blocks)
    {
        if (blocksDataAsset != null)
        {
            blocksDataAsset.Blocks.Clear();
            blocksDataAsset.Blocks.AddRange(blocks);

            Debug.Log("ScriptableObject updated with new data!");
        }
        else
        {
            Debug.LogError("ScriptableObject reference is missing.");
        }
    }

    #endregion
}

[System.Serializable]
public class BlocksContainer
{
    public List<Block> blocks; // Matches the JSON structure
}
