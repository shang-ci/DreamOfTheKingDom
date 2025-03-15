using System.Collections.Generic;
using UnityEngine;

public class Status_UI : MonoBehaviour
{
    public Transform UIWindow;
    public Transform CardDec;
    public Transform Charaect;

    public GameObject cardInitPrefab;
    public GameObject cardSlotPrefab;
    public GameObject charaectPrefab;

    public List<CharacterBase> allCharacters;

    private void Awake()
    {
        allCharacters = GameManager.Instance.GetAllCharacters();

        foreach (var character in allCharacters)
        {
            GameObject newCharaect = Instantiate(charaectPrefab, Charaect);
            UI_Character charaect = newCharaect.GetComponent<UI_Character>();
            charaect.SetCharacter(character.deck, CardDec, UIWindow);
        }
    }
}
