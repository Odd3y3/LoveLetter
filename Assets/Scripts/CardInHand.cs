using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "손에 있는" 카드에 달아주는 컴포넌트 클래스. 마우스를 올렸을 때, 카드가 커지는 효과를 넣기 위해 만든 클래스.
/// </summary>

public class CardInHand : MonoBehaviour
{
    protected int cardNum;

    private Vector3 handCardPos = new Vector3(0f, -3.5f, 0f);
    private Vector3 handCardSizeUpPos = new Vector3(0f, -2.0f, 0f);


    private void MouseOnCard()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.position = handCardSizeUpPos;
    }

    private void MouseOffCard()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        transform.position = handCardPos;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MouseCursor>(out MouseCursor mouseCursor))
        {
            MouseOnCard();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MouseCursor>(out MouseCursor mouseCursor))
        {
            MouseOffCard();
        }
    }

    //private void OnMouseEnter()
    //{
    //    MouseOnCard();
    //}

    //private void OnMouseExit()
    //{
    //    MouseOffCard();
    //}
}
