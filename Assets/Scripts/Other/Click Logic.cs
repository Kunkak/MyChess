using UnityEngine;

public class ClickLogic : MonoBehaviour
{
    public Vector2Int position;

    void Start()
    {
        position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    
    private void OnMouseDown()
    {
        if (!GameManager.instance.playing)
            return;
        GameManager.instance.OnClick(position);
    }
}
