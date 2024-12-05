using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Variables

    public static SceneLoader Singleton;

    [SerializeField] private float loadDelay = 1f; // Delay before loading the scene
    [SerializeField] private Animator transitionAnim; // Animator for scene transitions

    private IEnumerator loadCoroutine = null;

    #endregion

    #region Initialization

    /// <summary>
    /// Ensures only one instance of SceneLoader exists (Singleton pattern).
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

    /// <summary>
    /// Plays the "UnLoad" animation at the start, if an animator is assigned.
    /// </summary>
    private void Start()
    {
        if (transitionAnim)
        {
            transitionAnim.Play("UnLoad");
        }
    }

    #endregion

    #region Scene Loading

    /// <summary>
    /// Public method to initiate scene loading by index.
    /// </summary>
    /// <param name="scene">Index of the scene to load.</param>
    public void LoadScene(int scene)
    {
        if (loadCoroutine == null)
        {
            loadCoroutine = LoadSystem(scene);
            StartCoroutine(loadCoroutine);
        }
    }

    /// <summary>
    /// Handles the scene loading process with optional transition animations and delay.
    /// </summary>
    /// <param name="index">Index of the scene to load.</param>
    private IEnumerator LoadSystem(int index)
    {
        // Play transition animation if available
        if (transitionAnim)
        {
            transitionAnim.Play("Load");
        }

        // Wait for the specified delay
        yield return new WaitForSeconds(loadDelay);

        // Load the scene asynchronously to prevent freezing the game
        SceneManager.LoadSceneAsync(index);

        // Reset the coroutine variable to allow subsequent scene loads
        loadCoroutine = null;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    #endregion
}
