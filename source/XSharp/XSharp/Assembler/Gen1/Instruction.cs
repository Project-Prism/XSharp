﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XSharp.Assembler
{
    public abstract class Instruction : BaseAssemblerElement
    {
        /// <summary>
        /// Cache for the default mnemonics.
        /// </summary>
        public static Dictionary<Type, string> defaultMnemonicsCache = new Dictionary<Type, string>();

        protected string mMnemonic;
        public string Mnemonic
        {
            get { return mMnemonic; }
        }

        protected int mMethodID;
        public int MethodID
        {
            get { return mMethodID; }
        }

        protected int mAsmMethodIdx;
        public int AsmMethodIdx
        {
            get { return mAsmMethodIdx; }
        }

        public override void WriteText(XSharp.Assembler.Assembler aAssembler, TextWriter aOutput)
        {
            aOutput.Write(mMnemonic);
        }

        protected Instruction(string mnemonic = null) : this(true)
        {
        }

        protected Instruction(bool aAddToAssembler, string mnemonic = null)
        {
            if (aAddToAssembler)
            {
                XSharp.Assembler.Assembler.CurrentInstance.Add(this);
            }
            mMnemonic = mnemonic;
            if (mMnemonic == null)
            {
                var type = GetType();
                mMnemonic = GetDefaultMnemonic(type);
            }
        }

        /// <summary>
        /// Gets default operation mnemonic for given type.
        /// </summary>
        /// <param name="type">Type for which gets default mnemonics.</param>
        /// <returns>Default mnemonics for the type.</returns>
        private static string GetDefaultMnemonic(Type type)
        {
            string xMnemonic;
            if (defaultMnemonicsCache.TryGetValue(type, out xMnemonic))
            {
                return xMnemonic;
            }

            var xAttribs = type.GetCustomAttributes(typeof(OpCodeAttribute), false)?.ToList();
            if (xAttribs != null && xAttribs.Any())
            {
                var xAttrib = (OpCodeAttribute)xAttribs[0];
                xMnemonic = xAttrib.Mnemonic;
            }
            else
            {
                xMnemonic = string.Empty;
            }

            lock (defaultMnemonicsCache)
            {
                if (!defaultMnemonicsCache.ContainsKey(type))
                {
                    defaultMnemonicsCache.Add(type, xMnemonic);
                }
            }
            return xMnemonic;
        }

        public override ulong? ActualAddress
        {
            get
            {
                // TODO: for now, we dont have any data alignment
                return StartAddress;
            }
        }

        public override void UpdateAddress(XSharp.Assembler.Assembler aAssembler, ref ulong aAddress)
        {
            base.UpdateAddress(aAssembler, ref aAddress);
        }

        public override bool IsComplete(Assembler aAssembler)
        {
            throw new NotImplementedException("Method not implemented for instruction " + this.GetType().FullName.Substring(typeof(Instruction).Namespace.Length + 1));
        }

        public override void WriteData(XSharp.Assembler.Assembler aAssembler, Stream aOutput)
        {
            throw new NotImplementedException("Method not implemented for instruction " + this.GetType().FullName.Substring(typeof(Instruction).Namespace.Length + 1));
        }
    }
}
