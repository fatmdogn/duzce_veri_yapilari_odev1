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
    }
}
