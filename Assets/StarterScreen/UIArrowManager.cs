using UnityEngine;
using UnityEngine.EventSystems;

public class UIArrowManager : MonoBehaviour
{
    public static UIArrowManager Instance;

    private RectTransform arrow; // Arrow RectTransform
    private RectTransform targetButtonRect; // Current target button RectTransform
    private Vector3 targetPosition; // Where the arrow should move to
    private float moveSpeed = 0.2f; // Speed of the arrow movement
    private bool isMoving = false; // Is the arrow currently moving

    void Awake()
    {
        // Singleton pattern to access UIArrowManager from anywhere
        if (Instance == null)
        {
            Instance = this;
        }

        arrow = GameObject.Find("ArrowIndicator")?.GetComponent<RectTransform>();
        if (arrow != null)
        {
            arrow.gameObject.SetActive(false); // Hide it initially
        }
    }

    public void MoveArrowTo(RectTransform buttonRect)
    {
        if (arrow == null) return;

        if (!arrow.gameObject.activeSelf)
        {
            arrow.gameObject.SetActive(true); // Make the arrow visible when needed
        }

        targetButtonRect = buttonRect; // Set the target button's RectTransform
        targetPosition = targetButtonRect.position; // Get the button's position

        // Move arrow smoothly using iTween
        if (!isMoving)
        {
            isMoving = true;
            iTween.MoveTo(arrow.gameObject, iTween.Hash(
                "position", targetPosition,
                "time", moveSpeed,
                "easetype", iTween.EaseType.easeOutQuad,
                "oncomplete", "OnArrowMoveComplete",
                "oncompletetarget", gameObject
            ));
        }
    }

    private void OnArrowMoveComplete()
    {
        isMoving = false; // Reset the moving state after completion
    }

    public void HideArrow()
    {
        if (arrow != null && !isMoving)
        {
            arrow.gameObject.SetActive(false); // Hide the arrow when not moving
        }
    }
}
