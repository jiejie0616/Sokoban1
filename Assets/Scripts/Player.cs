using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerDirection                 // 玩家方向
{
    up, 
    down,
    left,
    right,
    none
}

public class Player : MonoBehaviour {
    public Sprite[] playerSprites;          // 0上 1下 2左 3右

    private SpriteRenderer playerSr;        // 玩家渲染组件
    private int hitNum = 0;                 // 射线击中目标个数
    private RaycastHit2D[] hitResults;      // 射线击中目标


	// Use this for initialization
	void Start () {
        playerSr = this.GetComponent<SpriteRenderer>();         // 获取
        hitResults = new RaycastHit2D[4];
	}
	
	// Update is called once per frame
	void Update () {
        PlayerDirection dir = PlayerDirection.none;
        if (Input.GetKeyDown(KeyCode.W))                        // 上
        {
            playerSr.sprite = playerSprites[(int)PlayerDirection.up];
            dir = PlayerDirection.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))                   // 下
        {
            playerSr.sprite = playerSprites[(int)PlayerDirection.down];
            dir = PlayerDirection.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))                   // 左
        {
            playerSr.sprite = playerSprites[(int)PlayerDirection.left];
            dir = PlayerDirection.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))                   // 右
        {
            playerSr.sprite = playerSprites[(int)PlayerDirection.right];
            dir = PlayerDirection.right;
        }
        if (dir != PlayerDirection.none)
        {
            RayHit(dir);
            if (CanMove(dir))
            {
                //Debug.Log(hitNum);
                DoMove(dir);
                GameManage.Instance.ChangeStep();
            }
        }
	}

    // 射线检测
    private void RayHit(PlayerDirection dir)
    {
        hitNum = 0;
        ClearHitResults();
        Vector2 orign, direction;
        switch (dir)
        {
            case PlayerDirection.up:
                orign = new Vector2(transform.position.x, transform.position.y + 0.7f);
                direction = new Vector2(0, 1);
                hitNum = Physics2D.RaycastNonAlloc(orign, direction, hitResults, 1);
                break;
            case PlayerDirection.down:
                orign = new Vector2(transform.position.x, transform.position.y - 0.7f);
                direction = new Vector2(0, -1);
                hitNum = Physics2D.RaycastNonAlloc(orign, direction, hitResults, 1);
                break;
            case PlayerDirection.left:
                orign = new Vector2(transform.position.x-0.7f, transform.position.y);
                direction = new Vector2(-1, 0);
                hitNum = Physics2D.RaycastNonAlloc(orign, direction, hitResults, 1);
                break;
            case PlayerDirection.right:
                orign = new Vector2(transform.position.x + 0.7f, transform.position.y);
                direction = new Vector2(1, 0);
                hitNum = Physics2D.RaycastNonAlloc(orign, direction, hitResults, 1);
                break;
            default:
                break;
        }
    }

    // 判断是否可以移动
    private bool CanMove(PlayerDirection dir)
    {
        if (hitNum == 0)                                // 没有阻塞
        {
            return true;
        }
        else if (hitNum == 1)
        {
            string tag = hitResults[0].transform.gameObject.tag;        // 标签
            if (tag == Tags.wall)           // 遇到墙，不能移动
            {
                return false;
            }
            else if (tag == Tags.bean)      // 遇到豆子
            {
                return hitResults[0].transform.GetComponent<Bean>().CanMove(dir);
            }
        }
        else if (hitNum == 2)
        {
            for (int i = 0; i < 2; ++i)
            {
                if (hitResults[i].transform.gameObject.tag == Tags.bean)
                {
                    return hitResults[i].transform.GetComponent<Bean>().CanMove(dir);
                }
            }
        }
        return true;
    }

    // 玩家移动
    private void DoMove(PlayerDirection dir)
    {
        Vector3 moveDir;
        switch (dir)
        {
            case PlayerDirection.up:                // 上
                moveDir = new Vector3(0, 1.25f, 0);
                break;
            case PlayerDirection.down:              // 下
                moveDir = new Vector3(0, -1.25f, 0);
                break;
            case PlayerDirection.left:              // 左
                moveDir = new Vector3(-1.25f, 0, 0);
                break;
            case PlayerDirection.right:             // 右
                moveDir = new Vector3(1.25f, 0, 0);
                break;
            default:
                moveDir = new Vector3(0, 0, 0);
                break;
        }
        transform.Translate(moveDir);               // 移动
    }

    // 重置碰撞实体
    private void ClearHitResults()
    {
        hitResults = new RaycastHit2D[4];
    }
}
