using AsyncImageLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageExporter : MonoBehaviour
{

    public GameObject exportPanel;
    public RawImage imagePreview;

    public Dropdown resizeDropdown;
    public GameObject[] resizeValueFields;

    public Button resizeButton;
    public Button exportButton;

    private AsyncImage asyncImage;
    private ImageProcessor imageProcessor;
    public static ImageExporter instance;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        imageProcessor = GetComponent<ImageProcessor>();
    }

    public void OpenExportPanel(AsyncImage asyncImage)
    {
        this.asyncImage = asyncImage;
        imagePreview.texture = asyncImage.Texture;
        imagePreview.GetComponent<AspectRatioFitter>().aspectRatio = (float)asyncImage.Width / (float)asyncImage.Height;

        exportPanel.SetActive(true);
    }

    public void CloseExportPanel()
    {
        exportPanel.SetActive(false);
    }

    public void OnResizeDropdownValueChange()
    {
        for (int i = 0; i < resizeValueFields.Length; i++)
        {
            resizeValueFields[i].SetActive(false);
        }
        resizeValueFields[resizeDropdown.value].SetActive(true);
    }

    public void ResizeImage()
    {
        exportButton.interactable = false;
        resizeButton.interactable = false;

        switch (resizeDropdown.value)
        {
            case 0:
                imageProcessor.DivideByResize(asyncImage, int.Parse(resizeValueFields[0].GetComponent<InputField>().text),
                () =>
                {
                    exportButton.interactable = true;
                    resizeButton.interactable = true;
                });
                break;
            case 1:
                Vector2 targetDimensions = new Vector2();
                targetDimensions.x = int.Parse(resizeValueFields[1].GetComponent<InputField>().text);
                targetDimensions.y = int.Parse(resizeValueFields[2].GetComponent<InputField>().text);
                
                imageProcessor.TargetDimensionResize(asyncImage, targetDimensions,
                () =>
                {
                    exportButton.interactable = true;
                    resizeButton.interactable = true;
                });
                break;
        }
    }

    public void Export()
    {
        string extension = "";

        var (info, format) = asyncImage.GetInfo();


        switch (format)
        {
            case SkiaSharp.SKEncodedImageFormat.Astc:
                extension = ".astc";
                break;
            case SkiaSharp.SKEncodedImageFormat.Bmp:
                extension = ".bmp";
                break;
            case SkiaSharp.SKEncodedImageFormat.Gif:
                extension = ".gif";
                break;
            case SkiaSharp.SKEncodedImageFormat.Ico:
                extension = ".ico";
                break;
            case SkiaSharp.SKEncodedImageFormat.Jpeg:
                extension = ".jpeg";
                break;
            case SkiaSharp.SKEncodedImageFormat.Png:
                extension = ".png";
                break;
            case SkiaSharp.SKEncodedImageFormat.Wbmp:
                extension = ".wbmp";
                break;
            case SkiaSharp.SKEncodedImageFormat.Webp:
                extension = ".webp";
                break;
            case SkiaSharp.SKEncodedImageFormat.Pkm:
                extension = ".pkm";
                break;
            case SkiaSharp.SKEncodedImageFormat.Ktx:
                extension = ".ktx";
                break;
            case SkiaSharp.SKEncodedImageFormat.Dng:
                extension = ".dng";
                break;
            case SkiaSharp.SKEncodedImageFormat.Heif:
                extension = ".heif";
                break;
        }

        var fileExplorer = GetComponent<FileExplorer>();
        string savePath = "";

#if UNITY_STANDALONE_WIN
        savePath = fileExplorer.WindowsSaveLocation(extension);
#elif UNITY_ANDROID || UNITY_IOS
        savePath = Application.streamingAssetsPath + "/" + DateTime.Now.Millisecond.ToString() + extension;
#endif

        asyncImage.Save(savePath);
    }

}
