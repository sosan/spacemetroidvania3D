using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCogerObjecto : MonoBehaviour
{

    
    [Header("Manager")]
    [SerializeField] private PlayerMovement playerMovement = null;

    [SerializeField] public GameObject objectCollide = null;
    [SerializeField] private Transform parentObjectTaken = null;
    [SerializeField] private Transform parentScene = null;
    [SerializeField] public bool isPresedTaken = false;
    [SerializeField] private LayerMask whatIsBox;


    public bool isTouchingBox = false;
    public bool isBoxCollide = false;
    public bool isBoxTaken = false;

    private RaycastHit[] resultsTouchBox = new RaycastHit[25];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CogerObjeto_started()
    {

        

# if UNITY_EDITOR
        print("apretado boton shift. istouching=" + isTouchingBox + " isBoxCollide=" + isBoxCollide);
# endif

        if (isTouchingBox == false)
        {
            
            bool colisions = 1 <= Physics.RaycastNonAlloc(this.transform.position, this.transform.right, resultsTouchBox, 2f * playerMovement.facingDirection, whatIsBox);
            print(colisions);
            if (colisions == false )
            { 
                return;
            
            }
            
        }

        isPresedTaken = true;
      
        //apretado boton shift. isTouchingBox=True isBoxCollide=False objectCollide.name=cajas

        if (isBoxCollide == true) return;
        if (objectCollide == null) return;
        if (objectCollide.GetComponent<BoxManager>().isMovable == false) return;
            

        isBoxCollide = true;
        isBoxTaken = true;
        playerMovement.canMove = true;
            
        if (objectCollide != null)
        { 
            objectCollide.transform.parent = parentObjectTaken;
            Transform linkedBox = objectCollide.GetComponent<BoxManager>().linkedBox;

            if (linkedBox != null)
            { 
                linkedBox.transform.parent = parentObjectTaken;
                
                var tempBox = linkedBox.GetComponent<BoxManager>();
                if (tempBox != null)
                { 
                    tempBox.transform.parent = parentObjectTaken; //linkedBox.transform;
                    
                
                }
                
            }

            //objectCollide.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

   

    public void CancelarObjetoCogido()
    {

        if (objectCollide == null) return;

# if UNITY_EDITOR
        print("BOTON MAYS CANCELADO sujeta box");
# endif


        var box = parentObjectTaken.GetComponentInChildren<BoxManager>();
        if (box != null)
        {

            if (objectCollide != null)
            {
                objectCollide.transform.parent = parentScene;

                var box1 = objectCollide.GetComponent<BoxManager>();
                if (box1 != null)
                { 
                    var tempBox2 = box1.linkedBox;
                    if (tempBox2 != null)
                    { 
                        var box2 = tempBox2.GetComponent<BoxManager>();

                        if (box2 != null)
                        { 
                            box2.transform.parent = parentScene;
                        
                        }
                    
                    
                    }

                    box1.transform.parent = parentScene;

                
                }

            }

            box.transform.parent = parentScene;
            isBoxTaken = false;

        
        }

        
        isPresedTaken = false;
        isBoxCollide = false;
        isTouchingBox = false;
        
        
        objectCollide = null;
    
    
    }



   

}
