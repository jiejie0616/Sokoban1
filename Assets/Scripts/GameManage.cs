using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour {
    // 0空白  1 墙  2地板  3空坑  4满坑  5豆 6玩家
    public GameObject[] brickPrefabs;           // 砖块预载体
    public GameObject[] brickParent;            // 砖块存放位置

    private int curLevel = 0;                   // 当前关卡
    private int curStep = 0;                    // 当前步数
    private GameObject playerGo;                // 玩家实体
    private List<GameObject> gos = new List<GameObject>();              // 游戏实体
    private List<GameObject> beans = new List<GameObject>();             // 豆子
    private List<Vector2> beansPos = new List<Vector2>();             // 豆子位置
    private List<Vector2> emptysPos = new List<Vector2>();            // 空坑位置

    public GameObject finishedGo;               // 胜利界面
    public Text curLevelText;                   // 当前关卡组件
    public Text curStepText;                   // 当前步数组件

    private static GameManage instance;         // 单例

    public static GameManage Instance
    {
        get { return GameManage.instance; }
        set { GameManage.instance = value; }
    }

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        curLevel = PlayerPrefs.GetInt("currentLevel");
        InitMap();
	}
	
	// Update is called once per frame
	void Update () {
        if (isFinished())
        {
            finishedGo.SetActive(true);
        }
	}

    // 创建实体，并将实体放在当前组件下
    private GameObject CreateItem(GameObject createCameObject, GameObject goParent, Vector3 createPosition, Quaternion createRotation)
    {
        GameObject itemGo = Instantiate(createCameObject, createPosition, createRotation);  // 创建实体
        itemGo.transform.SetParent(goParent.transform);                                   // 放在该目录下
        return itemGo;
    }

    // 初始化地图模型
    private void InitMap()
    {
        curStep = 0;            // 初始化当前步数为 0
        curStepText.text = string.Format("{0:000}", curStep);
        curLevelText.text = string.Format("{0:000}", curLevel + 1); 
        string[] level = Config.Instance.levels[curLevel].Split(",".ToCharArray());         // 加载当前关卡数据
        //Debug.Log(Config.Instance.levels.Length);
        for (int i = 0; i < level.Length; ++i)
        {
            string tmp = level[i];                                                          // 第i行数据
            for (int j = 0; j < tmp.Length; ++j)                                            // 创建每一个方块
            {
                Vector3 pos = transform.position + new Vector3(1.25f * j, -1.25f * i, 0);
                int brickData = int.Parse(tmp[j].ToString());
                // 实例化
                if (brickData != 2)
                {
                    GameObject itemGo = CreateItem(brickPrefabs[brickData], brickParent[brickData], pos, Quaternion.identity);
                    gos.Add(itemGo);
                    if (brickData == 6)         // 存储玩家实体
                    {
                        playerGo = itemGo;
                    }
                    else if (brickData == 3)    // 存储空坑位置
                    {
                        emptysPos.Add(itemGo.transform.position);
                    }
                    else if (brickData == 5)    // 存储豆子实体
                    {
                        beans.Add(itemGo);
                    }
                }
            }
        }
    }

    // 获取当前豆子位置
    public void GetBeanPos()
    {
        beansPos.Clear();
        for (int i = 0; i < beans.Count; ++i)
        {
            beansPos.Add(beans[i].transform.position);
        }
    }


    // 检测是否胜利
    private bool isFinished()
    {
        int count = 0;              // 满坑个数
        for (int i = 0; i < emptysPos.Count; ++i)
        {
            if (beansPos.Contains(emptysPos[i]))
            {
                count++;
            }
        }
        if (count == emptysPos.Count)       // 全部就位
        {
            return true;
        } 
        else
            return false;
    }

    // 摧毁所有实体
    private void DestroyAllGo()
    {
        for (int i = 0; i < gos.Count; ++i)
        {
            Destroy(gos[i]);
        }
        gos.Clear();                // 初始化
        beans.Clear();
        beansPos.Clear();
        emptysPos.Clear();
    }

    // 重玩游戏
    public void OnRestartBtnClick()
    {
        DestroyAllGo();
        InitMap();
    }

    // 上一关
    public void OnPreLevelBtnClick()
    {
        if (curLevel > 0)
        {
            curLevel--;
            DestroyAllGo();
            InitMap();
        }
    }

    // 下一局
    public void OnLastLevelBtnClick()
    {
        if (curLevel < 99)
        {
            curLevel++;
            DestroyAllGo();
            InitMap();
        }
    }

    // 回到菜单
    public void OnReturnToMenuBtnClick()
    {
        SceneManager.LoadScene(0);
    }

    // 退出游戏
    public void OnQuitGameBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();      
#endif
    }

    // 改变步数
    public void ChangeStep(int num = 1)
    {
        curStep += num;
        curStepText.text = string.Format("{0:000}", curStep);
    }

    // 胜利界面点击确定按钮
    public void OnFinishedOkBtnClick()
    {
        finishedGo.SetActive(false);
        OnLastLevelBtnClick();
    }
}
