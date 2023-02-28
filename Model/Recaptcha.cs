using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Model
{
    [DataContract]
    public class RecaptchaResult
    {
        
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        
        [DataMember(Name = "error-codes")]
        public string[] ErrorCodes { get; set; }
    }

}
