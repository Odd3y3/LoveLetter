using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private RaycastHit2D hit;

    private void Update()
    {
        TrackMouse();

    }
    private void TrackMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    //private void MouseClick()
    //{
    //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
    //    if (hit)
    //    {
    //        GameObject obj = hit.collider.gameObject;
    //        Card card;
    //        if (obj.TryGetComponent<Card>(out card))
    //        {
    //            card.MouseOnCard();
    //        }
    //    }
    //}

}
