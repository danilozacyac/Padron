﻿#pragma checksum "..\..\..\Ciudades\Mantenimiento.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "11C8D694425A3DE522AF6E4BF683C96F"
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


namespace Organismos.Ciudades {
    
    
    /// <summary>
    /// Mantenimiento
    /// </summary>
    public partial class Mantenimiento : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\Ciudades\Mantenimiento.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LstPaises;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Ciudades\Mantenimiento.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LstEstados;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Ciudades\Mantenimiento.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox LstCiudades;
        
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
            System.Uri resourceLocater = new System.Uri("/Organismos;component/ciudades/mantenimiento.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Ciudades\Mantenimiento.xaml"
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
            
            #line 7 "..\..\..\Ciudades\Mantenimiento.xaml"
            ((Organismos.Ciudades.Mantenimiento)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LstPaises = ((System.Windows.Controls.ListBox)(target));
            
            #line 27 "..\..\..\Ciudades\Mantenimiento.xaml"
            this.LstPaises.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.LstPaises_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.LstEstados = ((System.Windows.Controls.ListBox)(target));
            
            #line 44 "..\..\..\Ciudades\Mantenimiento.xaml"
            this.LstEstados.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.LstEstados_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LstCiudades = ((System.Windows.Controls.ListBox)(target));
            
            #line 62 "..\..\..\Ciudades\Mantenimiento.xaml"
            this.LstCiudades.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.LstCiudades_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
