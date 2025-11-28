using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TMP_Text textMesh;
    public float moveUpSpeed = 1f;
    public float fadeSpeed = 2f;

    private Color originalColor;

    void Start()
    {
        originalColor = textMesh.color;
    }

    void Update()
    {
        // gerak ke atas
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // fading
        Color c = textMesh.color;
        c.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = c;

        if (c.a <= 0)
            Destroy(gameObject);
    }

    public void Setup(string message, Color color)
    {
        textMesh.text = message;
        textMesh.color = color;
    }
}
