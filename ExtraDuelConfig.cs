using Rocket.API;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public class ExtraDuelConfig : IRocketPluginConfiguration
    {
        public int updateTime;
        public void LoadDefaults()
        {
            updateTime = 1;
        }
    }
}
