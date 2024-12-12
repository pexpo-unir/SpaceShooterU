using UnityEngine;

namespace UI.MainMenu
{
    public class RotateToMousePosition : MonoBehaviour
    {
        [field: SerializeField]
        public float RotationSpeed { get; set; } = 2;

        private RectTransform rectTransform;
        private Canvas canvas;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localMousePosition
            );

            var direction = localMousePosition - rectTransform.anchoredPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90; // TODO: (0, 1) = 0 deg (not 90 deg)
            var rotation = Quaternion.Euler(0, 0, angle);
            rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, rotation, RotationSpeed * Time.deltaTime);
        }
    }
}
