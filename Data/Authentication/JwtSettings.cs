﻿namespace ufoShopBack.Data.Authentication
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Lifetime {  get; set; }
    }
}
