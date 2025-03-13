using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float enlargedSize = 1.2f; // How much bigger the button gets
    public float animationSpeed = 0.2f; // Speed of the hover animation
    public AudioSource hoverSound; // Assign an AudioSource in the Inspector
    public RectTransform arrow; // Reference to the arrow RectTransform (assign this in the Inspector)

    private Vector3 originalScale;
    private Vector3 targetPosition; // Where the arrow should appear

    void Start()
    {
        originalScale = transform.localScale;

        // Hide the arrow initially
        if (arrow != null)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play hover sound if assigned
        if (hoverSound != null)
        {
            hoverSound.Play();
        }

        // Enlarge the button smoothly
        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", originalScale * enlargedSize,
            "time", animationSpeed,
            "easetype", iTween.EaseType.easeOutQuad
        ));

        // Show the arrow next to the button
        ShowArrowNextToButton(GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Return the button to its original size
        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", originalScale,
            "time", animationSpeed,
            "easetype", iTween.EaseType.easeOutQuad
        ));

        // Hide the arrow when no longer hovering
        HideArrow();
    }

    private void ShowArrowNextToButton(RectTransform buttonRect)
    {
        if (arrow == null) return;

        // Show the arrow only when it is needed
        if (!arrow.gameObject.activeSelf)
        {
            arrow.gameObject.SetActive(true);
        }

        // Set the arrow's position relative to the button (to the left of the button)
        targetPosition = buttonRect.position - new Vector3(buttonRect.rect.width / 2 + 10f, 0, 0); // Position to the left of the button

        // Directly set the arrow’s position (no tweening)
        arrow.position = targetPosition;
    }

    private void HideArrow()
    {
        if (arrow != null)
        {
            // Hide the arrow immediately when the hover ends
            arrow.gameObject.SetActive(false);
        }
    }
}
