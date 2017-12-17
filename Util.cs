using UnityEngine;

namespace ExtraConcentratedJuice.ExtraDuel
{
    public static class Util
    {
        public static ExtraDuelConfig getConfig() => ExtraDuel.instance.Configuration.Instance;

        public static string Translate(string TranslationKey, params object[] Placeholders) =>
            ExtraDuel.instance.Translations.Instance.Translate(TranslationKey, Placeholders);

        public static Vector2 Vector2(Vector3 p) => new Vector2(p.x, p.z);
    }
}
