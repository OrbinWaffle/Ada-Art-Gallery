using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ZoomManager : MonoBehaviour
{
    public Image ZoomImage;
    public static ZoomManager main;
    public Transform camera;
    private void Awake()
    {
        main = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ZoomIn(Texture2D ZoomTexture)
    {
        if(ZoomTexture == null)
        {
            return;
        }
        Rect rec = new Rect(0, 0, ZoomTexture.width, ZoomTexture.height);
        ZoomImage.sprite = Sprite.Create(ZoomTexture, rec, Vector2.zero);
        ZoomImage.preserveAspect = true;
        ZoomImage.gameObject.SetActive(true);
        transform.position = camera.position + camera.forward * 1;
        transform.forward = camera.forward;
    }

    public void ZoomOut()
    {
        ZoomImage.gameObject.SetActive(false);
    }
}