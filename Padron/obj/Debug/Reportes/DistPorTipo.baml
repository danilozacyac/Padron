   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M4clr-namespace:System.Globalization;assembly=mscorlibSystem.Globalization  5      
 Padron!   Padron.Reportes.DistPorTipo   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      NGlobalization4clr-namespace:System.Globalization;assembly=mscorlib     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      d `DevExpress.Xpf.Charts.v13.1, Version=13.1.10.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a>dxc4http://schemas.devexpress.com/winfx/2008/xaml/charts  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
    5	      -   5        $	��741��5
      $	��800��5      
�������$-��'Distribución por medio de publicación��5      )  #Telerik.Windows.Controls.WindowBase    WindowState$  	Maximized=�5      .��5      � -   5         �Name$ RadYear��5       � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $
��1.5* �6   �� 5      $��* �6   .+�5   
   * 
 $Telerik.Windows.Controls.RadGridView  -   5          Name$ GReporte��5      $��1��5      $��Stretch=�5      $��Stretch=�5      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     EnableRowVirtualization . $��14(�5         
IsReadOnly .    RowIndicatorVisibility$ 	Collapsed=�5"         ShowGroupPanel .  *  $Telerik.Windows.Controls.DataControl	   ItemsSource	 5!      �� 
   Columns
 5$      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5&      - 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 2* 5'      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Estado��5)         IsFilterable .     DataMemberBinding 5(      �� *Estado+5*        5+      $ * 5,      $ Total CD��5.       .     ShowFieldFilters .   5-      �� *Cd+50        51      $ * 52      $ 	Total Dvd��54       .   .   53      �� *Dvd+56        57      $ * 58      $ Total Libro��5:       .   59      �� *Libro+5;        5<      $ * 5=      $ Total eBook��5?       .   .   5>      �� *Ebook+5A        5B      $ * 5C      $ Total Libro y Disco��5E       .   .   5D      �� *Ambos+5G        5H      $	 120 5I      $ Total AudioLibro��5K       .   .   5J      �� *
AudioLibro+5M      5P      �� 5Q   
   $��	10,10,0,0q�5R      $
��Left=�5S      $	��Top=�5T      $(��"Visualizar información del año: ��5U      /	  )Telerik.Windows.Controls.RadNumericUpDown	  5V   
   -   5         	 Name$ UDYear��5W      $��96��5X      $��
190,10,0,0q�5Y      $
��Left=�5Z      $	��Top=�5[       	  	IsInteger . +
  %Telerik.Windows.Controls.RadRangeBase 
  Minimum$
 2016Q�5]       	  NumberFormatInfo 5_      +   %System.Globalization.NumberFormatInfo  5`         NumberDecimalDigits$ 0��61      NumberGroupSeparator$  ��6I   5b      (  "Telerik.Windows.Controls.RadButton  5c   
   -   5          Name$ 
RBtnExport��5d      $	��132��5e      $
��Auto��5f      $��	0,10,10,0q�5g      $��Right=�5h      $	��Top=�5i      $��Exportar a Excel��5k      (  "DevExpress.Xpf.Charts.ChartControl  5l   
   -   5          Name$ 	ChartDist��5m      $��2��5n      $��0q�5o      $��Stretch=�5p      $��Stretch=�5q         Legend 5r      "  DevExpress.Xpf.Charts.Legend  5s         Diagram. 5u      '  !DevExpress.Xpf.Charts.XYDiagram2D  #  DevExpress.Xpf.Charts.Diagram   Series. 5v      1  +DevExpress.Xpf.Charts.BarSideBySideSeries2D  "  DevExpress.Xpf.Charts.Series   DisplayName$ Series 1��6,   5w      5x      5�      5�      