using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderCommander : MonoBehaviour
{

    SceneLoadProperties loadProperties = new SceneLoadProperties();
    [SerializeField] SceneLoadProperties[] loadPropertiesArray;


    public void GoToSigleSceneByName(string sceneName)
    {
        loadProperties.loadBy = LoadBy.String;
        loadProperties.loadType = LoadType.SingleScene;
        loadProperties.sceneStringName = sceneName;
        loadProperties.dontUnload = false;
        ScenesLoader.LoadScene(loadProperties);
    }

    public void GoToSigleSceneByIndex(int buildIndex)
    {
        loadProperties.loadBy = LoadBy.Index;
        loadProperties.loadType = LoadType.SingleScene;
        loadProperties.sceneBuildIndex = buildIndex;
        loadProperties.dontUnload = false;
        ScenesLoader.LoadScene(loadProperties);
    }

    public void GoToSingleSceneByPath(string scenePath)
    {
        loadProperties.loadBy = LoadBy.AssetPath;
        loadProperties.loadType = LoadType.SingleScene;
        loadProperties.sceneAssetPath = scenePath;
        loadProperties.dontUnload = false;
        ScenesLoader.LoadScene(loadProperties);
    }

    public void GoToSceneUsingLoadPropertiesArray(int loadProtertiesIndex)
    {
        ScenesLoader.LoadScene(loadPropertiesArray[loadProtertiesIndex]);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
