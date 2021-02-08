﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanEquation
{
    public class OperaterNode { 
        public int MemberCount()
        {

            int sum = 0;
            if (Name != null) sum = 1;
            else
            {
                bool added = false;
                foreach (OperaterNode n in Operands)
                {
                    sum += n.MemberCount();
                    if (!added)
                    {
                        added = true;
                    }

                }
            }
            return sum;
        }
        public enum Operater
        {
            Operand, And, Or
        }
        public string Name { get; set; }
        public bool Value { get; set; }
        public int OperaterType { get; set; }
        public List<OperaterNode> Operands { get; set; }


        public OperaterNode Simplified()
        {
            var node = new OperaterNode() { Name = Name, Value = Value, Operands = Operands };
            return node;
        }

        //¬A=¬A,A=A
        public bool EqaulValue(OperaterNode n)
        {
            if (Name == null) return false;
            return n.Name == Name && n.Value == Value;
        }
        public bool IsInverse(OperaterNode n)
        {
            if (Name == null) return false;
            return n.Name == Name && n.Value == !Value;
        }
        //A=A,A=¬A
        public bool EqaulName(OperaterNode n)
        {
            if (Name == null) return false;
            return n.Name == Name && n.Value == Value;
        }


        public string Translate(bool started = false)
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

        public string ZoomIn()
        {
            string result = "";
            bool first = true;
            foreach (OperaterNode n in Operands.OrderBy(o => o.OperaterType).ToList())
            {
                if (OperaterType == (int)Operater.Or && !first)
                {
                    result += $" {Contants.OR} ";
                    first = true;
                }
                result += n.Translate(true);

                first = false;

            }
            return result;

        }

        public OperaterNode()
        {

        }
    }

}