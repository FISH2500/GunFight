using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="Card/CardDataManager")]
public class CardDataManager : ScriptableObject
{
    public List<CardData> Carddata;
}
