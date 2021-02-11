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
        public string OriginalTranslate { get; set; }
        // Only take place in Or Gate
        public bool Factorize()
        {
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

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
        public bool Expend()
        {
            if (OperaterNode.Operands == null || OperaterNode.Value == false || OperaterNode.Operands.Count < 2 || OperaterNode.OperaterType != (int)OperaterNode.Operater.And) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

            for (int i = OperaterNode.Operands.Count - 2; i >= 0; i--)
            {
                if (OperaterNode.Operands[i].OperaterType == (int)OperaterNode.Operater.Or)
                {
                    if (OperaterNode.Operands[i + 1].OperaterType == (int)OperaterNode.Operater.Operand)
                    {
                        for (int j = OperaterNode.Operands[i].Operands.Count - 1; j >= 0; j--)
                        {
                            OperaterNode.Operands[i].Operands.Add(new OperaterNode()
                            {
                                Value = true,
                                OperaterType = (int)OperaterNode.Operater.And,
                                Operands = new List<OperaterNode>() {
                                OperaterNode.Operands[i+1],OperaterNode.Operands[i].Operands[j]
                                }
                            });
                            OperaterNode.Operands[i].Operands.RemoveAt(j);

                        }
                        OperaterNode.Operands.RemoveAt(i + 1);
                        OperaterNode.LockExpend = true;
                        return true;
                    }
                    if (OperaterNode.Operands[i + 1].OperaterType == (int)OperaterNode.Operater.Or)
                    {
                        var result = new OperaterNode() { Value=true, LockExpend=true, OperaterType=(int)OperaterNode.Operater.Or , Operands = new List<OperaterNode>() { } };
                        for (int j = OperaterNode.Operands[i + 1].Operands.Count - 1; j >= 0; j--)
                        {
                            for (int y = OperaterNode.Operands[i].Operands.Count - 1; y >= 0; y--)
                            {
                                result.Operands.Add(new OperaterNode()
                                {
                                    Value = true,
                                    OperaterType = (int)OperaterNode.Operater.And,
                                    Operands = new List<OperaterNode>() {
                                    OperaterNode.Operands[i].Operands[y],OperaterNode.Operands[i+1].Operands[j]
                                 }
                                });

                            }


                        }
                        OperaterNode.Operands.Add(result);

                        OperaterNode.Operands.RemoveAt(i + 1);
                        OperaterNode.Operands.RemoveAt(i);

                        return true;
                    }
                }
            }

            return false;
        }

        //B1 = B || 0+A=A Done
        public bool IdentityLaw()
        {
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

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
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

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
        public bool RemoveRedundance()
        {
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
            {
                if (OperaterNode.Operands != null && OperaterNode.Operands[i].Operands?.Count == 1 && OperaterNode.Operands[i].Operands.First().Value == true)
                {
                    OperaterNode.Operands.Add(OperaterNode.Operands[i].Operands[0]);
                    OperaterNode.Operands.RemoveAt(i);
                    return true;
                }
            }
            //A(B)D+C=ABD+C
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i].OperaterType == (int)OperaterNode.Operater.And)
                    {
                        foreach (OperaterNode n in OperaterNode.Operands[i].Operands)
                        {
                            OperaterNode.Operands.Add(n);
                        }
                        OperaterNode.Operands.RemoveAt(i);
                        return true;
                    }
                }
            }
            //A+(C+B)
            else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i].OperaterType == (int)OperaterNode.Operater.Or)
                    {
                        foreach (OperaterNode n in OperaterNode.Operands[i].Operands)
                        {
                            OperaterNode.Operands.Add(n);
                        }
                        OperaterNode.Operands.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        //(¬A OR A) =1 //Done
        public bool InverseLaw()
        {

            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

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
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    if (OperaterNode.Operands[i].Name != null && OperaterNode.Operands[i].Value == true)
                    {
                        for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                        {
                            if (j != i && OperaterNode.Operands[j].OperaterType == (int)OperaterNode.Operater.And && OperaterNode.Operands[j].Operands != null && OperaterNode.Operands[j].Operands.Count > 1)
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

        //AA = A , A + A = A
        public bool IdempotentLaw()
        {
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

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

        public bool DeMorganLawExpend()
        {
            if (OperaterNode.Operands == null) return false;

            //¬(AB)=¬A+¬B
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And && OperaterNode.Value == false)
            {

                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    OperaterNode.Operands[i].Value = !OperaterNode.Operands[i].Value;
                }
                OperaterNode.OperaterType = (int)OperaterNode.Operater.Or;
                OperaterNode.Value = true;

                return true;

            }
            //¬(A+B)=¬A¬B
            else if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or && OperaterNode.Value == false)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    OperaterNode.Operands[i].Value = !OperaterNode.Operands[i].Value;
                }
                OperaterNode.OperaterType = (int)OperaterNode.Operater.And;
                OperaterNode.Value = true;

                return true;

            }

            return false;
        }
        public bool DeMorganLawFactorize()
        {
            if (OperaterNode.Operands == null) return false;
            //¬A¬B=¬(A+B)
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And && OperaterNode.Operands.Where(s => s.Value == false).Count() == OperaterNode.Operands.Count)
            {

                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    OperaterNode.Operands[i].Value = !OperaterNode.Operands[i].Value;
                }
                OperaterNode.OperaterType = (int)OperaterNode.Operater.Or;
                OperaterNode.Value = false;

                return true;

            }
            //¬A+¬B=¬(AB)
            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.Or && OperaterNode.Operands.Where(s => s.Value == false).Count() == OperaterNode.Operands.Count)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    OperaterNode.Operands[i].Value = !OperaterNode.Operands[i].Value;
                }
                OperaterNode.OperaterType = (int)OperaterNode.Operater.And;
                OperaterNode.Value = false;

                return true;

            }

            return false;
        }

        //A(A + B) = A , A + AB = A
        public bool AbsortptionLaw()
        {
            if (OperaterNode.Operands == null || OperaterNode.Value == false) return false;
            OriginalTranslate = OperaterNode.Translate();//Wired Bug

            if (OperaterNode.OperaterType == (int)OperaterNode.Operater.And)
            {
                for (int i = OperaterNode.Operands.Count - 1; i >= 0; i--)
                {
                    for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                    {
                        if (j != i && OperaterNode.Operands[j].OperaterType == (int)OperaterNode.Operater.Or &&
                           OperaterNode.Operands[j].Operands.Where(s => s.EqaulValue(OperaterNode.Operands[i])).Any())
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
                    for (int j = OperaterNode.Operands.Count - 1; j >= 0; j--)
                    {
                        if (j != i && OperaterNode.Operands[j].OperaterType == (int)OperaterNode.Operater.And &&
                           OperaterNode.Operands[j].Operands.Where(s => s.EqaulValue(OperaterNode.Operands[i])).Any())
                        {
                            OperaterNode.Operands.RemoveAt(j);
                            return true;
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

        private void LocksDeactivate()
        {
            OperaterNode.DeMorganLawLockExpend = false;
            OperaterNode.DeMorganLawLockFactor = false;
            OperaterNode.LockExpend = false;
            OperaterNode.LockFactor = false;
        }
        public bool Simplify()
        {
            while (RemoveRedundance())
            {
                //OperaterNode.DeMorganLawLockExpend = false;
                //OperaterNode.DeMorganLawLockFactor = false;
            };



            if (IdentityLaw())
            {
                LocksDeactivate();
                Console.WriteLine("IdentityLaw Applied : (" + OriginalTranslate + ") Removed");
                return true;
            }
            if (InverseLaw())
            {
                LocksDeactivate();
                Console.WriteLine("InverseLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (CommonIdentitiesLaw())
            {
                LocksDeactivate();
                Console.WriteLine("CommonIdentitiesLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (NullLaw())
            {
                LocksDeactivate();
                Console.WriteLine("NullLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (IdempotentLaw())
            {
                LocksDeactivate();
                Console.WriteLine("IdempotentLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }
            if (AbsortptionLaw())
            {
                LocksDeactivate();
                Console.WriteLine("AbsortptionLaw Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                return true;
            }

            if (!OperaterNode.LockExpend)
            {
                if (Factorize())
                {
                    OperaterNode.DeMorganLawLockExpend = false;
                    OperaterNode.DeMorganLawLockFactor = false;
                    OperaterNode.LockFactor = true;
                    Console.WriteLine("Factorize Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                    return true;
                }
            }
            if (!OperaterNode.LockFactor)
            {
                if (Expend())
                {
                    OperaterNode.DeMorganLawLockExpend = false;
                    OperaterNode.DeMorganLawLockFactor = false;
                    //OperaterNode.LockExpend = true; Need to put add changed node

                    Console.WriteLine("ExpendApplied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                    return true;
                }
            }


            if (!OperaterNode.DeMorganLawLockFactor)
            {
                if (DeMorganLawExpend())
                {
                    Console.WriteLine("DeMorganLawExpend Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());

                    OperaterNode.DeMorganLawLockExpend = true;
                    OperaterNode.LockExpend = false;
                    OperaterNode.LockFactor = false;
                    return true;
                }

            }

            if (!OperaterNode.DeMorganLawLockExpend)
            {
                if (DeMorganLawFactorize())
                {
                    Console.WriteLine("DeMorganLawFactorize Applied : (" + OriginalTranslate + ") to " + OperaterNode.Translate());
                    OperaterNode.DeMorganLawLockFactor = true;
                    OperaterNode.LockExpend = false;
                    OperaterNode.LockFactor = false;
                    return true;
                }

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
                    var zoomIn = new Simplifier(OperaterNode.Operands[i]);
                    Changed = zoomIn.SimplifyAll() == true || Changed;

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
