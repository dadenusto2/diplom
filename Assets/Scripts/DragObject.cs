using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
private Vector3 mOffset;
    private float mZCoord;
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
        Vector3 y = new Vector3(transform.position.x, -158f, transform.position.z);
        transform.position = y;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.Rotate(0, -90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            transform.Rotate(0, 90, 0);
        }
    }
    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = transform.position - GetMouseWorldPos();
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mosePoint = Input.mousePosition;
        mosePoint.z = mZCoord;
        var result = Camera.main.ScreenToWorldPoint(mosePoint);
        return result;
    }
}
