using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePointHandler : MonoBehaviour
{
    public float angularSpeed = 1f;
    public float circleRad = 1f;

    public List<GameObject> orbsList = new List<GameObject>();

    private Vector2 fixedPoint;
    private GameManager manager;

    void Start()
    {
        fixedPoint = transform.position;
        manager = GameManager.Instance;
        manager.OnGameStateChanged.AddListener(HandleGameStateChange);
    }

    void Update()
    {
        // Don't do anything if only 1 orb
        if(orbsList.Count <= 1)
        {
            return;
        }
        
        foreach (GameObject orb in orbsList)
        {
            orb.GetComponent<LifeBubble>().currentAngle += angularSpeed * Time.deltaTime;
            float currentAngle = orb.GetComponent<LifeBubble>().currentAngle;
            Vector2 offset = new Vector2(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * circleRad;
            orb.transform.position = fixedPoint + offset;
        }
    }

    private void HandleGameStateChange(GameState state)
    {
        StartCoroutine(HeheeRoopeEiLoydaTata(state));
    }

    IEnumerator HeheeRoopeEiLoydaTata(GameState state)
    {
        yield return null;

        if (orbsList.Count >= 2 && state == GameState.OnGoing)
        {
            for (int i = 0; i < orbsList.Count; i++)
            {
                GameObject orb = orbsList[i];
                orb.GetComponent<LifeBubble>().currentAngle = 2 * Mathf.PI * i / orbsList.Count;
            }
        }
    }
}
