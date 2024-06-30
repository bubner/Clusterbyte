using UnityEngine;

namespace Viewport
{
    /// <summary>
    /// Handles the box hover effect of the mouse on the grid.
    /// </summary>
    public class MouseHover : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject hoverObjectPrefab;

        public bool isHovering => hoverObject.activeSelf && shouldHover;

        public Vector3 hoveredPosition { get; private set; }
        private GameObject hoverObject;
        private MeshRenderer meshRenderer;
        private Vector3 velocity;
        private GameManager mgr;
        private bool shouldHover;

        internal void Awake()
        {
            hoverObject = Instantiate(hoverObjectPrefab, Vector3.zero, Quaternion.identity);
            hoverObject.SetActive(false);
        }

        internal void Start()
        {
            mgr = GameManager.instance;
            hoverObject.TryGetComponent(out meshRenderer);
        }

        internal void OnDisable()
        {
            if (hoverObject != null)
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
            hoverObject.SetActive(true);
            // Snap to grid
            hoveredPosition = new Vector3(
                Mathf.Clamp(Mathf.RoundToInt(hit.point.x), 0, Clusterbyte.GRID_WIDTH),
                0f, // y is always locked as it will be on the plane
                Mathf.Clamp(Mathf.RoundToInt(hit.point.z), 0, Clusterbyte.GRID_HEIGHT)
            );
            // Update hover object position
            hoverObject.transform.position = Vector3.SmoothDamp(hoverObject.transform.position, hoveredPosition + new Vector3(0, 0.5f, 0), ref velocity, 0.1f);
            shouldHover = true;
            if (mgr.IsOccupied(hoveredPosition))
            {
                meshRenderer.material.color = new Color(1f, 1f, 0, 0.4f);
                return;
            }
            if (mgr.IsPlaceable(hoveredPosition))
            {
                meshRenderer.material.color = new Color(0, 1f, 0, 0.4f);
                return;
            }

            shouldHover = false;
            meshRenderer.material.color = new Color(1f, 0, 0, 0.2f);
        }
    }
}