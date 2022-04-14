using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class SkipVideo : MonoBehaviour
{
    [SerializeField] private int m_sceneIndex = default;
    [SerializeField] private VideoPlayer m_videoPlayer = default;
    [SerializeField] private Framework.Input.InputHandler m_characterInputHandler = default;
    [SerializeField] private Framework.Input.InputHandler m_UIInputHandler = default;
    [SerializeField] private Image m_fillImage = default;
    [SerializeField] private float m_timeMultiplier = default;
         
    private Player m_player;
    private float m_timer = 0;
    private bool m_isButtonHeld;

    void Start()
    {
        m_characterInputHandler.DisableInput(this);
        m_UIInputHandler.DisableInput(this);

        m_player = ReInput.players.GetPlayer(0);
        m_videoPlayer.loopPointReached += LoadScene;

        m_fillImage.fillAmount = 0;
    }

    void LoadScene(VideoPlayer _videoPlayer)
    {
        SceneManager.LoadScene(m_sceneIndex);
        m_characterInputHandler.EnableInput(this);
        m_UIInputHandler.EnableInput(this);

    }


    void Update()
    {
        if (m_player.GetButtonSinglePressHold("Pause"))
        {
            m_isButtonHeld = true;
            m_timer += Time.unscaledDeltaTime * m_timeMultiplier;
            m_fillImage.fillAmount += m_timer;
            if (m_fillImage.fillAmount == 1)
            {
                m_timer = 0;
                SceneManager.LoadScene(m_sceneIndex);
            }
        }
        else
            m_isButtonHeld = false;

        if(!m_isButtonHeld && m_timer > 0)
        {
            m_timer -= Time.unscaledDeltaTime * m_timeMultiplier;
            m_fillImage.fillAmount -= m_timer;
            if (m_fillImage.fillAmount == 0)
            {
                m_timer = 0;
            }
        }
    }
}
