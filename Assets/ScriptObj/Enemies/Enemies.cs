using UnityEngine;
using System.Collections;

public class Enemies : ScriptableObject {


	public string emname;
	public int health, atk, def, agi;
    public float atkTime;
    public bool isMagic;
    public int manaPool;
	public Sprite mySprite;
}
