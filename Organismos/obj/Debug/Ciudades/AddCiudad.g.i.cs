﻿#pragma checksum "..\..\..\Ciudades\AddCiudad.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "433B8697AB1934699AA509AC0767F0A2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.36392
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PadronApi.Converter;
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
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Shapes;


namespace Organismos.Ciudades {
    
    
    /// <summary>
    /// AddCiudad
    /// </summary>
    public partial class AddCiudad : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\Ciudades\AddCiudad.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CbxPais;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Ciudades\AddCiudad.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CbxEstado;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Ciudades\AddCiudad.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtCiudad;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Ciudades\AddCiudad.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSalir;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Ciudades\AddCiudad.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Organismos;component/ciudades/addciudad.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Ciudades\AddCiudad.xaml"
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
            
            #line 9 "..\..\..\Ciudades\AddCiudad.xaml"
            ((Organismos.Ciudades.AddCiudad)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\Ciudades\AddCiudad.xaml"
            ((Organismos.Ciudades.AddCiudad)(target)).PreviewClosed += new System.EventHandler<Telerik.Windows.Controls.WindowPreviewClosedEventArgs>(this.RadWindow_PreviewClosed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CbxPais = ((System.Windows.Controls.ComboBox)(target));
            
            #line 30 "..\..\..\Ciudades\AddCiudad.xaml"
            this.CbxPais.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CbxPais_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CbxEstado = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.TxtCiudad = ((System.Windows.Controls.TextBox)(target));
            
            #line 54 "..\..\..\Ciudades\AddCiudad.xaml"
            this.TxtCiudad.LostFocus += new System.Windows.RoutedEventHandler(this.TxtCiudad_LostFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BtnSalir = ((System.Windows.Controls.Button)(target));
            
            #line 63 "..\..\..\Ciudades\AddCiudad.xaml"
            this.BtnSalir.Click += new System.Windows.RoutedEventHandler(this.BtnSalir_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.BtnGuardar = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\Ciudades\AddCiudad.xaml"
            this.BtnGuardar.Click += new System.Windows.RoutedEventHandler(this.BtnGuardar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

