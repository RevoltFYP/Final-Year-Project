using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour {

    public Texture2D cursorTexture;
    public Texture2D enemyCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public LayerMask layerMask;

    private Ray ray;
    private float rayLength = 100f;

    void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            //Debug.Log(hit.collider);
            if (hit.collider.tag.Contains("Enemy"))
            {
                //Debug.Log("true");
                Cursor.SetCursor(enemyCursor, hotSpot, cursorMode);
            }
            else
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            }
        }

        //Debug.DrawRay(ray.origin, ray.direction, Color.blue);
    }
}
