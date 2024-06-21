using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ImageManager : MonoBehaviour
{
    public string ImagePath = "/Users/adazhou/Desktop/二次元/Screen Shot 2023-07-15 at 3.17.32 AM.png";
    // Start is called before the first frame update
    public GameObject Obj;
    public TMP_InputField LocalUploadInputField;
    public TMP_InputField WebUploadInputField;
    public UnityEvent OnImageLoad = new UnityEvent();
    public UnityEvent OnFrameSelect = new UnityEvent();
    public UnityEvent OnRemoveSelect = new UnityEvent();
    public UnityEvent OnImageRemove = new UnityEvent();
    public static ImageManager Main;
    public Texture2D LoadedTexture;
    public string description;
    public string ArtistName;
    public string title;
    public TMP_InputField DescriptionInput;
    public TMP_InputField ArtistNameInput;
    public TMP_InputField TitleInput;
    static int MAXSIZE = 2048;
    void Start()
    {
        //LoadDataCloud();
        
        //LoadDataLocal();
       


    }

    public void DeleteAll()
    {
        if (File.Exists(Application.persistentDataPath + "/savedata.dat"))
        {
            File.Delete(Application.persistentDataPath + "/savedata.dat");
            LoadDataLocal();
        }
        Scene currentscene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentscene.buildIndex);
    }
    private void Awake()
    {
        Main = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaceInAllFrame();
        }
    }

    public void PlaceInAllFrame()
    {
        PictureController[] pictureControllers = FindObjectsOfType<PictureController>();
        foreach(PictureController pic in pictureControllers)
        {
            pic.PlacePicture();
        }
    }

    public void LoadTextureFromPath()
    {
        ImagePath = LocalUploadInputField.text;
        byte[] bytes = File.ReadAllBytes(ImagePath);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        LoadTextureToObject(texture);
    }

    public void LoadTextureToObject(Texture2D texture)
    {
        Debug.Log("texture size; " + texture.width + "x" + texture.height);
        /*if(texture.height > MAXSIZE)
        {
            texture.Reinitialize(500, 500);
        }*/
        Obj.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
        LoadedTexture = texture;
    }

    public void SubmitInfo()
    {
        description = DescriptionInput.text;
        ArtistName = ArtistNameInput.text;
        title = TitleInput.text;
        OnImageLoad.Invoke();
    }

    public void SaveDataLocal()
    {
        SavePackage DataToSave = GenerateSavePackage();
        FileStream file = File.Create(Application.persistentDataPath + "/savedata.dat");
        print(Application.persistentDataPath + "/savedata.dat");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, DataToSave);
        file.Close();
        print("Data sucessfully saved");
      
    }

    public void SaveDataCloud()
    {
        SavePackage DataToSave = GenerateSavePackage();
        FireBaseManager.main.UploadData(DataToSave);
        print("Data sucessfully saved");

    }


    SavePackage GenerateSavePackage()
    {
        List<PictureController> PictureList = new List<PictureController>();
        PictureController[] AllPictures = FindObjectsOfType<PictureController>();
        foreach (PictureController pic in AllPictures)
        {
            if (pic.HasImage)
            {
                PictureList.Add(pic);
            }
        }
        PictureController[] pictureControllers = PictureList.ToArray();
        PictureInformation[] StackOfPaper = new PictureInformation[pictureControllers.Length];
        for (int i = 0; i < pictureControllers.Length; i++)
        {
            StackOfPaper[i] = pictureControllers[i].GetData();
        }
        SavePackage DataToSave = new SavePackage();
        DataToSave.StackOfPaper = StackOfPaper;
        DataToSave.score = 10;
        return DataToSave;
    }

    public void LoadDataLocal()
    {
        if (File.Exists(Application.persistentDataPath + "/savedata.dat"))
        {
            PictureController[] pictureControllers = FindObjectsOfType<PictureController>();
            foreach (PictureController pc in pictureControllers)
            {
                pc.RemovePicture();
            }
            //("load in data");
            FileStream file = File.Open(Application.persistentDataPath + "/savedata.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            SavePackage loadeddata = (SavePackage)bf.Deserialize(file);
            PictureInformation[] loadedstack = loadeddata.StackOfPaper;
            foreach(PictureInformation pieceofpaper in loadedstack)
            {
                foreach(PictureController pc in pictureControllers)
                {
                    if(pc.id == pieceofpaper.id)
                    {
                        pc.SetData(pieceofpaper);
                    }
                }
            }
        }
        else
        {
            print("no save data");
        }
    }
    
    public void LoadDataCloud()
    {
        FireBaseManager.main.DownloadData();
    }

    public void FirebaseReciever(SavePackage package)
    {
        if (true)
        {
            PictureController[] pictureControllers = FindObjectsOfType<PictureController>();
            foreach (PictureController pc in pictureControllers)
            {
                pc.RemovePicture();
            }
            //print("load in data");
            //print(package.StackOfPaper[0].id);
            
            SavePackage loadeddata = package;
            PictureInformation[] loadedstack = loadeddata.StackOfPaper;
            //print("hello");
            foreach (PictureInformation pieceofpaper in loadedstack)
            {
                //print(pieceofpaper.ArtistName);
                foreach (PictureController pc in pictureControllers)
                {
                    if (pc.id == pieceofpaper.id)
                    {
                        pc.SetData(pieceofpaper);
                    }
                }
            }
            //print("hi");
        }
        else
        {
            print("no save data");
        }
    }
   
    public void RemoveOptionSelected()
    {
        OnRemoveSelect.Invoke();
    }
    public void ImageRemove()
    {
        OnImageRemove.Invoke();
    }

    public void ImagePlace()
    {
        OnFrameSelect.Invoke();
    }

    public void GetImageFromWeb()
    {
        StartCoroutine(GetImageFromWebCoroutine(WebUploadInputField.text));
    }

    public IEnumerator GetImageFromWebCoroutine(string URL)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
        yield return request.SendWebRequest();
        if(request.isNetworkError||request.isHttpError)
        {
            print("There was an error: " + request.error);
        }
        else
        {
            Texture2D DownloadTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            LoadTextureToObject(DownloadTexture);
        }
    }

}
[System.Serializable]
public class PictureInformation
{
    public string description;
    public string title;
    public string ArtistName;
    public byte[] texture;
    public string id;
}

[System.Serializable]
public class SavePackage
{
    public PictureInformation[] StackOfPaper;
    public int score;
}

