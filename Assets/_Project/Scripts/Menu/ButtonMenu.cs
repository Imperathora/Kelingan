using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class ButtonMenu : MonoBehaviour
{
    [SerializeField] GameObject m_firstButton, m_openOptionsButton, m_closeOptionsButton, m_openControlsButton, m_closeControlsButton, m_openCreditsButton, m_closeCreditsButton, m_openPopUpButton, m_openControllerButton, m_openKeyboardButton, m_closeControllerButton, m_closeKeyboardButton;
    [SerializeField] GameObject m_optionsMenu, m_creditsMenu, m_titleScreen, m_mainMenu, m_playerControlls, m_popUpMenu, m_keyboardMenu, m_controllerMenu;

    [SerializeField] private Framework.Input.InputHandler m_UIInputHandler;
    [SerializeField] private Framework.Input.InputHandler m_characterInputHandler;

    private GameObject m_lastSelected;
    private Player m_player;

    private bool m_isTimer = false;

    private float m_timer;
    private float m_resetTimer;

    private void Start()
    {

        m_UIInputHandler.EnableInput(this);
        m_characterInputHandler.DisableInput(this);
        MenuController.m_menuEnabled = true;

        m_timer = 2f;
        m_resetTimer = m_timer;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_player = ReInput.players.GetPlayer(0);
        m_optionsMenu.SetActive(false);
        m_creditsMenu.SetActive(false);
        m_mainMenu.SetActive(false);
        m_titleScreen.SetActive(false);
        m_popUpMenu.SetActive(false);
        m_controllerMenu.SetActive(false);
        m_playerControlls.SetActive(false);
        m_keyboardMenu.SetActive(false);

        if (PauseMenu.m_gameIsPaused)
        {
            m_mainMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_firstButton);
        }
        else
            m_titleScreen.SetActive(true);
    }

    private void Update()
    {
        if (MenuController.m_menuEnabled)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                m_lastSelected = EventSystem.current.currentSelectedGameObject;

            if (Input.GetAxis("Mouse X") != 0 && MenuController.m_menuEnabled)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ResetTimer();
                m_isTimer = false;
            }

            if (Cursor.visible)
                EventSystem.current.SetSelectedGameObject(null);



            if (Input.GetAxis("Mouse X") == 0 && MenuController.m_menuEnabled)
            {
                m_isTimer = true;
            }

            if (m_isTimer)
            {
                m_timer -= Time.deltaTime;

                if (m_timer < 0)
                    m_timer = 0;
            }

            if (m_timer == 0f)
            {
                Cursor.lockState = CursorLockMode.Confined;
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

    public void OnMainMenu()
    {
        m_mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstButton);
    }

    public void OpenOptions()
    {
        m_playerControlls.SetActive(false);
        m_optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openOptionsButton);
    }

    public void CloseOptions()
    {
        m_optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeOptionsButton);
    }


    public void OpenCredits()
    {
        m_playerControlls.SetActive(false);
        m_creditsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openCreditsButton);
    }

    public void CloseCredits()
    {
        m_creditsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeCreditsButton);
    }

    public void OpenControls()
    {
        m_playerControlls.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openControlsButton);
    }

    public void CloseControls()
    {
        m_playerControlls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeControlsButton);
    }


    public void OpenKeyboard()
    {
        m_playerControlls.SetActive(false);
        m_keyboardMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openKeyboardButton);
    }

    public void CloseKeyboard()
    {
        m_keyboardMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeKeyboardButton);
    }

    public void OpenController()
    {
        m_playerControlls.SetActive(false);
        m_controllerMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openControllerButton);
    }

    public void CloseController()
    {
        m_controllerMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_closeControllerButton);
    }

    public void OnPopUp()
    {
        m_playerControlls.SetActive(false);
        m_popUpMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_openPopUpButton);
    }

    private void OnDestroy()
    {
        m_characterInputHandler.ClearInput();
        m_UIInputHandler.ClearInput();
    }
}
