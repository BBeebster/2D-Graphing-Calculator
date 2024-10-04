using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        public Camera _camera;

        [SerializeField]
        public float zoomSpeed = 2f;

        public float minZoom = 0.1f;
        public float maxZoom = 10f;

        private Vector3 dragOrigin;

        void Update()
        {
            PanCamera();
            ZoomCamera();
        }

        void PanCamera()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 difference = dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);
                _camera.transform.position += difference;
            }
        }

        void ZoomCamera()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float size = _camera.orthographicSize - scroll * zoomSpeed;
            _camera.orthographicSize = Mathf.Clamp(size, minZoom, maxZoom);
        }

        /*
        public Vector4 GetCameraBounds()
        {
            Vector3 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector3 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

            Vector3 center = (bottomLeft + topRight) / 2f;
            Vector3 size = topRight - bottomLeft;

            Bounds bounds = new Bounds(center, size);

            return new Vector4(bounds.min.x, bounds.max.x, bounds.min.y, bounds.max.y);
        }
        */
        public Vector4 GetCameraBounds()
        {
            float aspect = _camera.aspect; // Get the camera aspect ratio
            float height = _camera.orthographicSize * 2; // Full height of the camera view
            float width = height * aspect; // Calculate width based on aspect ratio

            Vector3 bottomLeft = _camera.transform.position - new Vector3(width / 2, height / 2, 0);
            Vector3 topRight = _camera.transform.position + new Vector3(width / 2, height / 2, 0);

            return new Vector4(bottomLeft.x, topRight.x, bottomLeft.y, topRight.y);
        }
    }
}
