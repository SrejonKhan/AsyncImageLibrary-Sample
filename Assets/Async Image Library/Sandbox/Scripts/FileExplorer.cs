using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_STANDALONE_WIN
using AnotherFileBrowser.Windows;
#endif


public class FileExplorer : MonoBehaviour
{

    public void OpenExplorer(Action<string[]> paths)
    {

#if UNITY_STANDALONE_WIN
        WindowsFileExplorer(paths);
#elif UNITY_ANDROID || UNITY_IOS
        AndroidIosPicker(paths);
#endif
    }

    void WindowsFileExplorer(Action<string[]> paths)
    {
#if UNITY_STANDALONE_WIN
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        new FileBrowser().OpenMultiSelectFileBrowser(bp, selectedPaths =>
        {
            paths(selectedPaths);
        });
#endif
    }
    void AndroidIosPicker(Action<string[]> paths)
    {
#if UNITY_ANDROID
        // Use MIMEs on Android
        string[] fileTypes = new string[] { "image/*", "video/*" };
#else
        // Use UTIs on iOS
        string[] fileTypes = new string[] { "public.image", "public.movie" };
#endif

        // Pick image(s) and/or video(s)
        NativeFilePicker.Permission permission = NativeFilePicker.PickMultipleFiles((selectedPaths) =>
        {
            if (selectedPaths == null)
                Debug.Log("Operation cancelled");
            else
            {
                paths(selectedPaths);
            }
        }, fileTypes);
    }

    public string WindowsSaveLocation(string defExtention)
    {
        string path = "";
#if UNITY_STANDALONE_WIN
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        new FileBrowser().SaveFileBrowser(bp, "Image", defExtention,  selectedPath =>
        {
            path = selectedPath;
        });
#endif
        return path;
    }
}
