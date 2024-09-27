using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        public float zoomSpeed = 2f;

        public float minZoom = 0f;
        public float maxZoom = Mathf.Infinity;

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

        public Vector4 GetCameraBounds()
        {
            Vector3 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
            Vector3 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

            Vector3 center = (bottomLeft + topRight) / 2f;
            Vector3 size = topRight - bottomLeft;

            Bounds bounds = new Bounds(center, size);

            return new Vector4(bounds.min.x, bounds.max.x, bounds.min.y, bounds.max.y);
        }

        public static Vector2 GetDomain()
        {
            Vector2 domain = Vector2.zero;
            return domain;
        }
    }
}
