using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GetBounds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector4 GetBounds()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Vector3 position = rectTransform.position;

            Vector2 size = rectTransform.rect.size;

            float left = position.x - (size.x * rectTransform.pivot.x);
            float right = position.x + (size.x * (1 - rectTransform.pivot.x));
            float top = position.y + (size.y * (1 - rectTransform.pivot.y));
            float bottom = position.y - (size.y * rectTransform.pivot.y); ;

            Debug.Log($"Bounds: {left}, {right}, {top}, {bottom}");

            return new Vector4(left, right, top, bottom);
        }

        return Vector4.zero;
    }

    private Vector4 DomainAndRange(Vector4 bounds)
    {
        return new Vector4(-10f, 10f, 6.2f, -6.2f);
    }
}
