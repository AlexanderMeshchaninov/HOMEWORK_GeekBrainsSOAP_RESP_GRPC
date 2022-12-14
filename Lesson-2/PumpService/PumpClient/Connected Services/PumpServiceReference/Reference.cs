//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PumpClient.PumpServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StatisticsService", Namespace="http://schemas.datacontract.org/2004/07/PumpService.Services")]
    [System.SerializableAttribute()]
    public partial class StatisticsService : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AllCountsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ErrorCountsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SuccessCountsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AllCounts {
            get {
                return this.AllCountsField;
            }
            set {
                if ((this.AllCountsField.Equals(value) != true)) {
                    this.AllCountsField = value;
                    this.RaisePropertyChanged("AllCounts");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ErrorCounts {
            get {
                return this.ErrorCountsField;
            }
            set {
                if ((this.ErrorCountsField.Equals(value) != true)) {
                    this.ErrorCountsField = value;
                    this.RaisePropertyChanged("ErrorCounts");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SuccessCounts {
            get {
                return this.SuccessCountsField;
            }
            set {
                if ((this.SuccessCountsField.Equals(value) != true)) {
                    this.SuccessCountsField = value;
                    this.RaisePropertyChanged("SuccessCounts");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Microsoft.ServiceModel.Samples", ConfigurationName="PumpServiceReference.IPumpService", CallbackContract=typeof(PumpClient.PumpServiceReference.IPumpServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IPumpService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IPumpService/RunScript", ReplyAction="http://Microsoft.ServiceModel.Samples/IPumpService/RunScriptResponse")]
        void RunScript();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IPumpService/RunScript", ReplyAction="http://Microsoft.ServiceModel.Samples/IPumpService/RunScriptResponse")]
        System.Threading.Tasks.Task RunScriptAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScript", ReplyAction="http://Microsoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScriptResponse" +
            "")]
        void UpdateAndCompileScript(string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScript", ReplyAction="http://Microsoft.ServiceModel.Samples/IPumpService/UpdateAndCompileScriptResponse" +
            "")]
        System.Threading.Tasks.Task UpdateAndCompileScriptAsync(string fileName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPumpServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Microsoft.ServiceModel.Samples/IPumpService/UpdateStatistics", ReplyAction="http://Microsoft.ServiceModel.Samples/IPumpService/UpdateStatisticsResponse")]
        void UpdateStatistics(PumpClient.PumpServiceReference.StatisticsService statistics);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPumpServiceChannel : PumpClient.PumpServiceReference.IPumpService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PumpServiceClient : System.ServiceModel.DuplexClientBase<PumpClient.PumpServiceReference.IPumpService>, PumpClient.PumpServiceReference.IPumpService {
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void RunScript() {
            base.Channel.RunScript();
        }
        
        public System.Threading.Tasks.Task RunScriptAsync() {
            return base.Channel.RunScriptAsync();
        }
        
        public void UpdateAndCompileScript(string fileName) {
            base.Channel.UpdateAndCompileScript(fileName);
        }
        
        public System.Threading.Tasks.Task UpdateAndCompileScriptAsync(string fileName) {
            return base.Channel.UpdateAndCompileScriptAsync(fileName);
        }
    }
}
