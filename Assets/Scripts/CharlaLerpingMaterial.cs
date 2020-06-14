using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jl no hace falta el editmode
//[ExecuteInEditMode]
public class CharlaLerpingMaterial : MonoBehaviour
{
    [SerializeField] private Material materialCharla = null;
    [SerializeField] private string variable = "";
    [SerializeField] private AnimationCurve anim = null;
    [SerializeField] private float multiplicador = 1;
    [SerializeField] private float speed = 1;
	
	private void Start () {
		
	}
	
	private void Update ()
    {
        if ((materialCharla is null) == false)
        { 
            materialCharla.SetFloat(variable, anim.Evaluate(Time.time * speed) * multiplicador);
        
        } 
    }
}
