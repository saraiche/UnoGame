﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Logic.DataServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DTOCredentials", Namespace="http://schemas.datacontract.org/2004/07/Services")]
    [System.SerializableAttribute()]
    public partial class DTOCredentials : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
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
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataServiceReference.IDataService")]
    public interface IDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddImages", ReplyAction="http://tempuri.org/IDataService/AddImagesResponse")]
        bool AddImages();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddImages", ReplyAction="http://tempuri.org/IDataService/AddImagesResponse")]
        System.Threading.Tasks.Task<bool> AddImagesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddCredentials", ReplyAction="http://tempuri.org/IDataService/AddCredentialsResponse")]
        bool AddCredentials(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddCredentials", ReplyAction="http://tempuri.org/IDataService/AddCredentialsResponse")]
        System.Threading.Tasks.Task<bool> AddCredentialsAsync(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/isUser", ReplyAction="http://tempuri.org/IDataService/isUserResponse")]
        bool isUser(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/isUser", ReplyAction="http://tempuri.org/IDataService/isUserResponse")]
        System.Threading.Tasks.Task<bool> isUserAsync(Logic.DataServiceReference.DTOCredentials credentials);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataServiceChannel : Logic.DataServiceReference.IDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataServiceClient : System.ServiceModel.ClientBase<Logic.DataServiceReference.IDataService>, Logic.DataServiceReference.IDataService {
        
        public DataServiceClient() {
        }
        
        public DataServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool AddImages() {
            return base.Channel.AddImages();
        }
        
        public System.Threading.Tasks.Task<bool> AddImagesAsync() {
            return base.Channel.AddImagesAsync();
        }
        
        public bool AddCredentials(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.AddCredentials(credentials);
        }
        
        public System.Threading.Tasks.Task<bool> AddCredentialsAsync(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.AddCredentialsAsync(credentials);
        }
        
        public bool isUser(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.isUser(credentials);
        }
        
        public System.Threading.Tasks.Task<bool> isUserAsync(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.isUserAsync(credentials);
        }
    }
}
