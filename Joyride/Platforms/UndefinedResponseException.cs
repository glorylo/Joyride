using System;

namespace Joyride.Platforms
{
    public class UndefinedResponseException : Exception
    {
        public UndefinedResponseException(string s) : base(s) { }
        public UndefinedResponseException(string s, Exception inner) : base(s, inner) { }
    }
}