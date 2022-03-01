using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyItem : MonoBehaviour
{
    public bool IsMoveX = true;
    public float MoveDistance = 1;
    public float MoveTime = 2;
    BoxCollider2D Collider;
    Vector3 localPos;

    public bool IsInVacuum = false;

    public bool IsDead;
    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
        localPos = transform.localPosition;
    }
    void Start()
    {
        if (IsMoveX)
        {
            transform.DOLocalMoveX(localPos.x + MoveDistance, MoveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).Play();
        }
        else
        {
            transform.DOLocalMoveY(localPos.y + MoveDistance, MoveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).Play();
        }
    }
    public void SetVacuum(bool enable)
    {
        IsInVacuum = enable;
        if (IsInVacuum == false)
        {
            Collider.enabled = false;
        }
    }

    public void MoveToPlayer(Transform player)
    {
        transform.DOLocalMove(player.localPosition, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }).Play();
    }
}
