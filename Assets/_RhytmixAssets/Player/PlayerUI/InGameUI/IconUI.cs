using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    [SerializeField] float speed  = 40f;
    [SerializeField] float amountX = 2f;
    [SerializeField] float amountY = 1f;
    private Image _iconImage;
    private Sprite originalSprite;
    private Vector3 initialPos;
    bool isHurt = false;

    [SerializeField] Sprite IconHurt;
    private void Start()
    {
        _iconImage = GetComponent<Image>();
        originalSprite = _iconImage.sprite;
        initialPos = transform.position;
    }

    public void HurtIcon()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchHurt());
    }

    IEnumerator SwitchHurt()
    {
        isHurt = true;
        _iconImage.sprite = IconHurt;
        yield return new WaitForSeconds(.5f);
        isHurt = false;
        _iconImage.sprite = originalSprite;
    }

    private void Update()
    {
        if(isHurt)
        {
            transform.position = new Vector3(initialPos.x + Mathf.Sin(Time.time * speed) * amountX, initialPos.y + Mathf.Sin(Time.time * speed) * amountY, initialPos.z);
        }
    }
}
