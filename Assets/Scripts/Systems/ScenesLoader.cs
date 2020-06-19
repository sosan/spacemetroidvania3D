using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;


public enum LoadBy
{
    Index,
    String,
    AssetPath
}

public enum LoadType
{
    SingleScene,
    MultiScenes
}

public class ScenesLoader
{
    public static List<string> loadedScenes;
    static int scenesToLoadAmount = 0;
    static List<bool> scenesLoadedComplete = new List<bool>();
    static int completedScenes = 0;
    static List<AsyncOperation> asyncOps = new List<AsyncOperation>();
    public static bool onLoading = false;

    public static void LoadScene(SceneLoadProperties loadProperties)
    {
        IDisposable corutine;
        IDisposable coroutine2;
        CollectLoadedScenes();
        onLoading = true;
        completedScenes = 0;
        scenesToLoadAmount = 0;
        scenesLoadedComplete = new List<bool>();
        asyncOps = new List<AsyncOperation>();

        switch (loadProperties.loadType)
        {
            case LoadType.SingleScene:
                scenesToLoadAmount = 1;
                switch (loadProperties.loadBy)
                {
                    case LoadBy.Index:
                        corutine = LoadAsyncSceneByIndex(loadProperties.sceneBuildIndex).ToObservable().Subscribe();
                        coroutine2 = FinishLoad(loadProperties).ToObservable().Delay(TimeSpan.FromSeconds(10)).Subscribe();
                        break;
                    case LoadBy.String:
                        corutine = LoadAsyncSceneByName(loadProperties.sceneStringName).ToObservable().Subscribe();
                        coroutine2 = FinishLoad(loadProperties).ToObservable().Delay(TimeSpan.FromSeconds(10)).Subscribe();
                        break;
                    case LoadBy.AssetPath:
                        corutine = LoadAsyncSceneByPath(loadProperties.sceneAssetPath).ToObservable().Subscribe();
                        coroutine2 = FinishLoad(loadProperties).ToObservable().Delay(TimeSpan.FromSeconds(10)).Subscribe();
                        break;
                    default:
                        break;
                }
                break;
            case LoadType.MultiScenes:
                switch (loadProperties.loadBy)
                {
                    case LoadBy.Index:
                        scenesToLoadAmount = loadProperties.scenesBuildIndexes.Length;
                        for (int i = 0; i < loadProperties.scenesBuildIndexes.Length; i++)
                        {
                            corutine = LoadAsyncSceneByIndex(loadProperties.scenesBuildIndexes[i]).ToObservable().Subscribe();
                        }
                        coroutine2 = FinishLoad(loadProperties).ToObservable().Delay(TimeSpan.FromSeconds(10)).Subscribe();
                        break;
                    case LoadBy.String:
                        scenesToLoadAmount = loadProperties.scenesStringNames.Length;
                        for (int i = 0; i < loadProperties.scenesStringNames.Length; i++)
                        {
                            corutine = LoadAsyncSceneByName(loadProperties.scenesStringNames[i]).ToObservable().Subscribe();
                        }
                        coroutine2 = FinishLoad(loadProperties).ToObservable().Delay(TimeSpan.FromSeconds(10)).Subscribe();
                        break;
                    case LoadBy.AssetPath:
                        scenesToLoadAmount = loadProperties.scenesAssetsPath.Length;
                        for (int i = 0; i < loadProperties.scenesAssetsPath.Length; i++)
                        {
                            corutine = LoadAsyncSceneByPath(loadProperties.scenesAssetsPath[i]).ToObservable().Subscribe();
                        }
                        coroutine2 = FinishLoad(loadProperties).ToObservable().DelayFrame(120).Delay(TimeSpan.FromSeconds(10)).Subscribe();
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

    }

    public static void CollectLoadedScenes()
    {
        if (!onLoading)
        {
            loadedScenes = new List<string>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).path);
            }
        }
    }

    static IEnumerator LoadAsyncSceneByName(string name)
    {
        scenesLoadedComplete.Add(false);
        asyncOps.Add(SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive));
        int opIndex = asyncOps.Count - 1;
        asyncOps[opIndex].allowSceneActivation = false;
        while (!asyncOps[opIndex].isDone)
        {
            //Output the current progress
            //Debug.Log("Loading progress (" + name + "): " + (asyncOps[opIndex].progress * 100) + "%");

            // Check if the load has finished
            if (asyncOps[opIndex].progress >= 0.9f)
            {
                scenesLoadedComplete[opIndex] = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }


    static IEnumerator LoadAsyncSceneByPath(string path)
    {
        scenesLoadedComplete.Add(false);
        asyncOps.Add(SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive));
        int opIndex = asyncOps.Count - 1;
        asyncOps[opIndex].allowSceneActivation = false;
        while (!asyncOps[opIndex].isDone)
        {
            //Output the current progress
            //Debug.Log("Loading progress: " + (asyncOps[opIndex].progress * 100) + "%");

            // Check if the load has finished
            if (asyncOps[opIndex].progress >= 0.9f)
            {
                scenesLoadedComplete[opIndex] = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    static IEnumerator LoadAsyncSceneByIndex(int index)
    {
        scenesLoadedComplete.Add(false);
        asyncOps.Add(SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive));
        int opIndex = asyncOps.Count - 1;
        asyncOps[opIndex].allowSceneActivation = false;
        while (!asyncOps[opIndex].isDone)
        {
            //Output the current progress
            //Debug.Log("Loading progress: " + (asyncOps[opIndex].progress * 100) + "%");

            // Check if the load has finished
            if (asyncOps[opIndex].progress >= 0.9f)
            {
                scenesLoadedComplete[opIndex] = true;
                break;
            }
            yield return null;
        }
        yield return null;
    }

    static IEnumerator FinishLoad(SceneLoadProperties loadProperties)
    {
        completedScenes = 0;
        yield return new WaitUntil(() => ((scenesLoadedComplete.Count > 0) & !scenesLoadedComplete.Contains(false)));

        for (int i = 0; i < scenesLoadedComplete.Count; i++)
        {
            asyncOps[i].allowSceneActivation = true;

            asyncOps[i].completed += addCompleted => completedScenes++;
        }
        yield return new WaitUntil(() => asyncOps[asyncOps.Count - 1].isDone);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        yield return new WaitUntil(() => completedScenes == asyncOps.Count);
        for (int i = 0; i < scenesLoadedComplete.Count; i++)
        {
            asyncOps[i].completed -= addCompleted => completedScenes++;
        }
        asyncOps = new List<AsyncOperation>();
        if (!loadProperties.dontUnload)
            UnloadPreviousScenes();
        else
            onLoading = false;

        yield return null;
    }

    public static void UnloadPreviousScenes()
    {
        for (int i = 0; i < loadedScenes.Count; i++)
        {
            //Debug.Log(loadedScenes[i]);
            SceneManager.UnloadSceneAsync(loadedScenes[i]);
        }
        Resources.UnloadUnusedAssets();

        onLoading = false;
    }
}

[Serializable]
public class SceneLoadProperties
{
    public LoadType loadType;
    public LoadBy loadBy;

    public string sceneAssetPath;
    public int sceneBuildIndex;
    public string sceneStringName;

    public string[] scenesAssetsPath;
    public int[] scenesBuildIndexes;
    public string[] scenesStringNames;
    public bool dontUnload = false;

    public SceneLoadProperties()
    {
        loadType = new LoadType();
        loadBy = new LoadBy();

        sceneAssetPath = "";
        sceneBuildIndex = 0;
        sceneStringName = "";

        scenesAssetsPath = new string[0];
        scenesBuildIndexes = new int[0];
        scenesStringNames = new string[0];
        dontUnload = false;
    }

    public SceneLoadProperties(string sceneStringName, bool dontUnload = false, LoadType loadType = LoadType.SingleScene, LoadBy loadBy = LoadBy.String)
    {
        this.loadBy = loadBy;
        this.loadType = loadType;
        this.dontUnload = dontUnload;
        this.sceneStringName = sceneStringName;
    }

    public SceneLoadProperties(int sceneBuildIndex, bool dontUnload = false, LoadType loadType = LoadType.SingleScene, LoadBy loadBy = LoadBy.Index)
    {
        this.loadBy = loadBy;
        this.loadType = loadType;
        this.dontUnload = dontUnload;
        this.sceneBuildIndex = sceneBuildIndex;
    }

    public SceneLoadProperties(string[] scenesStringNames, bool dontUnload = false, LoadType loadType = LoadType.MultiScenes, LoadBy loadBy = LoadBy.String)
    {
        this.loadBy = loadBy;
        this.loadType = loadType;
        this.dontUnload = dontUnload;
        this.scenesStringNames = scenesStringNames;
    }

    public SceneLoadProperties(int[] scenesBuildIndexes, bool dontUnload = false, LoadType loadType = LoadType.MultiScenes, LoadBy loadBy = LoadBy.Index)
    {
        this.loadBy = loadBy;
        this.loadType = loadType;
        this.dontUnload = dontUnload;
        this.scenesBuildIndexes = scenesBuildIndexes;
    }

    public SceneLoadProperties(LoadBy loadBy, string sceneAssetPath, bool dontUnload = false, LoadType loadType = LoadType.SingleScene)
    {
        this.loadBy = loadBy;
        this.loadType = loadType;
        this.dontUnload = dontUnload;
        this.sceneAssetPath = sceneAssetPath;
    }

    public SceneLoadProperties(LoadBy loadBy, string[] scenesAssetsPath, bool dontUnload = false, LoadType loadType = LoadType.MultiScenes)
    {
        this.loadBy = loadBy;
        this.loadType = loadType;
        this.dontUnload = dontUnload;
        this.scenesAssetsPath = scenesAssetsPath;
    }
}