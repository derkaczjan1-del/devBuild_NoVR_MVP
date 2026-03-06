using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    GameObject enemy;

    private void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
    }

    private void Update()
    {
        ReloadScene();
    }
    // Start is called before the first frame update
    public void ReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Space Pressed");
            if (enemy.gameObject.GetComponent<Enemy>().IsPlayerDead())
            {
                print("Reloading Scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
        
    }
}
