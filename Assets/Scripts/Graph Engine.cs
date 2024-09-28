using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Graph
{
    public class GraphEngine : MonoBehaviour
    {
        [SerializeField]
        Transform pointPrefab;

        [SerializeField, Range(10, 1000)]
        int resolution = 10;

        private LineRenderer lineRenderer;

        private Vector4 previousBounds;

        public CameraController cameraData;

        // Start is called before the first frame update
        void Awake()
        {
            InitializeLineRenderer();
            previousBounds = cameraData.GetCameraBounds();
            DrawGrid(previousBounds);
            DrawAxes(previousBounds);
            DrawPlot(previousBounds);
        }

        // Update is called once per frame
        void Update()
        {
            Vector4 currentBounds = cameraData.GetCameraBounds();
            if (currentBounds != previousBounds) {
                ClearPlot();
                DrawGrid(currentBounds);
                DrawAxes(currentBounds);
                DrawPlot(currentBounds);
                previousBounds = currentBounds;
            }

            UpdateLineThickness();
        }

        void DrawAxes(Vector4 bounds)
        {
            float minX = bounds.x, maxX = bounds.y, minY = bounds.z, maxY = bounds.w;
            /*
            if (maxY > 0f && minY < 0f)
            {
                axisRenderer.SetPosition(0, new Vector3(minX - 1f, 0, 0));
                axisRenderer.SetPosition(1, new Vector3(maxX + 1f, 0, 0));
            }

            if (maxX > 0f && minX < 0f)
            {

                axisRenderer.positionCount = 4;
                axisRenderer.SetPosition(2, new Vector3(0, minY - 1f, 0));
                axisRenderer.SetPosition(3, new Vector3(0, maxY + 1f, 0));
            }
            */
        }

        void DrawGrid(Vector4 bounds)
        {
            float minX = bounds.x, maxX = bounds.y, minY = bounds.z, maxY = bounds.w;

        }

        void DrawPlot(Vector4 bounds)
        {
            float offset = 100f;
            float minX = bounds.x, maxX = bounds.y, minY = bounds.z - offset, maxY = bounds.w + offset;
            float domain = maxX - minX;
            float step = domain / resolution;

            List<Vector3> positions = new List<Vector3>();
            //for each point drawn from left domain to right domain
            for (int i = -1; i <= resolution; i++)
            {
                float x = minX + i * step;
                float y = GetFunction(x);

                if (y < maxY && y > minY)
                {
                    //draw the line
                    positions.Add(new Vector3(x, y, 0f));
                }
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }

        void ClearPlot()
        {
            lineRenderer.positionCount = 0; // Destroy the old line
        }

        float GetFunction(float x)
        {
            float y = Mathf.Tan(x);
            return y;
        }

        void InitializeLineRenderer()
        {
            //create the lines between points
            lineRenderer = gameObject.AddComponent<LineRenderer>();

            //decide on line thickness
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;


            //set the material of the line and make it red
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;

            //set the number of points in the Line Renderer to match the array length of points
            lineRenderer.positionCount = resolution;
            lineRenderer.useWorldSpace = true;
        }

        void UpdateLineThickness()
        {
            float baseLineThickness = 1f;
            float orthographicSize = cameraData.GetComponent<Camera>().orthographicSize;
            float zoomCompensation = Mathf.Max(orthographicSize, 0.1f);
            lineRenderer.widthMultiplier = baseLineThickness / zoomCompensation;
        }
    }
}