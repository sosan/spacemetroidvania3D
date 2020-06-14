using UnityEngine;
using System.Collections;
using System;
using UniRx.Async;

[ExecuteInEditMode]
public class TransitionManager : MonoBehaviour
{
    public Material transitionMaterial;

    private void Awake()
    {
        transitionMaterial.SetFloat("_Cutoff", 0f);
    }

    private void OnDisable()
    {
        if (transitionMaterial != null)
        { 
            transitionMaterial.SetFloat("_Cutoff", 0f);
        }

    }

    private void OnDestroy()
    {
        
        if (transitionMaterial != null)
        { 
        
            transitionMaterial.SetFloat("_Cutoff", 0f);
        }
    }


    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (transitionMaterial != null)
        { 
            Graphics.Blit(src, dst, transitionMaterial);
        }
        
    }

    public async void EnterTransition()
    {
        transitionMaterial.SetFloat("_Cutoff", 0f);
        for (float i = 0; i < 1.1; i += 0.06f)
        { 
            await UniTask.Delay(TimeSpan.FromMilliseconds( 6));
            transitionMaterial.SetFloat("_Cutoff", i);
    
        
        }


    }

    public async void ExitTransition()
    { 
        for (float i = 1; i >= -0.1; i -= 0.06f)
        { 
            await UniTask.Delay(TimeSpan.FromMilliseconds( 6));
            transitionMaterial.SetFloat("_Cutoff", i);
    
        
        }
    
    }

}
