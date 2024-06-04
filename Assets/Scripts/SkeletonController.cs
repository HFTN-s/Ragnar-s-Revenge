using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public Transform target; // The target object to follow
    public float speed = 5f; // The speed at which the prefab will move
    private Animator animator;
    public BoxCollider headCollider;
    private TutorialLevelManager levelManager;
    public AudioSource skeletonAudioSource;
    public AudioClip skeletonDeathClip;
    public AudioClip skeletonShuffleClip;
    public AudioClip skeletonGroanClip;
    public AudioClip skeletonHitClip;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("TutorialLevelManager").GetComponent<TutorialLevelManager>();
        animator = GetComponent<Animator>();
        skeletonAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("crawling"))
        {
            FollowTarget();
        }
    }

    public void ActivateSkeleton()
    {
        // play groan clip one shot
        skeletonAudioSource.clip = skeletonGroanClip;
        skeletonAudioSource.PlayOneShot(skeletonGroanClip);
        headCollider.enabled = true;
        animator.SetBool("crawling", true);
        animator.SetBool("sleeping", false);
        //activate mesh collider
        GetComponent<MeshCollider>().enabled = true;
        //activate navmesh agent
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;

    }

    void FollowTarget()
    {
        skeletonAudioSource.clip = skeletonShuffleClip;
        if (!skeletonAudioSource.isPlaying)
        {
            skeletonAudioSource.Play();
        }
        // Calculate the direction towards the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Move the prefab towards the target
        transform.position += direction * speed * Time.deltaTime;

        // Calculate the rotation needed to face the target
        Vector3 targetDirection = target.position - transform.position;
        float step = speed * Time.deltaTime; // Rotation speed
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void SetDead()
    {
        animator.SetBool("crawling", false);
        animator.SetBool("sleeping", true);
        //disable animator
        animator.enabled = false;
        // play collapse sound
        skeletonAudioSource.clip = skeletonDeathClip;
        skeletonAudioSource.Play();
        //freeze rigidbody
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        // play jarl voice line 16
        levelManager.PlayJarlVoiceLine(16);
    }
}
