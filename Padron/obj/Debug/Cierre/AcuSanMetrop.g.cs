﻿#pragma checksum "..\..\..\Cierre\AcuSanMetrop.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0B1AF020EFDFD0D299FBFED87C55DDC0"
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
    /// AcuSanMetrop
    /// </summary>
    public partial class AcuSanMetrop : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\Cierre\AcuSanMetrop.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadDatePicker DpAcuse;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Cierre\AcuSanMetrop.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtArchivoPaq;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Cierre\AcuSanMetrop.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnArchivoGuia;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Cierre\AcuSanMetrop.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnCancelar;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Cierre\AcuSanMetrop.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Padron;component/cierre/acusanmetrop.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Cierre\AcuSanMetrop.xaml"
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
            
            #line 8 "..\..\..\Cierre\AcuSanMetrop.xaml"
            ((Padron.Cierre.AcuSanMetrop)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\Cierre\AcuSanMetrop.xaml"
            ((Padron.Cierre.AcuSanMetrop)(target)).PreviewClosed += new System.EventHandler<Telerik.Windows.Controls.WindowPreviewClosedEventArgs>(this.RadWindow_PreviewClosed);
            
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
            
            #line 42 "..\..\..\Cierre\AcuSanMetrop.xaml"
            this.BtnArchivoGuia.Click += new System.Windows.RoutedEventHandler(this.BtnArchivoGuia_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BtnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\Cierre\AcuSanMetrop.xaml"
            this.BtnCancelar.Click += new System.Windows.RoutedEventHandler(this.BtnCancelar_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BtnGuardar = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\Cierre\AcuSanMetrop.xaml"
            this.BtnGuardar.Click += new System.Windows.RoutedEventHandler(this.BtnGuardar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

