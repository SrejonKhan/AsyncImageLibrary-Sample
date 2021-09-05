using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AsyncImageLibrary;
using SkiaSharp;


public class ImageLoader : MonoBehaviour
{
    [Tooltip("TTF must be in Streaming Assets.")]
    public string ttfFileName = "Hack-Italic.ttf";

    public GameObject rawImgPrefab;
    public Transform gallaryGrid;

    public Text totalSelectedText;
    public Text waitingTimeText;

    public Button generateTexturesButton;
    public Button selectImagesButton;

    private SKTypeface typeface;
    private FileExplorer fileExplorer;

    private int totalImageStaged;
    private int totalImageLoaded;

    private void Start()
    {
        typeface = SKTypeface.FromFile(GetStreammingAssetsPath(ttfFileName), 0);
        fileExplorer = GetComponent<FileExplorer>();
    }

    public void SelectImages()
    {
        totalSelectedText.text = "0 Images Selected";
        KillAllChild(gallaryGrid);
        string[] paths = fileExplorer.OpenExplorer();
        StartCoroutine(StageImages(paths));
    }

    public void GenerateStagedTextures()
    {
        StartCoroutine(MainThreadQueuer.ExecuteProcessPerFrame());
    }

    IEnumerator StageImages(string[] paths)
    {
        if (paths.Length == 0)
            yield break;

        totalImageStaged = paths.Length;
        totalSelectedText.text = paths.Length + " Images Selected";
        selectImagesButton.interactable = false;
        generateTexturesButton.interactable = false;

        // Wait
        for (int i = 1; i >= 0; i--)
        {
            waitingTimeText.text = $"Loading in {i} seconds...";
            yield return new WaitForSecondsRealtime(1f);
        }

        // Load Images
        for (int i = 0; i < paths.Length; i++)
        {
            ImageCellView img = Instantiate(rawImgPrefab, gallaryGrid).GetComponent<ImageCellView>();
            LoadImage(paths[i], img);
            yield return null;
        }
        totalSelectedText.text = paths.Length + " Images Staged";
        waitingTimeText.text = "Press Generate Staged Textures.";
    }

    void ValidateQueueStatus()
    {
        totalImageLoaded++;
        if (totalImageLoaded == totalImageStaged)
        {
            generateTexturesButton.interactable = true;
            selectImagesButton.interactable = true;
        }
    }

    void LoadImage(string path, ImageCellView cellView)
    {
        AsyncImage image = new AsyncImage(path, true, true);
        
        var (info, format) = image.GetInfo();
        cellView.info = $"{format} ({info.Width} x {info.Height})";

        string text = "Leaves are on the ground, fall has come.";


        Color.RGBToHSV(Color.white, out float h, out float s, out float v);

        var paint = new SKPaint();
        paint.Color = SKColor.FromHsv(h * 360, s * 100, v * 100);
        paint.TextAlign = SKTextAlign.Center;
        // Loading Font from file
        paint.Typeface = typeface;
        // Adjust TextSize property so text is 90% of screen width
        float textWidth = paint.MeasureText(text);
        paint.TextSize = 0.9f * info.Width * paint.TextSize / textWidth;

        image.DrawText(text, new Vector2(info.Width / 2, info.Height / 2), paint);

        image.OnLoad += () =>
        {
            ValidateQueueStatus();
            cellView.ToggleSpinner();
            cellView.msgText.text = "Staged";
        };

        image.OnTextureLoad += () =>
        {
            cellView.image.texture = image.Texture;
            cellView.image.GetComponent<AspectRatioFitter>().aspectRatio = (float)image.Width / (float)image.Height;
            cellView.msgText.text = cellView.info;
            cellView.asyncImage = image;
        };

        image.Load();
    }

    void KillAllChild(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static string GetStreammingAssetsPath(string fileName)
    {
#if UNITY_EDITOR_OSX
            return "file://" + Application.streamingAssetsPath + "/" + fileName;
#elif UNITY_EDITOR
        return Application.streamingAssetsPath + "/" + fileName;
#elif UNITY_ANDROID
            return Path.Combine("jar:file://" + Application.dataPath + "!/assets" , fileName);
#elif UNITY_IOS
            return Path.Combine(Application.dataPath + "/Raw" , fileName);
#else
            return Application.streamingAssetsPath + "/" + fileName;
#endif
    }

}
