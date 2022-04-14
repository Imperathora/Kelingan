using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private int m_startGameScene = default;
    [SerializeField] private int m_startTutorialScene = default;
    [SerializeField] private Framework.Input.InputHandler m_characterInput = default;
    [SerializeField] private Framework.Input.InputHandler m_uiInput = default;
    [SerializeField] private AudioClip m_audioClip = default;

    public static bool m_menuEnabled = true;

    private float m_duration;


    private void Start()
    {
        m_duration = m_audioClip.length;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_menuEnabled = false;
        m_characterInput.ClearInput();
        m_uiInput.ClearInput();
        m_characterInput.EnableInput(this);
        m_uiInput.EnableInput(this);
        StartCoroutine(LoadAfterSound(m_startGameScene, m_duration));
    }

    IEnumerator LoadAfterSound(int _sceneID, float _time)
    {
        yield return new WaitForSeconds(_time);
        SceneManager.LoadScene(_sceneID);
    }

    public void StartTutorial()
    {
        Time.timeScale = 1f;
        m_uiInput.EnableInput(this);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_menuEnabled = false;
        m_characterInput.ClearInput();
        m_uiInput.ClearInput();
        m_characterInput.EnableInput(this);
        StartCoroutine(LoadAfterSound(m_startTutorialScene, m_duration));
    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
