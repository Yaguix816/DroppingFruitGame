using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoveCloud : MonoBehaviour
{
    public static MoveCloud instance;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float minXPosition = -2.6f;
    [SerializeField] private float maxXPosition = 2.6f;

    public FruitsList[] fruitsPrefabs;
    
    public GameObject fruitPreview;

    //Next fruit preview
    [SerializeField] private GameObject nextFruit;
    [SerializeField] private int nextFruitIndex;
    [SerializeField] private GameObject nextFruitPreview;
    //Next next fruit preview
    [SerializeField] private GameObject nextNextFruit;
    [SerializeField] private int nextNextFruitIndex;
    [SerializeField] private GameObject nextNextFruitPreview;

    //Score
    public Text scoreText;
    [HideInInspector] public int currentScore=0;

    [HideInInspector] public bool canDrop=false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Plus d'une instance de MoveCloud");
            return;
        }
        instance = this;
    }
    private void Start()
    {
        LoadNextNextFruit();
        LoadNextFruit();
        canDrop = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && transform.position.x > minXPosition)
        {
            transform.position -= new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D) && transform.position.x < maxXPosition)
        {
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space) && canDrop)
        {
            canDrop = false;
            GameObject newFruit = Instantiate(nextFruit);
            newFruit.transform.position = transform.position;

            Fruit newFruitScript = newFruit.GetComponent<Fruit>();
            newFruitScript.fruitIndex = nextFruitIndex;
            newFruitScript.moveCloud = this;
            newFruitScript.hasBeenDropped = true;

            LoadNextFruit();
        }
        
    }
    void LoadNextNextFruit()
    {
        //Decide which fruit will be summoned
        nextNextFruitIndex = (int)Random.Range(0, 5);
        nextNextFruit = fruitsPrefabs[nextNextFruitIndex].prefab;

        //Delete previous previews
        foreach (Transform child in fruitPreview.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject nextNextFruitPreview = Instantiate(nextNextFruit, fruitPreview.transform);
        nextNextFruitPreview.GetComponent<Collider2D>().isTrigger = true;
        nextNextFruitPreview.GetComponent<Rigidbody2D>().isKinematic = true;
        nextNextFruitPreview.transform.localPosition = Vector3.zero;
    }

    void LoadNextFruit() 
    {
        //Get the fruit from the next next fruit preview
        nextFruit = nextNextFruit;
        nextFruitIndex = nextNextFruitIndex;
        
        //Delete previous previews
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GameObject nextFruitPreview= Instantiate(nextFruit, transform);
        nextFruitPreview.GetComponent<Collider2D>().isTrigger = true;
        nextFruitPreview.GetComponent<Rigidbody2D>().isKinematic = true;
        nextFruitPreview.transform.localPosition = Vector3.zero;

        LoadNextNextFruit();
    }

    public void IncreaseScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
    }
}

[System.Serializable]
public class FruitsList
{
    public GameObject prefab;
    public int points;
}