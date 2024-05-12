using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

//Type d'ennemie
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
    [SerializeField] private float _MaxSpawnableX = 100.0f;
    [SerializeField] private float _MinSpawnableX = -100.0f;
    [SerializeField] private float _MaxSpawnableY = 70.0f;
    [SerializeField] private float _MinSpawnableY = -70.0f;
    [Header("Ennemies")]
    [SerializeField] private GameObject _Slime;
    [SerializeField] private GameObject _Wolf;
    [SerializeField] private GameObject _Dragon;

    private Vector2 _PlayerPosition;
    private Node head;
    private int enemyCount = 0;
    private int maxEnemies = 10; // Nombre maximal d'ennemis autorisés
    private EnnemiesSpawn _List;
    private bool _CanSpawn = false;
    private GameObject _Character;


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

        if (enemyCount >= maxEnemies)
        {
            Debug.Log("Limite d'ennemis atteinte !");
            return;
        }

        EnemyType randomEnemyType = GetRandomEnemyType();
        GameObject enemyPrefab = GetEnemyPrefab(randomEnemyType); // Obtenez le prefab de l'ennemi en fonction du type
        if (enemyPrefab == null)
        {
            Debug.LogError("Prefab d'ennemi introuvable pour le type: " + randomEnemyType);
            return;
        }

        while (!_CanSpawn)
        {
            float randomX = Random.Range(_MinSpawnableX, _MaxSpawnableX);
            float randomY = Random.Range(_MinSpawnableY, _MaxSpawnableY);

            // Créer un Vector2 aléatoire à partir des valeurs générées
            Vector2 ennemieSpawnPoint = new Vector2(randomX, randomY);

            Debug.Log(enemyPrefab);

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

        enemyCount++; // Incrémenter le compteur d'ennemis
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
                enemyCount--; // Décrémenter le compteur d'ennemis
                return;
            }
            prevNode = currentNode;
            currentNode = currentNode.next;
        }

        Debug.LogError("Ennemi à supprimer introuvable dans la liste !");
    }

    //Vérification de possibilité de spawn
    public bool CanSpawnArea(Vector2 ennemieSpawnPoint) {
        _PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        //Limite gauche
        bool leftLimit = ennemieSpawnPoint.x + _PlayerPosition.x  < -_XSpawnLimitStart
            && ennemieSpawnPoint.x + _PlayerPosition.x >= -_XSpawnLimitEnd;
        //Limite droite
        bool rightLimit = ennemieSpawnPoint.x + _PlayerPosition.x > _XSpawnLimitStart
            && ennemieSpawnPoint.x + _PlayerPosition.x <= _XSpawnLimitEnd;
        //Limite haute
        bool upperLimit = ennemieSpawnPoint.y + _PlayerPosition.y < -_YSpawnLimitStart
            && ennemieSpawnPoint.y + _PlayerPosition.y >= -_YSpawnLimitEnd;
        //Limite basse
        bool underLimit = ennemieSpawnPoint.y + _PlayerPosition.y > _YSpawnLimitStart
            && ennemieSpawnPoint.y + _PlayerPosition.y <= _YSpawnLimitEnd;

        if (leftLimit || rightLimit || upperLimit || underLimit)
            return true;
        else
            return false;
    }

    //Debug fonction
    public void PrintList()
    {
        Node currentNode = head;

        while (currentNode != null)
        {
            Debug.Log("Enemy Type: " + currentNode.enemyType);
            currentNode = currentNode.next;
        }
    }

    void Start()
    {
        _List = new EnnemiesSpawn();
        InvokeRepeating("AddNode", 5.0f, 5.0f);
        Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}
