using System.Collections.Generic;

namespace GATest.Generic
{
    public class Fragment {
        public int Start {get; set;}
        public int End {get; set;}

        public List<Monome> Monomes {get; set;}

        public Fragment(int start, int end) {
            Start = start;
            End = end;
        }

        public bool Overlap(Fragment fragment) {
            return ContainsPosition(fragment.Start) || ContainsPosition(fragment.End);
        }

        public bool ContainsPosition(int position) {
            return position >= Start && position <= End;
        }

        public override string ToString()  {
            return $"{Start} -> {End}";
        }
    }
}
