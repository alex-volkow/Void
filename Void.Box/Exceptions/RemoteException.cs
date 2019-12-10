using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public class RemoteException : Exception
    {
        public IError Error { get; }

        public override string Message => this.Error.Message;


        public RemoteException(IError error) {
            this.Error = error ?? throw new ArgumentNullException(nameof(error));
        }


        public override string ToString() {
            return this.Error.ToString();
        }
    }
}
