﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace XSharp {
  public class Compiler {
    protected readonly TextWriter Out;
    protected string Indent = "";
    public int LineNo { get; private set; }
    public bool EmitUserComments = true;
    public bool EmitSourceCode = true;

    public Compiler(TextWriter aOut) {
      Out = aOut;
    }

    public void WriteLine(string aText = "") {
      Out.WriteLine(Indent + aText);
    }

    public void Emit(TextReader aIn) {
      try {
        LineNo = 1;
        // Do not trim it here. We need spaces for colorizing
        // and also to keep indentation in the output.
        string xText = aIn.ReadLine();
        while (xText != null) {
          int i = xText.Length - xText.TrimStart().Length;
          Indent = xText.Substring(0, i);

          if (string.IsNullOrWhiteSpace(xText)) {
            WriteLine();
          } else {
            var xLine = new Lines.XSharp(this, xText);
            xLine.Emit();
          }

          xText = aIn.ReadLine();
          LineNo++;
        }
      } catch (Exception e) {
        throw new Exception("Generation error on line " + LineNo, e);
      }
    }
  }
}