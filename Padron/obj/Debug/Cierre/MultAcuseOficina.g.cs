﻿#pragma checksum "..\..\..\Cierre\MultAcuseOficina.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F0AC7B8F82746FACF0E45C23616A16C8A5EFA118"
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


namespace Padron.Cierre {
    
    
    /// <summary>
    /// MultAcuseOficina
    /// </summary>
    public partial class MultAcuseOficina : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\Cierre\MultAcuseOficina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadDatePicker DpAcuse;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Cierre\MultAcuseOficina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtArchivoPaq;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Cierre\MultAcuseOficina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnArchivoGuia;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Cierre\MultAcuseOficina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnCancelar;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\Cierre\MultAcuseOficina.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnGuardar;
        
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
            System.Uri resourceLocater = new System.Uri("/Padron;component/cierre/multacuseoficina.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Cierre\MultAcuseOficina.xaml"
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
            
            #line 9 "..\..\..\Cierre\MultAcuseOficina.xaml"
            ((Padron.Cierre.MultAcuseOficina)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\Cierre\MultAcuseOficina.xaml"
            ((Padron.Cierre.MultAcuseOficina)(target)).PreviewClosed += new System.EventHandler<Telerik.Windows.Controls.WindowPreviewClosedEventArgs>(this.RadWindow_PreviewClosed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DpAcuse = ((Telerik.Windows.Controls.RadDatePicker)(target));
            return;
            case 3:
            this.TxtArchivoPaq = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.BtnArchivoGuia = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\Cierre\MultAcuseOficina.xaml"
            this.BtnArchivoGuia.Click += new System.Windows.RoutedEventHandler(this.BtnArchivoGuia_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BtnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\Cierre\MultAcuseOficina.xaml"
            this.BtnCancelar.Click += new System.Windows.RoutedEventHandler(this.BtnCancelar_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BtnGuardar = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\..\Cierre\MultAcuseOficina.xaml"
            this.BtnGuardar.Click += new System.Windows.RoutedEventHandler(this.BtnGuardar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

