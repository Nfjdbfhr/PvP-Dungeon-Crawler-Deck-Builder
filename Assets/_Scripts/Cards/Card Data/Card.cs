using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour
{

    public ScriptableCard CardObj;
    public CardBattleRunner CardBattleRunner;
    public CardDragManager Dragger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = CardObj.Sprites[0];
        CardBattleRunner = FindFirstObjectByType<CardBattleRunner>();
        Dragger = FindFirstObjectByType<CardDragManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHovering() && Dragger.CanDrag)
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    bool IsHovering()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Cast a ray from mouse position into the scene
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // If we hit something and it's this object, it's the topmost
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            return true;
        }

        return false;
    }

    public void PlayCard()
    {

        Dragger.CanDrag = false;

        gameObject.tag = "Untagged";
        CardBattleRunner.Deck.PlayerHand.CardsInHand.Remove(gameObject);
        Dragger.PlayerHand.ArrangeHand();
        CardBattleRunner.SpendEnergy(CardObj.EnergyCost);
        PlayCardActions();
    }

    void PlayCardActions()
    {
        PlayerAnimator playerAnims = GameObject.FindWithTag("PlayerSprite").GetComponent<PlayerAnimator>();

        for (int i = 0; i < CardObj.Functions.Count; i++)
        {
            switch (CardObj.Functions[i].Effect)
            {
                case FunctionType.Shielding:
                    CardBattleRunner.AddShielding(CardObj.Functions[i].Min, CardObj.Functions[i].Max);
                    playerAnims.Shield();
                    break;
                case FunctionType.Damaging:
                    playerAnims.Attack();
                    break;
                case FunctionType.Drawing:
                    CardBattleRunner.Deck.DrawCard(CardObj.Functions[i].Min, CardObj.Functions[i].Max);
                    break;
                case FunctionType.Energizing:
                    CardBattleRunner.AddEnergy(CardObj.Functions[i].Min, CardObj.Functions[i].Max);
                    break;
            }
        }

        StartCoroutine(MoveCardOut());
    }

    IEnumerator MoveCardOut()
    {
        yield return new WaitForSeconds(0.75f);

        transform.position = new Vector3(999, 999, 0);

        Dragger.CanDrag = true;
    }
}
