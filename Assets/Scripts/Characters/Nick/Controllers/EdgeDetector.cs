using System.Collections.Generic;
using UnityEngine;

public class EdgeDetector : MonoBehaviour
{
    private List<GameObject> groundObjects;
    public bool pIsOnGround { get => groundObjects.Count > 0; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(groundObjects == null)
        {
            groundObjects = new List<GameObject>();
        }

        var gameObject = collision.gameObject;
        if (!groundObjects.Contains(gameObject))
        {
            groundObjects.Add(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (groundObjects == null)
        {
            groundObjects = new List<GameObject>();
        }

        var gameObject = collision.gameObject;
        if (groundObjects.Contains(gameObject))
        {
            groundObjects.Remove(gameObject);
        }
    }
}
