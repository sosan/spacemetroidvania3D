using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
using System;

public class BoxManager : MonoBehaviour
{


    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private Animation anim = null;

    [Header("Cajas")]
    [SerializeField] private GameObject cajaSinFractura = null;
    [SerializeField] private GameObject cajaConFractura = null;
    [SerializeField] private BoxCollider colliderCajasFin = null;


    [SerializeField] public bool isUnbreakeable = false;
    [SerializeField] public bool isMovable = true;
    [SerializeField] private bool isBroken = false;
    [SerializeField] private bool startBroken = false;

    [Header("General Settings")]
    [SerializeField] private float force = 100f;
    [SerializeField] private AudioClip[] clips = null;

    private AudioSource sfxBreak = null;
    private ParticleSystem particula = null;

    private Vector3 puntoContacto = Vector3.zero;

    [SerializeField] private Rigidbody rigid = null;
    [SerializeField] private Collider colliderBox = null;
    [SerializeField] private Rigidbody[] rigidsBoxFracture = null;

    [SerializeField] public ushort pisosActuales = 0;
    [SerializeField] private const ushort pisosMaximo = 2;


    // Start is called before the first frame update
    void Start()
    {
        //por si acaso hay alguno que no es kinematic
        SetKinematic();
        
    }

   

    private void SetKinematic()
    {

        for (ushort i = 0; i < rigidsBoxFracture.Length; i++)
        { 
            rigidsBoxFracture[i].isKinematic = true;
            rigidsBoxFracture[i].GetComponent<Collider>().enabled = true;
        
        }

        colliderBox.enabled = true;

    
    }

