using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControlesGlobal;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public enum Status {
	accel,
	freno,
	idle,
}

public class RuedaPropulsion : MonoBehaviour 
{


	private Controles inputActions;
	
	[SerializeField] private float maxSpeed = 175;
	[SerializeField] private float accelTime = 10f;
	[SerializeField] private float breakTime = 3f;
	[SerializeField] private float idleTime = 20f;
	[SerializeField] private float accelBrightness = 0.15f;
	[SerializeField] private float brakeBrightness = 0.15f;
	[SerializeField] private float lerpTime = 0.2f;
	private float interpStartTime = 0f;
	private Color targetColor;
	[SerializeField] private float initialRotationSpeed;
	[SerializeField] private float initialFillPercentage;

	[SerializeField] private Material materialRueda;

	[SerializeField] private float rotationMultiplier;
	private float accelAmount;
	private float breakAmount;
	private float idleAmount;

	private float fillPercentage;
	
	private Color initialMaxColor;

	private Status status;

	

	private void OnEnable()
    {

        inputActions?.Enable();

    }


    private void OnDisable()
    {

        inputActions?.Disable();
        

    }


	private void Awake()
	{
		//inputActions = new Controles();
		//inputActions.PlayerMovement.saltar.started += Saltar_started;
  //      inputActions.PlayerMovement.saltar.canceled += Saltar_canceled;

		//inputActions.PlayerMovement.frenar.started += Frenar_started;
  //      inputActions.PlayerMovement.frenar.canceled += Frenar_canceled;

		//inputActions?.Enable();
	}

	private void Start() 
	{

		
		materialRueda.SetFloat("_Fillpercentage", initialFillPercentage);
		materialRueda.SetFloat("_Maintexrotationspeed", initialRotationSpeed);
		initialMaxColor = materialRueda.GetColor("_Barmaxcolor");
		
		fillPercentage = initialFillPercentage;
		targetColor = initialMaxColor;
		status = Status.idle;
		accelAmount = 1f / accelTime * Time.fixedDeltaTime;
		breakAmount = 1f / breakTime * Time.fixedDeltaTime;
		idleAmount = 1f / idleTime * Time.fixedDeltaTime;
		
		materialRueda.SetFloat("_Fillpercentage", fillPercentage);

	}

	private void Saltar_canceled(InputAction.CallbackContext obj)
	{
		status = Status.idle;
		interpStartTime = Time.time;
		targetColor = initialMaxColor;
		setRotation(1f);
	}

	private void Saltar_started(InputAction.CallbackContext obj)
	{
		print("dsdf");
		status = Status.accel;
		interpStartTime = Time.time;
		targetColor = shiftValue(initialMaxColor, accelBrightness);
		setRotation(rotationMultiplier);
	}

	private void Frenar_canceled(InputAction.CallbackContext obj)
	{
		status = Status.idle;
		interpStartTime = Time.time;
		targetColor = initialMaxColor;
		setRotation(1f);
	}

	private void Frenar_started(InputAction.CallbackContext obj)
	{
		status = Status.freno;
		targetColor = shiftValue(initialMaxColor, brakeBrightness);
		interpStartTime = Time.time;
	}


	void FixedUpdate() 
	{
		if (status == Status.accel) 
		{
			fillPercentage -= accelAmount;
		}

		else if (status == Status.freno)
		{
			fillPercentage += breakAmount;
		}

		else 
		{
			fillPercentage += idleAmount;
		}
		
		fillPercentage = Mathf.Clamp(fillPercentage, 0f, 1f);
		materialRueda.SetFloat("_Fillpercentage", fillPercentage);

		float elapsedTime = Time.time - interpStartTime;
		if (elapsedTime < lerpTime) 
		{
			materialRueda.SetColor("_Barmaxcolor", Color.Lerp(materialRueda.GetColor("_Barmaxcolor"), targetColor, elapsedTime / lerpTime));
		}
	}


	private Color shiftValue(Color color, float amount) 
	{
		float h;
		float s;
		float v;
		Color.RGBToHSV(color, out h, out s, out v);
		v += amount;
		return Color.HSVToRGB(h,s,v);
	}


	private void setRotation(float multiplier) 
	{
		materialRueda.SetFloat("_Maintexrotationspeed", initialRotationSpeed * multiplier);
	}

	private void OnApplicationQuit() 
	{
		fillPercentage = initialFillPercentage;
		materialRueda.SetFloat("_Fillpercentage", fillPercentage);
		materialRueda.SetColor("_Barmaxcolor", initialMaxColor);
		materialRueda.SetFloat("_Maintexrotationspeed", initialRotationSpeed);
	}

}
