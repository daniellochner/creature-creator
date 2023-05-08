using System;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

using UnityEngine.Networking;


namespace MobileDependencyResolverLP.Installer.Editor
{
    static class MobileDependencyResolverInstallerLp
    {
        const string k_UnityMediationPackage = "com.unity.services.mediation";
        const string k_PackageUrl   = @"https://s3.amazonaws.com/ssa.public/MDR/mobile-dependency-resolver.unitypackage";
        const string k_DownloadPath = @"Temp/MDR.unitypackage";

        const string k_DoNotAskAgain = "Unity.Mediation.MobileDependencyResolver.DoNotAskAgain";
        const string k_DialogTitle = "Mobile Dependency Resolver required";
        const string k_DialogText = "Mediation requires Mobile Dependency Resolver to resolve native dependencies.\n" +
            " Would you like to import the package?";
        const string k_DialogTitleError = "Error";
        const string k_DialogTextError = "Failed to download MDR; ";
        const string k_ButtonOk = "OK";
        const string k_ButtonImport = "Import";
        const string k_ButtonCancel = "Cancel";
        const string k_ButtonDontAskAgain = "Ignore - Do not ask again during this session";


        [InitializeOnLoadMethod]
        static void InstallPlayServicesResolverIfNeeded()
        {
            if (!IsPackageInstalled(k_UnityMediationPackage))
            {
                EditorApplication.quitting += EditorApplicationOnQuitting;

                if (IsPlayServicesResolverInstalled())
                    return;

                // The user will have a choice to ignore this dialog for the entire session.
                if ((Application.isBatchMode && Environment.GetEnvironmentVariable("UNITY_THISISABUILDMACHINE") != null) || AskUserToInstallPackage())
                {
                    InstallPackage();
                }
            }
        }

        static bool IsPackageInstalled(string package)
        {
            if (File.Exists("Packages/manifest.json"))
            {
                var packageList = File.ReadAllText("Packages/manifest.json");
                return packageList.Contains(package);
            }

            return false;
        }

        static void EditorApplicationOnQuitting()
        {
            EditorPrefs.DeleteKey(k_DoNotAskAgain);
        }

        static bool AskUserToInstallPackage()
        {
            if (EditorPrefs.GetBool(k_DoNotAskAgain)) return false;

            var response = EditorUtility.DisplayDialogComplex(k_DialogTitle, k_DialogText,
                k_ButtonImport, k_ButtonCancel, k_ButtonDontAskAgain);

            switch (response)
            {
                case 0:
                    return true;
                case 1:
                    return false;
                case 2:
                    EditorPrefs.SetBool(k_DoNotAskAgain, true);
                    return false;
                default:
                    return false;
            }
        }

        internal static bool IsPlayServicesResolverInstalled()
        {
            try
            {
                return Type.GetType("Google.VersionHandler, Google.VersionHandler") != null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        static void InstallPackage()
        {
            bool downloaded = false;
            using (var wc = new WebClient())
            {
                try
                {
                    wc.DownloadFile(k_PackageUrl, k_DownloadPath);
                    downloaded = true;
                }
                catch (WebException e)
                {
                    EditorUtility.DisplayDialog(
                        k_DialogTitleError,
                        k_DialogTextError + e.Message,
                        k_ButtonOk);
                }
            }

            if (downloaded)
            {
                var absolutePath = Path.GetFullPath(k_DownloadPath);
                AssetDatabase.ImportPackage(absolutePath, false);
                File.Delete(absolutePath);
            }
        }
    }
}
