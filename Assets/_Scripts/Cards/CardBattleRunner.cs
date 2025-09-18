using UnityEngine;

public class CardBattleRunner : MonoBehaviour
{

    public Deck Deck;
    public int StartingHandSize;
    public PlayerStats PlayerStats;
    public CardUiManager CardUi;
    public CardDragManager CardDragger;

    public int TurnNumber;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CardDragger.CanDrag = false;
        Deck.Shuffle();
        Deck.DrawCard(StartingHandSize, StartingHandSize);
        StartTurn();
    }

    public void StartTurn()
    {
        TurnNumber += 1;
        PlayerStats.Energy = TurnNumber;
        CardUi.UpdateEnergy(PlayerStats.Energy);
        if (TurnNumber != 1)
            Deck.DrawCard(1, 1);
    }

    public void AddShielding(int min, int max)
    {
        int amountToAdd = Random.Range(min, max + 1);
        PlayerStats.Shielding += amountToAdd;
        CardUi.UpdateShield(PlayerStats.Shielding);
    }

    public void AddEnergy(int min, int max)
    {
        PlayerStats.Energy += Random.Range(min, max + 1);
        CardUi.UpdateEnergy(PlayerStats.Energy);
    }

    public void SpendEnergy(int amount)
    {
        PlayerStats.Energy -= amount;
        PlayerStats.Energy = Mathf.Max(PlayerStats.Energy, 0);
        CardUi.UpdateEnergy(PlayerStats.Energy);
    }
}
