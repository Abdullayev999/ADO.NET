using Academy.Base.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Base.Messages
{
    public class LoginUserMessage : Messenger
    {
        public Student Student { get; set; }
    }
}
