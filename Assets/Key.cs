using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private BoxCollider lockCollider;
    public event System.Action OnKeyUsed;
    [SerializeField] private string gameObjectName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == gameObjectName)
        {
            //unfreeze other gameobject
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //delete key
            Destroy(gameObject);
        }
    }
}
