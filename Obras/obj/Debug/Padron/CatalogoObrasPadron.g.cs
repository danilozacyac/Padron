﻿#pragma checksum "..\..\..\Padron\CatalogoObrasPadron.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B2E740A9757FD85F1ED1BC3AA7F9AD26"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.36392
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Obras;
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


namespace Obras.Padron {
    
    
    /// <summary>
    /// CatalogoObrasPadron
    /// </summary>
    public partial class CatalogoObrasPadron : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\Padron\CatalogoObrasPadron.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadBusyIndicator BusyIndicator;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Padron\CatalogoObrasPadron.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LblTotales;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Padron\CatalogoObrasPadron.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView GObras;
        
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
            System.Uri resourceLocater = new System.Uri("/Obras;component/padron/catalogoobraspadron.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Padron\CatalogoObrasPadron.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 12 "..\..\..\Padron\CatalogoObrasPadron.xaml"
            ((Obras.Padron.CatalogoObrasPadron)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(target));
            return;
            case 3:
            this.LblTotales = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            
            #line 38 "..\..\..\Padron\CatalogoObrasPadron.xaml"
            ((UIControls.SearchTextBox)(target)).Search += new System.Windows.RoutedEventHandler(this.SearchTextBox_Search);
            
            #line default
            #line hidden
            return;
            case 5:
            this.GObras = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 46 "..\..\..\Padron\CatalogoObrasPadron.xaml"
            this.GObras.Filtered += new System.EventHandler<Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs>(this.GObras_Filtered);
            
            #line default
            #line hidden
            
            #line 49 "..\..\..\Padron\CatalogoObrasPadron.xaml"
            this.GObras.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.GObras_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 51 "..\..\..\Padron\CatalogoObrasPadron.xaml"
            this.GObras.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.GObras_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

