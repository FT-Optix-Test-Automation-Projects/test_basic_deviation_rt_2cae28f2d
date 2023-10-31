#region Using directives
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.Alarm;

#endregion


public class BasicDeviationRT: BaseNetLogic
{

    [ExportMethod]
    public void generateProject()
    {

        // ProjectCreation.generateBase();
        var projectRoot = Project.Current;
        var mainWindow = projectRoot.Get("UI").Get("MainWindow");
        var window = Owner.Get("UI").Get<WindowType>("MainWindow");
        window.Width = 1300;

        // var modelFolder = InformationModel.MakeObject<Folder>("Model");
        // projectRoot.Add(modelFolder);
        // var alarmsFolder = InformationModel.MakeObject<Folder>("Alarms");
        // projectRoot.Add(alarmsFolder);

        var styleSheet = InformationModel.MakeObject<StyleSheet>("StyleSheet");

        var user = InformationModel.MakeObject<User>("Guest");
        var usersFolder = projectRoot.Get("Security").Get("Users");
        usersFolder.Add(user);

        var uiFolder = projectRoot.Get("UI");
        var webPresentationEngine = InformationModel.MakeObject<WebUIPresentationEngine>("WebPresentationEngine");
        webPresentationEngine.Protocol = FTOptix.WebUI.Protocol.HTTP;
        webPresentationEngine.StyleSheet = styleSheet.NodeId;
        webPresentationEngine.StartingUser = user.NodeId;
        webPresentationEngine.StartWindow = window;
        webPresentationEngine.Hostname = "localhost";
        webPresentationEngine.Port = 8080;

        uiFolder.Add(styleSheet);
        uiFolder.Add(webPresentationEngine);

        var modelFolder = projectRoot.Get("Model");
        var alarmsFolder = projectRoot.Get("Alarms");

        var variable1 = InformationModel.MakeVariable("Variable1", OpcUa.DataTypes.Int32);
        modelFolder.Add(variable1);
        modelFolder.GetVariable("Variable1").RemoteWrite(10);

        var exclusiveDeviationAlarm1 = InformationModel.MakeObject<ExclusiveDeviationAlarmController>("ExclusiveDeviationAlarm1");
        exclusiveDeviationAlarm1.InputValueVariable.SetDynamicLink(variable1);
        exclusiveDeviationAlarm1.Setpoint = 10;
        exclusiveDeviationAlarm1.HighHighLimit = 2;
        exclusiveDeviationAlarm1.HighLimit = 1;
        exclusiveDeviationAlarm1.LowLimit = -1;
        exclusiveDeviationAlarm1.LowLowLimit = -2;
        alarmsFolder.Add(exclusiveDeviationAlarm1);

        var exclusiveDeviationAlarm2 = InformationModel.MakeObject<ExclusiveDeviationAlarmController>("ExclusiveDeviationAlarm2");
        exclusiveDeviationAlarm2.InputValueVariable.SetDynamicLink(variable1);
        exclusiveDeviationAlarm2.Setpoint = 10;
        exclusiveDeviationAlarm2.LowLowLimit = -2;
        exclusiveDeviationAlarm2.AutoAcknowledge = true;
        alarmsFolder.Add(exclusiveDeviationAlarm2);

        var nonExclusiveDeviationAlarm1 = InformationModel.MakeObject<NonExclusiveDeviationAlarmController>("NonExclusiveDeviationAlarm1");
        nonExclusiveDeviationAlarm1.InputValueVariable.SetDynamicLink(variable1);
        nonExclusiveDeviationAlarm1.Setpoint = 10;
        nonExclusiveDeviationAlarm1.HighHighLimit = 2;
        nonExclusiveDeviationAlarm1.HighLimit = 1;
        nonExclusiveDeviationAlarm1.LowLimit = -1;
        nonExclusiveDeviationAlarm1.LowLowLimit = -2;
        alarmsFolder.Add(nonExclusiveDeviationAlarm1);

        var nonExclusiveDeviationAlarm2 = InformationModel.MakeObject<NonExclusiveDeviationAlarmController>("NonExclusiveDeviationAlarm2");
        nonExclusiveDeviationAlarm2.InputValueVariable.SetDynamicLink(variable1);
        nonExclusiveDeviationAlarm2.Setpoint = 10;
        nonExclusiveDeviationAlarm2.LowLowLimit = -2;
        nonExclusiveDeviationAlarm2.AutoAcknowledge = true;
        nonExclusiveDeviationAlarm2.AutoConfirm = true;
        alarmsFolder.Add(nonExclusiveDeviationAlarm2);

        var spinBox1 = InformationModel.MakeObject<SpinBox>("SpinBox1");
        spinBox1.ValueVariable.SetDynamicLink(variable1);
        spinBox1.LeftMargin = 580;
        spinBox1.TopMargin = 280;
        mainWindow.Add(spinBox1);

        Log.Info("Project Generation Complete");
    }
}
