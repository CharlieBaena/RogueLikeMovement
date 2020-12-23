using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ReproducirVideo : MonoBehaviour
{
    public RawImage panelVideo;
    public VideoPlayer videoPlayer;
    
    private AudioSource musica;

    public VideoClip vc1;

    private void Start()
    {
        StartCoroutine(PlayVideo());
    }

    public IEnumerator PlayVideo()
    {


        videoPlayer.clip = vc1;
        videoPlayer.Prepare();
        videoPlayer.frame = 0;
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while (!videoPlayer.isPrepared)
        {
            yield return null;
            Debug.Log("Preparing Video");
        }

        panelVideo.texture = videoPlayer.texture;
        panelVideo.gameObject.SetActive(true);

        videoPlayer.Play();



    }



    
}
