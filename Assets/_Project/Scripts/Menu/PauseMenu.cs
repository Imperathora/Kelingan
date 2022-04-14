using OOB;
using OOB.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;

public class PauseMenu : MonoBehaviour
{
    public static bool m_gameIsPaused = false;

    [SerializeField] GameObject m_pauseMenu, m_pauseOptions, m_pauseControls, m_pauseKeyboard, m_pauseController;
    [SerializeField]
    GameObject m_pauseFirstButton, m_optionsFirstButton, m_optionsClosedButton, m_openControlsButton, m_closeControlsButton,
        m_openControllerButton, m_openKeyboardButton, m_closeControllerButton, m_closeKeyboardButton;
    [SerializeField] private int m_mainMenuScene = default;
    [SerializeField] private Framework.Input.InputHandler m_characterInputHandler;
    [SerializeField] private Framework.Input.InputHandler m_UIInputHandler;

    [SerializeField] private AudioClip m_audioClip = default;
    [SerializeField] private AudioSource[] m_audioSources = default;

    [SerializeField] private AudioSource m_menuSoundsSource = default;

    private float m_duration;

    private GameObject m_lastSelected;
    private Player m_player;

    private bool m_isTimer = false;

    private float m_timer;
    private float m_resetTimer;

    private void Start()
    {
        m_duration = m_audioClip.length;

        m_timer = 2f;
        m_resetTimer = m_timer;

        m_player = ReInput.players.GetPlayer(0);
        m_pauseMenu.SetActive(false);
        m_pauseOptions.SetActive(false);
        m_pauseControls.SetActive(false);
        m_pauseKeyboard.SetActive(false);
        m_pauseController.SetActive(false);

        Time.timeScale = 1f;
        m_gameIsPaused = false;
        m_characterInputHandler.EnableInput(this);
        m_UIInputHandler.EnableInput(this);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    private void Update()
    {
        if (m_player.GetButtonDown("Pause"))
        {
            if (m_gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
                m_characterInputHandler.ClearInput();
            }
        }

        if (m_gameIsPaused)
        {


            if (EventSystem.current.currentSelectedGameObject != null)
                m_lastSelected = EventSystem.current.currentSelectedGameObject;

            if (Input.GetAxis("Mouse X") != 0 && m_gameIsPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ResetTimer();
                m_isTimer = false;
            }

            if (Cursor.visible)
                EventSystem.current.SetSelectedGameObject(null);

            if (Input.GetAxis("Mouse X") == 0 && m_gameIsPaused)
            {
                m_isTimer = true;
            }

            if (m_isTimer)
            {
                m_timer -= Time.unscaledDeltaTime;

                if (m_timer < 0)
                    m_timer = 0;
            }

            if (m_timer == 0f)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                EventSystem.current.SetSelectedGameObject(m_lastSelected);
                ResetTimer();
                m_isTimer = false;
            }
        }
    }


    private void ResetTimer()
    {
        m_timer = m_resetTimer;
    }

    public void Resume()
    {
        m_pauseControls.SetActive(false);

        if (m_audioSources == null)
            return;
        if (m_audioSources.Length > 0)
        {
            foreach (AudioSource source in m_audioSources)
                source.UnPause();
        }

        m_pauseMenu.SetActive(false);
        m_pauseOptions.SetActive(false);
        m_pauseKeyboard.SetActive(false);
        m_pauseController.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        m_gameIsPaused = false;
        m_characterInputHandler.EnableInput(this);
    }

    private void Pause()
    {
        if (m_audioSources == null)
            return;

        if (m_audioSources.Length > 0)
        {
            foreach (AudioSource source in m_audioSources)
                source.Pause();
        }


        m_menuSoundsSource.UnPause();
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        m_gameIsPaused = true;
        m_characterInputHandler.DisableInput(this);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_pauseFirstButton);
    }

    public void LoadMenu()
    {
        m_menuSoundsSource.UnPause();
        m_characterInputHandler.ClearInput();
        m_UIInputHandler.ClearInput();
        m_pauseControls.SetActive(false);
        m_pauseMenu.SetActive(false);
        m_gameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(m_mainMenuScene);
    }

    public void OpenOptions()
    {
        m_pauseControls.SetActive(false);
        m_pauseOptions.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_optionsFirstButton);
    }

    public void CloseOptions()
    {
        m_pauseOptions.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_optionsClosedButton);
    }


    public void OpenControls()
    {
        m_pauseControls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openControlsButton);
    }

    public void CloseControls()
    {
        m_pauseControls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeControlsButton);
    }

    public void OpenKeyboard()
    {
        m_pauseKeyboard.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openKeyboardButton);
    }

    public void CloseKeyboard()
    {
        m_pauseKeyboard.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeKeyboardButton);
    }

    public void OpenController()
    {
        m_pauseController.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openControllerButton);
    }

    public void CloseController()
    {
        m_pauseController.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeControllerButton);
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
