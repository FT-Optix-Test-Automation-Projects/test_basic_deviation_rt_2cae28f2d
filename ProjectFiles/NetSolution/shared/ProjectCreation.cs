using FTOptix.Core;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.WebUI;
using UAManagedCore;

namespace NetlogicTesting.shared
{
    public class ProjectCreation : BaseNetLogic
    {

        public static void generateBase()
        {
            // Get project
            var projectRoot = Project.Current;
            // Create UI folder
            var uiFolder = InformationModel.MakeObject<Folder>("UI");
            projectRoot.Add(uiFolder);
            // Create Window
            var mainWindow = InformationModel.MakeObjectType<WindowType>("MainWindow");
            mainWindow.Height = 800;
            mainWindow.Width = 800;
            uiFolder.Add(mainWindow);
            // Create Style sheet
            var styleSheet = InformationModel.MakeObject<StyleSheet>("StyleSheet");
            uiFolder.Add(styleSheet);
            // Create User
            var usersFolder = InformationModel.MakeObject<Folder>("Users");
            projectRoot.Add(usersFolder);
            var user = InformationModel.MakeObject<User>("Guest");
            usersFolder.Add(user);
            // Create Native presentation engine
            var nativePresentationEngine = InformationModel.MakeObject<NativeUIPresentationEngine>("NativePresentationEngine");
            nativePresentationEngine.StartWindow = mainWindow;
            nativePresentationEngine.StyleSheet = styleSheet.NodeId;
            nativePresentationEngine.StartingUser = user.NodeId;
            var webPresentationEngine = InformationModel.MakeObject<WebUIPresentationEngine>("WebPresentationEngine");
            webPresentationEngine.StartWindow = mainWindow;
            webPresentationEngine.StyleSheet = styleSheet.NodeId;
            webPresentationEngine.StartingUser = user.NodeId;
            webPresentationEngine.Hostname = "localhost";
            webPresentationEngine.Port = 8080;
            webPresentationEngine.Protocol = FTOptix.WebUI.Protocol.HTTP;
            uiFolder.Add(nativePresentationEngine);
            uiFolder.Add(webPresentationEngine);
        }

    }
}
