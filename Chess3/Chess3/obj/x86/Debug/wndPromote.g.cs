﻿#pragma checksum "..\..\..\wndPromote.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "43A1674EFCC6A55F74FEF53E2242B376"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Chess3 {
    
    
    /// <summary>
    /// wndPromote
    /// </summary>
    public partial class wndPromote : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\wndPromote.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNominate;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\..\wndPromote.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas1;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\wndPromote.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdRook;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\..\wndPromote.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdKnight;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\wndPromote.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdBishop;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\wndPromote.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdQueen;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Chess3;component/wndpromote.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\wndPromote.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblNominate = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.canvas1 = ((System.Windows.Controls.Canvas)(target));
            return;
            case 3:
            this.grdRook = ((System.Windows.Controls.Grid)(target));
            
            #line 8 "..\..\..\wndPromote.xaml"
            this.grdRook.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.grdRook_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.grdKnight = ((System.Windows.Controls.Grid)(target));
            
            #line 9 "..\..\..\wndPromote.xaml"
            this.grdKnight.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.grdKnight_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.grdBishop = ((System.Windows.Controls.Grid)(target));
            
            #line 10 "..\..\..\wndPromote.xaml"
            this.grdBishop.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.grdBishop_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.grdQueen = ((System.Windows.Controls.Grid)(target));
            
            #line 11 "..\..\..\wndPromote.xaml"
            this.grdQueen.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.grdQueen_MouseDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
