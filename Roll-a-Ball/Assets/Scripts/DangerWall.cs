using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerWall : MonoBehaviour
{
    void OnCollisionEnter(Collision hit)
    {
        // collision with Player
        if (hit.gameObject.CompareTag("Player")) {
            // get current sceneIndex
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            // reload scene
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
