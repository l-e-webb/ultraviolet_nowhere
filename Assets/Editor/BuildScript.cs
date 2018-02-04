using UnityEditor;

public class BuildScript {

    private const string EXECUTABLE_NAME = "UltravioletNowhere";
    private const string WINDOWS_PATH = "_build/UltravioletNowhere_win";
    private const string MAC_PATH = "_build/UltravioletNowhere_mac";
    private const string LINUX_PATH = "_build/UltravioletNowhere_linux";
    private const string WEB_PATH = "_build/UltravioletNowhere_web";
    //private const string GH_PAGES_DISTRO_PATH = "webGL";

    [MenuItem("BuildTools/Build all")]
    public static void BuildAll() {
        string[] scenes = new string[] { "Assets/Scenes/MainScene.unity" };

        BuildPipeline.BuildPlayer(scenes, WINDOWS_PATH + "/" + EXECUTABLE_NAME + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, MAC_PATH + "/" + EXECUTABLE_NAME, BuildTarget.StandaloneOSX, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, LINUX_PATH + "/" + EXECUTABLE_NAME, BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, WEB_PATH, BuildTarget.WebGL, BuildOptions.None);
        //FileUtil.DeleteFileOrDirectory(GH_PAGES_DISTRO_PATH + "/");
        //FileUtil.CopyFileOrDirectory(WEB_PATH, GH_PAGES_DISTRO_PATH + "/");
    }

}