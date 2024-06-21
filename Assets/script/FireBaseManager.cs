using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
using System.Threading.Tasks;

public class FireBaseManager : MonoBehaviour
{
    public string password;
    DatabaseReference DataBaseRoot;
    public static FireBaseManager main;

    // Start is called before the first frame update

    void Start()
    {
        DataBaseRoot = FirebaseDatabase.DefaultInstance.RootReference;
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        main = this;
    }

    public void UploadData(SavePackage savePackage)
    {
        PictureInformation[] PaperStack = savePackage.StackOfPaper;
        for (int i = 0; i < PaperStack.Length; i++)
        {
            PictureInformation paper = PaperStack[i];
            DatabaseReference PaperEntry = DataBaseRoot.Child(password).Child("pictures").Child(paper.id);
            PaperEntry.Child("ArtistName").SetValueAsync(paper.ArtistName);
            PaperEntry.Child("ArtName").SetValueAsync(paper.title);
            PaperEntry.Child("Description").SetValueAsync(paper.description);
            string PictureString = "";
            if (paper.texture != null)
            {
                PictureString = Convert.ToBase64String(paper.texture);
            }
            PaperEntry.Child("Texture").SetValueAsync(PictureString);
        }
    }

    public void DownloadData()
    {
        //Debug.Log("loaded");
        float starttime = Time.time;
        SavePackage package = new SavePackage();
        DataBaseRoot.Child(password).Child("pictures").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log(task.Exception);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
                return;
            }
            DataSnapshot dataSnapshot = task.Result;
            package.StackOfPaper = new PictureInformation[dataSnapshot.ChildrenCount];
            int index = 0;
            int TotalSize = 0;
            foreach (DataSnapshot snap in dataSnapshot.Children)
            {
                PictureInformation paper = new PictureInformation();
                paper.id = snap.Key;
                paper.ArtistName = snap.Child("ArtistName").Value.ToString();
                paper.title = snap.Child("ArtName").Value.ToString();
                paper.description = snap.Child("Description").Value.ToString();
                string texturestring = snap.Child("Texture").Value.ToString();
                if (texturestring == "")
                {
                    paper.texture = null;
                }
                else
                {
                    paper.texture = Convert.FromBase64String(texturestring);
                    TotalSize += Convert.FromBase64String(texturestring).Length;
                }
                package.StackOfPaper[index] = paper;
                index += 1;
            }
            //Debug.Log("file size " + TotalSize + " time taken " + (Time.time - starttime));
            ImageManager.Main.FirebaseReciever(package);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
