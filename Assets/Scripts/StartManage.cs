using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class StartManage : MonoBehaviour {
    public GameObject chooseLevelGo;
    private Dropdown chooseLevelDropdown;

    public void Awake()
    {
        chooseLevelDropdown = chooseLevelGo.GetComponent<Dropdown>();
        List<string> levelsList = new List<string>();
        for (int i = 1; i <= 100; ++i)
        {
            levelsList.Add(i + "");
        }
        chooseLevelDropdown.ClearOptions();
        chooseLevelDropdown.AddOptions(levelsList);
    }

    // 新游戏
    public void OnNewGameBtnClick()
    {
        PlayerPrefs.SetInt("currentLevel", 0);
        SceneManager.LoadScene(1);
    }

    // 选择关卡
    public void OnChooseLevelBtnClick()
    {
        chooseLevelGo.SetActive(true);
    }

    // 选择关卡确定按钮
    public void OnChooseLevelOkBtnClick()
    {
        chooseLevelGo.SetActive(false);
        PlayerPrefs.SetInt("currentLevel", chooseLevelDropdown.value);
        SceneManager.LoadScene(1);
    }

    public void OnCancelBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();      
#endif
    }
}
