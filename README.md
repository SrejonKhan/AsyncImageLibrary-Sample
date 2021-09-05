# Async Image Library - Sample

This is a sandbox type project of [Async Image Library](https://github.com/SrejonKhan/AsyncImageLibrary).

Please keep in mind, current status of this project is unstable and sketchy.

# Dependencies
* [Another File Browser](https://github.com/SrejonKhan/AnotherFileBrowser) for opening File Explorer in Windows Standalone.
* [Unity Native File Picker Plugin]("https://github.com/yasirkula/UnityNativeFilePicker") for opening File Picker in Android & IOS.
* [Hack Font]("https://github.com/source-foundry/Hack") for checking Typeface loading in runtime. 

# Known Issues
- You may face this following error after opening it in 2020.3.x for first time 
    ```
    Assembly 'Library/ScriptAssemblies/Unity.PlasticSCM.Editor.dll' will not be loaded due to errors: Reference has errors 'unityplastic'.
    ```
    ```
    Assembly 'Packages/com.unity.collab-proxy/Lib/Editor/PlasticSCM/unityplastic.dll' will not be loaded due to errors:
    unityplastic references strong named System.Windows.Forms Assembly references: 4.0.0.0 Found in project: 2.0.0.0.
    Assembly Version Validation can be disabled in Player Settings "Assembly Version Validation"
    ```
    To fix this issue, please **Restart Unity**.