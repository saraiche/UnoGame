﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace unoProyect.DataServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataServiceReference.IDataService")]
    public interface IDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddImages", ReplyAction="http://tempuri.org/IDataService/AddImagesResponse")]
        bool AddImages();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddImages", ReplyAction="http://tempuri.org/IDataService/AddImagesResponse")]
        System.Threading.Tasks.Task<bool> AddImagesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddCredentials", ReplyAction="http://tempuri.org/IDataService/AddCredentialsResponse")]
        int AddCredentials(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddCredentials", ReplyAction="http://tempuri.org/IDataService/AddCredentialsResponse")]
        System.Threading.Tasks.Task<int> AddCredentialsAsync(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/IsUser", ReplyAction="http://tempuri.org/IDataService/IsUserResponse")]
        bool IsUser(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/IsUser", ReplyAction="http://tempuri.org/IDataService/IsUserResponse")]
        System.Threading.Tasks.Task<bool> IsUserAsync(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/SearchUser", ReplyAction="http://tempuri.org/IDataService/SearchUserResponse")]
        bool SearchUser(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/SearchUser", ReplyAction="http://tempuri.org/IDataService/SearchUserResponse")]
        System.Threading.Tasks.Task<bool> SearchUserAsync(Logic.DataServiceReference.DTOCredentials credentials);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/SendMail", ReplyAction="http://tempuri.org/IDataService/SendMailResponse")]
        bool SendMail(string to, string emailSubject, string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/SendMail", ReplyAction="http://tempuri.org/IDataService/SendMailResponse")]
        System.Threading.Tasks.Task<bool> SendMailAsync(string to, string emailSubject, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataServiceChannel : unoProyect.DataServiceReference.IDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataServiceClient : System.ServiceModel.ClientBase<unoProyect.DataServiceReference.IDataService>, unoProyect.DataServiceReference.IDataService {
        
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
        
        public int AddCredentials(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.AddCredentials(credentials);
        }
        
        public System.Threading.Tasks.Task<int> AddCredentialsAsync(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.AddCredentialsAsync(credentials);
        }
        
        public bool IsUser(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.IsUser(credentials);
        }
        
        public System.Threading.Tasks.Task<bool> IsUserAsync(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.IsUserAsync(credentials);
        }
        
        public bool SearchUser(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.SearchUser(credentials);
        }
        
        public System.Threading.Tasks.Task<bool> SearchUserAsync(Logic.DataServiceReference.DTOCredentials credentials) {
            return base.Channel.SearchUserAsync(credentials);
        }
        
        public bool SendMail(string to, string emailSubject, string message) {
            return base.Channel.SendMail(to, emailSubject, message);
        }
        
        public System.Threading.Tasks.Task<bool> SendMailAsync(string to, string emailSubject, string message) {
            return base.Channel.SendMailAsync(to, emailSubject, message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataServiceReference.IChatService", CallbackContract=typeof(unoProyect.DataServiceReference.IChatServiceCallback))]
    public interface IChatService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Join", ReplyAction="http://tempuri.org/IChatService/JoinResponse")]
        int Join(string username, int code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Join", ReplyAction="http://tempuri.org/IChatService/JoinResponse")]
        System.Threading.Tasks.Task<int> JoinAsync(string username, int code);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/SendMessage")]
        void SendMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/SendMessage")]
        System.Threading.Tasks.Task SendMessageAsync(string message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/GetInvitationCode", ReplyAction="http://tempuri.org/IChatService/GetInvitationCodeResponse")]
        int GetInvitationCode();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/GetInvitationCode", ReplyAction="http://tempuri.org/IChatService/GetInvitationCodeResponse")]
        System.Threading.Tasks.Task<int> GetInvitationCodeAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/RecieveMessage")]
        void RecieveMessage(string user, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceChannel : unoProyect.DataServiceReference.IChatService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatServiceClient : System.ServiceModel.DuplexClientBase<unoProyect.DataServiceReference.IChatService>, unoProyect.DataServiceReference.IChatService {
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public int Join(string username, int code) {
            return base.Channel.Join(username, code);
        }
        
        public System.Threading.Tasks.Task<int> JoinAsync(string username, int code) {
            return base.Channel.JoinAsync(username, code);
        }
        
        public void SendMessage(string message) {
            base.Channel.SendMessage(message);
        }
        
        public System.Threading.Tasks.Task SendMessageAsync(string message) {
            return base.Channel.SendMessageAsync(message);
        }
        
        public int GetInvitationCode() {
            return base.Channel.GetInvitationCode();
        }
        
        public System.Threading.Tasks.Task<int> GetInvitationCodeAsync() {
            return base.Channel.GetInvitationCodeAsync();
        }
    }
}