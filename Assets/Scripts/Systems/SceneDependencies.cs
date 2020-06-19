using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDependencies : MonoBehaviour
{
    [SerializeField] SceneLoadProperties neededScenes;

    List<string> scenesToLoad;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckLoadedScenes());
    }

    IEnumerator CheckLoadedScenes()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !ScenesLoader.onLoading);
        scenesToLoad = new List<string>();
        ScenesLoader.CollectLoadedScenes();

        switch (neededScenes.loadType)
        {
            case LoadType.SingleScene:
                switch (neededScenes.loadBy)
                {
                    case LoadBy.Index:
                        if (!ScenesLoader.loadedScenes.Contains(SceneManager.GetSceneByBuildIndex(neededScenes.sceneBuildIndex).path))
                            scenesToLoad.Add(SceneManager.GetSceneByBuildIndex(neededScenes.sceneBuildIndex).name);
                        break;
                    case LoadBy.String:
                        if (!ScenesLoader.loadedScenes.Contains(SceneManager.GetSceneByName(neededScenes.sceneStringName).path))
                            scenesToLoad.Add(neededScenes.sceneStringName);
                        break;
                    case LoadBy.AssetPath:
                        if (!ScenesLoader.loadedScenes.Contains(neededScenes.sceneAssetPath))
                            scenesToLoad.Add(SceneManager.GetSceneByPath(neededScenes.sceneAssetPath).name);
                        break;
                    default:
                        break;
                }
                break;
            case LoadType.MultiScenes:
                switch (neededScenes.loadBy)
                {
                    case LoadBy.Index:
                        for (int i = 0; i < neededScenes.scenesBuildIndexes.Length; i++)
                        {
                            if (!ScenesLoader.loadedScenes.Contains(SceneManager.GetSceneByBuildIndex(neededScenes.scenesBuildIndexes[i]).path))
                                scenesToLoad.Add(SceneManager.GetSceneByBuildIndex(neededScenes.scenesBuildIndexes[i]).name);
                        }

                        break;
                    case LoadBy.String:
                        for (int i = 0; i < neededScenes.scenesStringNames.Length; i++)
                        {
                            if (!ScenesLoader.loadedScenes.Contains(SceneManager.GetSceneByName(neededScenes.scenesStringNames[i]).path))
                                scenesToLoad.Add(neededScenes.scenesStringNames[i]);
                        }
                        break;
                    case LoadBy.AssetPath:
                        for (int i = 0; i < neededScenes.scenesAssetsPath.Length; i++)
                        {
                            if (!ScenesLoader.loadedScenes.Contains(neededScenes.scenesAssetsPath[i]))
                                scenesToLoad.Add(SceneManager.GetSceneByPath(neededScenes.scenesAssetsPath[i]).name);
                        }

                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        //for (int i = 0; i < ScenesLoader.loadedScenes.Count; i++)
        //{
        //    Debug.Log(ScenesLoader.loadedScenes[i] + " currently loaded.");
        //}

        //for (int i = 0; i < scenesToLoad.Count; i++)
        //{
        //    Debug.Log(scenesToLoad[i] + " need to be loaded.");
        //}

        if (scenesToLoad.Count != 0)
        {
            ScenesLoader.LoadScene(new SceneLoadProperties(scenesToLoad.ToArray(), true));
        }
        yield return null;
    }
}
