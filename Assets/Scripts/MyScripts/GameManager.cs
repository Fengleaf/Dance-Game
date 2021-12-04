using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelMenu;

    public Transform levelButtonContent;
    public GameObject levelButtonTemplate;

    public Text nowLevelText;

    private List<GameObject> levelButtons;

    private GameObject nowSelectButton;
    private int nowSelectIndex;

    // Start is called before the first frame update
    void Start()
    {
        levelButtons = new List<GameObject>();
        List<string> bvhFiles = new List<string>(Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "BVHs"), "*.bvh"));
        int index = 0;
        foreach (string s in bvhFiles)
        {
            string[] names = s.Split('\\');
            string levelName = names[names.Length - 1].Split('.')[0];
            GameObject levelButton = Instantiate(levelButtonTemplate, levelButtonContent);
            levelButton.transform.GetChild(0).GetComponent<Text>().text = levelName;
            levelButtons.Add(levelButton);
            levelButton.SetActive(true);
            int buttonIndex = index;
            levelButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel(buttonIndex));
            index++;
            SelectLevel(0);
        }
    }

    public void OnMenuStartClick()
    {
        MainMenu.SetActive(false);
        LevelMenu.SetActive(true);
    }

    public void OnMenuExitClick()
    {
        Application.Quit();
    }

    public void OnLevelStartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnLevelExitClick()
    {
        MainMenu.SetActive(true);
        LevelMenu.SetActive(false);
    }

    private void SelectLevel(int i)
    {
        if (nowSelectButton != null)
        {
            nowSelectButton.GetComponent<Image>().color = Color.white;
            nowSelectButton.transform.GetComponentInChildren<Text>().color = Color.black;
        }
        nowSelectButton = levelButtons[i];
        nowSelectButton.GetComponent<Image>().color = Color.red;
        nowSelectButton.transform.GetComponentInChildren<Text>().color = Color.white;
        nowSelectIndex = i;
        nowLevelText.text = nowSelectButton.transform.GetComponentInChildren<Text>().text;
        LevelData.levelName = nowLevelText.text;
    }
}
