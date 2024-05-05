using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawController : MonoBehaviour
{
    public GameObject gemParent;
    public GameObject jigsawParent;
    private GameObject[] gems;
    private GameObject[] jigsawPieces;
    private Transform[] jigsawSlots;

    private int jigsawPieceCount = 0;

    public Material gemMaterial;
    public Material gemCompletedMaterial;
    // Start is called before the first frame update
    void Start()
    {
        //fill arrays by finding children of parents
        FillArrays();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jigsawPieceCount == jigsawPieces.Length)
        {
            Debug.Log("Jigsaw Complete");
        }
    }

    void FillArrays()
    {
        //fill gems array with gem gameobjects
        gems = GetChildren(gemParent);
        foreach (GameObject gem in gems)
        {
            SetGemMaterial(gem);
        }
        //fill jigsaw pieces array with jigsaw pieces
        jigsawPieces = GetChildren(jigsawParent);
        
        //fill jigsaw slots array with jigsaw piece intial world positions
        jigsawSlots = new Transform[jigsawPieces.Length];
        for (int i = 0; i < jigsawPieces.Length; i++)
        {
            jigsawSlots[i] = jigsawPieces[i].transform;
            jigsawSlots[i].position = jigsawPieces[i].transform.position;
            jigsawSlots[i].rotation = jigsawPieces[i].transform.rotation;   
            //create a new empty gameobject to hold the jigsaw piece slot
            GameObject slot = new GameObject();
            //create box collider for slot
            BoxCollider slotCollider = slot.AddComponent<BoxCollider>();
            //set box collider to trigger
            slotCollider.isTrigger = true;
        }
    }

    GameObject[] GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
            Debug.Log(child.gameObject.name);
        }
        return children.ToArray();
    }

    void SetGemMaterial(GameObject gem)
    {
        gem.GetComponent<Renderer>().material = gemMaterial;
    }
}
