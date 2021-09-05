using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_STANDALONE_WIN
using AnotherFileBrowser.Windows;
#endif


public class FileExplorer : MonoBehaviour
{

    public string[] OpenExplorer()
    {

#if UNITY_STANDALONE_WIN
        return WindowsFileExplorer();
#elif UNITY_ANDROID || UNITY_IOS
        return AndroidIosPicker();
#endif
    }

    string[] WindowsFileExplorer()
    {
#if UNITY_STANDALONE_WIN 
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        string[] paths = new string[0];

        new FileBrowser().OpenMultiSelectFileBrowser(bp, selectedPaths =>
        {
            paths = selectedPaths;
        });

        return paths;
#endif
    }
    string[] AndroidIosPicker()
    {
#if UNITY_ANDROID
        // Use MIMEs on Android
        string[] fileTypes = new string[] { "image/*", "video/*" };
#else
        // Use UTIs on iOS
        string[] fileTypes = new string[] { "public.image", "public.movie" };
#endif
        string[] paths = new string[0];

        // Pick image(s) and/or video(s)
        NativeFilePicker.Permission permission = NativeFilePicker.PickMultipleFiles((selectedPaths) =>
        {
            if (selectedPaths == null)
                Debug.Log("Operation cancelled");
            else
            {
                paths = selectedPaths;
            }
        }, fileTypes);

        return paths;
    }

    public string WindowsSaveLocation(string defExtention)
    {
#if UNITY_STANDALONE_WIN
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        string path = "";

        new FileBrowser().SaveFileBrowser(bp, "Image", defExtention,  selectedPath =>
        {
            path = selectedPath;
        });
        return path;
#endif
    }
}
