using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// if video player component is playing a clip then hide line renderer of vr controllers otherwise show them
public class DisableLines : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public VideoPlayer fadeToBlackVideoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeToBlackVideoPlayer.isPlaying)
        {
            foreach (LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = false;
            }
        }
        else
        {
            foreach (LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.enabled = true;
            }
        }
    }
}
