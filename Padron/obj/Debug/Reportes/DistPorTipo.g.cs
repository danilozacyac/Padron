﻿#pragma checksum "..\..\..\Reportes\DistPorTipo.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2E7F6D49E153819BA59B5CE966CED5F31FA1BB1E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Xpf.Charts;
using System;
using System.Diagnostics;
using System.Globalization;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;
using Telerik.Windows.Controls.Carousel;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.RibbonView;
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Shapes;


namespace Padron.Reportes {
    
    
    /// <summary>
    /// DistPorTipo
    /// </summary>
    public partial class DistPorTipo : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\Reportes\DistPorTipo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RadYear;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Reportes\DistPorTipo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView GReporte;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\Reportes\DistPorTipo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadNumericUpDown UDYear;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\Reportes\DistPorTipo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton RBtnExport;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\Reportes\DistPorTipo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Charts.ChartControl ChartDist;
        
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
            System.Uri resourceLocater = new System.Uri("/Padron;component/reportes/distportipo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Reportes\DistPorTipo.xaml"
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
            
            #line 14 "..\..\..\Reportes\DistPorTipo.xaml"
            ((Padron.Reportes.DistPorTipo)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RadYear = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.GReporte = ((Telerik.Windows.Controls.RadGridView)(target));
            return;
            case 4:
            this.UDYear = ((Telerik.Windows.Controls.RadNumericUpDown)(target));
            
            #line 94 "..\..\..\Reportes\DistPorTipo.xaml"
            this.UDYear.ValueChanged += new System.EventHandler<Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs>(this.UDYear_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RBtnExport = ((Telerik.Windows.Controls.RadButton)(target));
            
            #line 106 "..\..\..\Reportes\DistPorTipo.xaml"
            this.RBtnExport.Click += new System.Windows.RoutedEventHandler(this.RBtnExport_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ChartDist = ((DevExpress.Xpf.Charts.ChartControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