    public async void AnimBox(GameObject boxAbajo, Rigidbody arribaRigid)
    {

        playerMovement.CancelarObjetoCogido();

        pisosActuales++;

        AnimationClip clip = new AnimationClip();
        clip.name = "subir_caja_t";
        clip.legacy = true;
        clip.wrapMode = WrapMode.Once;

        Keyframe[] keysX = new Keyframe[4];

        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;

        keysX[0] = new Keyframe(0f, x );
        keysX[1] = new Keyframe(0.07f, x );
        keysX[2] = new Keyframe(0.19f, x += 1.21f );
        keysX[3] = new Keyframe(0.30f, x += 0.63f /*1.26f*/);

        Keyframe[] keysY = new Keyframe[4];

        keysY[0] = new Keyframe(0f, y);
        keysY[1] = new Keyframe(0.07f, y += (2.04f * pisosActuales)  );
        keysY[2] = new Keyframe(0.19f, y += 0.52f );
        keysY[3] = new Keyframe(0.30f, y -= 0.63f);

        Keyframe[] keysZ = new Keyframe[1];
        keysZ[0] = new Keyframe(0, z);

        
        AnimationCurve curvex = new AnimationCurve(keysX);
        AnimationCurve curvey = new AnimationCurve(keysY);
        AnimationCurve curvez = new AnimationCurve(keysZ);

        clip.SetCurve("", typeof(Transform), "localPosition.x", curvex);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curvey);
        clip.SetCurve("", typeof(Transform), "localPosition.z", curvez);

        

        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);

        await UniTask.Delay(TimeSpan.FromMilliseconds(anim.GetClip(clip.name).length * 1000 + 200));

        rigid.useGravity = true;

        rigid.constraints = RigidbodyConstraints.FreezeRotation;

        await UniTask.Delay(500);
        //rigid.useGravity = false;
        FixedJoint juntaFija = boxAbajo.AddComponent<FixedJoint>();
        juntaFija.connectedBody = arribaRigid;

        

        this.GetComponentInChildren<BoxEsquinasManager>().izquierdaArriba.enabled = false;
        this.GetComponentInChildren<BoxEsquinasManager>().derechaArriba.enabled = false;
        arribaRigid.GetComponentInChildren<BoxEsquinasManager>().izquierdaAbajo.enabled = false;
        arribaRigid.GetComponentInChildren<BoxEsquinasManager>().derechaAbajo.enabled = false;

    }
    

    private void OnCollisionEnter(Collision collision)
    {

# if UNITY_EDITOR
        print("Box Manager => OnCollision -> tag=" + collision.transform.tag + " name=" + collision.transform.name  );
#endif

        if (collision.transform.CompareTag("CajaSuelta") == true)
        { 
            if (playerMovement.isTouchingBox == true && playerMovement.objectCollide == collision.transform.gameObject && playerMovement.isPresedTaken == true)
            { 
                if (pisosActuales >= pisosMaximo) return;
                
                
                playerMovement.objectCollide.GetComponent<BoxManager>().AnimBox(this.gameObject, 
                    collision.gameObject.GetComponent<Rigidbody>());

                
                

            }

        }

        if (collision.transform.CompareTag("Ataque") == true)
        { 
            if (isUnbreakeable == true) return;
            if (isBroken == true) return;

            //print(collision.contacts[0].point);
            DestruirBoxPorAtaque(collision.contacts[0].point);

           

        }

        


    }


    private async void DestruirBoxPorAtaque(Vector3 puntoContacto)
    { 

        //print("puntocontacto="  + puntoContacto);
        isBroken = true;

        cajaSinFractura.SetActive(false);
        cajaConFractura.SetActive(true);

        colliderBox.enabled = false;
        colliderCajasFin.enabled = false;

        for (ushort i = 0; i < rigidsBoxFracture.Length; i++)
        {
            rigidsBoxFracture[i].isKinematic = false;
            rigidsBoxFracture[i].GetComponent<MeshCollider>().enabled = true;
                
            rigidsBoxFracture[i].AddExplosionForce(force, puntoContacto, 100);
                
        }

        await UniTask.Delay(300);

        for (ushort i = 0; i < rigidsBoxFracture.Length; i++)
        {
            rigidsBoxFracture[i].GetComponent<MeshCollider>().enabled = false;
                
        }

        await UniTask.Delay(1000);
        cajaSinFractura.SetActive(false);
        cajaConFractura.SetActive(false);
        this.gameObject.SetActive(false);
        return;
    
    
    
    }


    public async void DestruirBox(float fuerza)
    { 
        cajaSinFractura.SetActive(false);
        cajaConFractura.SetActive(true);

        //cajaConFractura.layer = LayerMask.NameToLayer("IgnoreRaycast");

        //rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rigid.constraints = RigidbodyConstraints.None;
        puntoContacto = this.gameObject.transform.position;

        colliderCajasFin.enabled = false;

        var arribaRigid = rigid.GetComponent<FixedJoint>();
        if (arribaRigid != null)
        { 


            Rigidbody[] rigidBodiesFracture = arribaRigid.GetComponent<BoxManager>().rigidsBoxFracture;

            for (ushort i = 0; i < rigidsBoxFracture.Length; i++)
            {
                rigidsBoxFracture[i].isKinematic = false;
                rigidBodiesFracture[i].isKinematic = false;
                
                rigidsBoxFracture[i].GetComponent<MeshCollider>().enabled = true;
                rigidBodiesFracture[i].GetComponent<MeshCollider>().enabled = true;
                
                rigidsBoxFracture[i].AddExplosionForce(fuerza, puntoContacto, 100);
                rigidBodiesFracture[i].AddExplosionForce(fuerza, puntoContacto, 100);

                
            }

                        
        
        }
        else
        { 
            for (ushort i = 0; i < rigidsBoxFracture.Length; i++)
            {
                rigidsBoxFracture[i].isKinematic = false;
                rigidsBoxFracture[i].GetComponent<MeshCollider>().enabled = true;

                
                rigidsBoxFracture[i].AddExplosionForce(fuerza, puntoContacto, 100);
                
            }

        
        }

        
        
        colliderBox.enabled = false;

        await UniTask.Delay(3000);

        for (ushort i = 0; i < rigidsBoxFracture.Length; i++)
        {
            //rigidsBoxFracture[i].isKinematic = true;
            rigidsBoxFracture[i].GetComponent<Collider>().enabled = false;
                
        }

        cajaSinFractura.SetActive(false);
        cajaConFractura.SetActive(false);
        this.gameObject.SetActive(false);
    
    
    
    }

    private void OnTriggerEnter(Collider other)
    {

        //print("Box Manager => On Tirgger enter=>" + other.tag + " name=" + other.name);
        if (other.CompareTag("CajaCollider") == true)
        {

            playerMovement.isTouchingBox = true;
            playerMovement.objectCollide = this.gameObject;

        }


    }


    private void OnParticleCollision(GameObject other)
    {
        
        if (other.CompareTag("Player") == true)
        {
            if (isUnbreakeable == true) return;
            if (isBroken == true) return;
            DestruirBoxPorAtaque(Vector3.left);

        }


    }



}
