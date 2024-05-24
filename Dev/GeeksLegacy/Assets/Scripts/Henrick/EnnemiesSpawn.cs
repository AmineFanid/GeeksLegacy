// EnnemiesSpawner
// Classes: EnnemiesSpawn, Node
// Permet de généré les ennemies
// Auteurs: Henrick Baril
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

//Type d'ennemis
public enum EnemyType
{
    Slime,
    Wolf,
    Dragon
}

//Class pour la liste chainée
public class Node
{
    public EnemyType enemyType;
    public GameObject enemyObject;
    public Node next;

    public Node(EnemyType type, GameObject obj)
    {
        enemyType = type;
        enemyObject = obj; // Stocker la référence à l'ennemi
        next = null;
    }
}

//Script pour généré les ennemis
public class EnnemiesSpawn : MonoBehaviour
{
    [Header("Limitation")]
    [SerializeField] private float _XSpawnLimitStart = 12.0f;
    [SerializeField] private float _XSpawnLimitEnd = 21.0f;
    [SerializeField] private float _YSpawnLimitStart = 8.0f;
    [SerializeField] private float _YSpawnLimitEnd = 13.0f;
    [SerializeField] private float _MaxSpawnableX = 21.0f;
    [SerializeField] private float _MinSpawnableX = -21.0f;
    [SerializeField] private float _MaxSpawnableY = 13.0f;
    [SerializeField] private float _MinSpawnableY = -13.0f;
    [Header("Ennemies")]
    [SerializeField] private GameObject _Slime;
    [SerializeField] private GameObject _Wolf;
    [SerializeField] private GameObject _Dragon;

    private Vector2 _PlayerPosition;
    private Node head;
    private int _EnemyCount = 0;
    [SerializeField]
    private int _MaxEnemies = 1; // Nombre maximal d'ennemis autorisés
    private EnnemiesSpawn _List;
    private bool _CanSpawn = false;
    private GameObject _Character;
    private ProceduralGeneration _Generation;


    //Donne un type d'ennemi aléatoire selon un % de chance donné
    private EnemyType GetRandomEnemyType()
    {
        // Définir les pourcentages d'apparition pour chaque type d'ennemi (devrait etre séréalisé)
        float SlimeChance = 1.0f;
        float WolfChance = 0.0f;
        float DragonChance = 0.0f;

        // Générer un nombre aléatoire entre 0 et 1
        float randomValue = Random.value;

        // Comparer le nombre aléatoire avec les pourcentages d'apparition pour déterminer le type d'ennemi
        if (randomValue < SlimeChance)
            return EnemyType.Slime;
        else if (randomValue < SlimeChance + WolfChance)
            return EnemyType.Wolf;
        else
            return EnemyType.Dragon;
    }

    //Ajoute un noeud à la liste chainé, donc génère un ennemi
    public void AddNode()
    {

        GameObject enemyInstance = null;
        Node newNode = null;
        _CanSpawn = false;

        if (_EnemyCount >= _MaxEnemies)
        {
            //Limite d'ennemis atteinte
            return;
        }

        EnemyType randomEnemyType = GetRandomEnemyType();
        GameObject enemyPrefab = GetEnemyPrefab(randomEnemyType); // Obtenez le prefab de l'ennemi en fonction du type
        if (enemyPrefab == null)
        {
            //Prefab d'ennemi introuvable pour le type randomEnemyType
            return;
        }

        while (!_CanSpawn)
        {
            _PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position; //OnEnnemySpawn
            float randomX = Random.Range(_MinSpawnableX + _PlayerPosition.x, _MaxSpawnableX + _PlayerPosition.x);
            float randomY = Random.Range(_MinSpawnableY + _PlayerPosition.y, _MaxSpawnableY + _PlayerPosition.y);

            // Créer un Vector2 aléatoire à partir des valeurs générées
            Vector2 ennemieSpawnPoint = new Vector2(randomX, randomY);


            if (CanSpawnArea(ennemieSpawnPoint))
            {
                _CanSpawn = true;
                enemyInstance = Instantiate(enemyPrefab, ennemieSpawnPoint, Quaternion.identity); // Instancier l'ennemi
            }
        }

        if (enemyInstance != null)
        {
            newNode = new Node(randomEnemyType, enemyInstance); //Attribution de l'ennemie pour le node
        }

        if (head == null) //Attribution du dernier node
        {
            if (newNode != null)
                head = newNode;
        }
        else
        {
            Node lastNode = head;
            while (lastNode.next != null)
            {
                lastNode = lastNode.next;
            }
            lastNode.next = newNode;
        }

        _EnemyCount++; // Incrémenter le compteur d'ennemis
    }

    //Détermine quel prefab d'ennemi doit etre retourner
    public GameObject GetEnemyPrefab(EnemyType ennemie)
    {
        switch (ennemie) {
            default: 
                return null;
            case EnemyType.Slime:
                return _Slime;
            case EnemyType.Wolf:
                return _Wolf;
            case EnemyType.Dragon:
                return _Dragon;
        }
    }

    //Permet de retirer un node donné
    public void RemoveNode(GameObject enemyInstanceToRemove)
    {
        Node currentNode = head;
        Node prevNode = null;

        // Parcourir la liste pour trouver le noeud correspondant à l'ennemi à supprimer
        while (currentNode != null)
        {
            if (currentNode.enemyObject == enemyInstanceToRemove)
            {
                // Supprimer le noeud de la liste
                if (prevNode == null)
                {
                    head = currentNode.next;
                }
                else
                {
                    prevNode.next = currentNode.next;
                }
                Destroy(enemyInstanceToRemove); // Détruire l'ennemi
                _EnemyCount--; // Décrémenter le compteur d'ennemis
                return;
            }
            prevNode = currentNode;
            currentNode = currentNode.next;
        }
    }

    //Vérification de possibilité de spawn
    public bool CanSpawnArea(Vector2 ennemieSpawnPoint) {
        _PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;        
        //Limite gauche
        bool leftLimit = ennemieSpawnPoint.x < -_XSpawnLimitStart - _PlayerPosition.x
            && ennemieSpawnPoint.x >= -_XSpawnLimitEnd - _PlayerPosition.x;
        //Limite droite
        bool rightLimit = ennemieSpawnPoint.x > _XSpawnLimitStart + _PlayerPosition.x
            && ennemieSpawnPoint.x <= _XSpawnLimitEnd + _PlayerPosition.x;
        //Limite haute
        bool upperLimit = ennemieSpawnPoint.y < -_YSpawnLimitStart - _PlayerPosition.y
            && ennemieSpawnPoint.y >= -_YSpawnLimitEnd - _PlayerPosition.y;
        //Limite basse
        bool underLimit = ennemieSpawnPoint.y > _YSpawnLimitStart + _PlayerPosition.y
            && ennemieSpawnPoint.y <= _YSpawnLimitEnd + _PlayerPosition.y;

        if (leftLimit || rightLimit || upperLimit || underLimit)
            return true;
        else
            return false;
    }
    public void PrintList()
    {
        Node currentNode = head;

        while (currentNode != null)
        {
            currentNode = currentNode.next;
        }
    }

    void Start()
    {
        _List = new EnnemiesSpawn();
        InvokeRepeating("AddNode", 5.0f, 5.0f);
    }
}
