using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour {
    private int hitNum = 0;                 // 射线击中目标个数
    private RaycastHit2D[] hitResults;      // 射线击中目标

    public bool isAlign;

    // Use this for initialization
    void Start()
    {
        hitResults = new RaycastHit2D[4];
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
                orign = new Vector2(transform.position.x - 0.7f, transform.position.y);
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
    public bool CanMove(PlayerDirection dir)
    {
        RayHit(dir);
        //Debug.Log(hitNum);
        bool canMove = false;
        if (hitNum == 0)                                // 没有阻塞
        {
            canMove = true;
        }
        else if (hitNum == 1)
        {
            if (hitResults[0].transform.gameObject.tag == Tags.empty)
            {
                canMove = true;
            }
        }
        else
        {
            canMove = false;
        }
        if (canMove)
        {
            DoMove(dir);
        }
        return canMove;
    }

    // 豆子移动
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
        GameManage.Instance.GetBeanPos();
    }

    // 重置碰撞实体
    private void ClearHitResults()
    {
        hitResults = new RaycastHit2D[4];
    }
}
