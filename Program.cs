namespace data_odev
{
    public class Program
    {
        // Kurallar (odev gereksinimleri) sabit olarak tutuluyor.
        private const int MIN_NODE = 3;
        private const int MAX_NODE = 20;
        private const int MIN_SAYI = -100;
        private const int MAX_SAYI = 100;

        // Rastgele sayi uretici. Tek bir Random nesnesi kullaniyoruz;
        // her cagirmada -100 ile +100 arasinda (ikisi de dahil) sayi verir.
        private static readonly Random rastgele = new Random();

        // Odev gereksinimi: rastgele uretim dogrudan dongude toplu calismasin.
        // Bu metot sadece TEK bir sayi uretir ve her node icin ayri ayri tetiklenir.
        private static int RastgeleSayiUret()
        {
            // Next(min, max) ust siniri dahil etmez, o yuzden MAX_SAYI + 1 veriyoruz.
            return rastgele.Next(MIN_SAYI, MAX_SAYI + 1);
        }

        public static void Main()
        {
            YazdirBaslik();

            // 1) Kullanicidan kac node olusturulacagini al (3-20 arasi, dogrulamali).
            int nodeSayisi = InputHelper.IntAlAralikta(
                $"Kac node'luk bir liste olusturmak istersiniz? ({MIN_NODE}-{MAX_NODE}): ",
                MIN_NODE,
                MAX_NODE);

            Console.WriteLine($"\n{nodeSayisi} adet node olusturulacak. Sayilar rastgele uretiliyor...\n");

            var liste = new DoublyLinkedList();

            // Sayilarin uretildigi sirayi ayrica tutuyoruz. Bagli liste sirali
            // oldugu icin uretim sirasini kaybediyoruz; sonucta gostermek icin
            // ayri bir listede saklamamiz gerekiyor.
            var uretimSirasi = new List<int>();

            // 2) Node sayisi kadar don. Her adimda:
            //    once TEK bir rastgele sayi uret, sonra o sayi icin node olusturup
            //    sirali sekilde ekle, sonra listeyi goster. Bir sonraki sayi ancak
            //    bu adim bitince uretilir.
            for (int i = 1; i <= nodeSayisi; i++)
            {
                int sayi = RastgeleSayiUret();
                uretimSirasi.Add(sayi);

                Console.WriteLine($"--- Adim {i}/{nodeSayisi} ---");
                Console.WriteLine($"Uretilen sayi : {sayi}");

                liste.SortedInsert(sayi);

                Console.WriteLine($"Liste (ileri) : {liste.ToStringForward()}");
                Console.WriteLine();
            }

            // 3) Sonuclari goster: cift yonlulugu ispatlamak icin ileri ve geri yaz.
            YazdirSonuc(liste, uretimSirasi);
        }

        private static void YazdirBaslik()
        {
            Console.WriteLine("========================================================");
            Console.WriteLine("   CIFT YONLU BAGLI LISTE - SIRALI EKLEME UYGULAMASI");
            Console.WriteLine("========================================================");
            Console.WriteLine($"- Olusturulacak node sayisi: {MIN_NODE} ile {MAX_NODE} arasinda olabilir.");
            Console.WriteLine($"- Uretilecek sayilar: {MIN_SAYI} ile {MAX_SAYI} arasinda (rastgele).");
            Console.WriteLine("- Sayilar listeye her zaman kucukten buyuge sirali eklenir.");
            Console.WriteLine("========================================================\n");
        }

        private static void YazdirSonuc(DoublyLinkedList liste, List<int> uretimSirasi)
        {
            Console.WriteLine("========================================================");
            Console.WriteLine("   SONUC");
            Console.WriteLine("========================================================");
            Console.WriteLine($"Toplam node sayisi : {liste.Count}");
            Console.WriteLine($"Uretim sirasi      : [ {string.Join(", ", uretimSirasi)} ]");
            Console.WriteLine($"Ileri  (Head->Tail): {liste.ToStringForward()}");
            Console.WriteLine($"Geri   (Tail->Head): {liste.ToStringBackward()}");
            Console.WriteLine($"Head degeri        : {(liste.Head != null ? liste.Head.Value.ToString() : "-")}");
            Console.WriteLine($"Tail degeri        : {(liste.Tail != null ? liste.Tail.Value.ToString() : "-")}");
            Console.WriteLine("========================================================");
        }
    }
}
