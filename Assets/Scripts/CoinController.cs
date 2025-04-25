using UnityEngine;
using System;
using System.Collections;

public class CoinController : MonoBehaviour
{
    private Action<int> onFlipComplete; // Callback to notify GameManager
    private int resultSide; // 1 = Head, 2 = Tail
    private int lastResult = 1;

    public void Flip(int result, Action<int> callback)
    {
        Debug.Log(result);
        resultSide = result;
        onFlipComplete = callback;
        StartCoroutine(FlipAnimation());
    }

    private IEnumerator FlipAnimation()
    {
        float duration = 1.0f;
        float elapsed = 0f;
        float totalRotation = resultSide == lastResult ? 720f : 900f; // Coin Rotation
        lastResult = resultSide;

        Vector3 initialRotation = transform.eulerAngles;
    
        while (elapsed < duration)
        {
            float angle = Mathf.Lerp(0, totalRotation, elapsed / duration);
            transform.eulerAngles = initialRotation + new Vector3(0, angle, 0); // Rotate on Y-axis
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to result side
        if (resultSide == 1)
            transform.eulerAngles = new Vector3(0, 0, 0); // Head
        else
            transform.eulerAngles = new Vector3(0, 180, 0); // Tail

        // Notify GameManager
        onFlipComplete?.Invoke(resultSide);
    }
}
