using System;
using System.Collections.Generic;
using System.Linq;

namespace BooleanEquation
{
    class Program
    {
        static void Main(string[] args)
        {


            var compute = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>() {
                    new OperaterNode { Name = "A", Value = false },
                    new OperaterNode { Name = "B", Value = true }
                }
            };
            var computeQ = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>() {
                    new OperaterNode { Name = "A", Value = false },
                    new OperaterNode { Name = "B", Value = true }
                }
            };
            var compute1 = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>() {
                    new OperaterNode { Name = "B", Value = false },
                                        new OperaterNode { Name = "A", Value = true },


                }
            };


            var compute2 = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>() {
                    new OperaterNode { Name = "A", Value = true },
                },

            };

            var OrCompute = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.Or,
                Operands = new List<OperaterNode>()
                {
                    compute,compute1,compute2
                }

            };

            var compute3 = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.Or,
                Operands = new List<OperaterNode>() {
                    new OperaterNode { Name = "B", Value = true },
                    new OperaterNode { Name = "B", Value = false },
                },

            };
            var compute4 = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>() {
                    new OperaterNode { Name = "A", Value = true },
                   compute3

                },

            };

            var OrCompute2 = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.Or,
                Operands = new List<OperaterNode>()
                {
                    compute,compute4
                }

            };


            var p = new Simplifier(OrCompute);
            while (p.Simplify()) ;


        }

        //public static void add(ref a)
        //{
        //    a = +2;
        //}
    }
    public class Contants
    {
        public const string Not = "¬";
        public const string OR = "OR";
    }

    public class OperaterNode : IComparable<OperaterNode>
    {
        public int CompareTo(OperaterNode other)
        {
            return this.Name.CompareTo(other.Name);
        }
        public int Layer { get; set; }
        public int MemberCount()
        {

            int sum = 0;
            if (Name != null) sum = 1;
            else
            {
                Layer = 1;
                bool added = false;
                foreach (OperaterNode n in Operands)
                {
                    sum += n.MemberCount();
                    if (!added)
                    {
                        added = true;
                        Layer++;
                    }

                }
            }
            return sum;
        }
        public enum Operater
        {
            And, Or
        }

        public string Name { get; set; }
        public bool Value { get; set; }

        public string Outputer(bool started = false)
        {
            if (Name == null)
            {

                if (OperaterType == (int)Operater.Or && started)
                {
                    return $" ({ZoomIn()}) ";
                }
                else return ZoomIn();
            }
            else return Value == true ? Name : Contants.Not + Name;
        }
        public int OperaterType { get; set; }
        public List<OperaterNode> Operands { get; set; }

        public string ZoomIn()
        {
            //Operands.Sort();
            string result = "";
            bool first = true;

            foreach (OperaterNode n in Operands)
            {
                if (OperaterType == (int)Operater.Or && !first)
                {
                    result += $" {Contants.OR} ";
                    first = true;
                }
                result += n.Outputer(true);

                first = false;

            }
            return result;

        }

        public OperaterNode()
        {

        }
    }

    public class Simplifier
    {
        public bool Changed { get; set; } = false;

        public OperaterNode OperaterNode { get; set; }
        // Only take place in Or Gate
        public bool Factorize()
        {
            bool changed = false;
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {

                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i].OperaterType == (int)OperaterNode.Operater.And && OperaterNode.Operands[i].Operands != null)
                    { //Basic node
                        for (int j = OperaterNode.Operands[i].Operands.Count - 1; j >= 0; j--)
                        {
                            //Next Layer
                            for (int z = i - 1; z >= 0; z--)
                            {
                                for (int y = OperaterNode.Operands[z].Operands.Count - 1; y >= 0; y--)
                                {

                                    if (OperaterNode.Operands[i].Operands[j].Name == OperaterNode.Operands[z].Operands[y].Name &&
                                        OperaterNode.Operands[i].Operands[j].Value == OperaterNode.Operands[z].Operands[y].Value && OperaterNode.Operands[i].Operands[j].Name != "1")
                                    {
                                        var factor = new OperaterNode()
                                        {
                                            Name = OperaterNode.Operands[i].Operands[j].Name,
                                            Value = OperaterNode.Operands[i].Operands[j].Value,
                                            Operands = OperaterNode.Operands[i].Operands[j].Operands,
                                            OperaterType = OperaterNode.Operands[i].Operands[j].OperaterType,

                                        };
                                        OperaterNode.Operands[i].Operands[j].Name = "1";
                                        OperaterNode.Operands[z].Operands[y].Name = "1";

                                        OperaterNode.Operands.Add(new OperaterNode()
                                        {
                                            OperaterType = (int)OperaterNode.Operater.And,
                                            Operands = new List<OperaterNode>()
                                            {
                                                factor,
                                                new OperaterNode()
                                                {
                                                    OperaterType= (int)OperaterNode.Operater.Or,
                                                    Operands = new List<OperaterNode>()
                                                    {
                                                        OperaterNode.Operands[i],
                                                        OperaterNode.Operands[z]
                                                    }
                                                }
                                            }
                                        });

                                        //Sequence is important
                                        OperaterNode.Operands.RemoveAt(i);
                                        OperaterNode.Operands.RemoveAt(z);

                                        changed = true;

                                        return changed;

                                    }
                                    else if (OperaterNode.Operands[i].Operands[j].Operands != null)
                                    {
                                        var zoomIn = new Simplifier(OperaterNode.Operands[i].Operands[j]);
                                        while (zoomIn.Factorize())
                                        {
                                            OperaterNode.Operands[i].Operands.RemoveAt(j);
                                            OperaterNode.Operands[i].Operands.Add(zoomIn.OperaterNode);
                                            changed = true;

                                        }

                                    }

                                }

                            }

                        }
                    }

                }

            }

            return changed;
        }
        //B1 = B || 0+A=A
        public bool IdentityLaw()
        {
            bool changed = false;

            for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
            {
                //And Law
                if (OperaterNode.Operands[i].OperaterType == (int)OperaterNode.Operater.And)
                { //Basic Element
                    if (OperaterNode.Operands[i].Operands != null)
                    {
                        for (int j = OperaterNode.Operands[i].Operands.Count - 1; j >= 0; j--)
                        {

                            if (OperaterNode.Operands[i].Operands[j].Name == "1" && OperaterNode.Operands[i].Operands.Count > 1)
                            {
                                OperaterNode.Operands[i].Operands.RemoveAt(j);
                                changed = true;
                                return changed;
                            }
                            else if (OperaterNode.Operands[i].Operands[j].Operands != null)
                            {
                                var zoomIn = new Simplifier(OperaterNode.Operands[i].Operands[j]);
                                while (zoomIn.IdentityLaw())
                                {
                                    OperaterNode.Operands[i].Operands.RemoveAt(j);
                                    OperaterNode.Operands[i].Operands.Add(zoomIn.OperaterNode);
                                    changed = true;

                                }
                            }

                        }
                    }


                }
                //OR Law
                else if (OperaterNode.Operands[i].OperaterType == (int)OperaterNode.Operater.Or)
                { //Basic Element
                    if (OperaterNode.Operands[i].Operands != null)
                    {
                        for (int j = OperaterNode.Operands[i].Operands.Count - 1; j >= 0; j--)
                        {

                            if (OperaterNode.Operands[i].Operands[j].Name == "0" && OperaterNode.Operands[i].Operands.Count > 1)
                            {
                                OperaterNode.Operands[i].Operands.RemoveAt(j);
                                changed = true;
                                return changed;
                            }
                            else if (OperaterNode.Operands[i].Operands[j].Operands != null)
                            {
                                var zoomIn = new Simplifier(OperaterNode.Operands[i].Operands[j]);
                                while (zoomIn.IdentityLaw())
                                {
                                    OperaterNode.Operands[i].Operands.RemoveAt(j);
                                    OperaterNode.Operands[i].Operands.Add(zoomIn.OperaterNode);
                                    changed = true;
                                }
                            }

                        }
                    }


                }

            }



            return changed;
        }


        //B0 = 0 || 1+A=1
        public bool NullLaw()
        {
            bool changed = false;
            //And Law
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
            { //Basic Element
                if (OperaterNode.Operands != null)
                {
                    for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                    {

                        if (OperaterNode.Operands[j].Name == "0")
                        {
                            OperaterNode.Name = "0";
                            OperaterNode.Value = true;
                            OperaterNode.Operands = null;
                            OperaterNode.OperaterType = 0;
                            changed = true;
                            return changed;
                        }
                        if (OperaterNode.Operands[j].Operands != null)
                        {
                            var zoomIn = new Simplifier(OperaterNode.Operands[j]);
                            while (zoomIn.NullLaw())
                            {
                                    OperaterNode.Operands.RemoveAt(j);
                                    OperaterNode.Operands.Add(zoomIn.OperaterNode);
                                    changed = true;
                            }
                        }
                    }
                }
               
            }
            //OR Law
            else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            { //Basic Element
                if (OperaterNode.Operands != null)
                {
                    for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                    {

                        if (OperaterNode.Operands[j].Name == "1")
                        {
                            OperaterNode.Name = "1";
                            OperaterNode.Value = true;
                            OperaterNode.Operands = null;
                            OperaterNode.OperaterType = 0;

                            changed = true;
                            return changed;
                        }

                        if (OperaterNode.Operands[j].Operands != null)
                        {
                            var zoomIn = new Simplifier(OperaterNode.Operands[j]);
                            while (zoomIn.NullLaw())
                            {
                                changed = true;
                                    OperaterNode.Operands.RemoveAt(j);
                                    OperaterNode.Operands.Add(zoomIn.OperaterNode);
                            }
                        }

                    }
                }


            }




            return changed;
        }
        public bool RemoveRedundance()
        {
            bool changed = false;

            for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
            {

                if (OperaterNode.Operands[i].Operands != null)
                {
                    var zoomIn = new Simplifier(OperaterNode.Operands[i]);
                    while (zoomIn.RemoveRedundance())
                    {
                        OperaterNode.Operands.RemoveAt(i);
                        OperaterNode.Operands.Add(zoomIn.OperaterNode);
                        changed = true;
                    }
                }
                if (OperaterNode.Operands != null && OperaterNode.Operands[i].Operands?.Count == 1)
                {
                    OperaterNode.Operands.Add(OperaterNode.Operands[i].Operands[0]);
                    OperaterNode.Operands.RemoveAt(i);
                    //for (int j = OperaterNode.Operands[i].Operands.Count - 1; j >= 0; j--)
                    //{
                    //    if (OperaterNode.Operands[i].Operands[j].Operands?.Count == 1)
                    //    {
                    //        OperaterNode.Operands[i].Operands.Add(OperaterNode.Operands[i].Operands[j].Operands[0]);
                    //        OperaterNode.Operands[i].Operands[j].Operands.RemoveAt(0);
                    //        changed = true;
                    //    }
                    //}
                    changed = true;
                }
            }
            return changed;
        }

        //¬BA OR B (¬A OR A) = ¬BA OR B1
        public bool InverseLaw()
        {
            bool changed = false;
            if (OperaterNode.Operands != null)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {

                    if (OperaterNode.Operands[i].Operands != null)
                    {
                        var zoomIn = new Simplifier(OperaterNode.Operands[i]);
                        while (zoomIn.InverseLaw())
                        {
                            OperaterNode.Operands.RemoveAt(i);
                            OperaterNode.Operands.Add(zoomIn.OperaterNode);
                            changed = true;
                        }
                    }
                }
            }
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or && OperaterNode.Operands.Count == 2)
            {
                if (OperaterNode.Operands[0].Name == OperaterNode.Operands[1].Name && OperaterNode.Operands[0].Value == !OperaterNode.Operands[1].Value && OperaterNode.Operands[0].Name != null)
                {
                    OperaterNode = new OperaterNode() { Name = "1", Value = true };
                    changed = true;

                }

            }

            return changed;
        }
        //¬BA OR B1 = B OR A
        public bool CommonIdentitiesLaw()
        {
            bool changed = false;
            if (OperaterNode.Operands != null && OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i].Name != null && OperaterNode.Operands[i].Value == true)
                    { //Basic node
                        for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                        {
                            if (j != i && OperaterNode.Operands[j].OperaterType == (int)OperaterNode.Operater.And && OperaterNode.Operands[j].Operands != null)
                            {
                                for (int z = OperaterNode.Operands[j].Operands.Count - 1; z >= 0; z--)
                                {
                                    if (OperaterNode.Operands[j].Operands[z].Name == OperaterNode.Operands[i].Name && !OperaterNode.Operands[j].Operands[z].Value)
                                    {
                                        OperaterNode.Operands[j].Operands.RemoveAt(z);
                                        changed = true;
                                        return changed;
                                    }
                                }

                            }

                        }
                    }
                }

            }


            return changed;
        }
        public Simplifier(OperaterNode node)
        {
            OperaterNode = node;
        }
        public string Temp { get; set; }
        public bool Simplify()
        {
            Changed = false;

            Changed = Factorize() == true || Changed;
            string result = OperaterNode.Outputer();
            if (Temp?.Trim() != result.Trim()) { Temp = result; Console.WriteLine(result); }

            Changed = IdentityLaw() == true || Changed;
            result = OperaterNode.Outputer();
            if (Temp.Trim() != result.Trim()) { Temp = result; Console.WriteLine(result); }

            Changed = InverseLaw() == true || Changed;
            result = OperaterNode.Outputer();
            if (Temp.Trim() != result.Trim()) { Temp = result; Console.WriteLine(result); }

            Changed = CommonIdentitiesLaw() == true || Changed;
            result = OperaterNode.Outputer();
            if (Temp.Trim() != result.Trim()) { Temp = result; Console.WriteLine(result); }

            Changed = NullLaw() == true || Changed;
            result = OperaterNode.Outputer();
            if (Temp.Trim() != result.Trim()) { Temp = result; Console.WriteLine(result); }

            Changed = RemoveRedundance() == true || Changed;
            result = OperaterNode.Outputer();
            if (Temp.Trim() != result.Trim()) { Temp = result; Console.WriteLine(result); }


            if (Changed == false) Temp = string.Empty;
            return Changed;
        }

    }
}
