using UnityEngine;

namespace Viewport
{
    public class MouseHover : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject hoverObjectPrefab;

        public bool isHovering => hoverObject.activeSelf;
        public Vector2 worldHoveredPosition => Clusterbyte.ConvertTo2D(snappedPosition);

        private Vector3 snappedPosition;
        private GameObject hoverObject;
        private Vector3 velocity;

        internal void Awake()
        {
            hoverObject = Instantiate(hoverObjectPrefab, Vector3.zero, Quaternion.identity);
            hoverObject.SetActive(false);
        }

        internal void Update()
        {
            // Convert input of mouse to world coordinates against the Plane by raycasting its collider
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit))
            {
                hoverObject.SetActive(false);
                return;
            }
            // Snap to grid
            snappedPosition = new Vector3(
                Mathf.Clamp(Mathf.RoundToInt(hit.point.x), Clusterbyte.WORLD_LEFT, Clusterbyte.WORLD_LEFT + Clusterbyte.WORLD_WIDTH),
                0f, // y is always locked as it will be on the plane
                Mathf.Clamp(Mathf.RoundToInt(hit.point.z), Clusterbyte.WORLD_BOTTOM, Clusterbyte.WORLD_BOTTOM + Clusterbyte.WORLD_HEIGHT)
            );
            // Update hover object position
            hoverObject.SetActive(true);
            hoverObject.transform.position = Vector3.SmoothDamp(hoverObject.transform.position, snappedPosition, ref velocity, 0.1f);
        }
    }
}