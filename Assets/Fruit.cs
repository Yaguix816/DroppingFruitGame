using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public MoveCloud moveCloud;
    public int fruitIndex;
    public bool hasLanded = false;
    public bool hasBeenDropped=false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBeenDropped && !hasLanded &&(collision.gameObject.CompareTag("Fruit") || collision.gameObject.CompareTag("BottomBorder")))
        {
            hasLanded = true;
            MoveCloud.instance.canDrop = true; //Once the fruit has landed, the cloud can continue dropping
        }
        
        if (collision.gameObject.CompareTag("Fruit"))
        {
            Fruit collidedFruit = collision.gameObject.GetComponent<Fruit>();
            
            if(!gameObject.activeSelf || !collidedFruit.gameObject.activeSelf) //Anti double merge
            {
                return;
            }

            if (fruitIndex == 10 && collidedFruit.fruitIndex == fruitIndex)
            {
                collidedFruit.gameObject.SetActive(false);
                gameObject.SetActive(false);
                MoveCloud.instance.IncreaseScore(MoveCloud.instance.fruitsPrefabs[fruitIndex].points);
                Destroy(collidedFruit.gameObject);
                Destroy(gameObject);
            }

            else if (collidedFruit.fruitIndex == fruitIndex) //Merging
            {
                collidedFruit.gameObject.SetActive(false);
                GameObject newFruit = Instantiate(moveCloud.fruitsPrefabs[fruitIndex+1].prefab);
                MoveCloud.instance.IncreaseScore(MoveCloud.instance.fruitsPrefabs[fruitIndex].points);              //Increment score
                newFruit.transform.position = (transform.position+collidedFruit.gameObject.transform.position)/2;   //New fruit position is at the average
                newFruit.GetComponent<Fruit>().moveCloud = moveCloud;
                gameObject.SetActive(false);
                Destroy(collidedFruit.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
