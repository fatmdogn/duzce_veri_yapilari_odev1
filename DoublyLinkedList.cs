namespace data_odev
{
    // Sirali (kucukten buyuge) tutulan cift yonlu bagli liste.
    // Yeni gelen her sayi, listedeki dogru yerine "araya sokularak" eklenir.
    public class DoublyLinkedList
    {
        // Listenin bas (en kucuk) ve son (en buyuk) dugumleri.
        public Node? Head { get; private set; }
        public Node? Tail { get; private set; }

        // Listedeki node sayisi.
        public int Count { get; private set; }

        // Yeni bir sayiyi sirali sekilde ekler.
        // Ekleme yaparken 4 durum var: (1) liste bos, (2) basa ekleme,
        // (3) sona ekleme, (4) araya ekleme. Hepsinde Prev/Next baglarini
        // tek tek elle kuruyoruz ("kes/yapistir" mantigi).
        public void SortedInsert(int value)
        {
            Node yeni = new Node(value);

            // DURUM 1: Liste bos. Ilk node hem head hem tail olur.
            if (Head == null)
            {
                Head = yeni;
                Tail = yeni;
                Count++;
                return;
            }

            // DURUM 2: Yeni deger head'den kucuk (veya esit) -> en basa ekle.
            // Duplicate'lerde esitlik durumunda basa alarak da sirayi bozmuyoruz.
            if (value < Head.Value)
            {
                yeni.Next = Head;   // yeninin next'i eski head
                Head.Prev = yeni;   // eski head'in prev'i yeni
                Head = yeni;        // artik yeni node head
                Count++;
                return;
            }

            // Head'den baslayarak, yeni degerin girecegi yeri buluyoruz.
            // "current", yeni node'un ONUNE gelecegi node olacak sekilde ilerliyoruz.
            Node current = Head;
            while (current.Next != null && current.Next.Value < value)
            {
                current = current.Next;
            }

            // Simdi yeni node, current ile current.Next arasina girecek.

            // DURUM 3: Sona ekleme. current zaten son node ise (Next == null),
            // yeni node yeni tail olur.
            if (current.Next == null)
            {
                current.Next = yeni;
                yeni.Prev = current;
                Tail = yeni;
                Count++;
                return;
            }

            // DURUM 4: Araya ekleme (kes/yapistir).
            // current  <->  yeni  <->  sonraki
            Node sonraki = current.Next;

            yeni.Prev = current;    // yeninin previ current
            yeni.Next = sonraki;    // yeninin nexti eski sonraki
            current.Next = yeni;    // current artik yeniyi gosteriyor
            sonraki.Prev = yeni;    // sonrakinin previ artik yeni

            Count++;
        }

        // Listeyi bastan sona (Head -> Tail) yazdirir.
        // Ornek cikti: [ 3 <-> 5 <-> 7 ]
        public string ToStringForward()
        {
            if (Head == null)
            {
                return "[ bos ]";
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("[ ");
            Node? current = Head;
            while (current != null)
            {
                sb.Append(current.Value);
                if (current.Next != null)
                {
                    sb.Append(" <-> ");
                }
                current = current.Next;
            }
            sb.Append(" ]");
            return sb.ToString();
        }

        // Listeyi sondan basa (Tail -> Head) yazdirir.
        // Bu metot, listenin gercekten cift yonlu oldugunu ispatlar:
        // geri yonde de gezebiliyoruz.
        public string ToStringBackward()
        {
            if (Tail == null)
            {
                return "[ bos ]";
            }

            var sb = new System.Text.StringBuilder();
            sb.Append("[ ");
            Node? current = Tail;
            while (current != null)
            {
                sb.Append(current.Value);
                if (current.Prev != null)
                {
                    sb.Append(" <-> ");
                }
                current = current.Prev;
            }
            sb.Append(" ]");
            return sb.ToString();
        }

        // ================== KES / YAPISTIR (CUT / PASTE) ==================
        // Asagidaki metotlar, bir node'u bulundugu yerden koparip (kes)
        // listede baska bir indise (iki komsunun arasina) tasimak icin kullanilir.
        // Bu islem sirayi bilerek bozabilir; amac pointer (Prev/Next) tasima
        // mantigini gostermektir.

        // 0'dan baslayan index'teki node'u dondurur. Gecersiz index -> null.
        public Node? NodeAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                return null;
            }

            // Head'den baslayip index adim ilerliyoruz.
            Node? current = Head;
            for (int i = 0; i < index && current != null; i++)
            {
                current = current.Next;
            }
            return current;
        }

        // Verilen index'teki node'u listeden koparir ve o node'u geri dondurur.
        // Koparilan node'un Prev/Next baglari temizlenir ki disarida "serbest"
        // bir dugum olarak tekrar eklenebilsin. Head/Tail/Count guncellenir.
        public Node RemoveAt(int index)
        {
            Node? hedef = NodeAt(index);
            if (hedef == null)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Gecersiz index: {index}. Liste 0..{Count - 1} araliginda.");
            }

            Node? onceki = hedef.Prev;
            Node? sonraki = hedef.Next;

            // Sol komsuyu sag komsuya bagla (hedefi atla).
            if (onceki != null)
            {
                onceki.Next = sonraki;
            }
            else
            {
                // Hedef head idi -> yeni head sonraki olur.
                Head = sonraki;
            }

            if (sonraki != null)
            {
                sonraki.Prev = onceki;
            }
            else
            {
                // Hedef tail idi -> yeni tail onceki olur.
                Tail = onceki;
            }

            // Koparilan node'u serbest birak.
            hedef.Prev = null;
            hedef.Next = null;
            Count--;

            return hedef;
        }

        // Serbest (koparilmis) bir node'u, sonucta 'index'e denk gelecek sekilde
        // araya sokar. index anlamı: 0 = yeni head, Count = yeni tail (sona ekle).
        // Kes/yapistir mantigi: sadece Prev/Next baglarini elle yeniden kuruyoruz.
        public void InsertNodeAt(int index, Node node)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Gecersiz index: {index}. Ekleme icin 0..{Count} araligi gecerli.");
            }

            // Guvenlik: gelen node serbest olmali.
            node.Prev = null;
            node.Next = null;

            // DURUM 1: Liste bos.
            if (Head == null)
            {
                Head = node;
                Tail = node;
                Count++;
                return;
            }

            // DURUM 2: Basa ekleme (index 0).
            if (index == 0)
            {
                node.Next = Head;
                Head.Prev = node;
                Head = node;
                Count++;
                return;
            }

            // DURUM 3: Sona ekleme (index == Count).
            if (index == Count)
            {
                node.Prev = Tail;
                Tail!.Next = node;
                Tail = node;
                Count++;
                return;
            }

            // DURUM 4: Araya ekleme. index'te bulunan node'un ONUNE giriyoruz;
            // yani node, (index-1) ile (index) arasinda yer alacak.
            Node sonraki = NodeAt(index)!;   // index < Count oldugu icin null degil
            Node onceki = sonraki.Prev!;     // index > 0 oldugu icin null degil

            node.Prev = onceki;
            node.Next = sonraki;
            onceki.Next = node;
            sonraki.Prev = node;
            Count++;
        }

        // Listeyi indisleriyle birlikte yazdirir. Kullanici hangi indisi
        // sececegini gorebilsin diye kes/yapistir adimlarinda kullaniyoruz.
        // Ornek: 0:33  1:40  2:44
        public string ToStringWithIndices()
        {
            if (Head == null)
            {
                return "[ bos ]";
            }

            var sb = new System.Text.StringBuilder();
            Node? current = Head;
            int i = 0;
            while (current != null)
            {
                sb.Append($"{i}:{current.Value}");
                if (current.Next != null)
                {
                    sb.Append("   ");
                }
                current = current.Next;
                i++;
            }
            return sb.ToString();
        }
    }
}
