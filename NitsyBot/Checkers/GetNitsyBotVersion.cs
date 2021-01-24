using System.Diagnostics;
using System.Reflection;
namespace NitsyBot.Checkers
{
    public class GetNitsyBotVersion
    {
        public string NitsyVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
    }
}