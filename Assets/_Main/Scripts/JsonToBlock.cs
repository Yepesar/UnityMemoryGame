using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonToBlock : MonoBehaviour
{
    [SerializeField] private string jsonFolderPath = "Assets/_Main/JSONs/Blocks"; // Folder path for JSON files
    [SerializeField] private SO_BlocksData blocksDataAsset; // Reference to the ScriptableObject

    private void Awake()
    {
        ProcessRandomJson();
    }

    public void ProcessRandomJson()
    {
        // Ensure the folder exists
        if (!Directory.Exists(jsonFolderPath))
        {
            Debug.LogError($"Folder not found: {jsonFolderPath}");
            return;
        }

        // Get all JSON files in the folder
        string[] jsonFiles = Directory.GetFiles(jsonFolderPath, "*.json");
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

        if (blocksContainer == null || blocksContainer.blocks == null)
        {
            Debug.LogError("Failed to parse JSON or JSON structure is invalid.");
            return;
        }

        // Update the ScriptableObject
        if (blocksDataAsset != null)
        {
            blocksDataAsset.Blocks.Clear();
            blocksDataAsset.Blocks.AddRange(blocksContainer.blocks);

            // Save changes to the ScriptableObject
#if UNITY_EDITOR
            EditorUtility.SetDirty(blocksDataAsset);
            AssetDatabase.SaveAssets();
#endif

            Debug.Log("ScriptableObject updated with new data!");
        }
        else
        {
            Debug.LogError("ScriptableObject reference is missing.");
        }
    }
}

[System.Serializable]
public class BlocksContainer
{
    public List<Block> blocks; // Matches the JSON structure
}
