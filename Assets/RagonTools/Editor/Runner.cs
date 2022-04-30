using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ragon.Tools
{
  public class Runner : UnityEditor.Editor
  {
    public static void RunEditorAndClient(int clients)
    {
      EditorApplication.isPlaying = true;
      RunApps(clients);
    }

    public static void RunOnlyClients(int clients)
    {
      EditorApplication.isPlaying = false;
      RunApps(clients);
    }

    static void RunApps(int num)
    {
      var buildTarget = EditorUserBuildSettings.activeBuildTarget;
      var tuple = Builder.Executables[buildTarget];
      var buildExe = tuple.Item1;
      var buildPath = tuple.Item2;
      var projectPath = Application.dataPath.Replace("Assets", "");
      var executablePath = "";

      switch (buildTarget)
      {
        case BuildTarget.StandaloneWindows64:
          executablePath = Path.Combine(new[] {projectPath, buildPath, buildExe});
          break;
        case BuildTarget.StandaloneOSX:
          executablePath = Path.Combine(new[]
            {projectPath, buildPath, $"{buildExe}.app", "Contents/MacOS", Application.productName});
          break;
      }

      for (int i = 0; i < num; i++)
      {
        var process = new System.Diagnostics.Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = executablePath;
        process.StartInfo.WorkingDirectory = buildPath;
        process.Start();
      }
    }
  }
}