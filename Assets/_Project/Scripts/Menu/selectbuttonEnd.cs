using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;
using UnityEngine.SceneManagement;

public class selectbuttonEnd : MonoBehaviour
{
    [SerializeField] GameObject m_button;
    [SerializeField] GameObject m_endMenu;
    [SerializeField] float m_time;

    [SerializeField] private int m_scene = default;


    private Player m_player;


    private void Start()
    {

        StartCoroutine(LoadAfterSEconds(m_scene, m_time));
    }


    IEnumerator LoadAfterSEconds(int _sceneID, float _time)
    {
        yield return new WaitForSeconds(_time);
        SceneManager.LoadScene(_sceneID);
    }
}
