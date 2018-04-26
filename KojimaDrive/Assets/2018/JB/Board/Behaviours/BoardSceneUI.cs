using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JB
{

public class BoardSceneUI : MonoBehaviour
{
    [Header("Player Progress Sliders")]
    [SerializeField] List<Slider> playerSliders;

    [Header("GameMode Selection")]
    [SerializeField] GameObject selectionPanel;
    [SerializeField] Text lblModeTitle;
    [SerializeField] Image imgModeScreenshot;

    private IBoardManager iboard;


    public void Init(IBoardManager _iboard)
    {
        iboard = _iboard;
        for (int i = 0; i < playerSliders.Count; i++)
        {
            playerSliders[i].fillRect.GetComponent<Image>().color = LT.PlayerMaterialsManager.getPlayerCollection(i).playerColor;
        }
    }


    public void ShowModeSelection(KojimaParty.GameModeInfo _info)
    {
        selectionPanel.SetActive(true);

        lblModeTitle.text = _info.modeName;
        imgModeScreenshot.sprite = _info.modeScreenshot;
    }


    void Update()
    {
        if (iboard != null)
        {
            UpdateSliders();
        }
    }


    void UpdateSliders()
    {
        int i = 0;
        foreach (var slider in playerSliders)
        {
            float progress = iboard.GetPlayerProgress(i);
            slider.value = slider.value > progress ? 1 : progress;

            ++i;
        }
    }

}

} // namespace JB
