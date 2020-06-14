using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System;

public class Laser : MonoBehaviour
{
    int damage = 100;

    [SerializeField] float speed = 10f;

    [SerializeField] float lifeTime = 3f;

    public int Damage { get => damage; set => damage = value; }

    private void Start()
    {
        StartCoroutine(LifeTimeCoundown());
    }

    // Start is called before the first frame update
    void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (other == PlayerMovement.instancePlayer.GetComponent<Collider>())
        {
            PlayerMovement.instancePlayer.SetHealth(damage);

            Destroy(gameObject);
        }
    }

    IEnumerator LifeTimeCoundown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}