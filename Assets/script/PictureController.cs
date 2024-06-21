using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PictureController : MonoBehaviour
{
    public bool HasImage;
    public MeshRenderer canvas;
    public Button PlaceButton;
    public Button RemoveButton;
    public string id;

    public TextMeshProUGUI nameText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI titleText;

    public Texture2D defaultTexture;
    // Start is called before the first frame update
    private void Awake()
    {
        id = transform.position.ToString().Replace(".", "_");
    }

    void Start()
    {
        ImageManager.Main.OnImageLoad.AddListener(EnablePlaceButton);
        ImageManager.Main.OnFrameSelect.AddListener(DisablePlaceButton);
        ImageManager.Main.OnRemoveSelect.AddListener(EnableRemoveButton);
        ImageManager.Main.OnImageRemove.AddListener(DisableRemoveButton);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlacePicture()
    {
        canvas.material.SetTexture("_MainTex", ImageManager.Main.LoadedTexture);
        ImageManager.Main.ImagePlace();
        nameText.text = ImageManager.Main.ArtistName;
        print(ImageManager.Main.ArtistName.ToString());
        descriptionText.text = ImageManager.Main.description;
        titleText.text = ImageManager.Main.title;
        HasImage = true;
    }

    public void RemovePicture()
    {
        nameText.text = "";
        descriptionText.text = "";
        titleText.text = "";
        canvas.material.SetTexture("_MainTex", defaultTexture);
        ImageManager.Main.ImageRemove();
        HasImage = false;
    }

    public void EnablePlaceButton()
    {
        PlaceButton.gameObject.SetActive(true);
    }

    public void DisablePlaceButton()
    {
        PlaceButton.gameObject.SetActive(false);
    }

    public void EnableRemoveButton()
    {
        RemoveButton.gameObject.SetActive(true);
    }

    public void DisableRemoveButton()
    {
        RemoveButton.gameObject.SetActive(false);
    }
    //take all your stuff and put it on a piece of paper
    public PictureInformation GetData()
    {
        PictureInformation NewInformation = new PictureInformation();
        NewInformation.description = descriptionText.text;
        NewInformation.ArtistName = nameText.text;
        NewInformation.title = titleText.text;
        Debug.Log(id+" "+((Texture2D)canvas.material.GetTexture("_MainTex")).GetRawTextureData().Length) ;
        NewInformation.texture = ImageConversion.EncodeToJPG((Texture2D)canvas.material.GetTexture("_MainTex"));
        NewInformation.id = id;
        return NewInformation;
    }
    //take a piece of paper, read it and put all of the information on you
    public void SetData(PictureInformation information)
    {
        descriptionText.text = information.description;
        nameText.text = information.ArtistName;
        titleText.text = information.title;
        Texture2D texture = new Texture2D(1, 1);
        if(information.texture == null)
        {
            texture = defaultTexture;
            HasImage = false;
        }
        else
        {
            Debug.Log(id + " " + information.texture.Length);
            texture.LoadImage(information.texture);
            HasImage = true;
        }
        canvas.material.SetTexture("_MainTex", texture);
    }

    public void DoZoom()
    {
        ZoomManager.main.ZoomIn((Texture2D)canvas.material.GetTexture("_MainTex"));
    }
    
}
