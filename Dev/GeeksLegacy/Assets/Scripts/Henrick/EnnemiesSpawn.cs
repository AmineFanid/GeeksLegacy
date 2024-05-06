using NUnit.Framework.Constraints;
using UnityEngine;

public enum EnemyType
{
    Slime,
    Wolf,
    Dragon
}

public class Node
{
    public EnemyType enemyType;
    public Node next;

    public Node(EnemyType type)
    {
        enemyType = type;
        next = null;
    }
}

public class EnnemiesSpawn : MonoBehaviour
{
    [SerializeField] private ControlCharacters _Character;
    [SerializeField] private float _XSpawnLimitStart = 12.0f;
    [SerializeField] private float _XSpawnLimitEnd = 21.0f;
    [SerializeField] private float _YSpawnLimitStart = 8.0f;
    [SerializeField] private float _YSpawnLimitEnd = 13.0f;
    [SerializeField] private float _MaxSpawnableX = 100.0f;
    [SerializeField] private float _MinSpawnableX = -100.0f;
    [SerializeField] private float _MaxSpawnableY = 70.0f;
    [SerializeField] private float _MinSpawnableY = -70.0f;

    private Vector2 _PlayerPosition;
    private Node head;
    private int enemyCount = 0;
    private int maxEnemies = 10; // Nombre maximal d'ennemis autorisés
    private EnnemiesSpawn _List;
    private bool _CanSpawn = false;

    private EnemyType GetRandomEnemyType()
    {
        // Définir les pourcentages d'apparition pour chaque type d'ennemi
        float SlimeChance = 1.0f;
        float WolfChance = 0.0f;
        float DragonChance = 0.0f;

        // Générer un nombre aléatoire entre 0 et 1
        float randomValue = Random.value;

        // Comparer le nombre aléatoire avec les pourcentages d'apparition pour déterminer le type d'ennemi
        if (randomValue < SlimeChance)
        {
            return EnemyType.Slime;
        }
        else if (randomValue < SlimeChance + WolfChance)
        {
            return EnemyType.Wolf;
        }
        else
        {
            return EnemyType.Dragon;
        }
    }

    public void AddNode()
    {
        if (enemyCount >= maxEnemies)
        {
            Debug.Log("Limite d'ennemis atteinte !");
            return;
        }

        EnemyType randomEnemyType = GetRandomEnemyType();
        Node newNode = new Node(randomEnemyType);

        float randomX = Random.Range(_MinSpawnableX, _MaxSpawnableX); // Remplacez les valeurs minimale et maximale par celles de votre choix
        float randomY = Random.Range(_MinSpawnableY, _MaxSpawnableY); // Remplacez les valeurs minimale et maximale par celles de votre choix

        // Créer un Vector2 aléatoire à partir des valeurs générées
        Vector2 ennemieSpawnPoint = new Vector2(randomX, randomY);

        while (!_CanSpawn)
        {
            if (CanSpawnArea(ennemieSpawnPoint))
            {
                _CanSpawn = true;
                //Instantiate();
            }

        } 

        if (head == null)
        {
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

    public void RemoveNode() { }

    public bool CanSpawnArea(Vector2 ennemieSpawnPoint) {
        _PlayerPosition = _Character.gameObject.transform.position;

        //Left limit for ennemies spawning
        bool leftLimit = ennemieSpawnPoint.x + _PlayerPosition.x  < -_XSpawnLimitStart
            && ennemieSpawnPoint.x + _PlayerPosition.x >= -_XSpawnLimitEnd;
        //right limit for ennemies spawning
        bool rightLimit = ennemieSpawnPoint.x + _PlayerPosition.x > _XSpawnLimitStart
            && ennemieSpawnPoint.x + _PlayerPosition.x <= _XSpawnLimitEnd;
        //upper limit for ennemies spawning 
        bool upperLimit = ennemieSpawnPoint.y + _PlayerPosition.y < -_YSpawnLimitStart
            && ennemieSpawnPoint.y + _PlayerPosition.y >= -_YSpawnLimitEnd;
        //under limit for ennemies spawning
        bool underLimit = ennemieSpawnPoint.y + _PlayerPosition.y > _YSpawnLimitStart
            && ennemieSpawnPoint.y + _PlayerPosition.y <= _YSpawnLimitEnd;

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
            Debug.Log("Enemy Type: " + currentNode.enemyType);
            currentNode = currentNode.next;
        }
    }

    void Start()
    {
        _List = new EnnemiesSpawn();
        InvokeRepeating("AddNode", 0f, 5f);
    }
}
