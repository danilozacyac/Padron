﻿#pragma checksum "..\..\..\EstructuraKiosko\ArbolKiosko.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8BCAB07BB7C54EFD6D5AD440E0F13570"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Kiosko.EstructuraKiosko.Behavior;
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
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Shapes;
using UIControls;


namespace Kiosko.EstructuraKiosko {
    
    
    /// <summary>
    /// ArbolKiosko
    /// </summary>
    public partial class ArbolKiosko : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadBusyIndicator BusyIndicator;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox wishlistView;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadTreeView allProductsView;
        
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
            System.Uri resourceLocater = new System.Uri("/Kiosko;component/estructurakiosko/arbolkiosko.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
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
            this.BusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.wishlistView = ((System.Windows.Controls.ListBox)(target));
            
            #line 110 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
            this.wishlistView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.wishlistView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.allProductsView = ((Telerik.Windows.Controls.RadTreeView)(target));
            
            #line 143 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
            this.allProductsView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.allProductsView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 155 "..\..\..\EstructuraKiosko\ArbolKiosko.xaml"
            ((UIControls.SearchTextBox)(target)).Search += new System.Windows.RoutedEventHandler(this.SearchTextBox_Search);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

