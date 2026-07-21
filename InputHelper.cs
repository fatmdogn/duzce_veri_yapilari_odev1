using System.Globalization;

namespace data_odev
{
    // Kullanicidan guvenli sekilde sayi okumak icin yardimci metotlar.
    // Amac: yanlis giris yapilsa bile program PATLAMASIN, tekrar sorsun.
    public static class InputHelper
    {
        // Belirtilen [min, max] araliginda bir tam sayi okur.
        // - Sayi disi giris, bos giris, ondalikli giris -> uyarir, tekrar sorar.
        // - Cok buyuk sayi (int tasmasi) -> TryParse yakalar, tekrar sorar.
        // - Aralik disi -> uyarir, tekrar sorar.
        public static int IntAlAralikta(string mesaj, int min, int max)
        {
            while (true)
            {
                Console.Write(mesaj);
                string? giris = Console.ReadLine();

                // Bos veya sadece bosluk kontrolu.
                if (string.IsNullOrWhiteSpace(giris))
                {
                    Console.WriteLine("  ! Bos giris yaptiniz. Lutfen bir sayi girin.\n");
                    continue;
                }

                // Bastaki/sondaki bosluklari temizle.
                giris = giris.Trim();

                // InvariantCulture ile parse ediyoruz ki her makinede ayni davransin
                // (ornegin virgul/nokta ayrimi sorun cikarmasin).
                bool basarili = int.TryParse(
                    giris,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out int sonuc);

                if (!basarili)
                {
                    Console.WriteLine("  ! Gecersiz giris. Sadece tam sayi girebilirsiniz (ornek: 5).\n");
                    continue;
                }

                // Aralik kontrolu.
                if (sonuc < min || sonuc > max)
                {
                    Console.WriteLine($"  ! Lutfen {min} ile {max} arasinda bir deger girin.\n");
                    continue;
                }

                // Buraya geldiyse giris gecerli.
                return sonuc;
            }
        }
    }
}
