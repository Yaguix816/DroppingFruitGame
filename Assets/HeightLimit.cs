using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeightLimit : MonoBehaviour
{
    public MoveCloud moveCloud;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            Fruit fruitScript = collision.gameObject.GetComponent<Fruit>();

            if (fruitScript.hasLanded)
            {
                print("You made "+MoveCloud.instance.currentScore.ToString()+" points !");
                GameVariables.endScoreString = MoveCloud.instance.scoreText.text;
                SceneManager.LoadScene(1);
            }
        }
    }
}
