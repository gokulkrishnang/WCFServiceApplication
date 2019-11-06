using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
//additional Namespace
using System.Runtime.Serialization;

namespace FSE.PO.SVCAPP
{
    [DataContract]
    public class ExceptionMessage
    {
        private string infoExceptionMessage;

        //Parameter Constructor 
        public ExceptionMessage(string Message)
        {
            this.infoExceptionMessage = Message;
        }

        public string errorMessageOfAction
        {
            get { return this.infoExceptionMessage; }
            set { this.infoExceptionMessage = value; }
        }
    }
}