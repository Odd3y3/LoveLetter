using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "�տ� �ִ�" ī�忡 �޾��ִ� ������Ʈ Ŭ����. ���콺�� �÷��� ��, ī�尡 Ŀ���� ȿ���� �ֱ� ���� ���� Ŭ����.
/// </summary>

public class CardInHand : MonoBehaviour
{
    protected int cardNum;

    private Vector3 handCardPos = new Vector3(0f, -3.5f, 0f);
    private Vector3 handCardSizeUpPos = new Vector3(0f, -2.0f, 0f);


    public void MouseOnCard()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.position = handCardSizeUpPos;
    }

    public void MouseOffCard()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        transform.position = handCardPos;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        MouseCursor mouseCursor;
        if(collision.TryGetComponent<MouseCursor>(out mouseCursor))
        {
            MouseOnCard();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MouseCursor mouseCursor;
        if (collision.TryGetComponent<MouseCursor>(out mouseCursor))
        {
            MouseOffCard();
        }
    }
}