using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class GraphEngine : MonoBehaviour
    {
        [SerializeField]
        Transform pointPrefab;

        [SerializeField, Range(10, 200)]
        int resolution = 10;

        public float domainLeft = 100f;
        public float domainRight = 100f;
        public float rangeBottom = 100f;
        public float rangeTop = 100f;

        Transform[] points;

        private Vector4 previousBounds;

        public CameraController cameraData;

        // Start is called before the first frame update
        void Awake()
        {
            previousBounds = cameraData.GetCameraBounds();
            DrawPlot(previousBounds);
        }

        // Update is called once per frame
        void Update()
        {
            Vector4 currentBounds = cameraData.GetCameraBounds();
            if (currentBounds != previousBounds) {
                ClearPlot();
                DrawPlot(currentBounds);
            }
        }

        void DrawPlot(Vector4 bounds)
        {
            float minX = bounds.x, maxX = bounds.y, minY = bounds.z, maxY = bounds.w;
            float domain = maxX - minX;
            float step = domain / resolution;
            Vector2 scale = Vector2.one * step;
            Vector2 position = Vector2.zero;
            //find amount of points to draw
            //right domain - left domain * resolution
            points = new Transform[Mathf.CeilToInt(domain) * resolution];
            //for each point drawn from left domain to right domain
            for (int i = 0; i < resolution * Mathf.CeilToInt(domain); i++)
            {  
                position.x = (i + 0.5f) * step - Mathf.Abs(minX);
                position.y = GetFunction(position.x);
                
                //check if the point is visible in the domain
                if (position.x < maxX && position.x > minX) {
                    //check if the point is visible in the range
                    if (position.y < maxY && position.y > minY)
                    {
                        //draw the point
                        Transform point = points[i] = Instantiate(pointPrefab);
                        point.localPosition = position;
                        point.localScale = scale;
                    }
                }
            }     
        }

        void ClearPlot()
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != null)
                {
                    Destroy(points[i].gameObject); // Destroy the object if it exists
                    points[i] = null;   // Set the array element to null
                }
            }
        }

        float GetFunction(float x)
        {
            float y = x * x;
            return y;
        }
    }
}