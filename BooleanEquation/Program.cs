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
                    new OperaterNode { Name = "B", Value = true },
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
            var OrIdempotentLaw = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>()
                {
                    new OperaterNode(){ Name="C"},new OperaterNode(){ Name="C"}
                }

            };

            var distributiveLawTest = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>()
                {
                    new OperaterNode(){ OperaterType=(int)OperaterNode.Operater.Or,Operands=new List<OperaterNode>(){ new OperaterNode() { Name="A", Value = true }, new OperaterNode() { Name = "B" ,Value=true} } },
                    new OperaterNode(){ OperaterType=(int)OperaterNode.Operater.Or,Operands=new List<OperaterNode>(){ new OperaterNode() { Name="D", Value = true }, new OperaterNode() { Name = "C", Value = true } } },
                }

            };
            var FdistributiveLawTest = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.And,
                Operands = new List<OperaterNode>()
                {
                    new OperaterNode(){ Name="D" , Value=true},
                    new OperaterNode(){ OperaterType=(int)OperaterNode.Operater.Or,Operands=new List<OperaterNode>(){ new OperaterNode() { Name="A", Value = true }, new OperaterNode() { Name = "C", Value = true } } },
                }

            };
            var absorption = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.Or,
                Operands = new List<OperaterNode>()
                {
                    new OperaterNode(){ Name="A",Value=true },
                    new OperaterNode(){ OperaterType=(int)OperaterNode.Operater.And,Operands=new List<OperaterNode>(){ new OperaterNode() { Name="A", Value = true }, new OperaterNode() { Name = "C", Value = true } } },
                }

            };
            var De = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.Or,
                Operands = new List<OperaterNode>()
                {
                    new OperaterNode(){ Name="A",Value=true },
                    new OperaterNode(){ Name="B",Value=true },
                    new OperaterNode(){ OperaterType=(int)OperaterNode.Operater.And,Operands=new List<OperaterNode>(){ new OperaterNode() { Name="A", Value = false }, new OperaterNode() { Name = "B", Value = false } } },
                }

            };
            var DeMorganLawExpend = new OperaterNode()
            {
                OperaterType = (int)OperaterNode.Operater.Or,
                Value = false,
                Operands = new List<OperaterNode>()
                {
                    new OperaterNode(){OperaterType=(int)OperaterNode.Operater.And,Operands=new List<OperaterNode>(){ new OperaterNode() { Name="A",Value=true}, new OperaterNode() { Name = "B", Value = true } } },
                    new OperaterNode(){ Name="C",Value=true },
                }

            };
            var notDe = new OperaterNode() { Value = false, OperaterType = (int)OperaterNode.Operater.And, Operands =new List<OperaterNode>() { DeMorganLawExpend } };
            
            var p = new Simplifier(FdistributiveLawTest);
            Console.WriteLine("start "  );
            Console.WriteLine(p.OperaterNode.Translate());
           
            while (p.SimplifyAll())
            {
                p.Changed = false;
                Console.WriteLine(p.OperaterNode.Translate());
            }

            

        }

        //public static void add(ref a)
        //{
        //    a = +2;
        //}
    }
 


}
