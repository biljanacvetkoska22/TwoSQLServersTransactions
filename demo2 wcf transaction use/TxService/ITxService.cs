using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TxService
{
    [ServiceContract]
    public interface ITxService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        bool TxOperation();

    }


    
}
