using BCMWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb
{
    public class TokenModel : GenericModel
    {

        private string _accessToken;

        [JsonProperty(PropertyName = "access_token")]
        public string access_token
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        private string _tokentype;

        [JsonProperty(PropertyName = "token_type")]
        public string token_type
        {
            get { return _tokentype; }
            set { _tokentype = value; }
        }

        private int _expiresIn;

        [JsonProperty(PropertyName = "expires_in")]
        public int expires_in
        {
            get { return _expiresIn; }
            set { _expiresIn = value; }
        }

    }
}
