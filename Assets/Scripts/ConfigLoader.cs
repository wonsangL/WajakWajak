using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigLoader : MonoBehaviour {
    public static ConfigLoader Instance = null;
    public bool ConfigLoaded = false;
    public GameObject prefab = null;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;  
    }

    private void Start()
    {
        StartCoroutine(LoadCofig());
    }

    IEnumerator LoadCofig()
    {
        WWW request = new WWW(Application.streamingAssetsPath + "/Config.json");
      
        while (!request.isDone)
        {
            Debug.Log(request);
            yield return null;
        }

        Instantiate(prefab);

        yield break;
    }
}
