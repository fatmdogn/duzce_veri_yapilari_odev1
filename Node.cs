namespace data_odev
{
    // Cift yonlu bagli listenin tek bir dugumu (node).
    // Her node hem kendinden onceki (Prev) hem de sonraki (Next) node'u tutar.
    public class Node
    {
        // Node'un icinde tuttugu sayi degeri.
        public int Value { get; set; }

        // Bir onceki node. Eger bu node head (bas) ise Prev = null olur.
        public Node? Prev { get; set; }

        // Bir sonraki node. Eger bu node tail (son) ise Next = null olur.
        public Node? Next { get; set; }

        public Node(int value)
        {
            Value = value;
            Prev = null;
            Next = null;
        }
    }
}
