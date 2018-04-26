using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace JB
{

public class GameRoomUI : MonoBehaviour
{

    public CanvasGroup brokenGlass;
    public GameObject MusicBox;
    public GameObject CameraWall;
    [SerializeField] ParticleSystem Notes;
    private ParticleSystem ps;
    public int[] scores;

    // Update is called once per frame
    void Start()
    {
        if (MusicBox !=null)
        {
            MusicBox.name = "MusicBox";
        }
        if (CameraWall != null)
        {
            CameraWall.name = "CameraPunchWall";
        }
    }

    void Update()
    {
        if (brokenGlass.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            brokenGlass.GetComponent<CanvasGroup>().alpha -= 0.1f * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PunchMusicBox();
        }
    }

    //brokenGlass
    public void BreakGlass()
    {
        AudioManager.PlayOneShot("window_glassBreak");
        brokenGlass.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    public void PunchMusicBox()
    {
        MusicBox.GetComponent<Animator>().SetTrigger("Pressed");
        AudioManager.instance.PlayRandomMusic();
        ps = Instantiate(Notes);
        Notes.transform.position = MusicBox.transform.position;
    }

}

} // namespace JB
