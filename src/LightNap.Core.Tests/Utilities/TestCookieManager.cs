using LightNap.Core.Interfaces;

namespace LightNap.Core.Tests
{
    internal class TestCookieManager : ICookieManager
    {
        private readonly Dictionary<string, (string Value, DateTime? Expires)> _cookies = [];

        public string? GetCookie(string key)
        {
            if (this._cookies.TryGetValue(key, out var cookie))
            {
                if (cookie.Expires == null || cookie.Expires > DateTime.Now)
                {
                    return cookie.Value;
                }
                else
                {
                    this._cookies.Remove(key);
                }
            }
            return null;
        }

        public void SetCookie(string key, string value, bool isPersistent, DateTime expires)
        {
            this._cookies[key] = (value, isPersistent ? expires : (DateTime?)null);
        }

        public void RemoveCookie(string key)
        {
            this._cookies.Remove(key);
        }
    }
}
