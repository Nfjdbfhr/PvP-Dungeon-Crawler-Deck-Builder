using UnityEngine;

public class CardDragManager : MonoBehaviour
{
    [Header("Settings")]
    public string cardTag = "Card";           // Tag for draggable objects
    public float activationDistance = 2f;     // Distance threshold to "activate" card
    public GameObject outline;                // Reference to your outline sprite
    public PlayerStats PlayerStats;
    public bool CanDrag;
    public Hand PlayerHand;

    private Camera cam;
    private GameObject selectedCard;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool dragging = false;
    private bool activated = false;

    void Start()
    {
        cam = Camera.main;
        if (outline != null)
            outline.SetActive(false); // Hide outline initially
    }

    void Update()
    {
        if (!CanDrag)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit.CompareTag(cardTag))
            {
                selectedCard = hit.gameObject;
                selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 999;
                startPos = selectedCard.transform.position;
                startRot = selectedCard.transform.rotation;
                selectedCard.transform.rotation = Quaternion.Euler(0, 0, 0);
                dragging = true;
                activated = false;

                if (outline != null)
                    outline.SetActive(false);
            }
        }

        if (dragging && selectedCard != null)
        {
            // Follow mouse
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            selectedCard.transform.position = mousePos;

            // Check distance
            float dist = Vector2.Distance(startPos, selectedCard.transform.position);
            if (dist >= activationDistance && !activated)
            {
                activated = true;
                if (outline != null && selectedCard.GetComponent<Card>().CardObj.EnergyCost <= PlayerStats.Energy)
                {
                    outline.SetActive(true);
                    outline.transform.parent = selectedCard.transform;
                    outline.transform.localPosition = Vector3.zero;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    outline.transform.localScale = new Vector3(0.24f, 0.32f, 1);
                }
            }
            else if (dist < activationDistance && activated)
            {
                activated = false;
                if (outline != null)
                    outline.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            if (activated && selectedCard.GetComponent<Card>().CardObj.EnergyCost <= PlayerStats.Energy)
            {
                // Snap card to center
                Vector3 center = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                center.z = 0;
                selectedCard.transform.position = center;
                selectedCard.GetComponent<Card>().PlayCard();

                if (outline != null)
                    outline.SetActive(false);
            }
            else
            {
                // Snap back to start
                selectedCard.transform.position = startPos;
                selectedCard.transform.rotation = startRot;
                selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 0;
                PlayerHand.ArrangeHand();
            }

            dragging = false;
            selectedCard = null;
        }
    }
}
