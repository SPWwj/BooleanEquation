using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanEquation
{
    public class Simplifier
    {
        public bool Changed { get; set; } = false;

        public OperaterNode OperaterNode { get; set; }

        public readonly OperaterNode OriginalNode;
        public string OriginalTranslate = "";
        // Only take place in Or Gate
        public bool Factorize()
        {
            if (OperaterNode.Operands == null) return false;
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
                                if (OperaterNode.Operands[z].Operands == null) return false;
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
                                        return true;

                                    }

                                }

                            }

                        }
                    }

                }

            }

            return false;
        }
        //B1 = B || 0+A=A Done
        public bool IdentityLaw()
        {
            if (OperaterNode.Operands == null) return false;

            for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
            {
                if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
                {
                    if (OperaterNode.Operands[i].Name == "1" && OperaterNode.Operands.Count > 1)
                    {
                        OperaterNode.Operands.RemoveAt(i);
                        return true;
                    }
                }
                else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
                {
                    if (OperaterNode.Operands[i].Name == "0" && OperaterNode.Operands.Count > 1)
                    {
                        OperaterNode.Operands.RemoveAt(i);
                        return true;
                    }
                }

            }
            return false;
        }


        //B0 = 0 || 1+A=1 //Done
        public bool NullLaw()
        {
            if (OperaterNode.Operands == null) return false;
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
            {
                for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                {
                    if (OperaterNode.Operands[j].Name == "0")
                    {
                        OperaterNode.Name = "0";
                        OperaterNode.Value = true;
                        OperaterNode.Operands = null;
                        OperaterNode.OperaterType = 0;
                        return true;
                    }
                }
            }
            else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {
                for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                {
                    if (OperaterNode.Operands[j].Name == "1")
                    {
                        OperaterNode.Name = "1";
                        OperaterNode.Value = true;
                        OperaterNode.Operands = null;
                        OperaterNode.OperaterType = 0;
                        return true;
                    }
                }
            }
            return false;
        }
        //Done
        public bool RemoveRedundance()
        {
            if (OperaterNode.Operands == null) return false;
            for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
            {
                if (OperaterNode.Operands != null && OperaterNode.Operands[i].Operands?.Count == 1)
                {
                    OperaterNode.Operands.Add(OperaterNode.Operands[i].Operands[0]);
                    OperaterNode.Operands.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        //(¬A OR A) =1 //Done
        public bool InverseLaw()
        {

            if (OperaterNode.Operands == null) return false;

            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {

                foreach (OperaterNode n in OperaterNode.Operands)
                {
                    var result = OperaterNode.Operands.Where(s => s.IsInverse(n));
                    if (result.Any())
                    {
                        OperaterNode = new OperaterNode() { Name = "1", Value = true };
                        return true;
                    }
                }

            }

            else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
            {
                if (OperaterNode.Operands.Where(s => s.Name != null && s.Name != string.Empty).Count() == OperaterNode.Operands.Count)
                {
                    foreach (OperaterNode n in OperaterNode.Operands)
                    {
                        var result = OperaterNode.Operands.Where(s => s.IsInverse(n));
                        if (result.Any())
                        {
                            OperaterNode = new OperaterNode() { Name = "0", Value = true };
                            return true;
                        }
                    }
                }


            }

            return false;
        }
        //A¬B OR B = A OR B
        public bool CommonIdentitiesLaw()
        {
            if (OperaterNode.Operands == null) return false;
            //OriginalTranslate = OperaterNode.Translate();

            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i].Name != null && OperaterNode.Operands[i].Value == true)
                    { 
                        for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                        {
                            if (j != i && OperaterNode.Operands[j].OperaterType == (int)OperaterNode.Operater.And && OperaterNode.Operands[j].Operands != null && OperaterNode.Operands[j].Operands.Count>1)
                            {
                                for (int z = OperaterNode.Operands[j].Operands.Count - 1; z >= 0; z--)
                                {
                                    
                                    if (OperaterNode.Operands[i].IsInverse(OperaterNode.Operands[j].Operands[z]))
                                    {
                                        OperaterNode.Operands[j].Operands.RemoveAt(z);
                                        return true;
                                    }
                                }

                            }

                        }
                    }
                }

            }


            return false;
        }
        public Simplifier(OperaterNode node)
        {
            OperaterNode = node;
        }
        public Simplifier(OperaterNode node,string translated)
        {
            OperaterNode = node;
            OriginalNode = node;
            this.OriginalTranslate = node.Translate();
        }
        public string Temp { get; set; }



        //New
        //AA = A , A + A = A
        public bool IdempotentLaw()
        {
            if (OperaterNode.Operands == null) return false;

            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (OperaterNode.Operands[i].EqaulValue(OperaterNode.Operands[j]))
                        {
                            OperaterNode.Operands.RemoveAt(j);
                            return true;
                        }
                    }
                }
            }
            else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (OperaterNode.Operands[i].EqaulValue(OperaterNode.Operands[j]))
                        {
                            OperaterNode.Operands.RemoveAt(j);
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public bool Simplify()
        {
            while (RemoveRedundance());


            if (Factorize())
            {
                Console.WriteLine("Factorize Applied : (" + OriginalTranslate + ") to "+ OperaterNode.Translate());
                return true;
            }
            if (IdentityLaw())
            {
                Console.WriteLine("IdentityLaw Applied : (" + OriginalTranslate + ") Removed");
                return true;
            }
            if (InverseLaw())
            {
                Console.WriteLine("InverseLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (CommonIdentitiesLaw())
            {
                Console.WriteLine("CommonIdentitiesLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (NullLaw())
            {
                Console.WriteLine("NullLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (IdempotentLaw())
            {
                Console.WriteLine("IdempotentLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }



            return false;
        }

        public bool SimplifyAll()
        {

            bool changed = false;
                if (OperaterNode.Operands == null) return changed;
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i] != null)
                    {
                        var zoomIn = new Simplifier(OperaterNode.Operands[i], OperaterNode.Operands[i].Translate());
                        Changed = zoomIn.SimplifyAll()==true||Changed;

                        OperaterNode.Operands.RemoveAt(i);
                        OperaterNode.Operands.Add(zoomIn.OperaterNode);
                    }
                }
                if (Changed) changed = true;
                else changed = Simplify();


            return changed;
        }
    }

}
