using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyEditor : EditorWindow {
    //enemyList is a list of object Enemies
    public List<Enemies> enemyList = new List<Enemies>();

    string nameString = "";
    Sprite image;
    int health, attack, defence, agility;
    float attackTime;
    bool magicUser;
    int mana;
    bool nameFlag;
    bool spriteFlag;

    int currentChoice = 0;
    int lastChoice = 0;
    string[] enemyNames;
    //Creates a tab called "Custom Tools" and a tool called "Enemy Editor" with the Ctrl+G shortcut to open
    [MenuItem("Custom Tools/Enemy Editor %g")]
	private static void OpenWindow()
    {
        //this gets the window that will be opened when clicking on the tool
        EditorWindow.GetWindow<EnemyEditor>();
    }

    //OnGUI is a method that will draw things within the tool
    private void OnGUI()
    {
        GetEnemies();
        currentChoice = EditorGUILayout.Popup(currentChoice, enemyNames);
        //creates a button that calls GetEnemies() to refresh the list of enemies
        nameString = EditorGUILayout.TextField("Name: ", nameString);
        image = EditorGUILayout.ObjectField(image, typeof(Sprite), true) as Sprite;
        if(image != null)
        {
            Texture2D atext = SpriteUtility.GetSpriteTexture(image, false);
            GUILayout.Label(atext);
        }
        EditorGUILayout.PrefixLabel("Health");
        health = EditorGUILayout.IntSlider(health, 1, 300);
        EditorGUILayout.PrefixLabel("Attack");
        attack = EditorGUILayout.IntSlider(attack, 1, 100);
        EditorGUILayout.PrefixLabel("Defence");
        defence = EditorGUILayout.IntSlider(defence, 1, 100);
        EditorGUILayout.PrefixLabel("Agility");
        agility = EditorGUILayout.IntSlider(agility, 1, 100);
        EditorGUILayout.PrefixLabel("Attack Time");
        attackTime = EditorGUILayout.Slider(attackTime, 1, 20);
        EditorGUILayout.PrefixLabel("Magic User");
        magicUser = EditorGUILayout.Toggle(magicUser);

        if (magicUser)
        {
            EditorGUILayout.PrefixLabel("Mana");
            mana = EditorGUILayout.IntSlider(mana, 0, 100);
        }
        //checks if the nameFlag is true or false and give corresponding message
        if (nameFlag || spriteFlag)
        {
            EditorGUILayout.HelpBox("Name or Sprite is invalid", MessageType.Error);
        }
        if(currentChoice == 0)
        {
            if (GUILayout.Button("Create"))
            {
                CreateEnemy();
            }
        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                AlterEnemy();
            }
        }
        if (currentChoice != lastChoice)
            {
            if (currentChoice == 0)
            {
                nameString = "";
                health = 1;
                defence = 1;
                agility = 1;
                attack = 1;
                attackTime = 1;
                magicUser = false;
                mana = 0;
                image = null;
            }
            else
            {
                nameString = enemyList[currentChoice - 1].emname;
                image = enemyList[currentChoice - 1].mySprite;
                health = enemyList[currentChoice - 1].health;
                attack = enemyList[currentChoice - 1].atk;
                defence = enemyList[currentChoice - 1].def;
                agility = enemyList[currentChoice - 1].agi;
                attackTime = enemyList[currentChoice - 1].atkTime;
                magicUser = enemyList[currentChoice - 1].isMagic;
                mana = enemyList[currentChoice - 1].manaPool;
            }
        }
        lastChoice = currentChoice;

    }

    //Awake() is called when the window if first opened and calls the GetEnemies() method
    void Awake()
    {
        GetEnemies();
    }

    //GetEnemies() gets an array of strings holding the guid ID of each asset that is derived of the class type "Enemies"
    //And then converts those into asset paths, loads those assets and then adds them to the enemyList
    private void GetEnemies()
    {
        List<string> enemyNameList = new List<string>();
        foreach (Enemies e in enemyList)
        {
            enemyNameList.Add(e.emname);
        }
        enemyNameList.Insert(0, "New");
        enemyNames = enemyNameList.ToArray();
        enemyList.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Enemies");
        foreach (string guid in guids)
        {
            string myString = AssetDatabase.GUIDToAssetPath(guid);
            Enemies enemyInst = AssetDatabase.LoadAssetAtPath(myString, typeof(Enemies)) as Enemies;
            enemyList.Add(enemyInst);
        }
    }

    private void CreateEnemy()
    {
        //checks if there is no name in the name field and sets a boolean
        if(nameString == "")
        {
            nameFlag = true;
        }
        else if (image == null)
        {
            spriteFlag = true;
        }
        else
        {
            nameFlag = false;
            spriteFlag = false;
            Enemies myEnemy = ScriptableObject.CreateInstance<Enemies>();
            myEnemy.emname = nameString;
            myEnemy.health = this.health;
            myEnemy.atk = attack;
            myEnemy.def = defence;
            myEnemy.agi = agility;
            myEnemy.atkTime = attackTime;
            myEnemy.isMagic = magicUser;
            myEnemy.manaPool = mana;
            myEnemy.mySprite = image;
            AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/" + myEnemy.emname.Replace(" ","_")+".asset");
            GetEnemies();
            currentChoice = 0;
        }
    }

    private void AlterEnemy()
    {
        nameFlag = false;
        spriteFlag = false;
        enemyList[currentChoice - 1].emname = nameString;
        enemyList[currentChoice - 1].health = health;
        enemyList[currentChoice - 1].def = defence;
        enemyList[currentChoice - 1].atk = attack;
        enemyList[currentChoice - 1].agi = agility;
        enemyList[currentChoice - 1].atkTime = attackTime;
        enemyList[currentChoice - 1].isMagic = magicUser;
        enemyList[currentChoice - 1].manaPool = mana;
        enemyList[currentChoice - 1].mySprite = image;
        AssetDatabase.SaveAssets();
        currentChoice = 0;
        GetEnemies();
    }
}
