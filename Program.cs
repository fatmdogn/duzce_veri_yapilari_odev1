namespace data_odev
{
    public class Program
    {
        // Kurallar (odev gereksinimleri) sabit olarak tutuluyor.
        private const int NODE_SAYISI = 25;     // Odev: tam 25 sayi eklenecek.
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
           

            var liste = new DoublyLinkedList();

            // Sayilarin uretildigi sirayi ayrica tutuyoruz. Bagli liste sirali
            // oldugu icin uretim sirasini kaybediyoruz; sonucta gostermek icin
            // ayri bir listede saklamamiz gerekiyor.
            var uretimSirasi = new List<int>();

            // ============ 1. BOLUM: EKLEME METODU ============
            // 25 kez don. Her adimda once TEK bir rastgele sayi uret, sonra
            // o sayiyi sirali (kucukten buyuge) sekilde ekle, sonra listeyi goster.
            // Bir sonraki sayi ancak bu adim bitince uretilir.
            for (int i = 1; i <= NODE_SAYISI; i++)
            {
                int sayi = RastgeleSayiUret();
                uretimSirasi.Add(sayi);

                Console.WriteLine($"--- Adim {i}/{NODE_SAYISI} ---");
                Console.WriteLine($"Uretilen sayi : {sayi}");

                liste.SortedInsert(sayi);

                Console.WriteLine($"Liste (ileri) : {liste.ToStringForward()}");
                Console.WriteLine();
            }

            YazdirEklemeSonucu(liste, uretimSirasi);

            // ============ 2. BOLUM: KES / YAPISTIR METODU ============
            KesYapistir(liste);

            // Program bitince pencerenin hemen kapanmamasi icin bekle.
            Console.WriteLine("\nCikmak icin bir tusa basin...");
            Console.ReadKey();
        }

        // Kullanicidan bir indis alip o node'u koparir, ardindan yeni bir hedef
        // indis alip node'u iki komsunun arasina yerlestirir.
        private static void KesYapistir(DoublyLinkedList liste)
        {
            Console.WriteLine();
            Console.WriteLine("========================================================");
            Console.WriteLine("   KES / YAPISTIR METODU");
            Console.WriteLine("========================================================");
            Console.WriteLine("Bir sayiyi bulundugu indisten cikarip, listede baska");
            Console.WriteLine("iki sayinin arasina (yeni bir indise) tasiyacagiz.");
            Console.WriteLine("Not: Bu islem siralamayi bozabilir; amac tasima islemidir.\n");

            // Indisli gosterim: kullanici hangi indisi sececegini gorsun.
            Console.WriteLine($"Mevcut liste (indisli):\n  {liste.ToStringWithIndices()}\n");

            // 1) Cikarilacak node'un indisini al (0 .. Count-1).
            int cikarIndis = InputHelper.IntAlAralikta(
                $"Cikarilacak node'un indisi (0-{liste.Count - 1}): ",
                0,
                liste.Count - 1);

            // Node'u kopar (kes). Cikarilan degeri saklıyoruz ki tekrar ekleyelim.
            Node kopan = liste.RemoveAt(cikarIndis);
            Console.WriteLine($"\n-> Indis {cikarIndis}'teki sayi cikarildi: {kopan.Value}");
            Console.WriteLine($"Cikarma sonrasi liste:\n  {liste.ToStringWithIndices()}\n");

            // 2) Hedef indisi al. Artik listede Count node var; ekleme icin
            //    gecerli aralik 0 .. Count (Count = en sona ekle).
            int hedefIndis = InputHelper.IntAlAralikta(
                $"Sayinin yerlestirilecegi yeni indis (0-{liste.Count}): ",
                0,
                liste.Count);

            // Kopan node'u hedef indise yapistir.
            liste.InsertNodeAt(hedefIndis, kopan);
            Console.WriteLine($"\n-> {kopan.Value} sayisi {hedefIndis}. indise yerlestirildi.\n");

            YazdirKesYapistirSonucu(liste);
        }

      

        private static void YazdirEklemeSonucu(DoublyLinkedList liste, List<int> uretimSirasi)
        {
            Console.WriteLine("========================================================");
            Console.WriteLine("   EKLEME SONUCU");
            Console.WriteLine("========================================================");
            Console.WriteLine($"Toplam node sayisi : {liste.Count}");
            Console.WriteLine($"Uretim sirasi      : [ {string.Join(", ", uretimSirasi)} ]");
            Console.WriteLine($"Ileri  (Head->Tail): {liste.ToStringForward()}");
            Console.WriteLine($"Geri   (Tail->Head): {liste.ToStringBackward()}");
            Console.WriteLine($"Head degeri        : {(liste.Head != null ? liste.Head.Value.ToString() : "-")}");
            Console.WriteLine($"Tail degeri        : {(liste.Tail != null ? liste.Tail.Value.ToString() : "-")}");
            Console.WriteLine("========================================================");
        }

        private static void YazdirKesYapistirSonucu(DoublyLinkedList liste)
        {
            Console.WriteLine("========================================================");
            Console.WriteLine("   KES / YAPISTIR SONUCU");
            Console.WriteLine("========================================================");
            Console.WriteLine($"Toplam node sayisi : {liste.Count}");
            Console.WriteLine($"Indisli liste      : {liste.ToStringWithIndices()}");
            Console.WriteLine($"Ileri  (Head->Tail): {liste.ToStringForward()}");
            Console.WriteLine($"Geri   (Tail->Head): {liste.ToStringBackward()}");
            Console.WriteLine($"Head degeri        : {(liste.Head != null ? liste.Head.Value.ToString() : "-")}");
            Console.WriteLine($"Tail degeri        : {(liste.Tail != null ? liste.Tail.Value.ToString() : "-")}");
            Console.WriteLine("========================================================");
        }
    }
}
