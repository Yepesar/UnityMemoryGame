using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Singleton;
    
    [SerializeField] private float loadDelay = 1f;
    [SerializeField] private Animator transitionAnim;

    private IEnumerator loadCourutine = null;

    // Start is called before the first frame update
    void Awake()
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

    private void Start()
    {
        if (transitionAnim)
        {
            transitionAnim.Play("UnLoad");
        }
    }

    public void LoadScene(int scene)
    {
        if (loadCourutine == null)
        {
            loadCourutine = LoadSystem(scene);
            StartCoroutine(loadCourutine);
        }
    }

    private IEnumerator LoadSystem(int index)
    {     
        if (transitionAnim)
        {
            transitionAnim.Play("Load");
        }

        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadSceneAsync(index);
        loadCourutine = null;
    }
}
