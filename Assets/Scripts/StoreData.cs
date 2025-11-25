using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreData", menuName = "Game/Store Data")]
public class StoreData : ScriptableObject
{
    public int coins;
    public List<string> purchasedCars;
    public string currentCar;

    public bool IsPurchased(string id) => purchasedCars.Contains(id);
    public bool IsUsing(string id) => currentCar == id;
}
