using UnityEngine;
using System.Collections;
using UICore;
using UnityEngine.SceneManagement;
enum GameState
{
    Playing,
    Unmanned
}
public class StateTime : MonoBehaviour
{
    //无人操作时间
    public float noBadyTime = 5f;
    private float currentTime;
    private bool isStartScene = false;
    private GameState _gameState;
    void Start()
    {
        noBadyTime = float.Parse(Configs.Instance.LoadText("无人操作返回", "time"));
        string cursorstate = Configs.Instance.LoadText("隐藏鼠标", "false/true");
        if (cursorstate == "false")
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        //切换场景不会被删除
        GameObject.DontDestroyOnLoad(this.gameObject);
        _gameState = GameState.Unmanned;
    }

    void Update()
    {
        //switch (_gameState)
        //{
        //    case GameState.Playing:
        //        if (isStartScene)
        //        {
        //            _gameState = GameState.Unmanned;
        //        }
        //        break;
        //    case GameState.Unmanned:
        //        UIManager.Instance.HideAllUI();
        //        UIManager.Instance.ShowUI(EUiId.MainUI);
        //        isStartScene = false;
        //        _gameState = GameState.Playing;
        //        break;
        //}
        MoreTimeDetect();

        if (isStartScene)
        {
            currentTime = 0;
            if (UIManager.Instance.CurrentId != EUiId.MainUI)
            {
                UIManager.Instance.ShowUI(EUiId.MainUI,UIManager.Instance.CurrentId);
            }
        }
    }

    /// <summary>
    /// 超时检测
    /// </summary>
    void MoreTimeDetect()
    {
        if (Input.anyKey || Input.anyKeyDown)
        {
            currentTime = Time.timeSinceLevelLoad;
        }

        if (Time.timeSinceLevelLoad >= currentTime + noBadyTime)
        {
            isStartScene = true;
        }
        else
        {
            isStartScene = false;
        }
    }
}

