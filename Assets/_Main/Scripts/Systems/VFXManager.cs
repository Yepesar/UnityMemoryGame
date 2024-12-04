using System.Collections.Generic;
using UnityEngine;

public enum VFXTypes
{
    None,
    Hit,
    FireHit,
    IceHit,
    RockHit,
    SlashHit,
    Poison,
    Explotion,
    Nature,
    Thunder
}

public class VFXManager : MonoBehaviour
{
    #region Variables

    public static VFXManager Singleton;

    [SerializeField] private List<VFXProfile> vfxProfiles; // List of VFX profiles containing prefabs and types

    #endregion

    #region Initialization

    /// <summary>
    /// Ensures only one instance of VFXManager exists (Singleton pattern).
    /// </summary>
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Spawns a VFX at the given transform's position and rotation.
    /// </summary>
    /// <param name="VFXType">Type of VFX to spawn.</param>
    /// <param name="spawnTransform">Transform where the VFX should be spawned.</param>
    public void SpawnVFX(VFXTypes VFXType, Transform spawnTransform)
    {
        if (VFXType == VFXTypes.None)
        {
            Debug.LogWarning("Attempted to spawn a VFX with type 'None'.");
            return;
        }

        // Find the VFX profile matching the VFXType
        VFXProfile selectedProfile = vfxProfiles.Find(profile => profile.VFXType == VFXType);

        if (selectedProfile == null)
        {
            Debug.LogError($"No VFX profile found for type: {VFXType}");
            return;
        }

        if (selectedProfile.VFXPrefab == null)
        {
            Debug.LogError($"VFX prefab for type {VFXType} is missing in the profile.");
            return;
        }

        // Instantiate the VFX at the desired position and rotation
        Instantiate(selectedProfile.VFXPrefab, spawnTransform.position, spawnTransform.rotation);
    }

    #endregion
}

[System.Serializable]
public class VFXProfile
{
    #region Variables

    public string VFXName; // Name of the VFX (for identification purposes)
    public VFXTypes VFXType; // Type of the VFX
    public GameObject VFXPrefab; // Prefab to spawn

    #endregion
}
