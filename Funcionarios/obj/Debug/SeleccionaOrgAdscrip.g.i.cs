﻿#pragma checksum "..\..\SeleccionaOrgAdscrip.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4F7C5129E9AAAE35C42E052B29E0A12880725E83"
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
using UIControls;


namespace Funcionarios {
    
    
    /// <summary>
    /// SeleccionaOrgAdscrip
    /// </summary>
    public partial class SeleccionaOrgAdscrip : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal UIControls.SearchTextBox SearchBox;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PanelOrganismo;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView GOrganismos;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LstTipoObra;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView GTirajes;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadComboBox CbxFunciones;
        
        #line default
        #line hidden
        
        
        #line 210 "..\..\SeleccionaOrgAdscrip.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnCancelar;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\SeleccionaOrgAdscrip.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Funcionarios;component/seleccionaorgadscrip.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SeleccionaOrgAdscrip.xaml"
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
            
            #line 10 "..\..\SeleccionaOrgAdscrip.xaml"
            ((Funcionarios.SeleccionaOrgAdscrip)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SearchBox = ((UIControls.SearchTextBox)(target));
            
            #line 28 "..\..\SeleccionaOrgAdscrip.xaml"
            this.SearchBox.Search += new System.Windows.RoutedEventHandler(this.SearchTextBox_Search);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PanelOrganismo = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.GOrganismos = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 45 "..\..\SeleccionaOrgAdscrip.xaml"
            this.GOrganismos.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.GOrganismos_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.LstTipoObra = ((System.Windows.Controls.ListBox)(target));
            return;
            case 6:
            this.GTirajes = ((Telerik.Windows.Controls.RadGridView)(target));
            return;
            case 7:
            this.CbxFunciones = ((Telerik.Windows.Controls.RadComboBox)(target));
            return;
            case 8:
            this.BtnCancelar = ((System.Windows.Controls.Button)(target));
            
            #line 216 "..\..\SeleccionaOrgAdscrip.xaml"
            this.BtnCancelar.Click += new System.Windows.RoutedEventHandler(this.BtnCancelar_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.BtnGuardar = ((System.Windows.Controls.Button)(target));
            
            #line 227 "..\..\SeleccionaOrgAdscrip.xaml"
            this.BtnGuardar.Click += new System.Windows.RoutedEventHandler(this.BtnGuardar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

