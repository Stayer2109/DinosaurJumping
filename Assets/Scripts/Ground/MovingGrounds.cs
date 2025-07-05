using System.Collections.Generic;
using UnityEngine;

public class MovingGrounds : MonoBehaviour
{

    [Header("Grounds List")]
    [SerializeField]
    private List<GameObject> groundPieces;

    private Camera mainCamera;
    private float parentYPosition;

    void Start()
    {
        mainCamera = Camera.main;
        parentYPosition = transform.position.y;
        InitGroundsPos();
    }

    void Update()
    {
        MoveGrounds();
    }

    private void InitGroundsPos()
    {
        float currentX = -15f;
        foreach (GameObject ground in groundPieces)
        {
            float width = GetWidth(ground);
            ground.transform.position = new Vector3(currentX + width / 2, parentYPosition, 0f);
            currentX += width;
        }
    }

    private float GetWidth(GameObject go)
    {
        return go.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void MoveGrounds()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        float moveStep = DifficultyManager.Instance.CurrentSpeed * Time.deltaTime;

        foreach (var ground in groundPieces)
        {
            ground.transform.Translate(Vector3.left * moveStep);
        }

        float halfCamWidth = GetHalfScreenWidthInWorldUnits();

        foreach (var ground in groundPieces)
        {
            float width = GetWidth(ground);
            // The width is calculated from the middle of the sprite, because the pivot is at the center.
            // Therefore, width must be divided by 2 to get the right edge position.
            if (ground.transform.position.x + width / 2 < mainCamera.transform.position.x - halfCamWidth)
            {
                GameObject rightmost = GetRightmostGround();
                float newX = rightmost.transform.position.x + GetWidth(rightmost);
                ground.transform.position = new Vector3(newX - 1f, parentYPosition, 0f);
            }
        }
    }

    private float GetHalfScreenWidthInWorldUnits()
    {
        float halfHeight = mainCamera.orthographicSize;
        float aspect = (float)Screen.width / Screen.height;
        return halfHeight * aspect;
    }

    private GameObject GetRightmostGround()
    {
        GameObject rightmost = groundPieces[0];
        foreach (var ground in groundPieces)
        {
            if (ground.transform.position.x > rightmost.transform.position.x)
                rightmost = ground;
        }
        return rightmost;
    }

}
