using System;
using Microsoft.Web.Services3.Security.Tokens;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Xml;

namespace OnVifManager
{
    public class PasswordDigestBehavior : IEndpointBehavior
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordDigestBehavior(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        #region IEndpointBehavior Members
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new PasswordDigestMessageInspector(this.Username, this.Password));
        }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {

        }
        public void Validate(ServiceEndpoint endpoint)
        {

        }
        #endregion
    }
    public class PasswordDigestMessageInspector : IClientMessageInspector
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordDigestMessageInspector(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            UsernameToken token = new UsernameToken(this.Username, this.Password, PasswordOption.SendHashed);
            // Serialize the token to XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement securityToken = token.GetXml(xmlDoc);
            // find nonce and add EncodingType attribute for BSP compliance
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            XmlNodeList nonces = securityToken.SelectNodes("//wsse:Nonce", nsMgr);
            XmlAttribute encodingAttr = xmlDoc.CreateAttribute("EncodingType");
            encodingAttr.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
            if (nonces.Count > 0)
            {
                nonces[0].Attributes.Append(encodingAttr);
            }
            MessageHeader securityHeader = MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", securityToken, false);
            request.Headers.Add(securityHeader);
            // complete
            return Convert.DBNull;
        }
    }

}
