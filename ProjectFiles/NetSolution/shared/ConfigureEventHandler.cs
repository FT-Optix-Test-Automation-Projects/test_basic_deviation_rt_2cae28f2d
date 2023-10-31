using FTOptix.Core;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using OpcUa = UAManagedCore.OpcUa;
using System.Collections.Generic;
using System;
using System.Linq;
using UAManagedCore;

namespace NetlogicTesting.shared
{
    public class ConfigureEventHandler : BaseNetLogic
    {

        public FTOptix.CoreBase.EventHandler MakeEventHandler(
                IUANode parentNode,                                  // The parent node to which the event handler is to be added
                NodeId listenEventTypeId,                            // The NodeID of the event to be listened
                IUAObject callingObject,                             // The object on which the method is to be executed <IUAObject>
                string methodName,                                   // The name of the method to be called
                List<Tuple<string, NodeId, object>> arguments = null // List of input arguments (name, data type NodeID, value)
            )
        {
            // Create event handler object
            var eventHandler = InformationModel.MakeObject<FTOptix.CoreBase.EventHandler>("EventHandler");
            parentNode.Add(eventHandler);

            // Set the ListenEventType variable value to the Node ID of the event to be listened
            eventHandler.ListenEventType = listenEventTypeId;

            // Create method container
            var methodContainer = InformationModel.MakeObject("MethodContainer1");
            eventHandler.MethodsToCall.Add(methodContainer);

            // Create the ObjectPointer variable and set its value to the object on which the method is to be executed
            var objectPointerVariable = InformationModel.MakeVariable<NodePointer>("ObjectPointer", OpcUa.DataTypes.NodeId);
            objectPointerVariable.Value = callingObject.NodeId;
            methodContainer.Add(objectPointerVariable);

            // Create the Method variable and set its value to the name of the method to be called
            var methodNameVariable = InformationModel.MakeVariable("Method", OpcUa.DataTypes.LocaleId);
            methodNameVariable.Value = methodName;
            methodContainer.Add(methodNameVariable);

            if (arguments != null)
                CreateInputArguments(methodContainer, arguments);

            return eventHandler;
        }

        public FTOptix.CoreBase.EventHandler MakeEventHandler(
                IUANode parentNode,                                  // The parent node to which the event handler is to be added
                NodeId listenEventTypeId,                            // The NodeID of the event to be listened
                string pathToCallingObject,                          // The object on which the method is to be executed <String>
                string methodName,                                   // The name of the method to be called
                List<Tuple<string, NodeId, object>> arguments = null // List of input arguments (name, data type NodeID, value)
            )
        {
            // Create event handler object
            var eventHandler = InformationModel.MakeObject<FTOptix.CoreBase.EventHandler>("EventHandler");
            parentNode.Add(eventHandler);
            // Set the ListenEventType variable value to the Node ID of the event to be listened
            eventHandler.ListenEventType = listenEventTypeId;
            // Create method container
            var methodContainer = InformationModel.MakeObject("MethodContainer1");
            eventHandler.MethodsToCall.Add(methodContainer);
            // Create the ObjectPointer variable and set its value to the object on which the method is to be executed
            var objectPointerVariable = InformationModel.MakeVariable<NodePointer>("ObjectPointer", OpcUa.DataTypes.NodeId);
            methodContainer.Add(objectPointerVariable);
            // Create the Method variable and set its value to the name of the method to be called
            var methodNameVariable = InformationModel.MakeVariable("Method", OpcUa.DataTypes.LocaleId);
            methodNameVariable.Value = methodName;
            methodContainer.Add(methodNameVariable);
            objectPointerVariable.SetDynamicLink(methodNameVariable);
            var objectPointerVariableDynamicLink = objectPointerVariable.GetVariable("DynamicLink");
            objectPointerVariableDynamicLink.Value = pathToCallingObject;
            if (arguments != null)
                CreateInputArguments(methodContainer, arguments);
            return eventHandler;
        }

        private void CreateInputArguments(
            IUANode methodContainer,
            List<Tuple<string, NodeId, object>> arguments)
        {
            IUAObject inputArguments = InformationModel.MakeObject("InputArguments");
            methodContainer.Add(inputArguments);

            foreach (var arg in arguments)
            {
                var argumentVariable = inputArguments.Context.NodeFactory.MakeVariable(
                    NodeId.Random(inputArguments.NodeId.NamespaceIndex),
                    arg.Item1,
                    arg.Item2,
                    OpcUa.VariableTypes.BaseDataVariableType,
                    false,
                    arg.Item3);

                inputArguments.Add(argumentVariable);
            }
        }

    }

}
